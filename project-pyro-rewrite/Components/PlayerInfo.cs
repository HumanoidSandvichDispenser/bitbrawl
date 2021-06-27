using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_pyro_rewrite.Components
{
    public enum PlayerClass
    {
        None,
        Templar,
        Mage,
        Hunter
    }

    public enum PlayerTeam
    {
        None,
        Yellow,
        Purple
    }

    public class PlayerInfo : Component
    {
        public float Health { get; set; }

        public float MaxHealth { get; set; }

        public PlayerClass Class { get; set; } = PlayerClass.Mage;

        public PlayerTeam Team { get; set; } = PlayerTeam.Purple;

        public float LastFireTime { get; set; }

        public bool Firing { get; set; } = false;

        public float AbilityChargeTime { get; set; }

        public float DamageAbilityChargeTime { get; set; }

        public Vector2 LookAt { get; set; }

        public bool AbilityActive { get; set; } = false;

        public bool DamageAbilityActive { get; set; } = false;
    }
}
