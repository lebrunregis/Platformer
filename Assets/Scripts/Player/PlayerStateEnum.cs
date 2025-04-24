using System;

namespace Player
{
    public class PlayerStateEnums
    {
        [Flags]
        public enum PlayerState : byte
        {
            None = 0,
            Grounded = 1 << 0,
            Airborne = 1 << 1,
            Swimming = 1 << 2,
            Attacking = 1 << 3,
        }

        public enum PlayerAttackState : byte
        {
            None,
            Attacking,
            Shooting,
            Rolling,
            Charging,
        }

        public enum PlayerGroundState : byte
        {
            None,
            Idle,
            Walking,
            Running,
            Dashing,
            LookingUp,
            Crouch,
            CrouchWalking,
        }

        public enum PlayerWaterState : byte
        {
            None,
            Swimming,
            Rising,
            Treading,
            Diving,
        }

        public enum PlayerAirState : byte
        {
            None,
            Hop,
            Jumping,
            DoubleJumping,
            TripleJumping,
            QuadrupleJumping,
            QuintupleJumping,
            Falling,
            Flying,
            Floating,
            Dodging,
            Diving,
            FastFalling,
            GroundPounding,
            BLGing,
            LongJump,
            Hooked,
            Hover,
        }

        public enum PlayerDamageState : byte
        {
            None,
            Damaged,
        }

        public enum PlayerDeathState : byte
        {
            None,
            Alive,
            Dead,
        }
    }
}
