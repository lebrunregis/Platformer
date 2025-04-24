using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Vector2 _gravity = new Vector2(0f, 9.81f);
        private Vector2 _move;
        private Vector2 _look;
        private bool _jump;
        private bool _crouch;
        private float _moveSpeed = 2f;

        [Flags]
        public enum InputFilter
        {
            None = 0,
            Jump = 1 << 0,
            Crouch = 1 << 1,
        }
        private InputFilter inputFilter = InputFilter.None;

        [CustomEditor(typeof(PlayerStateEnumFlagsField))]
        public PlayerStateEnums.PlayerState playerState;
        public PlayerStateEnums.PlayerAirState playerAirState;
        public PlayerStateEnums.PlayerGroundState playerGroundState;
        public PlayerStateEnums.PlayerWaterState playerWaterState;

        public PlayerStateEnums.PlayerAirState PlayerAirState { get => playerAirState; set => SetPlayerAirState(value); }

        private void SetPlayerAirState(PlayerStateEnums.PlayerAirState value)
        {
            switch (value)
            {
                case PlayerStateEnums.PlayerAirState.None:
                    SetPlayerState(PlayerStateEnums.PlayerState.Airborne, false);
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
            return mask == state ;
        }

        public void SetPlayerState(PlayerStateEnums.PlayerState state, bool enabled)
        {
            if (enabled) { 
                playerState ^= state;
            }
            else {
                playerState |= state;
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

        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        private void Move()
        {
            if (_move.x != 0)
            {
                transform.Translate(_move.x * _moveSpeed * Time.deltaTime, 0, 0);
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed && InputFilter.Jump != (inputFilter & InputFilter.Jump) && GetPlayerState(PlayerStateEnums.PlayerState.Grounded))
            {
                StartJump();

            }
        }

        private void StartJump()
        {
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

        public void OnCollisionEnter(Collision other)
        {

        }
    }
}
