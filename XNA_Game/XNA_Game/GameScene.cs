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
    class GameScene {
        protected ContentManager content;
        protected KeyboardState previousState;

        public virtual void LoadContent(ContentManager Content) { 
            content = new ContentManager(Content.ServiceProvider, "Content");
            previousState = Keyboard.GetState();
        }

        public virtual void UnloadContent() {
            content.Unload();
        }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }

        public virtual Vector2 GetCamera() {
            return new Vector2(0, 0);
        }
    }
}
