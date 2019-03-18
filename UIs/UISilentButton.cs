using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace TBM.UIs
{
    public class UISilentButton : UIImageButton
    {
        public Texture2D _texture;
        public Vector2 position;
        public int ID;
        public SpriteEffects sfx = SpriteEffects.None;
        public UISilentButton(Texture2D texture) : base(texture)
        {
            _texture = texture;
            this.Width.Set(texture.Width, 0);
            this.Height.Set(texture.Width, 0);
            this.SetPadding(0);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Top.Set(position.X, 0);
            Left.Set(position.Y, 0);
        }
        public override void MouseOver(UIMouseEvent evt)
        {
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = base.GetDimensions();
            spriteBatch.Draw(
                _texture,
                dimensions.Position(),
                null,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                sfx,
                1f
                );
        }
    }
}
