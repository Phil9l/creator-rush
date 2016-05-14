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
    class Enemy : GameObject {
        double angel;

        // directions for cells
        float[] dirCellX;
        float[] dirCellY;

        //directions for moving
        int[] directionX;
        int[] directionY;

        //queue for bfs
        Queue<Vector2> bfsQueue;

        public Enemy(string spriteName, Vector2 position) {
            this.spriteName = spriteName;
            this.position = position;
            
            dirCellX = new float[] { 0, Map.cellSize.X, 0, -Map.cellSize.X };
            dirCellY = new float[] { Map.cellSize.Y, 0, -Map.cellSize.Y, 0 };
            
            directionX = new int[] { 0, 1, 0, -1 };
            directionY = new int[] { 1, 0, -1, 0 };

            bfsQueue = new Queue<Vector2>();
        }

        public void LoadContent(ContentManager content) {
            sprite = new Sprite(spriteName, position);
            sprite.LoadContent(content);
        }

        public void Update(GameTime gameTime) {
            var nextStep = FindMainCharacter(Map.mainCharacter.Position());
            sprite.Move(nextStep);
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch) {
            sprite.Draw(spriteBatch);
        }

        private Vector2 FindMainCharacter(Vector2 mainCharacterPos) {
            var vectorParent = new Dictionary<Vector2, Vector2>();
            var mapCellSize = Map.cellSize;
            bool[,] used = new bool[(int)Map.globalSize.X, (int)Map.globalSize.Y];
            bool finded = false;

            var mcCellPos = GlobalToCellCoord(mainCharacterPos);

            bfsQueue.Enqueue(GlobalToCellCoord(sprite.Position()));

            while (bfsQueue.Count != 0) {
                var current = bfsQueue.Peek();

                if (finded) break;

                bfsQueue.Dequeue();
                used[(int)current.X, (int)current.Y] = true;

                for (int i = 0; i < dirCellX.Count(); i++) {
                    var newPos = current + new Vector2(directionX[i], directionY[i]);
                    if (InBounds(newPos) && !used[(int)newPos.X, (int)newPos.Y]) {
                        used[(int)newPos.X, (int)newPos.Y] = true;
                        bfsQueue.Enqueue(newPos);
                        vectorParent[newPos] = current;
                        if (newPos == mcCellPos) {
                            finded = true;
                            break;
                        }
                    }
                }
            }
            bfsQueue.Clear();

            var nextCell = FindGoingCell(vectorParent, mcCellPos, GlobalToCellCoord(sprite.Position()));

            for (int i = 0; i < 4; i++) {
                var offset = new Vector2(directionX[i], directionY[i]);
                if (nextCell == GlobalToCellCoord(sprite.Position()) + offset) {
                    return offset;
                }
            }
            return new Vector2(0, 0);
        }

        private Vector2 FindGoingCell(Dictionary<Vector2, Vector2> dir, Vector2 start, Vector2 end) {
            var prev = new Vector2();
            while (start != end) {
               prev = start;
               start = dir[start];
            }
            return prev;
        }

        private bool InBounds(Vector2 pos) {
            return (pos.X > 0 && pos.X < Map.mapSize.X) && (pos.Y > 0 && pos.Y < Map.mapSize.Y);
        }

        private Vector2 GlobalToCellCoord(Vector2 pos) {
            return new Vector2((int)(pos.X / Map.cellSize.X), (int)(pos.Y / Map.cellSize.Y));
        }
    }
}
