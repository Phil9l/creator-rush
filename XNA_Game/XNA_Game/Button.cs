using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XNA_Game {
    class Button {
        string caption;
        SpriteFont font;
        Vector2 position;
        Texture2D texture;
        CallBack callback;
        bool leftButtonState;

        public void LoadContent(string caption, Texture2D texture, SpriteFont font, Vector2 position, CallBack callback, bool leftButtonState) {
            this.caption = caption;
            this.font = font;
            this.position = position;
            this.texture = texture;
            this.callback = callback;
            this.leftButtonState = leftButtonState;
        }

        public void Draw(SpriteBatch spriteBatch) {
            var captionRect = font.MeasureString(caption);
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.DrawString(
                font, caption,
                new Vector2(
                    position.X + (texture.Width - captionRect.X) / 2,
                    position.Y + (texture.Height - captionRect.Y) / 2
                ),
                Color.Black
            );
        }

        public void Update(GameTime gameTime, MouseState mouseState) {
            Rectangle buttonRect = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            if (!leftButtonState && mouseState.LeftButton == ButtonState.Pressed && buttonRect.Contains((int)mouseState.X, (int)mouseState.Y)) {
                callback();
            }
            this.leftButtonState = mouseState.LeftButton == ButtonState.Pressed;
        }
    }
}
