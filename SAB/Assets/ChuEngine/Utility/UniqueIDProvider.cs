namespace Chu.Utility
{
    public static class UniqueIDProvider
    {
        private static int _lastID = 0;

        public static int Count
        {
            get => _lastID;
        }

        public static int Next()
        {
            _lastID++;
            return _lastID;
        }
    }
}
