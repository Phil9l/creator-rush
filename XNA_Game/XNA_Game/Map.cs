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
    class Edge {
        public Vector2 st;
        public Vector2 fn;

        public Edge(Vector2 s, Vector2 f) {
            st = s;
            fn = f;
        }
    }

    class Map {
        public static Vector2 cellSize;
        public static Vector2 mapSize;
        public static Vector2 globalSize;

        public static GameObject[,] mapMask;

        public static List<Bullet> bullets;
        List<Enemy> enemies;
        
        public static Player player { get; set; }

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

            player = new Player("MainCharacter", new Vector2(300, 200), 100);
            player.LoadContent(Content);

            var enemy1 = new Enemy("Enemy", new Vector2(cellSize.X * 4, cellSize.Y * 6), 100, 500, 250, 150, this);
            enemy1.LoadContent(Content);

            var enemy2 = new Enemy("Enemy", new Vector2(cellSize.X * 8, cellSize.Y * 4), 50, 300, 250, 150, this);
            enemy2.LoadContent(Content);

            enemies.Add(enemy1);
            enemies.Add(enemy2);
            
            var box = new StaticOject("Box", new Vector2(cellSize.X * 4, cellSize.Y * 5));
            box.LoadContent(Content);
            mapMask[4, 5] = box;

            //mainCharacter = new Sprite("MainCharacter", new Vector2(300, 200), 4, 4);
            //mainCharacter.LoadContent(Content);
        }

        private void CreateObjectOnMap(int posX, int posY, GameObject obj) {

        }

        public void UnloadContent() {
        }

        public void PlayerShoot() {
            var playerBullet = player.Shoot(5000, 20);
            playerBullet.LoadContent(content);
            bullets.Add(playerBullet);
        }

        public void EnemyShoot(Enemy enemy) {
            var bullet = enemy.Shoot(5000, 20);
            bullet.LoadContent(content);
            bullets.Add(bullet);
        }

        static bool InBounds(int dirX, int dirY) {
            return (dirX > 0 && dirX < mapMask.GetLength(0)) &&
                   (dirY > 0 && dirY < mapMask.GetLength(1));
        }

        public static bool IsCellFree(Vector2 position) {
            var dirX = (int)(position.X / cellSize.X);
            var dirY = (int)(position.Y / cellSize.Y);

            if (!InBounds(dirX, dirY) || mapMask[dirX, dirY] != null) {
                return false;
            }
            return true;
        }

        public void DeleteDeadObjects() {

            // TODO: pass array as argument

            for (var i = bullets.Count - 1; i >= 0; i--) {
                if (!bullets[i].IsAlive) {
                    bullets.RemoveAt(i);
                }
            }
            for (var i = enemies.Count - 1; i >= 0; i--) {
                if (!enemies[i].IsAlive) {
                    enemies.RemoveAt(i);
                }
            }
        }

        public bool Update(GameTime gameTime, Vector2 direction) {
            // mapMask[2, 2].Update(gameTime);
            DeleteDeadObjects();
      
            foreach (var enemy in enemies) {
                enemy.Update(gameTime);
            }
            foreach (var bullet in bullets) {
                bullet.Update(gameTime);

                if (bullet.DamagesPlayer) {
                    if (player.IsIntersected(bullet)) {
                        player.HP -= bullet.Damage;
                        bullet.IsAlive = false;
                    }
                    continue;
                }
                foreach (var enemy in enemies) {
                    if (enemy.IsIntersected(bullet)) {
                        enemy.HP -= bullet.Damage;
                        bullet.IsAlive = false;
                    }
                }
            }

            mapMask[4, 5].Update(gameTime);


            return player.Update(gameTime, direction);
        }

        public void Draw(SpriteBatch spriteBatch) {
            player.Draw(spriteBatch);
            foreach (var enemy in enemies) {
                enemy.Draw(spriteBatch);
            }
            foreach (var bullet in bullets) {
                bullet.Draw(spriteBatch);
            }
            mapMask[4, 5].Draw(spriteBatch);
        }

        public static bool IsWayClear(Vector2 start, Vector2 finish) {
            var edge = new Edge(start + cellSize / 2, finish);
            for (int i = 0; i < mapSize.X; i++) {
                for (int j = 0; j < mapSize.Y; j++) {
                    if (IsSquareCrossed(edge, new Vector2(i, j)) && mapMask[i, j] != null) {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool IsSquareCrossed(Edge curEdge, Vector2 squareIndex) {
            Vector2 p = squareIndex * cellSize;
            var edgeList = new List<Edge>();
            edgeList.Add(new Edge(p, new Vector2(p.X, p.Y + cellSize.Y)));
            edgeList.Add(new Edge(p, new Vector2(p.X + cellSize.X, p.Y)));
            edgeList.Add(new Edge(new Vector2(p.X + cellSize.X, p.Y), p + cellSize));
            edgeList.Add(new Edge(new Vector2(p.X, p.Y + cellSize.Y), p + cellSize));

            foreach (var edge in edgeList) {
                if (IsSegmentCrossed(curEdge, edge)) {
                    return true;
                }
            }
            return false;
        }

        private static bool IsSegmentCrossed(Edge e1, Edge e2) {
            if (CrossProduct(e2.st - e1.st, e2.fn - e2.st) * CrossProduct(e2.st - e1.fn, e2.fn - e1.fn) <= 0
                && CrossProduct(e1.st - e2.st, e1.fn - e2.st) * CrossProduct(e1.st - e2.fn, e1.fn - e2.fn) <= 0) {
                return true;
            }
            return false;
        }

        private static double CrossProduct(Vector2 s1, Vector2 s2) {
            return s1.X * s2.Y - s1.Y * s2.X;
        }
    }
}
