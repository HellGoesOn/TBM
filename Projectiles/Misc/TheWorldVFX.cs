using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TBM.Projectiles.Misc
{
    public class TheWorldVFX : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 128;
            projectile.height = 128;
            projectile.timeLeft = 620;
            projectile.alpha = 255;
            projectile.scale = 0.01f;
            projectile.tileCollide = false;
        }
        public override void AI()
        {
            projectile.netUpdate = true;
            projectile.scale = projectile.ai[0];
            if(projectile.timeLeft > 590)
            {
                projectile.ai[0]++;
            }
            if(projectile.timeLeft <= 210)
            {
                projectile.ai[0]--;
            }
            if(projectile.ai[0] <= 0)
            {
                projectile.Kill();
            }
            projectile.Center = Main.player[projectile.owner].Center;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.Draw
                (
                Main.projectileTexture[projectile.type],
                projectile.Center - Main.screenPosition,
                null,
                Color.White * 0.3f,
                0f,
                new Vector2(64, 64),
                projectile.scale,
                SpriteEffects.None,
                1f
                );
        }
    }
}
