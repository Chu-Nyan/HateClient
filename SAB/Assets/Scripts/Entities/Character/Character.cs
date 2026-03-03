using Chu.AI;
using Chu.Art;
using Chu.Collision;
using SAB.Item;
using SAB.Unit;
using SAB.Unit.Combat;
using SAB.Unit.State;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour, IMovementReceiver, IOffenseReceiver, IDefendable
{
    [SerializeField]
    private Transform _attackOrigin;

    private int _instanceID;

    private CharacterStats _stats;
    private StateContext _stateContext;
    private FSM<CharacterState, StateContext> _state;

    private NavMeshAgent _nav;
    private MeshSlotHub _meshHub;
    private CharacterAnimator _animator;

    private CharacterBody _body;
    private OffenseSystem _combatSystem;
    private DefenseSystem _defenseSystem;

    private EquipmentSystem _equipmentSys;

    private event Action<Character> Deactivated;

    public int InstanceID
    {
        get => _instanceID;
    }

    public int ReceiverID
    {
        get => _instanceID;
    }

    public CharacterStats Stats
    {
        get => _stats;
    }

    public List<Skill> Skills
    {
        get => _combatSystem.SkillList;
    }

    public void Init(int instanceID)
    {
        _instanceID = instanceID;
        _combatSystem = new OffenseSystem(instanceID, _attackOrigin);
        _defenseSystem = new DefenseSystem();
        _stats = new CharacterStats();
        _stateContext = new StateContext();
        _body = new(_instanceID, transform, new CircleShape(new CircleRangeData(Vector2.zero, 1)));
        _state = GenerateStateHandler();
        _nav = GetComponent<NavMeshAgent>();
        _meshHub = GetComponent<MeshSlotHub>();
        _animator = new CharacterAnimator(GetComponent<Animator>());
        _equipmentSys = new();

        _body.RegisterOnSkillHit(Defend);
        _state.Setup(_stateContext);
        _animator.RegisterAnimationEvent(AniState.Attack, "AttackFinished", new AniEventData(), 1, a => _stateContext.IsAttacking = false);
        _animator.RegisterAnimationEvent(AniState.Attack, "BasicAttack", new AniEventData(), 0.5f, _combatSystem.AttackWithAnimator);
    }

    private void Update()
    {
        StateUpdate();
        if (_stateContext.IsMoveing == true)
            _body.RefreshTransform();

        _state.Tick(_stateContext);
        _combatSystem.Tick();
        _defenseSystem.Tick(_stats);
        _animator.Tick(_stateContext, 1); // 1 << 이동속도 퍼센트로 넣기
    }

    private void StateUpdate()
    {
        _stateContext.IsMoveing = _nav.velocity.sqrMagnitude > 0.1f;
    }

    private FSM<CharacterState, StateContext> GenerateStateHandler()
    {
        var stateHandler = new FSM<CharacterState, StateContext>(new StateResolver());
        stateHandler.AddStates(new MovementState());
        stateHandler.AddStates(new AttackState());
        return stateHandler;
    }

    public void SetupStats(BaseStats baseStats)
    {
        _stats.SetBaseData(baseStats);
        var skill = SkillGenerator.Instance.GetSkill(SkillID.BasicMelee);
        _combatSystem.AddSkill(skill);
    }

    public void SetDestination(Vector3 destination)
    {
        if (_stateContext.IsAttacking == true)
            return;

        _nav.SetDestination(destination);
    }

    public void StopMovement()
    {
        _nav.ResetPath();
    }

    public void SetCustomizing(CustomizingPart part, int number)
    {
        string path = HumanoidCustomizingConst.GetPath(part, number);
        Mesh mesh = AssetManager.LoadAssetSync<Mesh>(path);
        _meshHub.SetMesh(part.ToString(), mesh);
    }

    public void Attack(int skillIndex, Vector3 targetPoint)
    {
        if (_stateContext.IsAttacking == true)
            return;
        if (_combatSystem.IsUsed(skillIndex) == true)
            return;

        _stateContext.IsAttacking = true;
        StopMovement();
        transform.rotation = Quaternion.LookRotation(targetPoint - transform.position);
        _animator.SetAttack();
        float dmg = _stats.GetDamage();
        AniEventData data = _combatSystem.TriggerAttackAndGetAniEventData(dmg, skillIndex, targetPoint);
        _animator.ChangeEventData(AniState.Attack, "BasicAttack", data);
    }

    public void Defend(AttackContext context)
    {
        _defenseSystem.Attack(context);

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

        gameObject.SetActive(value);
    }

    public void RegisterDeactivated(Action<Character> callback)
    {
        Deactivated += callback;
    }

    public void Equip(IHasEquipmentData equipment)
    {
        _equipmentSys.Equip(equipment);

        EquipmentTemplateData template = equipment.EquipmentTemplateData.Template;
        Mesh mesh = AssetManager.LoadAssetSync<Mesh>(template.MeshPath);
        _meshHub.SetMesh(template.Slot.ToString(), mesh);

        WeaponStance stance = _equipmentSys.Stance;
        _animator.ChangeStance(stance);
        float BasicAttackTimeing = 0.5f;// 무기 데이터에서 추출할 것

    }
}
