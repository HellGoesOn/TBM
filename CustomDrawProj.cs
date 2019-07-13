using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TBM
{
    public abstract class CustomDrawProj : ModProjectile
    {
        public string TexturePath;
        public Vector2 DrawOrigin;
        public SpriteEffects fx = SpriteEffects.None;
        public float BonusRotation;
        public bool AddRotation = true;
        public float Alpha = 1f;
        public Color color = new Color();
        public float Scale = 1f;
        public bool RotateByVel = true;
        public int FrameHeight;
        public int CurrentFrame;
        public override string Texture
        {
            get
            {
                return "TBM/Textures/Empty";
            }
        }

        public override void SetDefaults()
        {
            projectile.alpha = 255;
            projectile.aiStyle = -1;
            SafeSetDefaults();
        }
        public virtual void SafeSetDefaults() { }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float offset = AddRotation ? (float)(Math.PI / 4) : 0;
            float rot = RotateByVel ? projectile.velocity.ToRotation() : 0f;
            Color clr = color != new Color() ? color : lightColor;
            Texture2D tex = ModContent.GetTexture(TexturePath);
            Rectangle Rect = FrameHeight != 0 ? new Rectangle(0, CurrentFrame * FrameHeight, tex.Width, FrameHeight) : new Rectangle(0, 0, tex.Width, tex.Height);
            spriteBatch.Draw(
                tex,
                projectile.Center - Main.screenPosition,
                Rect,
                clr * Alpha,
                rot + offset + BonusRotation,
                DrawOrigin,
                Scale,
                fx,
                1
                );
        }
    }
}
