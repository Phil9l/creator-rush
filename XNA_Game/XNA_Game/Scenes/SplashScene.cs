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

namespace XNA_Game.Scenes {
    class SplashScene : GameScene {
        KeyboardState keyState;
        SpriteFont font;

        public override void LoadContent(ContentManager Content) {
             base.LoadContent(Content);
             if (font == null) {
                 font = content.Load<SpriteFont>("Font1");
             }
        }

        public override void UnloadContent() {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime) {
            keyState = Keyboard.GetState();
            if (previousState.IsKeyUp(Keys.Space) && keyState.IsKeyDown(Keys.Space)) {
                SceneManager.Instance.AddScene(new Scenes.TitleScene());
            }
            previousState = keyState;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(
                font, "Super Mega\nFucking Game\n\nClick Space To Skip This Intro",
                new Vector2(100, 100), 
                Color.Black
            );
        }
    }
}
