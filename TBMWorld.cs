using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBM.Enums;
using TBM.Projectiles.Stands.Melee;
using Terraria;
using Terraria.ModLoader;

namespace TBM
{
    public class TBMWorld : ModWorld
    {
        public int TimeStopDuration;
        public int TimeStopOwner;
        private int _timer;
        public static TBMWorld Get()
        {
            return TBM.instance.GetModWorld<TBMWorld>();
        }
        public override void Initialize()
        {
            TimeStopOwner = -1;
            TimeStopDuration = 0;
        }
        public override void PostUpdate()
        {
            if (TimeStopDuration > 0)
            {
                TimeStopDuration--;
            }
            else
            {

                TimeStopOwner = -1;
            }
            if (++_timer >= 15 || TimeStopDuration == 300)
            {
                if(Main.netMode == 2)
                {
                    var netmessage = mod.GetPacket();
                    netmessage.Write((byte)MessageType.TimeStopDuration);
                    netmessage.Write((int)TimeStopOwner);
                    netmessage.Write((int)TimeStopDuration);
                    netmessage.Send();
                }
                _timer = 0;
            }
        }
    }
}
