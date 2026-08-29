using Chu.AI;
using Chu.Core;
using Chu.Utility;
using SAB.Cutscene;
using SAB.EntityAgent;
using SAB.GameSystem;
using SAB.Item;
using SAB.Skill;
using SAB.UI;
using SAB.Unit;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour, IMovementReceiver, IOffenseReceiver, IDefendable, ISpeachable, IEntity
{
    private int _instanceID;
    [SerializeField]
    private BrainType _brainType;
    [SerializeField]
    private CharacterStats _stats;
    private StateContext _stateContext;
    private FSM<CharacterState, StateContext> _stateMachine;

    [SerializeField]
    private NavMeshAgent _nav;
    [SerializeField]
    private MeshSlotHub _meshHub;
    private CharacterAnimator _animator;

    private OffenseSystem _combatSys;
    private DefenseSystem _defenseSys;
    private EquipmentSystem _equipmentSys;

    [SerializeField]
    private Transform _attackOrigin;
    [SerializeField]
    private Transform _speechAnchor;

    private event Action<Character> Deactivated;

    public int InstanceID
    {
        get => _instanceID;
    }

    public int ReceiverID
    {
        get => _instanceID;
    }

    public BrainType BrainType
    {
        get => _brainType;
    }

    public CharacterStats Stats
    {
        get => _stats;
    }

    public List<SkillKernel> Skills
    {
        get => _combatSys.SkillList;
    }

    public FactionType FactionType
    {
        get => _stats.BaseStats.Faction;
    }

    private void Awake()
    {
        _instanceID = IDGenerator.Next();

        _stateContext = new StateContext();
        _defenseSys = new DefenseSystem(_instanceID, transform.position.ToVector2XZ(), transform.eulerAngles.y, this);
        _combatSys = new OffenseSystem(_instanceID, _attackOrigin);
        _equipmentSys = new();
        _animator = new CharacterAnimator(GetComponent<Animator>());
        _stateMachine = GenerateStateMachine();

        _stateMachine.Setup(_stateContext);
        _animator.RegisterAnimationEvent(AniState.Attack, "AttackFinished", new AniEventData(), 1, a => _stateContext.IsAttacking = false);
        _animator.RegisterAnimationEvent(AniState.Attack, "BasicAttack", new AniEventData(), 0.5f, _combatSys.AttackWithAnimator);
    }

    private FSM<CharacterState, StateContext> GenerateStateMachine()
    {
        var stateMachine = new FSM<CharacterState, StateContext>(new StateResolver());
        stateMachine.AddStates(new MovementState());
        stateMachine.AddStates(new AttackState());
        return stateMachine;
    }

    public void SetupStats(BaseStats baseStats, int[] skills)
    {
        _stats.SetBaseData(baseStats);
        _combatSys.SetFaction(baseStats.Faction);
        for (int i = 0; i < skills.Length; i++)
        {
            _combatSys.AddSkill(SkillFactory.Instance.CreateKernel(skills[i]));
        }
    }

    public void SetupNavMesh(Vector3 pos)
    {
        if (NavMesh.SamplePosition(pos, out var hit, 2f, NavMesh.AllAreas))
            transform.position = hit.position;

        _nav.enabled = true;
    }

    private void Update()
    {
        StateUpdate();
        if (_stateContext.IsMoveing == true)
        {
            var pos = transform.position.ToVector2XZ();
            var eulerY = transform.eulerAngles.y;
            _defenseSys.OnPositionChanged(pos, eulerY);
        }

        _stateMachine.Tick(_stateContext);
        _combatSys.Tick();
        _defenseSys.Tick(_stats);
        _animator.Tick(_stateContext, 1); // 1 << 이동속도 퍼센트로 넣기

        void StateUpdate()
        {
            _stateContext.IsMoveing = _nav.velocity.sqrMagnitude > 0.1f;
        }
    }

    public void SetDestination(Vector3 destination)
    {
        if (_stateContext.IsAttacking == true)
            return;

        _nav.SetDestination(destination);
    }

    public void SetPositionWithNavMash(Vector3 position)
    {
        if (NavMesh.SamplePosition(position, out var hit, 2f, NavMesh.AllAreas))
        {
            _nav.Warp(hit.position);
        }
    }

    public void StopMovement()
    {
        _nav.ResetPath();
    }

    public void SetCustomizing(CustomizingData data)
    {
        SetCustomizing(CustomizingPart.Eye, data.Eye);
        SetCustomizing(CustomizingPart.Eyebrow, data.Eyebrow);
        SetCustomizing(CustomizingPart.Hair, data.Hair);
        SetCustomizing(CustomizingPart.Mouth, data.Mouth);
    }

    public void SetCustomizing(CustomizingPart part, int number)
    {
        Mesh mesh = HumanoidCustomizingUtility.GetMesh(part, number);
        _meshHub.SetMesh(part.ToString(), mesh);
    }

    public void Attack(int skillIndex, Vector3 targetPoint)
    {
        if (_stateContext.IsAttacking == true)
            return;
        if (_combatSys.IsUsed(skillIndex) == true)
            return;

        if (_combatSys.TriggerAttackAndGetAniEventData(_stats.GetDamage(), skillIndex, targetPoint, out var data) == false)
            return;

        _stateContext.IsAttacking = true;
        StopMovement();
        transform.rotation = Quaternion.LookRotation(targetPoint - transform.position);
        _animator.SetAttack();
        _animator.ChangeEventData(AniState.Attack, "BasicAttack", data);
    }

    public void Defend(AttackContext attack, HitResult hit)
    {
        _defenseSys.Attack(attack, hit);

        if (_stats.IsDead == true)
            Die();
    }

    public void SetCombatMode(bool value)
    {
        _stateContext.IsCombatMode = value;
        _animator.SetCombatMode(value);
    }

    public void Die()
    {
        //TODO : 죽음 처리 로직이 끝난 후 SetActive로 마무리
        SetActive(false);
    }

    public void SetActive(bool value)
    {
        if (gameObject.activeSelf == value)
            return;

        if (value == false)
        {
            Deactivated?.Invoke(this);
            Deactivated = null;
        }

        _nav.enabled = false;
        gameObject.SetActive(value);
    }

    public void RegisterDeactivated(Action<Character> callback)
    {
        Deactivated += callback;
    }

    public void Equip(IHasEquipmentData equipment)
    {
        _equipmentSys.Equip(equipment);

        EquipmentBaseData template = equipment.EquipmentTemplateData.Template;
        Mesh mesh = AssetManager.LoadAssetSync<Mesh>(template.MeshPath);
        _meshHub.SetMesh(template.Slot.ToString(), mesh);

        WeaponStance stance = _equipmentSys.Stance;
        _animator.ChangeStance(stance);
        //float BasicAttackTimeing = 0.5f;// 무기 데이터에서 추출할 것

    }

    public void OnOwnerChanged(BrainType type)
    {
        _brainType = type;
        _defenseSys.OnOwnerChanged(type == BrainType.Player);
    }

    public void PlayEmote(AnimationClip clip)
    {
        _animator.SetPlayEmote(clip);
    }

    public void Speech(DialogueData data, bool isOverwrite)
    {
        var ui = UIManager.Instance.GetUI<SpeechBubbleUI>();
        if (ui == null)
            Debug.Log($"Name : {gameObject.name} Sequence ID : {data.SequenceID}, Text ID : {data.TextID}");
        else
            ui.ShowDialogue(_speechAnchor, data, isOverwrite);
    }
}
