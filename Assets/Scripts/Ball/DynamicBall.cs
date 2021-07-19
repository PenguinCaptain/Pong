using UnityEngine;

namespace Ball
{
    public class DynamicBall : BallBase
    {
        protected override bool CheckPlayerCollision(Player player)
        {
            return player.Rigidbody.velocity.y != 0;
        }
    }
}