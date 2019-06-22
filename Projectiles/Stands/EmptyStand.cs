using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBM.Projectiles.Stands
{
    public class EmptyStand : StandBase
    {
        public override bool PreAI()
        {
            projectile.Kill();
            return base.PreAI();
        }
    }
}
