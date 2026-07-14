using Chu.Collision;
using Chu.Utility;

namespace Chu.Core
{
    public class GeneratorHub : Singleton<GeneratorHub>
    {
        public readonly NyanColliderGenerator NyanColliderGenerator;

        public GeneratorHub()
        {
            NyanColliderGenerator = new();
        }

        public void InitNyanColliderGenerator(NyanCollisonSystem system)
        {
            NyanColliderGenerator.Init(system);
        }
    }
}
