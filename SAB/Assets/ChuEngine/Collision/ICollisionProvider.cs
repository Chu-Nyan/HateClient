namespace Chu.Collision
{
    public interface ICollisionProvider
    {
        public NyanCollider Collider { get; set; }
        public void OnNyanCollisionEnter(ICollisionProvider collider);
        public void OnNyanCollisionExit(ICollisionProvider collider);
    }
}