using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour, IMovementReceiver
{
    [SerializeField]
    private int _objectID;

    [SerializeField]
    private NavMeshAgent _nav;

    public int ObjectID 
    { 
        get => _objectID;
    }

    public int ReceiverID
    {
        get => _objectID;
    }

    public void Init(int objID)
    {
        _objectID = objID ;
    }

    public void SetDestination(Vector3 destination)
    {
        _nav.SetDestination(destination);
    }

    public void Move(Vector3 dir)
    {
        _nav.Move(dir);
    }
}
