using SAB.Unit.Combat;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour, IMovementReceiver, ICombatReceiver
{
    [SerializeField]
    private int _objectID;

    [SerializeField]
    private NavMeshAgent _nav;
    private PatrolData _patrolData;
    private IdleData _idleData;
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
    }

    public void Init(int objID, PatrolData patrolData, IdleData idleData)
    {
        _combatSystem = new CombatSystem();
        _combatSystem.Add(new());
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
    }

    public void Attack(int skillIndex, Vector3 targetPoint)
    {
        _combatSystem.Attack(skillIndex, targetPoint);
        Debug.DrawRay(transform.position, targetPoint);
    }
}
