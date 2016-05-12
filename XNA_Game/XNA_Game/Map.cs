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
    class Map {
        Sprite mainCharacter;

        Vector2 cellSize;
        Vector2 mapSize;

        GameObject[,] mapMask;

        public Map(Vector2 cellSize, Vector2 mapSize) {
            this.cellSize = cellSize;
            this.mapSize = mapSize;

            mapMask = new GameObject[(int)mapSize.X, (int)mapSize.Y];
        }

        public void LoadContent(ContentManager Content) {
            mapMask[2, 2] = new GameObject("Box", new Vector2(128, 128));
            mapMask[2, 2].LoadContent(Content);

            mainCharacter = new Sprite("MainCharacter", new Vector2(300, 300), 4, 4);
            mainCharacter.LoadContent(Content);
        }

        public void UnloadContent() {
        }

        public bool Update(GameTime gameTime, Vector2 direction) {
            // mapMask[2, 2].Update(gameTime);
            Vector2 newPos = mainCharacter.Position() + direction;

            if (mapMask[(int)(newPos.X / cellSize.X), (int)(newPos.Y / cellSize.Y)] != null ||
                mapMask[(int)(newPos.X / cellSize.X), (int)((newPos.Y + mainCharacter.Height()) / cellSize.Y)] != null ||
                mapMask[(int)((newPos.X + mainCharacter.Width()) / cellSize.X), (int)(newPos.Y / cellSize.Y)] != null ||
                mapMask[(int)((newPos.X + mainCharacter.Width()) / cellSize.X), (int)((newPos.Y + mainCharacter.Height()) / cellSize.Y)] != null
            ) {
                return false;
            }
            
            mainCharacter.Move(direction);

            if (direction.X > 0) {
                mainCharacter.StartOrContinueAnimation(2, 0);
            } else if (direction.X < 0) {
                mainCharacter.StartOrContinueAnimation(1, 0);
            } else if (direction.Y < 0) {
                mainCharacter.StartOrContinueAnimation(3, 0);
            } else if (direction.Y > 0) {
                mainCharacter.StartOrContinueAnimation(0, 0);
            } else {
                mainCharacter.StopAnimation();
            }

            mainCharacter.Update(gameTime);
            return true;
        }

        public void Draw(SpriteBatch spriteBatch) {
            mapMask[2, 2].Draw(spriteBatch);
            mainCharacter.Draw(spriteBatch);
        }
    }
}
