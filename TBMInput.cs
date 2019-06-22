using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace TBM
{
    public static class TBMInput
    {
        public static ModHotKey ActivateStand;
        public static ModHotKey CommandAttack;
        public static void Load(Mod mod)
        {
            ActivateStand = mod.RegisterHotKey("Summon stand", "Z");
            CommandAttack = mod.RegisterHotKey("Command stand to attack", "X");
        }
        public static void Unload()
        {
            ActivateStand = null;
            CommandAttack = null;
        }
    }
}
