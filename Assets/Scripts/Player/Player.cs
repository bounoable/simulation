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

        public int Health
        {
            get { return health; }
            private set {
                value = value < 0 ? 0 : value;
                value = value > MaxHealth ? MaxHealth : value;
                
                health = value;
            }
        }

        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

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

        [SerializeField]
        int health;

        [SerializeField]
        int maxHealth;

        bool grounded = false;

        Rigidbody rb;

        [SerializeField]
        float damageCooldownTimeout = 3f;
        float damageCooldown = 0f;
    
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

            damageCooldown -= Time.fixedDeltaTime;
        }

        void OnCollisionEnter(Collision collision)
        {
            CheckCollisionDamage(collision);
        }

        void OnCollisionStay(Collision collision)
        {
            if (UnityEngine.Physics.Raycast(transform.position, -transform.up, 1f, 1 << 9))
                grounded = true;
            
            CheckCollisionDamage(collision);
        }

        void CheckCollisionDamage(Collision collision)
        {
            if (damageCooldown > 0)
                return;
            
            damageCooldown = damageCooldownTimeout;

            Collider collider = collision.collider;
            var npc = collider.GetComponent<NPC>();

            if (!npc)
                return;
            
            Health -= npc.Damage;
        }
    
        float CalculateJumpVerticalSpeed()
        {
            return Mathf.Sqrt(2 * jumpHeight * gravity);
        }
    }
}