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
using System.IO;

namespace TBM.Projectiles
{
    public class UnzippedStickyFist : CustomDrawProj
    {
        private float _velX;
        private float _velY;
        private Vector2 _pos;
        private float _dist;
        public override void SafeSetDefaults()
        {
            AddRotation = false;
            projectile.timeLeft = 500;
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
            _pos = Vector2.Zero;
            TexturePath = "TBM/Textures/StickyFist";
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            TBMPlayer.Get(Main.player[projectile.owner]).Stamina += 1;
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Punch" + Main.rand.Next(1, 5)).WithVolume(0.35f));
            target.immune[projectile.owner] = 10;
            TBMUtils.CircleDust(projectile.Center, projectile.velocity, DustID.AncientLight);
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            TBMPlayer.Get(Main.player[projectile.owner]).Stamina += 1;
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            TBMPlayer.Get(Main.player[projectile.owner]).Stamina += 1;
        }
        public override void AI()
        {
            projectile.netUpdate = true;
            projectile.netUpdate2 = true;
            if (projectile.velocity.X < 0) CurrentFrame = 1;
            else CurrentFrame = 0;
            if (TBMPlayer.Get(Main.player[projectile.owner]).MyStandProjectile != null)
            {
                Vector2 pos = Main.projectile[TBMPlayer.Get(Main.player[projectile.owner]).MyStandProjectile.whoAmI].Center + projectile.velocity;
                projectile.ai[0] = pos.X;
                projectile.ai[1] = pos.Y;
                this._pos = Main.projectile[TBMPlayer.Get(Main.player[projectile.owner]).MyStandProjectile.whoAmI].Center + new Vector2(24 * Main.player[projectile.owner].direction, -3);
            }
            if (projectile.timeLeft <= 480)
            {
                if (TBMPlayer.Get(Main.player[projectile.owner]).MyStandProjectile != null)
                {
                    Vector2 pos = Main.projectile[TBMPlayer.Get(Main.player[projectile.owner]).MyStandProjectile.whoAmI].Center;

                    _velX -= new Vector2(5, 0).RotatedBy(projectile.velocity.ToRotation()).X;
                    _velY -= new Vector2(5, 0).RotatedBy(projectile.velocity.ToRotation()).Y;
                    if (Vector2.Distance(projectile.Center, pos + new Vector2(24 * Main.player[projectile.owner].direction, -5)) <= 10)
                        projectile.Kill();
                }
                else
                    projectile.Kill();
            }
            else
            {
                _velX += new Vector2(5, 0).RotatedBy(projectile.velocity.ToRotation()).X;
                _velY += new Vector2(5, 0).RotatedBy(projectile.velocity.ToRotation()).Y;
            }
            projectile.velocity = new Vector2(_velX, _velY);
            projectile.Center = new Vector2(projectile.ai[0], projectile.ai[1]) + new Vector2(24 * Main.player[projectile.owner].direction, -5);
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(_dist);
            writer.Write(_pos.X);
            writer.Write(_pos.Y);
            writer.Write(_velX);
            writer.Write(_velY);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            _dist = reader.ReadSingle();
            _pos.X = reader.ReadSingle();
            _pos.Y = reader.ReadSingle();
            _velX = reader.ReadSingle();
            _velY = reader.ReadSingle();
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            _dist = Vector2.Distance(projectile.Center, _pos) / 3;
            for (int i = 0; i < _dist; i++)
            {
                spriteBatch.Draw
                    (
                        TBMTextures.Zipper,
                        projectile.Center - new Vector2(i * 3, 0).RotatedBy(projectile.velocity.ToRotation()) - new Vector2(4, 0).RotatedBy(projectile.velocity.ToRotation()) - Main.screenPosition,
                        null,
                        Color.White,
                        projectile.velocity.ToRotation(),
                        new Vector2(3, 4),
                        1f,
                        projectile.Center.X < _pos.X ? SpriteEffects.FlipVertically : SpriteEffects.None,
                        1f
                    );
            }
            base.PostDraw(spriteBatch, lightColor);
        }
    }
}
