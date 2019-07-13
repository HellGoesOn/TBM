using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TBM.Items.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class JotaroCapP3 : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Jotaro's Hat");
            Tooltip.SetDefault("Stardust Crusader's most precious relic");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = 10000;
            item.rare = -12;
            item.vanity = true;
        }
        public override void UpdateEquip(Player player)
        {
            TBMPlayer.Get(player).StaminaMax += 150;
        }
    }
    public class JotaroPlayer : ModPlayer
    {
        private bool HasJotaroCap(Player player)
        {
            foreach (Item item in player.armor)
            {
                if (item.type == mod.ItemType<JotaroCapP3>())
                    return true;
            }
            return false;
        }
        public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
        {
            if (HasJotaroCap(player))
                drawInfo.headArmorShader = 0;
        }
        public override void FrameEffects()
        {
            if (HasJotaroCap(player) && (player.dye[0].Name.Contains("Purple") || player.dye[0].Name.Contains("Pink")))
            {
                player.head = mod.GetEquipSlot("P6Cap", EquipType.Head);
            }
            if (HasJotaroCap(player) && (player.dye[0].Name.Contains("White") || player.dye[0].Name.Contains("Silver")))
            {
                player.head = mod.GetEquipSlot("P4Cap", EquipType.Head);
            }
        }
    }
    public class P4Cap : EquipTexture
    {
    }
    public class P6Cap : EquipTexture
    {
    }
}
