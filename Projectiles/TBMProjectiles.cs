using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TBM.NPCs
{
    public class TBMProjectiles : GlobalProjectile
    {
        private Projectile _projectile;
        public bool IsStopped
        {
            get
            {
                return mod.GetModWorld<TBMWorld>().TimeStopDuration > 0 && (_projectile.owner != mod.GetModWorld<TBMWorld>().TimeStopOwner || _projectile.hostile);
            }
        }
        public override void SetDefaults(Projectile projectile)
        {
            _projectile = projectile;
        }
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public override bool PreAI(Projectile projectile)
        {
            if(IsStopped)
            {
                projectile.frameCounter = 0;
                projectile.timeLeft++;
            }
            return !IsStopped;
        }
        public override bool ShouldUpdatePosition(Projectile projectile)
        {
            return !IsStopped;
        }
    }
}
