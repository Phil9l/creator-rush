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
    public class Game1 : Microsoft.Xna.Framework.Game {
        private static Game1 instance;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            SceneManager.Instance.Initialize();
            SceneManager.Instance.Dimensions = new Vector2(800, 600);
            
            graphics.PreferredBackBufferWidth = (int)SceneManager.Instance.Dimensions.X;
            graphics.PreferredBackBufferHeight= (int)SceneManager.Instance.Dimensions.Y;
            IsMouseVisible = true;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SceneManager.Instance.LoadContent(Content);

            SceneManager.Instance.AddScene(new Scenes.SplashScene());
        }

        protected override void UnloadContent() {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
                this.Exit();
            }

            SceneManager.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            
            GraphicsDevice.Clear(Color.DarkCyan);
            Vector2 camera = SceneManager.Instance.GetCamera();
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Matrix.CreateTranslation(camera.X, camera.Y, 0));
            SceneManager.Instance.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Quit() {
            Exit();
        }
    }
}
