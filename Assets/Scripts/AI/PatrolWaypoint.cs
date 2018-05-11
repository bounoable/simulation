using UnityEngine;

namespace Simulation.AI
{
    class PatrolWaypoint: MonoBehaviour
    {
        [SerializeField]
        Transform circle;

        Vector3 initialScale;

        void Awake()
        {
            initialScale = circle.localScale;
        }

        void Update()
        {
            circle.localScale = new Vector3(
                initialScale.x + (initialScale.x * Mathf.Abs(Mathf.Sin(Time.time)) * 0.5f),
                initialScale.y,
                initialScale.z + (initialScale.z * Mathf.Abs(Mathf.Sin(Time.time)) * 0.5f)
            );
        }
    }
}