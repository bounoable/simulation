using UnityEngine;

namespace Simulation.AI
{
    interface ICharacter
    {
        void MoveTo(Vector3 target);
        void StopMoving();
    }
}