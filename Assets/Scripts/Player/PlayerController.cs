using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.PlayerInput;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        private Vector2 m_velocity = new();

        private Vector2 _move;
        private Vector2 _look;
        private bool _jump;
        private bool _crouch;

        private float _runSpeed = 2f;
        private float _walkSpeed = 1f;
        private float _jumpSpeed = 1f;
        private float _crouchSpeed = 0.5f;

        public float maxFallSpeed = 5f;
        private Vector2 _gravity;

        public float jumpUpwardVelocity = 10f;
        public float maxJumpHeight = 2f;
        private float _currentJumpHeight;

        public int maxDoubleJumps = 1;
        private int _currentDoubleJumps = 0;
        public LayerMask walkableLayers;

        Rigidbody2D rb2D;

        [Flags]
        public enum InputFilter
        {
            None = 0,
            Jump = 1 << 0,
            Crouch = 1 << 1,
        }
        private InputFilter inputFilter = InputFilter.None;

        [CustomEditor(typeof(PlayerStateEnumFlagsField))]
        public PlayerStateEnums.PlayerState playerState = PlayerStateEnums.PlayerState.Grounded;
        public PlayerStateEnums.PlayerAirState playerAirState = PlayerStateEnums.PlayerAirState.None;
        public PlayerStateEnums.PlayerGroundState playerGroundState = PlayerStateEnums.PlayerGroundState.Idle;
        public PlayerStateEnums.PlayerWaterState playerWaterState = PlayerStateEnums.PlayerWaterState.None;
        public PlayerStateEnums.PlayerAirState PlayerAirState
        {
            get => playerAirState; set => SetPlayerAirState(value);
        }
     

        private void SetPlayerAirState(PlayerStateEnums.PlayerAirState value)
        {
            switch (value)
            {
                case PlayerStateEnums.PlayerAirState.None:
                    SetPlayerState(PlayerStateEnums.PlayerState.Airborne, false);
                    m_velocity.y = 0f;
                    break;
                default:
                    SetPlayerState(PlayerStateEnums.PlayerState.Airborne, true);
                    break;
            }
            playerAirState = value;
        }

        public PlayerStateEnums.PlayerGroundState PlayerGroundState { get => playerGroundState; set => SetPlayerGroundState(value); }

        private void SetPlayerGroundState(PlayerStateEnums.PlayerGroundState value)
        {
            switch (value)
            {
                case PlayerStateEnums.PlayerGroundState.None:
                    SetPlayerState(PlayerStateEnums.PlayerState.Grounded, false);
                    break;
                default:
                    SetPlayerState(PlayerStateEnums.PlayerState.Grounded, true);
                    break;
            }
            playerGroundState = value;
        }

        public PlayerStateEnums.PlayerWaterState PlayerWaterState { get => playerWaterState; set => SetPlayerWaterState(value); }

        public bool GetPlayerState(PlayerStateEnums.PlayerState state)
        {
            PlayerStateEnums.PlayerState mask = playerState & state;
            return mask == state;
        }

        public void SetPlayerState(PlayerStateEnums.PlayerState state, bool enabled)
        {
            if (enabled)
            {
                playerState |= state;
            }
            else
            {
                playerState &= ~state;
            }
        }

        private void SetPlayerWaterState(PlayerStateEnums.PlayerWaterState value)
        {
            switch (value)
            {
                case PlayerStateEnums.PlayerWaterState.None:
                    SetPlayerState(PlayerStateEnums.PlayerState.Swimming, false);
                    break;
                default:
                    SetPlayerState(PlayerStateEnums.PlayerState.Swimming, true);
                    break;
            }
            playerWaterState = value;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            rb2D = GetComponent<Rigidbody2D>();
            m_velocity = new Vector3();
            _gravity = new Vector2(0f,-rb2D.gravityScale);
        }

        // Update is called once per frame
        void Update()
        {
            if (GetPlayerState(PlayerStateEnums.PlayerState.Airborne))
            {
                ProcessAirState();
            }
            if (GetPlayerState(PlayerStateEnums.PlayerState.Grounded))
            {
                ProcessGroundState();
            }
            if (GetPlayerState(PlayerStateEnums.PlayerState.Swimming))
            {
                ProcessWaterState();
            }
        }

        private void FixedUpdate()
        {
            rb2D.MovePosition(rb2D.position + m_velocity * Time.fixedDeltaTime);
        }

        private void ProcessWaterState()
        {
            switch (playerWaterState)
            {
                default:
                    break;
            }
        }

        private void ProcessGroundState()
        {
            switch (playerGroundState)
            {
                case PlayerStateEnums.PlayerGroundState.None:
                    rb2D.bodyType = RigidbodyType2D.Dynamic;
                    break;
                case PlayerStateEnums.PlayerGroundState.Idle:
                    Move(_runSpeed);
                    rb2D.bodyType = RigidbodyType2D.Kinematic;
                    m_velocity = new Vector2();
                    break;
                case PlayerStateEnums.PlayerGroundState.Walking:
                    Move(_walkSpeed);
                    break;
                case PlayerStateEnums.PlayerGroundState.Running:
                    Move(_runSpeed);
                    break;
                case PlayerStateEnums.PlayerGroundState.Crouch:
                    Move(_crouchSpeed);
                    break;
            }
        }

        private void ProcessAirState()
        {
            switch (playerAirState)
            {
                case PlayerStateEnums.PlayerAirState.None:
                    break;
                case PlayerStateEnums.PlayerAirState.Jumping:
                    ProcessJump();
                    Move(_jumpSpeed);
                    break;
                case PlayerStateEnums.PlayerAirState.DoubleJumping:
                    ProcessJump();
                    Move(_jumpSpeed);
                    break;
                case PlayerStateEnums.PlayerAirState.Falling:
                    ProcessFall();
                    break;
                default:
                    ApplyGravity();
                    break;
            }
        }

        private void ProcessFall()
        {
            ApplyGravity();
            Move(_jumpSpeed);
        }

        private void ProcessJump()
        {
            if (_currentJumpHeight <= maxJumpHeight)
            {
                _currentJumpHeight += jumpUpwardVelocity * Time.deltaTime;
                m_velocity = new Vector2(m_velocity.x, jumpUpwardVelocity);
            }
            else
            {
                EndJump();
            }
        }

        private void ApplyGravity()
        {
            m_velocity += _gravity * Time.deltaTime;
        }

        private void Move(float speed)
        {
            m_velocity = new Vector2(_move.x * speed, m_velocity.y);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (GetPlayerState(PlayerStateEnums.PlayerState.Grounded) && context.performed)
            {
                StartJump();
            }
            else if (PlayerAirState == PlayerStateEnums.PlayerAirState.Falling && context.performed)
            {
                DoubleJump();
            }
            else if (PlayerAirState == PlayerStateEnums.PlayerAirState.Jumping && context.canceled)
            {
                EndJump();
            }
        }

        private void EndJump()
        {
            SetPlayerAirState(PlayerStateEnums.PlayerAirState.Falling);
        }

        private void DoubleJump()
        {
            if (_currentDoubleJumps < maxDoubleJumps)
            {
            _currentJumpHeight = 0;
            _currentDoubleJumps++;
            SetPlayerAirState(PlayerStateEnums.PlayerAirState.DoubleJumping);
            }
        }

        private void StartJump()
        {
            _currentJumpHeight = 0;
            _currentDoubleJumps = 0;
            SetPlayerAirState(PlayerStateEnums.PlayerAirState.Jumping);
            SetPlayerGroundState(PlayerStateEnums.PlayerGroundState.None);
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.performed && InputFilter.Crouch != (inputFilter & InputFilter.Crouch))
            {
                SetPlayerGroundState(PlayerStateEnums.PlayerGroundState.Crouch);
                transform.localScale = new Vector3(1f, 0.5f, 1f);
            }
            else
            {
                SetPlayerGroundState(PlayerStateEnums.PlayerGroundState.Idle);
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {

        }

        public void OnLook(InputAction.CallbackContext context)
        {

        }

        public void OnInteract(InputAction.CallbackContext context)
        {

        }

        public void OnCollisionEnter2D(Collision2D other)
        {
            //Debug.Log("Collision detected!");
            //SetPlayerAirState(PlayerStateEnums.PlayerAirState.None);
            //SetPlayerGroundState(PlayerStateEnums.PlayerGroundState.Idle);
            //m_velocity.y = 0f;
        }

        public void OnHeadCollide(Collider2D other)
        {
            Debug.Log("Head collided");
            switch (PlayerAirState)
            {
                case PlayerStateEnums.PlayerAirState.None:
                    break;
                case PlayerStateEnums.PlayerAirState.Jumping:
                    SetPlayerAirState(PlayerStateEnums.PlayerAirState.Falling);
                    break;
                case PlayerStateEnums.PlayerAirState.Hop:
                    SetPlayerAirState(PlayerStateEnums.PlayerAirState.Falling);
                    break;
                case PlayerStateEnums.PlayerAirState.DoubleJumping:
                    SetPlayerAirState(PlayerStateEnums.PlayerAirState.Falling);
                    break;
                    default:
                    break;
            }
        }

        public void OnFeetCollide(Collider2D other)
        {
            Debug.Log("Feet collided");
            switch (PlayerGroundState)
            {
                case PlayerStateEnums.PlayerGroundState.None:
                    SetPlayerGroundState(PlayerStateEnums.PlayerGroundState.Idle);
                    SetPlayerAirState(PlayerStateEnums.PlayerAirState.None);
                    break;
                case PlayerStateEnums.PlayerGroundState.Idle:
                    break;
                case PlayerStateEnums.PlayerGroundState.Walking:
                    break;
                case PlayerStateEnums.PlayerGroundState.Running:
                    break;
                case PlayerStateEnums.PlayerGroundState.Dashing:
                    break;
                case PlayerStateEnums.PlayerGroundState.LookingUp:
                    break;
                case PlayerStateEnums.PlayerGroundState.Crouch:
                    break;
                case PlayerStateEnums.PlayerGroundState.CrouchWalking:
                    break;
                default:
                    break;
            }
        }

        public void OnCollideLeft(Collider2D other)
        {

        }

        public void OnCollideRight(Collider2D other)
        {

        }
    }
}
