using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace TBM
{
    /// <summary>
    /// Lesser version of SpriteProgressBar
    /// </summary>
    public class SpriteBar : UIElement
    {
        public Vector2 position;
        public Texture2D texture;
        public int MyLength;
        public SpriteBar(Texture2D tex, Vector2 pos)
        {
            position = pos;
            texture = tex;
        }
        public void DrawSelf(SpriteBatch batch, int value, int maxvalue, int length = 0, float opacity = 1f)
        {
            if(length == 0)
                length = texture.Width;
            int Length = (value * length) / maxvalue;
            MyLength = Length;
            batch.Draw
                (
                texture,
                position,
                new Rectangle(0, 0, Length, texture.Height),
                Color.White * opacity,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                1f
                );
        }
    }
}
