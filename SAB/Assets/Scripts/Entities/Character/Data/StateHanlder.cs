namespace SAB.Unit
{
    public class StateHanlder
    {
        private StateContext _stateData;

        public StateContext StateData
        {
            get => _stateData;
        }

        public StateHanlder()
        {
            _stateData = new();
        }
    }
}
