using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public static class AssetManager
{
    public static readonly ExternalFolderHandler ExternalFolder = new();

    // 비동기 로드
    public static void LoadGameObject(string path)
    {
        Addressables.LoadAssetAsync<GameObject>(path).Completed += OnLoadCompleted;
    }

    private static void OnLoadCompleted(AsyncOperationHandle<GameObject> handler)
    {
        if (handler.Status != AsyncOperationStatus.Succeeded)
            return;

        Object.Instantiate(handler.Result);
    }

    // 동기 로드
    public static T LoadAssetSync<T>(string path)
    {
        var handle = Addressables.LoadAssetAsync<T>(path);
        handle.WaitForCompletion();
        if (handle.Status != AsyncOperationStatus.Succeeded)
            Debug.LogError("handle.Status is not AsyncOperationStatus.Succeeded");

        return handle.Result;
    }

    // 동기 로드 후 오브젝트 생성
    public static T GenerateLoadAssetSync<T>(string path, string name = default) where T : Behaviour
    {
        var item = LoadAssetSync<GameObject>(path);
        var gameObj = Object.Instantiate(item);
        gameObj.name = name == default ? item.name : name;
        return gameObj.GetComponent<T>();
    }

    // Json 불러오기
    public static string LoadJson(string path)
    {
        return LoadAssetSync<TextAsset>(path).text;
    }

    public static Sprite GetSpriteWithAtlas(string path, string name)
    {
        var spriteAtlas = LoadAssetSync<SpriteAtlas>(path);
        return spriteAtlas.GetSprite(name);
    }
}
