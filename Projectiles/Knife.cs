using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TBM.Projectiles
{
    public class Knife : CustomDrawProj
    {
        private bool _setFrame;
        private Vector2 _prestopvelocity;
        private bool _startSlowDown;
        public override void SafeSetDefaults()
        {
            _startSlowDown = false;
            _prestopvelocity = Vector2.Zero;
            _setFrame = false;
            AddRotation = false;
            projectile.timeLeft = 500;
            projectile.width = 8;
            projectile.friendly = true;
            DrawOrigin = new Vector2(58, 10);
            projectile.height = 8;
            projectile.penetrate = 1;
            color = Color.White;
            FrameHeight = 10;
            TexturePath = "TBM/Projectiles/Knife";
        }
        public override bool PreAI()
        {
            if (_prestopvelocity == Vector2.Zero)
                _prestopvelocity = projectile.velocity;
            if (!_setFrame)
            {
                CurrentFrame = Main.rand.Next(2) == 0 ? 1 : 0;
                _setFrame = true;
            }
            if (TBMWorld.Get().TimeStopDuration > 0)
            {
                foreach (NPC npc in Main.npc)
                {
                    if (!npc.active)
                        continue;
                    if (_startSlowDown)
                        break;
                    for (float i = 0.5f; i < 4f; i += 0.5f)
                    {
                        if (new Rectangle((int)(projectile.Center.X + projectile.velocity.X * i), (int)(projectile.Center.Y + projectile.velocity.Y * i), 8, 8).Intersects(npc.Hitbox))
                        {
                            _startSlowDown = true;
                        }
                    }
                }
                foreach (Player player in Main.player)
                {
                    if (!player.active || player.whoAmI == projectile.owner)
                        continue;
                    if (_startSlowDown)
                        break;
                    for (float i = 0.5f; i < 4f; i += 0.5f)
                    {
                        if (new Rectangle((int)(projectile.Center.X + projectile.velocity.X * i), (int)(projectile.Center.Y + projectile.velocity.Y * i), 8, 8).Intersects(player.Hitbox))
                        {
                            _startSlowDown = true;
                        }
                    }
                }
            }
            else
            {
                _startSlowDown = false;
                projectile.velocity = _prestopvelocity;
            }
            
            if (_startSlowDown && Math.Abs(projectile.velocity.X) > Math.Abs(_prestopvelocity.X * 0.00005f))
                projectile.velocity *= 0.5f;
            return base.PreAI();
        }
        public override bool ShouldUpdatePosition()
        {
            return !(Math.Abs(projectile.velocity.X) <= Math.Abs(_prestopvelocity.X * 0.00005f));
        }
    }
}
