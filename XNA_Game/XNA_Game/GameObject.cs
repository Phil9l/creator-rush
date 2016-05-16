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
    class GameObject {
        protected Sprite sprite;
        protected string spriteName;
        protected Vector2 position;

        public GameObject() { }

        public GameObject(string spriteName, Vector2 position) {
            this.spriteName = spriteName;
            this.position = position;
        }

        public void LoadContent(ContentManager Content) {
            sprite = new Sprite(spriteName, position);
            sprite.LoadContent(Content);
        }

        public void Update(GameTime gameTime) {
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch) {
            sprite.Draw(spriteBatch);
        }

        public bool IsIntersected(GameObject other) {
            var bounds = new Rectangle((int)(sprite.Position().X - sprite.Width() / 2), (int)(sprite.Position().Y - sprite.Height() / 2), (int)sprite.Width(), (int)sprite.Height());
            var otherBounds = new Rectangle((int)(other.sprite.Position().X - other.sprite.Width() / 2), (int)(other.sprite.Position().Y - other.sprite.Height() / 2), (int)other.sprite.Width(), (int)other.sprite.Height());

            return bounds.Intersects(otherBounds);
        }
    }
}
