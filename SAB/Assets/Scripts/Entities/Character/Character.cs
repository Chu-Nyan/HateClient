using Chu.Collision;
using SAB.Unit.Combat;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour, IMovementReceiver, ICombatReceiver
{
    [SerializeField]
    private int _objectID;
    [SerializeField]
    private Transform _attackOrigin;

    [SerializeField]
    private NavMeshAgent _nav;
    private PatrolData _patrolData;
    private IdleData _idleData;
    private CharacterBody _body;
    private CombatSystem _combatSystem;

    public int ObjectID
    {
        get => _objectID;
    }

    public int ReceiverID
    {
        get => _objectID;
    }

    public PatrolData PatrolData
    {
        get=> _patrolData;
    }

    public IdleData IdleData
    {
        get => _idleData;
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
        _combatSystem.Update();
        if (IsMoving == true)
        {
            _body.Collider.RefreshTransform();
        }
    }

    public void Init(int objID, PatrolData patrolData, IdleData idleData)
    {
        _body = new(transform, new CircleShape(1));
        _combatSystem = new CombatSystem();
        _combatSystem.AddSkill(new());
        _body.Init(new CircleShape(1));
        _objectID = objID;
        _patrolData = patrolData;
        _idleData = idleData;
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
}
