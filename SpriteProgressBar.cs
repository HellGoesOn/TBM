using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace TBM
{
    class SpriteProgressBar
    {
        public Texture2D MainTexture = Main.magicPixel;
        public Texture2D BarTexture = Main.magicPixel;
        public Vector2 Position;
        public Vector2 PosOffset;
        public float Alpha = 1f;
        public int FrameCount = 0;
        public int FrameTimer = 0;
        public int CurrentFrame;
        public int FrameSpeed = 4;
        public Color[] Colors = new Color[] { Color.White, Color.White };
        public float[] Scales = new float[] { 1f, 1f };
        public int[] Widths = new int[] { 0, 0 };
        public int[] Heights = new int[] { 0, 0 };
        public int MaxVal;
        public int CurVal;
        public SpriteEffects spriteEffects = SpriteEffects.None;
        
        public SpriteProgressBar(Texture2D tex1, Texture2D tex2, Vector2 pos, Vector2 offset = new Vector2(), int frameCount = 1,  Color c1 = new Color(), Color c2 = new Color())
        {
            MainTexture = tex1;
            BarTexture = tex2;
            Position = pos;
            PosOffset = offset;
            if (c1 != new Color())
                Colors[0] = c1;

            if (c2 != new Color())
                Colors[1] = c2;
            FrameCount = frameCount;
        }
        public SpriteProgressBar(int width1, int height1, int width2, int height2, Vector2 pos, Vector2 offset)
        {
            Widths[0] = width1;
            Widths[1] = width2;
            Heights[0] = height1;
            Heights[1] = height2;
            Position = pos;
            PosOffset = offset;
        }
        public void DrawSelf(SpriteBatch spriteBatch, int currentValue, int maxValue = 100)
        {
            #region Drawing
            var frameHeight = FrameCount > 1 ? MainTexture.Height / FrameCount : MainTexture.Height; 
            var rect1 = new Rectangle(0, CurrentFrame * frameHeight, MainTexture.Width, frameHeight);
            if (Widths[0] != 0)
                rect1 = new Rectangle(0, 0, Widths[0], Heights[0]);
            var rect2 = new Rectangle(0, 0, CalcLength(currentValue, maxValue, BarTexture.Width), BarTexture.Height);
            if (Widths[1] != 0)
                rect2 = new Rectangle(0, 0, CalcLength(currentValue, maxValue, Widths[1]), Heights[1]);
            spriteBatch.Draw
                (
                    MainTexture,
                    new Vector2((int)Position.X, (int)Position.Y),
                    rect1,
                    Colors[0] * Alpha,
                    0f,
                    Vector2.Zero,
                    Scales[0],
                    spriteEffects,
                    1f
                );
            spriteBatch.Draw
                (
                    BarTexture,
                    new Vector2((int)Position.X, (int)Position.Y) + new Vector2((int)PosOffset.X, (int)PosOffset.Y),
                    rect2,
                    Colors[1] * Alpha,
                    0f,
                    Vector2.Zero,
                    Scales[1],
                    spriteEffects,
                    1f
                );
            #endregion
            if (FrameCount > 1)
            {
                if (++FrameTimer > FrameSpeed)
                {
                    if (CurrentFrame < FrameCount - 1)
                        CurrentFrame++;
                    else
                        CurrentFrame = 0;
                    FrameTimer = 0;
                }
            }
        }
        public int CalcLength(int val, int maxVal, int TextureWidth)
        {
            return (val * TextureWidth) / maxVal;
        }
    }
}
