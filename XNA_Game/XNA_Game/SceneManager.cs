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
    class SceneManager {
        #region Variables
        ContentManager content;
        private static SceneManager instance;
        Stack<GameScene> scenesStack = new Stack<GameScene>();
        Vector2 dimensions;
        #endregion

        #region Properties
        public static SceneManager Instance {
            get {
                if (instance == null) {
                    instance = new SceneManager();
                }
                return instance;
            }
        }

        public Vector2 Dimensions {
            get { return dimensions; }
            set { dimensions = value; }
        }
        #endregion

        #region Main Methods
        public void AddScene(GameScene scene) {
            if (scenesStack.Count() != 0) {
                scenesStack.Peek().UnloadContent();
            }
            scenesStack.Push(scene);
            scenesStack.Peek().LoadContent(content);
        }

        public void Initialize() {}

        public void LoadContent(ContentManager Content) {
            content = new ContentManager(Content.ServiceProvider, "Content");
            if (scenesStack.Count() != 0) {
                scenesStack.Peek().LoadContent(Content);
            }
        }

        public void Update(GameTime gameTime) {
            if (scenesStack.Count() != 0) {
                scenesStack.Peek().Update(gameTime);
            }        
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (scenesStack.Count() != 0) {
                scenesStack.Peek().Draw(spriteBatch);
            }
        }

        public Vector2 GetCamera() {
            if (scenesStack.Count() != 0) {
                return scenesStack.Peek().GetCamera();
            }
            return new Vector2(0, 0);
        }
        #endregion
    }
}
