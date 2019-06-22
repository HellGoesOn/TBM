using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBM.Projectiles.Stands.Melee;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Effects;

namespace TBM.Projectiles
{
    public abstract class FistBase : CustomDrawProj
    {
        public override void SafeSetDefaults()
        {
            AddRotation = false;
            projectile.timeLeft = 12;
            projectile.width = 32;
            projectile.friendly = true;
            DrawOrigin = new Vector2(19, 9);
            projectile.height = 32;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            Scale = 0.75f;
            fx = SpriteEffects.FlipHorizontally;
            color = Color.White * 1.1f;
            FrameHeight = 18;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if(TBMWorld.Get().TimeStopDuration <= 0)
                TBMPlayer.Get(Main.player[projectile.owner]).Stamina += 1;
            target.immuneTime = 10;
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (TBMWorld.Get().TimeStopDuration <= 0)
                TBMPlayer.Get(Main.player[projectile.owner]).Stamina += 1;
            target.immuneTime = 10;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (TBMWorld.Get().TimeStopDuration <= 0)
                TBMPlayer.Get(Main.player[projectile.owner]).Stamina += 1;
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Punch" + Main.rand.Next(1, 5)).WithVolume(0.35f));
            target.immune[projectile.owner] = 10;
            TBMUtils.CircleDust(projectile.Center, projectile.velocity, DustID.AncientLight);
        }
        public override void AI()
        {
            var _bonusVel = new Vector2(0, Main.rand.Next(-20, 20)).RotatedBy(projectile.velocity.ToRotation());
            projectile.velocity *= 1.1f;
            if (projectile.velocity.X < 0) CurrentFrame = 1;
            else CurrentFrame = 0;
            if (TBMPlayer.Get(Main.player[projectile.owner]).MyStandProjectile != null)
            {
                projectile.netUpdate = true;
                Vector2 pos = Main.projectile[TBMPlayer.Get(Main.player[projectile.owner]).MyStandProjectile.whoAmI].Center + _bonusVel + projectile.velocity;
                projectile.ai[0] = pos.X;
                projectile.ai[1] = pos.Y;
            }
            foreach(Projectile projectiles in Main.projectile)
            {
                if (projectiles.owner == projectile.owner || !projectiles.active)
                    continue;
                if(projectiles.Name.Contains("Fist") && Collision.CheckAABBvAABBCollision(projectile.position - Main.screenPosition, projectile.Hitbox.Size(), projectiles.position - Main.screenPosition, projectiles.Hitbox.Size()))
                {
                    Main.player[projectile.owner].velocity = (projectile.Center - projectiles.Center).SafeNormalize(-Vector2.UnitY) * 1.5f;
                }
            }
            projectile.Center = new Vector2(projectile.ai[0], projectile.ai[1]);
        }
        public override void PostAI()
        {
            if (projectile.timeLeft <= 6)
            {
                Alpha *= 0.78f;
            }
        }

    }
}
