using System;

namespace Player
{
    public class PlayerStateEnums
    {
        [Flags]
        public enum PlayerState : Int16
        {
            None = 0,
            Grounded = 1 << 0,
            Airborne = 1 << 1,
            Swimming = 1 << 2,
            Attacking = 1 << 3,
            WallClimb = 1 << 4,
            Flying = 1 << 5,
            Burning = 1 << 6,
            Frozen = 1 << 7,
            Rolling = 1 << 8,
            Invisible = 1 << 9,
            Drunk = 1 << 10,
            Zombie = 1 << 11,
            Ufo = 1 << 12,
            NoGravity = 1 << 13,
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
