using UnityEngine;

namespace Ball
{
    public class StaticBall : BallBase
    {
        protected override bool CheckPlayerCollision(Player player)
        {
            return player.Rigidbody.velocity.y == 0;
        }
    }
}