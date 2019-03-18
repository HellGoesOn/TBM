using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using TBM.UIs;
using Terraria.Graphics;

namespace TBM
{
	class TBM : Mod
	{
        public static TBM instance;
        public Texture2D EmptyTexture;
        public ModHotKey ActivateStand;
        public ModHotKey StandAttackMain;
        public override void Load()
        {
            instance = this;
            ActivateStand = RegisterHotKey("Summon stand", "F");
            StandAttackMain = RegisterHotKey("Stand Command: Attack", "R");
            #region Load Progress bars
            EmptyTexture = ModLoader.GetTexture("TBM/Textures/Empty");
            #endregion

        }
        public override void Unload()
        {
            ActivateStand = null;
            StandAttackMain = null;
            EmptyTexture = null;
            instance = null;
        }
    }
}
