using Chu.AI;
using Chu.Collision;
using SAB.EntityAgent.AI;
using SAB.MeshSlot;
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
    private FSM<CharacterState, CharacterStats> _state;
    private CharacterBody _body;
    private OffenseSystem _combatSystem;
    private DefenseSystem _defenseSystem;
    private NavMeshAgent _nav;
    private MeshSlotHub _meshHub;

    private event Action<Character> _deactivated;

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

    public IMovementAIDataView MovementAIData
    {
        get => _stats;
    }

    public bool IsMoving
    {
        get => _nav.hasPath;
    }

    public List<Skill> Skills
    {
        get => _combatSystem.SkillList;
    }

    private void Update()
    {
        _state.Tick(_stats);
        _combatSystem.Tick();
        _defenseSystem.Tick(_stats);
        if (IsMoving == true)
        {
            _body.Collider.RefreshTransform();
        }
    }

    public void Init(int instanceID)
    {
        _instanceID = instanceID;
        _combatSystem = new OffenseSystem(instanceID);
        _defenseSystem = new DefenseSystem();
        _stats = new CharacterStats();
        _body = new(_instanceID, transform, new CircleShape(1));
        _state = GenerateStateHandler();
        _nav = GetComponent<NavMeshAgent>();
        _meshHub = GetComponent<MeshSlotHub>();
        _body.RegisterOnSkillHit(Defend);
        _state.Setup(_stats);
    private FSM<CharacterState, CharacterStats> GenerateStateHandler()
    {
        var resolver = new StateResolver();
        var stateHandler = new FSM<CharacterState, CharacterStats>(resolver);
        stateHandler.AddStates(new IIdleState());
        stateHandler.AddStates(new IMovementState());
        stateHandler.AddStates(new IAttackState());
        return stateHandler;
    }

    public void SetupStats(CharacterBaseStats baseStats, IdleData idle, PatrolData patrol)
    {
        _stats.SetBaseData(baseStats, idle, patrol);
        var skill = SkillGenerator.Instance.GetSkill(SkillID.BasicMelee);
        _combatSystem.AddSkill(skill);
        _body.Init(new CircleShape(1));
    }

    public void SetCurrentStats(float hp)
    {
        _stats.SetCurrentStats(hp);
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
        // TODO : 연출 + 실제 충돌 처리
        _combatSystem.Attack(_stats.Damage, skillIndex, _attackOrigin.position, targetPoint);
    }

    public void Defend(AttackContext context)
    {
        _defenseSystem.Attack(context);

        if (_stats.IsDead == true)
            Die();
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
            _deactivated?.Invoke(this);
            _deactivated = null;
        }

        gameObject.SetActive(value);
    }

    public void RegisterDeactivated(Action<Character> callback)
    {
        _deactivated += callback;
    }
}
