using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBM.Projectiles;
using TBM.Projectiles.Stands;
using TBM.Projectiles.Stands.Melee;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TBM.Items
{
    public class StandArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Might grant you incredible power..." +
                "\nOr kill you...Who knows?");
        }
        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;
            item.useStyle = 2;
            item.scale = 0.5f;
            item.useAnimation = 20;
            item.rare = -12;
            item.useTime = 20;
        }
        public override bool CanUseItem(Player player)
        {
            StandBase stand = Main.rand.Next(StandManager.AllStands);
            TBMPlayer.Get(player).MyStand = (StandBase)Activator.CreateInstance(stand.GetType());
            PlayerDeathReason reason = new PlayerDeathReason();
            reason.SourceCustomReason = player.name + " could not handle the power within.";
            /*if (stand is EmptyStand)
                player.KillMe(reason, 0, 0);*/
            return base.CanUseItem(player);
        }
    }
}
