namespace ChampagneSupernova.Library.BehaviorTree
{
    public interface IBTNode<T> where T : class
    {
        public MethodResult Evaluate(T item);
    }
}
