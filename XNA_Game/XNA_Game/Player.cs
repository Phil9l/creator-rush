using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XNA_Game {
    class Player : GameObject {

        Vector2 currentPlayerDirection;
        Vector2 drawDirection;

        public override bool IsAlive { get { return HP > 0; } set { } }
        public int Damage { get; private set; }
        public int HP { get; set; }

        public Player(string spriteName, Vector2 position, int HP) : base(spriteName, position, 4, 4) {
            this.HP = HP;
        }

        public Bullet Shoot(int liveTime, int damage) {
            return new Bullet("Bullet", sprite.Position(), currentPlayerDirection, liveTime, damage);
        }

        public Vector2 Position() {
            return sprite.Position();
        }

        public bool Update(GameTime gameTime, Vector2 direction) {
            Vector2 newPos = sprite.Position() + direction;

            if (direction != Vector2.Zero) {
                currentPlayerDirection = direction;
            }

            var directionX = new double[] { 0.0f, sprite.Width(), sprite.Width(), 0.0f };
            var directionY = new double[] { 0.0f, sprite.Height(), 0.0f, sprite.Height() };
            var directions = new Vector2[directionX.Length];
            for (var i = 0; i < directionX.Length; i++) {
                directions[i] = new Vector2((float)directionX[i], (float)directionY[i]);
            }


            for (int i = 0; i < directionX.Count(); i++) {
                var position = newPos + directions[i];
                if (!Map.IsCellFree(position)) {
                    return false;
                }
            }
            drawDirection = direction;
            sprite.Move(direction);
            base.Update(gameTime);

            return true;
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (drawDirection.X > 0) {
                sprite.StartOrContinueAnimation(2, 0);
            } else if (drawDirection.X < 0) {
                sprite.StartOrContinueAnimation(1, 0);
            } else if (drawDirection.Y < 0) {
                sprite.StartOrContinueAnimation(3, 0);
            } else if (drawDirection.Y > 0) {
                sprite.StartOrContinueAnimation(0, 0);
            } else {
                sprite.StopAnimation();
            }

            base.Draw(spriteBatch);
        }
    }
}
