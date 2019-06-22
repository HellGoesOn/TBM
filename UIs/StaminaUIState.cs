using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace TBM.UIs
{
    public class StaminaUIState : UIState
    {
        private float _pulseAmount;
        public float PulseAmount
        {
            get
            {
                return _pulseAmount;
            }
            set
            {
                _pulseAmount = value > 0.5f ? 0 : value;
            }
        }
        public override void Update(GameTime gameTime)
        {
            PulseAmount += 0.01f;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(TBMPlayer.Get(Main.LocalPlayer).IsStandUser)
            DrawMeterUI(spriteBatch, TBMPlayer.Get(Main.LocalPlayer));
            Recalculate();
        }
        public void DrawMeterUI(SpriteBatch spriteBatch, TBMPlayer player)
        {
            float gradient = player.Stamina <= 50 ? (float)player.Stamina * 0.01f : (float)(player.Stamina - 50) * 0.01f;
            Color lerpColor1 = player.Stamina <= 50 ? Color.Red : Color.Lerp(Color.Red, Color.Yellow, 0.5f);
            Color lerpColor2 = player.Stamina <= 50 ? Color.Yellow : Color.SpringGreen;
            Color color = Color.Lerp(lerpColor1, lerpColor2, gradient);
            Color pulsingColor = Color.Lerp(color, Color.Black, _pulseAmount);


            float row2gradient = player.Stamina - 100 <= 50 ? (float)(player.Stamina - 100) * 0.01f : (float)(player.Stamina - 150) * 0.01f;
            Color row2LerpColor1 = player.Stamina - 100 <= 50 ? Color.DarkBlue : Color.Lerp(Color.DarkBlue, Color.MediumPurple, 0.5f);
            Color row2LerpColor2 = player.Stamina - 100 <= 50 ? Color.MediumPurple : Color.Violet;
            Color row2Color = Color.Lerp(row2LerpColor1, row2LerpColor2, row2gradient);
            Color row2pulsingColor = Color.Lerp(row2Color, Color.Black, _pulseAmount);

            float row3gradient = player.Stamina - 200 <= 50 ? (float)(player.Stamina - 200) * 0.01f : (float)(player.Stamina - 250) * 0.01f;
            Color row3LerpColor1 = player.Stamina - 200 <= 50 ? Color.PaleVioletRed : Color.Lerp(Color.PaleVioletRed, Color.Red, 0.5f);
            Color row3LerpColor2 = player.Stamina - 200 <= 50 ? Color.Red : Color.Crimson;
            Color row3Color = Color.Lerp(row3LerpColor1, row3LerpColor2, row3gradient);
            Color row3pulsingColor = Color.Lerp(row3Color, Color.Black, _pulseAmount);

            spriteBatch.Draw
                (
                    player.StaminaMax <= 50 ? TBMTextures.MeterMainTexture_Alt : TBMTextures.MeterMainTexture,
                    new Vector2(30, Main.screenHeight - 80),
                    null,
                    player.Stamina >= 300 ? row3pulsingColor : Color.White,
                    0,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    1f
                );

            DrawBar(spriteBatch, player, color, pulsingColor);
            if(player.Stamina > 100)
                DrawBar(spriteBatch, player, row2Color, row2pulsingColor, 1);
            if (player.Stamina > 200)
                DrawBar(spriteBatch, player, row3Color, row3pulsingColor, 2);
        }

        private static void DrawBar(SpriteBatch spriteBatch, TBMPlayer player, Color color, Color pulsingColor, int row = 0)
        {
            for (int i = 0; i < 10; i++)
            {
                spriteBatch.Draw
                    (
                        TBMTextures.MeterBarTexture,
                        new Vector2(30, Main.screenHeight - 80) + new Vector2(15 * 3 - i * 3, 9 + i * 3),
                        new Rectangle(0, i * 3, (i > 4 ? player.Stamina - row * 100 < 50 ? player.Stamina - row * 100 : 50 : player.Stamina - row * 100 < 100 ? player.Stamina - row * 100 : 100) * 3, 3),
                        player.Stamina == player.StaminaMax ? pulsingColor : color,
                        0,
                        Vector2.Zero,
                        1f,
                        SpriteEffects.None,
                        1f
                    );
            }
        }
    }
}
