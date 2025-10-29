namespace Chu.Collision
{
    /// <summary>
    /// NyanCollider를 제공하고 충돌 시점에 따라 함수를 실행하는 인터페이스
    /// </summary>
    /// 상속받은 객체는 반드시 하나의 NyanCollider를 제공해야함
    public interface INyanCollisionProvider
    {
        public NyanCollider Collider { get; set; }
        public void OnNyanCollisionEnter(INyanCollisionProvider collider);
        public void OnNyanCollisionExit(INyanCollisionProvider collider);
    }
}