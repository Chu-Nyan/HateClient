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

    public void Init(int objID, PatrolData patrolData, IdleData idleData)
    {
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

    public void Attack(Vector3 dir)
    {
        Debug.Log("공격");
    }
}
