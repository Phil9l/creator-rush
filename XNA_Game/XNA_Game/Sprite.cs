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
    class Sprite {
        Texture2D spriteSheet;
        Vector2 position;

        string name;
        int rowsNumber;
        int columnsNumber;
        int updateFrequency;

        int currentRow;
        int currentColumn;

        int timeSinceLastUpdate;

        bool animation;

        public Sprite(string name, Vector2 position, int rowsNumber = 1, int columnsNumber = 1, int updateFrequency = 100) {
            this.name = name;
            this.position = position;
            this.rowsNumber = rowsNumber;
            this.columnsNumber = columnsNumber;
            this.updateFrequency = updateFrequency;
            this.animation = false;

            this.currentRow = 0;
            this.currentColumn = 0;

            this.timeSinceLastUpdate = 0;
        }

        public void LoadContent(ContentManager Content) {
            this.spriteSheet = Content.Load <Texture2D>(this.name);
        }

        public void Update(GameTime gameTime) {
            if (animation) {
                timeSinceLastUpdate += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastUpdate > this.updateFrequency) {
                    this.currentColumn = (this.currentColumn + 1) % columnsNumber;
                    this.timeSinceLastUpdate = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(
                this.spriteSheet, this.position, 
                new Rectangle(
                    currentColumn * spriteSheet.Width / columnsNumber,
                    currentRow * spriteSheet.Height / rowsNumber, 
                    spriteSheet.Width / columnsNumber,
                    spriteSheet.Height / rowsNumber
                ), 
                Color.White
            );
        }

        public void Move(Vector2 direction) {
            position += direction;
        }

        public void StartAnimation(int row = 0, int column = 0) {
            this.currentRow = row;
            this.currentColumn = column;
            animation = true;
        }

        public void StartOrContinueAnimation(int row = 0, int column = 0) {
            if (!animation || row != this.currentRow) {
                StartAnimation(row, column);
            }
        }

        public void StopAnimation() {
            animation = false;
        }

        public void StopAnimation(int row = 0, int column = 0) {
            this.currentRow = row;
            this.currentColumn = column;
            animation = true;
        }

        public Vector2 Position() {
            return position;
        }

        public double Width() {
            return spriteSheet.Width / columnsNumber;
        }

        public double Height() {
            return spriteSheet.Height / rowsNumber;
        }
    }
}
