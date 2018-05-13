using UnityEngine;
using Simulation.AI;
using System.Collections.Generic;

namespace Simulation.Player
{
    [RequireComponent(typeof(Rigidbody))]
    class Player: Character, INPCTarget
    {
        public Transform Transform => transform;
        public Transform Head => head;

        [SerializeField]
        Transform head;

        [SerializeField]
        float rotationSpeed = 2f;

        [SerializeField]
        float gravity = 10.0f;

        [SerializeField]
        float maxVelocityChange = 10.0f;

        [SerializeField]
        float jumpHeight = 1.0f;

        bool grounded = false;

        Rigidbody rb;
    
        void Awake()
        {
            rb = GetComponent<Rigidbody>();

            rb.freezeRotation = true;
            rb.useGravity = false;
        }
    
        void FixedUpdate()
        {
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

            if (grounded) {
                Vector3 targetVelocity = new Vector3(0, 0, Input.GetAxis("Vertical"));
                targetVelocity = transform.TransformDirection(targetVelocity);
                targetVelocity *= moveSpeed;
    
                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;
                rb.AddForce(velocityChange, ForceMode.VelocityChange);
    
                if (Input.GetButton("Jump")) {
                    rb.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
                }
            }
    
            rb.AddForce(new Vector3 (0, -gravity * rb.mass, 0));
    
            grounded = false;
        }
    
        void OnCollisionStay()
        {
            if (UnityEngine.Physics.Raycast(transform.position, -transform.up, 1f, 1 << 9))
                grounded = true;
        }
    
        float CalculateJumpVerticalSpeed()
        {
            return Mathf.Sqrt(2 * jumpHeight * gravity);
        }
    }
}