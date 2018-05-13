using UnityEngine;
using System.Collections;

namespace Simulation.Procedural
{
    class MazeWall: MonoBehaviour
    {
        [SerializeField]
        float riseSpeed = 2f;

        public void Rise()
        {
            StartCoroutine(StartRise());
        }

        IEnumerator StartRise()
        {
            Vector3 targetPos = transform.position;

            transform.position += Vector3.down * transform.localScale.y;

            while (true) {
                if (transform.position.y >= targetPos.y)
                    yield break;
                
                transform.position = Vector3.MoveTowards(transform.position, targetPos, riseSpeed * Time.fixedDeltaTime);

                yield return new WaitForFixedUpdate();
            }
        }
    }
}