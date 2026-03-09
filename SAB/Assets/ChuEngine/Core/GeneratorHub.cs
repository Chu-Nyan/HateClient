using Chu.Collision;
using Chu.Utility;

namespace Chu
{
    public class GeneratorHub : Singleton<GeneratorHub>
    {
        public readonly NyanColliderGenerator NyanColliderGenerator;
        public readonly ShapeFactory ShapeFactory;

        public GeneratorHub()
        {
            NyanColliderGenerator = new();
            ShapeFactory = new();
        }

        public void InitNyanColliderGenerator(NyanCollisonSystem system)
        {
            NyanColliderGenerator.Init(system);
        }
    }
}
