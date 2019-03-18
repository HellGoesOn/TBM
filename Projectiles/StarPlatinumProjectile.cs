using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBM;
using Terraria;
using Terraria.ID;

namespace TBM.Projectiles
{
    public class StarPlatinumProjectile : StandBase
    {
        public int FramesNeeded;
        public override void SafeSetDefaults()
        {
            TexturePath = "TBM/Textures/StarPlatinum";
            FrameHeight = 96;
            projectile.tileCollide = false;
            fx = SpriteEffects.FlipHorizontally;
            AddRotation = false;
            RotateByVel = false;
            projectile.width = 64;
            projectile.height = FrameHeight;
            DrawOrigin = new Vector2(32, 48);
            Scale = 0.75f;
            Alpha = 0f;
        }
        public override void AI()
        {
            if(Alpha < 1.0f)
                Alpha += 0.05f;

            Player plr = Main.player[projectile.owner];
            projectile.timeLeft = 2;
            if (plr.HeldItem.melee)
            {
                if(!Main.hardMode)
                    projectile.damage = (int)(plr.HeldItem.damage * 1.75f);
                else if(NPC.downedPlantBoss)
                    projectile.damage = (int)(plr.HeldItem.damage * 2.25f);
                else
                    projectile.damage = (int)(plr.HeldItem.damage * 3f);
            }
            else projectile.damage = 20;
            if (projectile.ai[1] == 1)
            {
                if (plr.Center.X < Main.MouseWorld.X)
                    plr.direction = 1;
                else
                    plr.direction = -1;

                //projectile.position = plr.position - new Vector2(24 * (plr.direction * -1), 22);
                //projectile.Center = plr.Center - new Vector2(24 * (plr.direction * -1), 22);
                projectile.position = plr.position + new Vector2(8 * (plr.direction * -1), -24);
                projectile.Center = plr.Center + new Vector2(8 * (plr.direction * -1), -24);
                plr.velocity.X = Utils.DistanceToMouse(plr, .7f).X;
            }
            else
            {
                projectile.position = plr.position + new Vector2(8 * (plr.direction * -1), -24);
                projectile.Center = plr.Center + new Vector2(8 * (plr.direction * -1), -24);
            }
            //plr.heldProj = projectile.whoAmI;
            FramesNeeded = projectile.ai[1] == 0 ? 6 : 2;

            fx = plr.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            var directionOffset = fx == SpriteEffects.FlipHorizontally ? 1 : -1;
            var offset = fx == SpriteEffects.FlipHorizontally ? 1 : -1;


            CurrentFrame = projectile.frame;
            #region Animations
            if (++projectile.frameCounter >= FramesNeeded)
            {
                if(projectile.ai[1] == 0)
                {
                    if(++projectile.frame >= 3)
                    {
                        projectile.frame = 0;
                    }
                }
                else if(projectile.ai[1] == 1)
                {
                    Vector2 vel = Utils.DistanceToMouse(plr.Center, 32);
                    if (projectile.frame <= 3) projectile.frame = 4;
                    Projectile.NewProjectile(projectile.Center, vel.RotatedByRandom(MathHelper.ToRadians(36)) * .38f, mod.ProjectileType<StarFist>(), projectile.damage, 0f, projectile.owner);
                    if (++projectile.frame >= 8)
                    {
                        projectile.frame = 4;
                    }
                }
                projectile.frameCounter = 0;
            }
            #endregion
        }
    }
    public class StarFist : CustomDrawProj
    {
        public override void SafeSetDefaults()
        {
            TexturePath = "TBM/Textures/Starfist";
            AddRotation = false;
            projectile.timeLeft = 12;
            projectile.width = 16;
            projectile.friendly = true;
            DrawOrigin = new Vector2(19, 9);
            projectile.height = 16;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            Scale = 0.75f;
            fx = SpriteEffects.FlipHorizontally;
            FrameHeight = 18;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 5;
            Utils.CircleDust(projectile.Center, projectile.velocity, DustID.AncientLight);
        }
        public override void AI()
        {
            Player plr = Main.player[projectile.owner];
            projectile.velocity *= 1.1f;
            if (projectile.velocity.X < 0) CurrentFrame = 1;
            else CurrentFrame = 0;
            foreach (Projectile p in Main.projectile)
            {
                if(p.type == mod.ProjectileType<StarPlatinumProjectile>() && p.owner == projectile.owner)
                {
                    var myProj = (CustomDrawProj)p.modProjectile;
                    projectile.Center = p.Center + projectile.velocity;
                    break;
                }
            }
        }
        public override void PostAI()
        {
            if(projectile.timeLeft <= 6)
            {
                Alpha *= 0.78f;
            }
        }
    }
}
