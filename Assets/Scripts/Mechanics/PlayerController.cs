using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using UnityEngine.PlayerLoop;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float speed = 7;
        
        public float jumpMax = 10;
        public float jumpPower = 0;
        public float jumpTime;

        public float jumpCoolMax;
        public float jumpCool = 0;

        public JumpState jumpState = JumpState.JumpReady;
        private bool stopJump;
        /*internal new*/ public CircleCollider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        public Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        public bool isJumpable = true;
        public float lastVelocityY;

        public LayerMask groundMask;
        
        public Rigidbody2D body { get; private set; }
        public bool IsGrounded { get; private set; }

        private float bouncePeak;
        
        /// <summary>
        /// Bounce the object's vertical velocity.
        /// </summary>
        /// <param name="value"></param>
        public void Bounce(float value)
        {
            Vector2 velocity = body.velocity;
            velocity.y = value;
            body.velocity = velocity; 
        }

        /// <summary>
        /// Bounce the objects velocity in a direction.
        /// </summary>
        /// <param name="dir"></param>
        public void Bounce(Vector2 dir) {
            body.velocity = dir;
        }

        /// <summary>
        /// Teleport to some position.
        /// </summary>
        /// <param name="position"></param>
        public void Teleport(Vector3 position)
        {
            body.position = position;
            body.velocity *= 0;
        }
        
        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<CircleCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            body = GetComponent<Rigidbody2D>();
        }

        protected void Update()
        {
            if (controlEnabled)
            {
                if (jumpState == JumpState.JumpReady || jumpState == JumpState.PrepareToJump)
                    move.x = Input.GetAxis("Horizontal")*speed;
                if (jumpState == JumpState.JumpReady && Input.GetButton("Jump")) {
                    jumpState = JumpState.PrepareToJump;
                    jumpTime = 0;
                }
                else if (jumpState == JumpState.PrepareToJump && Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    jumpState = JumpState.Jumping;
                    controlEnabled = false;
                }
            }
            UpdateJumpState();
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jump = true;
                    stopJump = false;
                    jumpTime += Time.deltaTime;
                    
                    if (jumpTime >= jumpMax) {
                        stopJump = true;
                        jumpState = JumpState.Jumping;
                        controlEnabled = false;
                    }
                    break;
                case JumpState.Jumping:
                    jumpCool = jumpCoolMax;
                    jumpState = JumpState.Bounce;
                    break;
                case JumpState.Bounce:
                    if (body.velocity.y > 0) {
                        bouncePeak = Mathf.Max(bouncePeak, body.velocity.y);
                    }
                    else bouncePeak = 0;
                    if (bouncePeak != 0 && bouncePeak < 3f) {
                        move = Vector2.zero;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    var velocity = body.velocity;
                    velocity = new Vector2(velocity.x * 0.995f, velocity.y);
                    body.velocity = velocity;
                    if (jumpCool > 0 && Mathf.Abs(body.velocity.x) < 0.5f) jumpCool -= Time.deltaTime;
                    else if (jumpCool <= 0) {
                        controlEnabled = true;
                        jumpState = JumpState.JumpReady;
                    }
                    break;
            }
        }

        private void FixedUpdate() {
            IsGrounded = false;
            // RaycastHit2D raycastHit = Physics2D.CircleCast(Bounds.center, collider2d.radius, Vector2.down, 0, groundMask);
            // IsGrounded = raycastHit;
            if (jumpState == JumpState.PrepareToJump) {
                //move.y = jumpPower;
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            }
            else
                move.y = body.velocity.y;

            if (jumpState == JumpState.JumpReady)
                body.velocity = move;
        }

        public enum JumpState
        {
            JumpReady,
            PrepareToJump,
            Jumping,
            Bounce,
            Landed
        }
    }
}