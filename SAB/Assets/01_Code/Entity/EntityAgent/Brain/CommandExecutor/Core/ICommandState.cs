namespace SAB.EntityAgent.AI.StateMachine
{
    /// <summary>
    /// State 일괄 관리용
    /// </summary>
    /// <typeparam name="T">제공 받을 데이터</typeparam>
    public interface ICommandState<T>
    {
        void Enter(T data);
        void Update(T data);
        void Exit(T data);
        bool IsDone { get; }
    }
}
