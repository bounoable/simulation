using UnityEngine;
using Simulation.AI;
using System.Collections.Generic;

namespace Simulation.Player
{
    [RequireComponent(typeof(Rigidbody))]
    class Player: Character, INPCTarget
    {
        const int groundMask = 1 << 9;

        public Transform Transform => transform;
        public Transform Head => head;

        [SerializeField]
        Transform head;

        [SerializeField]
        float rotationSpeed;

        Rigidbody rb;

        bool isJumping = false;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.W)) {
                Move(Vector3.forward);
            }

            if (Input.GetKey(KeyCode.S)) {
                Move(Vector3.back);
            }

            if (Input.GetKey(KeyCode.A)) {
                Rotate(Quaternion.Euler(Vector3.down * rotationSpeed));
            }

            if (Input.GetKey(KeyCode.D)) {
                Rotate(Quaternion.Euler(Vector3.up * rotationSpeed));
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }

            rb.AddRelativeForce(Vector3.down * 1000);
        }

        void Move(Vector3 direction)
        {
            transform.Translate(direction * moveSpeed * Time.fixedDeltaTime);
        }

        void Rotate(Quaternion rotation)
        {
            transform.rotation *= rotation;
        }

        void Jump()
        {
            if (isJumping)
                return;

            isJumping = true;
            rb.AddRelativeForce(Vector3.up * 40000);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (isJumping) {
                Collider collider = collision.collider;


                if (((1 << collider.gameObject.layer) & groundMask) == groundMask) {
                    isJumping = false;
                }
            }
        }
    }
}