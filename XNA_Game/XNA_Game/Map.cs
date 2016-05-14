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

        Vector2 drawDirection;

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

        private bool InBounds(int dirX, int dirY) {
            return (dirX > -1 && dirX < mapMask.GetLength(0)) &&
                   (dirY > -1 && dirY < mapMask.GetLength(1));
        }

        public bool Update(GameTime gameTime, Vector2 direction) {
            // mapMask[2, 2].Update(gameTime);
            Vector2 newPos = mainCharacter.Position() + direction;

            var directionX = new double[] { 0.0f, mainCharacter.Width(), mainCharacter.Width(), 0.0f };
            var directionY = new double[] { 0.0f, mainCharacter.Height(), 0.0f , mainCharacter.Height() };

            for (int i = 0; i < directionX.Count(); i++) {
                var offsetX = (int)((newPos.X + directionX[i]) / cellSize.X);
                var offsetY = (int)((newPos.Y + directionY[i]) / cellSize.Y);
                if (!InBounds(offsetX, offsetY) || mapMask[offsetX, offsetY] != null) {
                    return false;
                }
            }

            mainCharacter.Move(direction);

            drawDirection = direction;

            mainCharacter.Update(gameTime);
            return true;
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (drawDirection.X > 0) {
                mainCharacter.StartOrContinueAnimation(2, 0);
            } else if (drawDirection.X < 0) {
                mainCharacter.StartOrContinueAnimation(1, 0);
            } else if (drawDirection.Y < 0) {
                mainCharacter.StartOrContinueAnimation(3, 0);
            } else if (drawDirection.Y > 0) {
                mainCharacter.StartOrContinueAnimation(0, 0);
            } else {
                mainCharacter.StopAnimation();
            }

            mapMask[2, 2].Draw(spriteBatch);
            mainCharacter.Draw(spriteBatch);
        }
    }
}
