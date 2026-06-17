using Chu.Core;
using Chu.Utility.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviourSingleton<SoundManager>
{
    private Dictionary<SoundType, AudioClip> _clipCaches;

    private SoundOptionData _optionData;
    private AudioSource _bgmSpeaker;
    private List<AudioSource> _soundEffectSpeakers;

    private SoundType _currentBGM;

    public Coroutine coroutine;

    public SoundOptionData SoundOptionData
    {
        get => _optionData;
    }

    public void Init(Transform speakerRoot)
    {
        _clipCaches = new();
        _bgmSpeaker = GenerateSpeaker(speakerRoot, true, 0);
        _soundEffectSpeakers = new(10);
        for (int i = 0; i < 10; i++)
        {
            var speaker = GenerateSpeaker(speakerRoot, false, _optionData.FinalySEVolume);
            speaker.loop = false;
            _soundEffectSpeakers.Add(speaker);
        }
    }

    public void CacheClip(SoundType type)
    {
        if (type == SoundType.None)
            return;

        var clip = GetAudioClip(type);
        _clipCaches.Add(type, clip);
    }

    public void PlayBGM(SoundType type)
    {
        // TODO : 오디오 클립으로도 재생가능하게
        if (type == SoundType.None || _optionData.IsOn == false || _currentBGM == type)
            return;

        if (coroutine != null)
            StopCoroutine(coroutine);

        _currentBGM = type;
        var clip = GetAudioClip(type);
        coroutine = StartCoroutine(PlayFadeInOutBGM(clip));
    }

    public void PlaySE(SoundType type)
    {
        if (type == SoundType.None || _optionData.IsOn == false)
            return;

        foreach (var speaker in _soundEffectSpeakers)
        {
            if (speaker.isPlaying == false)
            {
                var clip = GetAudioClip(type);
                PlaySoundEffect(speaker, clip);
                break;
            }
        }
    }

    private AudioClip GetAudioClip(SoundType type)
    {
        AssetManager.LoadAssetSync<AudioClip>(type.ToString());

        return null;
    }

    private IEnumerator PlayFadeInOutBGM(AudioClip clip)
    {
        while (_bgmSpeaker.volume > 0.01f)
        {
            _bgmSpeaker.volume -= SoundOptionData.DefaultFadeSpeed * Time.deltaTime;
            yield return null;
        }

        _bgmSpeaker.volume = 0;
        _bgmSpeaker.clip = clip;
        _bgmSpeaker.Play();

        while (_bgmSpeaker.volume < _optionData.FinalyBGMVolume)
        {
            _bgmSpeaker.volume += SoundOptionData.DefaultFadeSpeed * Time.deltaTime;
            yield return null;
        }

        _bgmSpeaker.volume = _optionData.FinalyBGMVolume;
    }

    private void PlaySoundEffect(AudioSource speaker, AudioClip clip)
    {
        speaker.clip = clip;
        speaker.volume = _optionData.FinalySEVolume;
        speaker.pitch = 1f + Random.Range(-SoundOptionData.DefaultPitch, SoundOptionData.DefaultPitch);
        speaker.Play();
    }

    private AudioSource GenerateSpeaker(Transform parent, bool isLoop, float volume)
    {
        var obj = new GameObject("Speaker");
        obj.transform.parent = parent;
        var audio = obj.AddComponent<AudioSource>();
        audio.loop = isLoop;
        audio.volume = volume;
        return audio;
    }
}
