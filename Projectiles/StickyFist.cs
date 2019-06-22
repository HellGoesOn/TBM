using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBM.Projectiles.Stands.Melee;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Effects;

namespace TBM.Projectiles
{
    public class StickyFist : FistBase
    {
        public override void SafeSetDefaults()
        {
            base.SafeSetDefaults();

            TexturePath = "TBM/Textures/StickyFist";
        }
        public override void PostAI()
        {
            base.PostAI();
            if (Main.rand.Next(2) == 0)
            {
                TexturePath = "TBM/Textures/StickyFist";
            }
            else
            {
                TexturePath = "TBM/Textures/StickyFistBack";
            }
        }
    }
}
