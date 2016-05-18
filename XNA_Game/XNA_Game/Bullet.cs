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
    class Bullet : GameObject {

        Vector2 speed;
        int liveTime;

        float totalTime;

        public override bool IsAlive { get; set; }
        public int Damage { get; }
        public bool DamagesPlayer { get; }

        public Bullet(string spriteName, Vector2 position, Vector2 speed, int liveTime, int damage, bool damagesPlayer=false) : base(spriteName, position) {
            this.liveTime = liveTime;
            this.speed = speed;
            this.totalTime = 0;
            this.IsAlive = true;
            this.Damage = damage;
            this.DamagesPlayer = damagesPlayer;
        }

        public void Update(GameTime gameTime) {
            totalTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            IsAlive &= totalTime < liveTime;
            sprite.Move(speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            base.Update(gameTime);
        }
    }
}
