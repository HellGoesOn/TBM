using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using TBM.UIs;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using System.IO;
using TBM.Projectiles;
using System;
using TBM.Enums;

namespace TBM
{
	class TBM : Mod
	{
        public static TBM instance;
        public static Mod Laugicality;
        private StaminaUIState _staminaUIState;
        internal UserInterface StaminaUserInterface;
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType type = (MessageType)reader.ReadByte();
            switch (type)
            {
                case MessageType.MyStandID:
                    Player player = Main.player[reader.ReadByte()];
                    TBMPlayer.Get(player).MyStandProjectile = Main.projectile[reader.ReadByte()];
                    break;
                case MessageType.IsInRush:
                    player = Main.player[reader.ReadByte()];
                    TBMPlayer.Get(player).IsStandUser = reader.ReadBoolean();
                    break;
                case MessageType.TimeStopDuration:
                    instance.GetModWorld<TBMWorld>().TimeStopOwner = reader.ReadInt32();
                    instance.GetModWorld<TBMWorld>().TimeStopDuration = reader.ReadInt32();
                    break;
            }
        }
        public override void Load()
        {
            Laugicality = ModLoader.GetMod("Laugicality");
            if (!Main.dedServ)
            {
                _staminaUIState = new StaminaUIState();
                _staminaUIState.Activate();
                StaminaUserInterface = new UserInterface();
                StaminaUserInterface.SetState(_staminaUIState);
                AddEquipTexture(null, EquipType.Head, "P4Cap", "TBM/Items/Vanity/JotaroCapP5_Head");
                AddEquipTexture(null, EquipType.Head, "P6Cap", "TBM/Items/Vanity/JotaroCapP6_Head");
                StandManager.Initalize();
                TBMInput.Load(this);
                TBMTextures.Load(this);
            }
            TimeStopManagement.Initialize();
            instance = this;
        }
        public override void UpdateUI(GameTime gameTime)
        {
            if(StaminaUserInterface != null)
            {
                _staminaUIState.Update(gameTime);
            }
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (MouseTextIndex != -1)
            {
                layers.Insert(MouseTextIndex - 1, new LegacyGameInterfaceLayer(
                    "TBM: UI",
                    delegate
                    {
                        _staminaUIState.Draw(Main.spriteBatch);
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
        public override object Call(params object[] args)
        {
            if((string)args[0] == "TimeStopImmuneNPC")
            {
                TimeStopManagement.TimeStopImmuneNPCs.Add((int)args[1]);
            }
            return base.Call(args);
        }
        public override void Unload()
        {
            TimeStopManagement.TimeStopImmuneNPCs.Clear();
            StandManager.Unload();
            TBMInput.Unload();
            TBMTextures.Unload();
            instance = null;
            Laugicality = null;
        }
    }
}
