using UnityEngine;

namespace Ball
{
    public class NormalBall : BallBase
    {
        protected override bool CheckPlayerCollision(Player player)
        {
            return true;
        }
    }
}