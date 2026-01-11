using Chu.Collision;
using SAB.Unit.Combat;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour, IMovementReceiver, ICombatReceiver, IDefendable
{
    [SerializeField]
    private int _instanceID;
    [SerializeField]
    private Transform _attackOrigin;

    [SerializeField]
    private NavMeshAgent _nav;
    private CharacterStats _stats;
    private CharacterBody _body;
    private CombatSystem _combatSystem;
    private DefenseSystem _defenseSystem;

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
        _combatSystem = new CombatSystem(instanceID);
        _defenseSystem = new DefenseSystem();
        _stats = new CharacterStats();
        _body = new(_instanceID, transform, new CircleShape(1));
        _body.RegisterOnSkillHit(Defend);
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

    public void Move(Vector3 dir)
    {
        _nav.Move(dir);
        _body.Collider.RefreshTransform();
    }

    public void Attack(int skillIndex, Vector3 targetPoint)
    {
        // TODO : 연출 + 실제 충돌 처리
        // 실제 충돌은 Combat 시스템에서 처리
        _combatSystem.Attack(skillIndex, _attackOrigin.position, targetPoint);
    }

    public void Defend(AttackContext context)
    {
        _defenseSystem.Attack(context);

        if (_stats.IsDead == true)
            Die();
    }

    public void Die()
    {
        SetActive(false);
    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

}
