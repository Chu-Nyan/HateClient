using Chu.Collision;
using Chu.Utility;

namespace Chu.Core
{
    public class GeneratorHub : Singleton<GeneratorHub>
    {
        private readonly NyanColliderFactory _nyanColliderGenerator;

        public GeneratorHub()
        {
            _nyanColliderGenerator = new();
        }

        public void InitNyanColliderGenerator(NyanCollisonSystem system)
        {
            _nyanColliderGenerator.Init(system);
        }
    }
}
