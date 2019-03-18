using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace TBM.Projectiles
{
    public class EmperorProjectile : CustomDrawProj
    {
        private bool _showGraphic;
        private int _useStyle;
        private MouseState _lastState;
        private float _rotation;
        private float _offset;
        private bool _hasClicked
        {
            get
            {
                return Mouse.GetState().LeftButton == ButtonState.Pressed && _lastState.LeftButton == ButtonState.Released && !Main.isMouseLeftConsumedByUI;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Emperor");
        }
        public override void SafeSetDefaults()
        {
            TexturePath = "TBM/Textures/Emperor";
            projectile.tileCollide = false;
            fx = SpriteEffects.None;
            AddRotation = false;
            projectile.width = 76;
            projectile.height = 46;
            DrawOrigin = new Vector2(38, 23);
            Scale = 0.5f;
            Alpha = 0f;
        }
        public override void AI()
        {
            Player plr = Main.player[projectile.owner];
            Vector2 vel = Utils.DistanceToMouse(plr, _offset);
            projectile.velocity = vel;
            projectile.timeLeft = 2;
            Utils.StayNearPosition(projectile, plr.Center + vel);
            Utils.ChanneledProjectile(plr, projectile);
            if (Alpha < 1f)
            {
                if (_offset < 12)
                    _offset += 0.5f;
                _rotation += 0.5f;
                Alpha += 0.03f;
            }
            else if(Alpha < 2f)
            {
                _rotation = 0.1f;
                Alpha += 1f;
            }
            if(_useStyle == 0)
            {
                _useStyle = plr.HeldItem.useStyle;
                _showGraphic = plr.HeldItem.noUseGraphic;
            }
            if(Main.MouseWorld.X < plr.Center.X)
            {
                BonusRotation =_rotation;
                fx = SpriteEffects.FlipVertically;
            }
            else
            {
                BonusRotation = -_rotation;
                fx = SpriteEffects.None;
            }
            if(_hasClicked)
            {
                Main.PlaySound(SoundID.Item11);
                _rotation = 1.3f;
                Vector2 pos = Utils.MuzzleOffsets(projectile.Center - new Vector2(0, 6), projectile.velocity.X, projectile.velocity.Y, 16f);
                Projectile.NewProjectile(pos, projectile.velocity, ProjectileID.Bullet, 50, 0f, projectile.owner);
            }
            if (_rotation > 0) _rotation -= 0.1f;
            plr.HeldItem.useStyle = 5;
            plr.HeldItem.noUseGraphic = true;
            _lastState = Mouse.GetState();
        }
        public override void Kill(int timeLeft)
        {
            Player plr = Main.player[projectile.owner];
            plr.HeldItem.noUseGraphic = _showGraphic;
            plr.HeldItem.useStyle = _useStyle;
        }
    }
}
