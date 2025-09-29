using Chu.Collision;
using Library.DesignPattern;

namespace Chu
{
    public class GeneratorHub : Singleton<GeneratorHub>
    {
        public readonly NyanColliderGenerator NyanColliderGenerator;

        public GeneratorHub()
        {
            NyanColliderGenerator = new();
        }

        public void InitNyanColliderGenerator(CollisionSystem system)
        {
            NyanColliderGenerator.Init(system);
        }
    }
}
