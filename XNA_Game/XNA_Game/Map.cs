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
        public static Vector2 cellSize;
        public static Vector2 mapSize;
        public static Vector2 globalSize;

        public static Sprite mainCharacter;
        public static GameObject[,] mapMask;

        List<Bullet> bullets;
        List<Enemy> enemies;
        Vector2 drawDirection;
        Vector2 currentPlayerDirection;

        ContentManager content;

        public Map(Vector2 cellSize, Vector2 mapSize) {
            Map.cellSize = cellSize;
            Map.mapSize = mapSize;
            Map.globalSize = new Vector2(cellSize.X * mapSize.X, cellSize.Y * mapSize.Y);

            mapMask = new GameObject[(int)mapSize.X, (int)mapSize.Y];
            enemies = new List<Enemy>();
            bullets = new List<Bullet>();
        }

        public void LoadContent(ContentManager Content) {
            content = Content;

            var enemy1 = new Enemy("Enemy", new Vector2(cellSize.X * 4, cellSize.Y * 6), 500);
            enemy1.LoadContent(Content);

            var enemy2 = new Enemy("Enemy", new Vector2(cellSize.X * 8, cellSize.Y * 4), 300);
            enemy2.LoadContent(Content);

            enemies.Add(enemy1);
            enemies.Add(enemy2);
            
            var box = new GameObject("Box", new Vector2(cellSize.X * 4, cellSize.Y * 5));
            box.LoadContent(Content);
            mapMask[4, 5] = box;

            mainCharacter = new Sprite("MainCharacter", new Vector2(300, 200), 4, 4);
            mainCharacter.LoadContent(Content);
        }

        private void CreateObjectOnMap(int posX, int posY, GameObject obj) {

        }

        public void UnloadContent() {
        }

        public void Shoot() {
            var bullet = new Bullet("Bullet", mainCharacter.Position(), currentPlayerDirection, 5000);
            bullet.LoadContent(content);
            bullets.Add(bullet);
        }

        private bool InBounds(int dirX, int dirY) {
            return (dirX > 0 && dirX < mapMask.GetLength(0)) &&
                   (dirY > 0 && dirY < mapMask.GetLength(1));
        }

        public void DeleteDeadBullets() {
            for (var i = bullets.Count - 1; i >= 0; i--) {
                if (!bullets[i].IsAlive) {
                    bullets.RemoveAt(i);
                }
            }
        }

        public bool Update(GameTime gameTime, Vector2 direction) {
            // mapMask[2, 2].Update(gameTime);
            DeleteDeadBullets();
            Vector2 newPos = mainCharacter.Position() + direction;

            if (direction != Vector2.Zero) {
                currentPlayerDirection = direction;
            }

            var directionX = new double[] { 0.0f, mainCharacter.Width(), mainCharacter.Width(), 0.0f };
            var directionY = new double[] { 0.0f, mainCharacter.Height(), 0.0f , mainCharacter.Height() };

            foreach (var enemy in enemies) {
                enemy.Update(gameTime);

                foreach (var bullet in bullets) {
                    bullet.IsAlive &= !enemy.IsIntersected(bullet);
                }
            }
            foreach (var bullet in bullets) {
                bullet.Update(gameTime);
            }

            mapMask[4, 5].Update(gameTime);

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

            foreach (var enemy in enemies) {
                enemy.Draw(spriteBatch);
            }
            foreach (var bullet in bullets) {
                bullet.Draw(spriteBatch);
            }
            mapMask[4, 5].Draw(spriteBatch);
            mainCharacter.Draw(spriteBatch);
        }
    }
}
