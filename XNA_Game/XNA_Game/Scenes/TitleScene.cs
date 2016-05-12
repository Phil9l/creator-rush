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

delegate void CallBack();

namespace XNA_Game.Scenes {
    class TitleScene : GameScene {
        KeyboardState keyState;
        SpriteFont font1, simpleiriska;
        Button tmp_button;

        public override void LoadContent(ContentManager Content) {
            base.LoadContent(Content);
            if (font1 == null) {
                font1 = content.Load<SpriteFont>("Font1");
            }
            if (simpleiriska == null) {
                simpleiriska = content.Load<SpriteFont>("simpleiriska");
            }

            #region CallBack Functions
            CallBack callBackNewGame = new CallBack(NewGame);
            #endregion

            tmp_button = new Button();
            Texture2D button_texture = content.Load<Texture2D>("Button");
            tmp_button.LoadContent("New Game", button_texture, font1, new Vector2(200, 200), callBackNewGame, Mouse.GetState().LeftButton == ButtonState.Pressed);
        }

        public override void UnloadContent() {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime) {
            MouseState mouseState = Mouse.GetState();
            keyState = Keyboard.GetState();

            tmp_button.Update(gameTime, mouseState);

            previousState = keyState;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(
                simpleiriska, "Fucing Mega Game",
                new Vector2(100, 100),
                Color.Black
            );
            tmp_button.Draw(spriteBatch);
        }
        
        #region Button Actions
        private static void NewGame() {
            SceneManager.Instance.AddScene(new Scenes.MainScene());
        }

        #endregion
    }
}
