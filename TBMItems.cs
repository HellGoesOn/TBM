using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TBM
{
    public class TBMItems : GlobalItem
    {
        public bool IsStopped
        {
            get
            {
                return mod.GetModWorld<TBMWorld>().TimeStopDuration > 0;
            }
        }
        public override void PostUpdate(Item item)
        {
            if(IsStopped)
            {
                Main.itemFrameCounter[item.whoAmI] = 0;
                item.velocity *= 0;
            }
        }
    }
}
