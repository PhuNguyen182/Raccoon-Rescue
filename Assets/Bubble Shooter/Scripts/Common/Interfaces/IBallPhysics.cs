using UnityEngine;

namespace BubbleShooter.Scripts.Common.Interfaces 
{
    public interface IBallPhysics
    {
        public void ChangeLayerMask(bool isFixed);
        public void SetBodyActive(bool active);
        public void AddForce(Vector2 force, ForceMode2D forceMode = ForceMode2D.Impulse);
    }
}
