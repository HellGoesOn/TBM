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
    public class TheWorldFist : FistBase
    {
        public override void SafeSetDefaults()
        {
            base.SafeSetDefaults();

            TexturePath = "TBM/Textures/TheWorldFists";
        }
        public override void PostAI()
        {
            base.PostAI();
            if (Main.rand.Next(2) == 1)
            {
                FrameHeight = 22;
                TexturePath = "TBM/Textures/TheWorldFists";
            }
            else
            {
                TexturePath = "TBM/Textures/TheWorldFistsBack";
                FrameHeight = 24;
            }
        }
    }
}
