using Nez;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_pyro_rewrite.Components
{
    public class Weapon : Component
    {
        public float Damage { get; set; }
        public ushort CurrentAmmo { get; set; }
        public ushort ReserveAmmo { get; set; }
        public ushort MaxClipSize { get; set; }
        public ushort MaxReserveAMmo { get; set; }
        public float ReloadTime { get; set; }
        public float Spread { get; set; }
    }
}
