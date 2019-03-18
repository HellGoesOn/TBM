using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using TBM.Projectiles;
using Microsoft.Xna.Framework.Input;

namespace TBM
{
    public class MyPlayer : ModPlayer
    {
        public bool CommandedToAttack;
        public bool CanUnsummon;
        public int Timer;
        public Vector2 PanTarget;
        public static MyPlayer Get(Player player)
        {
            return player.GetModPlayer<MyPlayer>();
        }

        public override void ModifyScreenPosition()
        {
            if (PanTarget != Vector2.Zero && PanTarget != null)
            {
                Utils.PanCamera(PanTarget);
            }
            PanTarget = Vector2.Zero;
        }
        public override void ResetEffects()
        {
            if (Timer > 0) Timer--;
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            var inst = TBM.instance;
            //Despawning Stand;
            if (inst.ActivateStand.JustPressed && CanUnsummon && Timer <= 0)
            {
                foreach (Projectile proj in Main.projectile)
                {
                    if (proj.type == mod.ProjectileType<StarPlatinumProjectile>() && proj.owner == player.whoAmI)
                    {
                        proj.Kill();
                        break;
                    }
                }
                CanUnsummon = false;
                Timer = 15;
            }
            //Spawning stand
            if (inst.ActivateStand.JustPressed && Timer <= 0 && !CanUnsummon)
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType<StarPlatinumProjectile>(), 20, 0f, player.whoAmI);
                CanUnsummon = true;
                Timer = 15;
            }
            //Make stand attacc
            if(inst.StandAttackMain.JustPressed && CanUnsummon && Timer <= 0)
            {
                foreach(Projectile proj in Main.projectile)
                {
                    if(proj.modProjectile is StarPlatinumProjectile && proj.owner == player.whoAmI)
                    {
                        if(proj.ai[1] == 0) proj.ai[1] = 1;
                        else proj.ai[1] = 0;
                        break;
                    }
                }
            }
        }
    }
}
