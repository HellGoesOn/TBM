using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace TBM
{
    public static class TBMTextures
    {
        public static Texture2D StarPlatinum;
        public static Texture2D StickyFingers;
        public static Texture2D Zipper;
        public static Texture2D EmptyTexture;
        public static Texture2D MeterMainTexture;
        public static Texture2D MeterMainTexture_Alt;
        public static Texture2D MeterBarTexture;
        public static Texture2D TheWorld;
        public static Texture2D TheWorldTimeStop;
        public const string EmptyFull = "TBM/Textures/Empty";
        public const string Empty = "Textures/Empty";
        public static void Load(Mod mod)
        {
            EmptyTexture = mod.GetTexture(Empty);
            StarPlatinum = mod.GetTexture("Textures/StarPlatinum");
            StickyFingers = mod.GetTexture("Textures/StickyFingers");
            Zipper = mod.GetTexture("Textures/ZipperLoop");
            MeterMainTexture = mod.GetTexture("Textures/MeterMainTexture");
            MeterMainTexture_Alt = mod.GetTexture("Textures/MeterMainTexture_Alternate");
            MeterBarTexture = mod.GetTexture("Textures/MeterBarTexture1");
            TheWorld = mod.GetTexture("Textures/TheWorld");
            TheWorldTimeStop = mod.GetTexture("Textures/TheWorldTimeStop");
        }
        public static void Unload()
        {
            EmptyTexture = null;
            StarPlatinum = null;
            StickyFingers = null;
            Zipper = null;
            MeterMainTexture = null;
            MeterBarTexture = null;
            MeterMainTexture_Alt = null;
            TheWorld = null;
            TheWorldTimeStop = null;
        }
    }
}
