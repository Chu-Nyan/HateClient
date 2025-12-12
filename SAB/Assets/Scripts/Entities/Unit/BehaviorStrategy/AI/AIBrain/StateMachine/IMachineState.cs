namespace SAB.AI.Brain
{
    /// <summary>
    /// State 일괄 관리용
    /// </summary>
    /// <typeparam name="T">제공 받을 데이터</typeparam>
    public interface IMachineState<T>
    {
        void Enter(T data);
        void Update(T data);
        void Exit(T data);
        bool IsDone { get; }
    }
}
