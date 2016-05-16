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
    class MainScene : GameScene {
        KeyboardState keyState;
        Vector2 camera;
        Map map;

        public override void LoadContent(ContentManager Content) {
            base.LoadContent(Content);
            keyState = Keyboard.GetState();
            camera = new Vector2(0, 0);
            map = new Map(new Vector2(64, 64), new Vector2(100, 100));
            map.LoadContent(Content);
        }

        public override void UnloadContent() {
            base.UnloadContent();
            map.UnloadContent();
        }

        public override void Update(GameTime gameTime) {
            var newKeyState = Keyboard.GetState();

            if (keyState.IsKeyUp(Keys.Space) && newKeyState.IsKeyDown(Keys.Space)) {
                map.Shoot();
            }

            keyState = newKeyState;
            Vector2 direction = new Vector2(0, 0);
            direction.X = 2 * (Convert.ToInt32(keyState.IsKeyDown(Keys.Right)) - Convert.ToInt32(keyState.IsKeyDown(Keys.Left)));
            direction.Y = 2 * (Convert.ToInt32(keyState.IsKeyDown(Keys.Down)) - Convert.ToInt32(keyState.IsKeyDown(Keys.Up)));

            if (map.Update(gameTime, direction)) {
                camera -= direction;
            }
        }

        public override Vector2 GetCamera() {
            return camera;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            map.Draw(spriteBatch);
        }
    }
}
