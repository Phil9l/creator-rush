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

        Vector2 nextGoingCell;
        Vector2 direction;
        bool inTransit = false;
        int rageDistance;

        public int HP { get; set; }
        public override bool IsAlive { get { return HP > 0; } set { } }

        //queue for bfs
        Queue<Vector2> bfsQueue;

        public Enemy(string spriteName, Vector2 position, int HP, int rageDistance) : base(spriteName, position) {
            this.spriteName = spriteName;
            this.position = position;
            this.rageDistance = rageDistance;
            this.HP = HP;
            
            dirCellX = new float[] { 0, Map.cellSize.X, 0, -Map.cellSize.X };
            dirCellY = new float[] { Map.cellSize.Y, 0, -Map.cellSize.Y, 0 };
            
            directionX = new int[] { 0, 1, 1, 0, -1, -1, -1 };
            directionY = new int[] { 1, 0, -1, -1, -1, 0, 1 };

            bfsQueue = new Queue<Vector2>();
        }

        public Bullet Shoot(int liveTime, int damage) {
            return new Bullet("Bullet", sprite.Position(), /* direction */ new Vector2(10, 0), liveTime, damage);
        }

        public bool Update(GameTime gameTime) {
            var curCellPos = GlobalToCellCoord(sprite.Position());

            if (Vector2.Distance(sprite.Position(), Map.player.Position()) < rageDistance) {
                if (!inTransit) {
                    var nextStep = FindMainCharacter(Map.player.Position());
                    direction = CellCoordToDirection(nextStep);
                    nextGoingCell = nextStep;
                    inTransit = true;
                } else {
                    if (sprite.Position() + Map.cellSize / 2 != CellCoordToGlobal(nextGoingCell)) {
                        sprite.Move(direction);
                    } else {
                        inTransit = false;
                    }
                }
            }
            sprite.Update(gameTime);
            return true;
        }

        private Vector2 FindMainCharacter(Vector2 mainCharacterPos) {
            var vectorParent = new Dictionary<Vector2, Vector2>();
            var mapCellSize = Map.cellSize;
            bool[,] used = new bool[(int)Map.globalSize.X, (int)Map.globalSize.Y];
            bool finded = false;

            var spritePosition = GlobalToCellCoord(sprite.Position());
            var mcCellPos = GlobalToCellCoord(mainCharacterPos);

            bfsQueue.Enqueue(GlobalToCellCoord(sprite.Position()));

            while (bfsQueue.Count != 0) {
                var current = bfsQueue.Peek();

                if (finded || current == mcCellPos) break;

                bfsQueue.Dequeue();
                used[(int)current.X, (int)current.Y] = true;

                for (int i = 0; i < directionX.Count(); i++) {
                    var newPos = current + new Vector2(directionX[i], directionY[i]);
                    if (InBounds(newPos) && !used[(int)newPos.X, (int)newPos.Y] && Map.mapMask[(int)newPos.X, (int)newPos.Y] == null) {
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

            var nextCell = FindGoingCell(vectorParent, mcCellPos, spritePosition);

            /*for (int i = 0; i < directionX.Count(); i++) {
                var offset = new Vector2(directionX[i], directionY[i]);
                if (nextCell == spritePosition + offset) {
                    return offset;
                }
            }*/
            return nextCell;
            //return new Vector2(0, 0);
        }

        private Vector2 FindGoingCell(Dictionary<Vector2, Vector2> dir, Vector2 start, Vector2 end) {
            var prev = start;
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
            return new Vector2((int)((pos.X + Map.cellSize.X / 2) / Map.cellSize.X), (int)((pos.Y + Map.cellSize.Y / 2 )/ Map.cellSize.Y));
        }

        private Vector2 CellCoordToGlobal(Vector2 pos) {
            return new Vector2((int)(pos.X * Map.cellSize.X + Map.cellSize.X / 2), (pos.Y * Map.cellSize.Y + Map.cellSize.Y / 2));
        }

        private Vector2 CellCoordToDirection(Vector2 pos) {
            for (int i = 0; i < directionX.Count(); i++) {
                var offset = new Vector2(directionX[i], directionY[i]);
                if (pos == GlobalToCellCoord(sprite.Position()) + offset) {
                    return offset;
                }
            }
            return new Vector2(0, 0);
        }
    }
}
