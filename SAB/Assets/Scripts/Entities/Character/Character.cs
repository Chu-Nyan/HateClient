using Chu.AI;
using Chu.Collision;
using SAB.MeshSlot;
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
        _combatSystem = new OffenseSystem(instanceID);
        _defenseSystem = new DefenseSystem();
        _stats = new CharacterStats();
        _stateContext = new StateContext();
        _body = new(_instanceID, transform, new CircleShape(1));
        _state = GenerateStateHandler();
        _nav = GetComponent<NavMeshAgent>();
        _meshHub = GetComponent<MeshSlotHub>();
        _animator = new CharacterAnimator(GetComponent<Animator>());

        _body.RegisterOnSkillHit(Defend);
        _state.Setup(_stateContext);
    }

    private void Update()
    {
        StateUpdate();
        if (_stateContext.IsMoveing == true)
            _body.RefreshTransform();
        _state.Tick(_stateContext);
        _combatSystem.Tick();
        _defenseSystem.Tick(_stats);
        _animator.Tick(_stateContext, _stats[StatType.SPD]);
    }

    private void StateUpdate()
    {
        _stateContext.IsMoveing = _nav.velocity.sqrMagnitude > 0.1f;
    }

    private FSM<CharacterState, StateContext> GenerateStateHandler()
    {
        var resolver = new StateResolver();
        var stateHandler = new FSM<CharacterState, StateContext>(resolver);
        stateHandler.AddStates(new IIdleState());
        stateHandler.AddStates(new IMovementState());
        stateHandler.AddStates(new IAttackState());
        return stateHandler;
    }

    public void SetupStats(BaseStats baseStats)
    {
        _stats.SetBaseData(baseStats);
        var skill = SkillGenerator.Instance.GetSkill(SkillID.BasicMelee);
        _combatSystem.AddSkill(skill);
        _body.Init(new CircleShape(1));
    }

    public void SetDestination(Vector3 destination)
    {
        _nav.SetDestination(destination);
    }

    public void SetMesh(SlotType type, Mesh mesh)
    {
        _meshHub.SetMesh(type, mesh);
    }

    public void Attack(int skillIndex, Vector3 targetPoint)
    {
        float dmg = _stats.GetDamage();
        if (_combatSystem.Attack(dmg, skillIndex, _attackOrigin.position, targetPoint) == true)
            _animator.SetAttack();
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
}
