using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace atestat
{
    public partial class Game : Form
    {
        Shape currentShape;
        Shape nextShape;

        public Form BackToMainForm { get; set; }
        public Game()
        {
            this.KeyPreview = true;
            InitializeComponent();
            loadCanvas();

            currentShape = getRandomShapeWithCenterAligned();
            nextShape = getNextShape();

            timer.Tick += Timer_Tick;
            timer.Interval = 500;
            timer.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Stop();
            string message = "Why do u want to stop?????";
            string title = "End Game";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                this.BackToMainForm.Visible = true;
                this.Close();
            }
            else
            {
                this.Visible = true;
                timer.Start();
            }
        }
        Bitmap canvasBitmap;
        Graphics canvasGraphics;
        int canvasWidth = 15;
        int canvasHeight = 20;
        int[,] canvasDotArray;
        Brush[,] canvasBrushArray;
        int dotSize = 20;
        Pen gridColor = new Pen(Color.Black);
        bool pressing_down;
        private void loadCanvas()
        {
            // Resize the picture box based on the dotsize and canvas size
            pictureBox1.Width = canvasWidth * dotSize;
            pictureBox1.Height = canvasHeight * dotSize;

            // Create Bitmap with picture box's size
            canvasBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            canvasGraphics = Graphics.FromImage(canvasBitmap);

            canvasGraphics.FillRectangle(Brushes.LightGray, 0, 0, canvasBitmap.Width, canvasBitmap.Height);

            #region Draw Grid
            for (int i = 0; i < canvasHeight; i++)
            {
                canvasGraphics.DrawLine(gridColor, i * dotSize, 0, i * dotSize, canvasHeight * dotSize);
            }

            for (int i = 0; i < canvasHeight; i++)
            {
                canvasGraphics.DrawLine(gridColor, 0, i * dotSize, canvasHeight * dotSize, i * dotSize);
            }
            #endregion

            // Load bitmap into picture box
            pictureBox1.Image = canvasBitmap;

            // Initialize canvas dot array. by default all elements are zero
            canvasDotArray = new int[canvasWidth, canvasHeight];
            canvasBrushArray = new Brush[canvasWidth, canvasHeight];
        }
        class Shape
        {
            public int Width;
            public int Height;
            public int[,] Dots;
            public Brush Color;

            private int[,] backupDots;
            public void turn()
            {
                // back the dots values into backup dots
                // so that it can be simply used for rolling back
                backupDots = Dots;

                Dots = new int[Width, Height];
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        Dots[i, j] = backupDots[Height - 1 - j, i];
                    }
                }

                var temp = Width;
                Width = Height;
                Height = temp;
            }

            // the rolling back occures when player rotating the shape
            // but it will touch other shapes and needs to be rolled back
            public void rollback()
            {
                Dots = backupDots;

                var temp = Width;
                Width = Height;
                Height = temp;
            }
        }
        static class ShapesHandler
        {
            private static Shape[] shapesArray;

            // static constructor : No need to manually initialize
            static ShapesHandler()
            {
                // Create shapes add into the array.
                shapesArray = new Shape[]
                    {
                    new Shape {
                        Width = 2,
                        Height = 2,
                        Dots = new int[,]
                        {
                            { 1, 1 },
                            { 1, 1 }
                        },
                        Color=Brushes.Yellow
                    },
                    new Shape {
                        Width = 1,
                        Height = 4,
                        Dots = new int[,]
                        {
                            { 1 },
                            { 1 },
                            { 1 },
                            { 1 }
                        },
                        Color=Brushes.Blue
                    },
                    new Shape {
                        Width = 3,
                        Height = 2,
                        Dots = new int[,]
                        {
                            { 0, 1, 0 },
                            { 1, 1, 1 }
                        },
                        Color=Brushes.Purple
                    },
                    new Shape {
                        Width = 3,
                        Height = 2,
                        Dots = new int[,]
                        {
                            { 0, 0, 1 },
                            { 1, 1, 1 }
                        },
                        Color=Brushes.DeepPink
                    },
                    new Shape {
                        Width = 3,
                        Height = 2,
                        Dots = new int[,]
                        {
                            { 1, 0, 0 },
                            { 1, 1, 1 }
                        },
                        Color=Brushes.Orange
                    },
                    new Shape {
                        Width = 3,
                        Height = 2,
                        Dots = new int[,]
                        {
                            { 1, 1, 0 },
                            { 0, 1, 1 }
                        },
                        Color=Brushes.Green
                    },
                    new Shape {
                        Width = 3,
                        Height = 2,
                        Dots = new int[,]
                        {
                            { 0, 1, 1 },
                            { 1, 1, 0 }
                        },
                        Color=Brushes.Red
                    }
                    };
            }

            // Get a shape form the array in a random basis
            public static Shape GetRandomShape()
            {
                var shape = shapesArray[new Random().Next(shapesArray.Length)];

                return shape;
            }
        }
        int currentX;
        int currentY;
        private Shape getRandomShapeWithCenterAligned()
        {
            var shape = ShapesHandler.GetRandomShape();

            // Calculate the x and y values as if the shape lies in the center
            currentX = 7;
            currentY = -shape.Height;

            return shape;
        }

        Bitmap workingBitmap;
        Graphics workingGraphics;
        private void Timer_Tick(object sender, EventArgs e)
        {
            var isMoveSuccess = moveShapeIfPossible(moveDown: 1);

            // if shape reached bottom or touched any other shapes
            if (!isMoveSuccess)
            {
                // copy working image into canvas image
                canvasBitmap = new Bitmap(workingBitmap);

                updateCanvasDotArrayWithCurrentShape();

                // get next shape
                currentShape = nextShape;
                nextShape = getNextShape();

                clearFilledRowsAndUpdateScore();
            }
            if (score / 1000 >= 10)
            {
                timer.Stop();
                string message ="Damn, you must be a god...";
                string title = "Bet you can't win again XDD. Wanna try??";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {
                    Game game = new Game();
                    game.BackToMainForm = BackToMainForm;
                    game.Show();
                    this.Close();
                }
                else
                {
                    this.BackToMainForm.Visible = true;
                    this.Close();
                }
            }
        }

        private void updateCanvasDotArrayWithCurrentShape()
        {
            for (int i = 0; i < currentShape.Width; i++)
            {
                for (int j = 0; j < currentShape.Height; j++)
                {
                    if (currentShape.Dots[j, i] == 1)
                    {
                        if (checkIfGameOver()) return;

                        canvasDotArray[currentX + i, currentY + j] = 1;
                        canvasBrushArray[currentX + i, currentY + j] = currentShape.Color;
                    }
                }
            }
        }

        private bool checkIfGameOver()
        {
            if (currentY < 0)
            {
                timer.Stop();
                MessageBox.Show("U fucked up, lol");
                Application.Restart();
                return true;
            }

            return false;
        }

        // returns if it reaches the bottom or touches any other blocks
        private bool moveShapeIfPossible(int moveDown = 0, int moveSide = 0)
        {
            var newX = currentX + moveSide;
            var newY = currentY + moveDown;

            // check if it reaches the bottom or side bar
            if (newX < 0 || newX + currentShape.Width > canvasWidth
                || newY + currentShape.Height > canvasHeight)
                return false;

            // check if it touches any other blocks 
            for (int i = 0; i < currentShape.Width; i++)
            {
                for (int j = 0; j < currentShape.Height; j++)
                {
                    if (newY + j > 0 && canvasDotArray[newX + i, newY + j] == 1 && currentShape.Dots[j, i] == 1)
                        return false;
                }
            }

            currentX = newX;
            currentY = newY;

            drawShape();
            if (pressing_down)
            {
                score = score + 5;
                label1.Text = "Score: " + score;
                label2.Text = "Level: " + score / 1000;
            }
            return true;
        }

        private void drawShape()
        {
            workingBitmap = new Bitmap(canvasBitmap);
            workingGraphics = Graphics.FromImage(workingBitmap);

            for (int i = 0; i < currentShape.Width; i++)
            {
                for (int j = 0; j < currentShape.Height; j++)
                {
                    if (currentShape.Dots[j, i] == 1)
                        workingGraphics.FillRectangle(currentShape.Color, (currentX + i) * dotSize, (currentY + j) * dotSize, dotSize, dotSize);
                }
            }

            #region Draw Grid
            for (int i = 0; i < canvasHeight; i++)
            {
                workingGraphics.DrawLine(gridColor, i * dotSize, 0, i * dotSize, canvasHeight * dotSize);
            }

            for (int i = 0; i < canvasHeight; i++)
            {
                workingGraphics.DrawLine(gridColor, 0, i * dotSize, canvasHeight * dotSize, i * dotSize);
            }
            #endregion

            pictureBox1.Image = workingBitmap;
        }

        // Used for key inputs
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var verticalMove = 0;
            var horizontalMove = 0;
            // calculate the vertical and horizontal move values
            // based on the key pressed
            switch (keyData)
            {
                case Keys.Escape:
                    timer.Stop();
                    string message = "I hope you'll start the game soon";
                    string title = "Need a break?";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, buttons);
                    if (result == DialogResult.Yes)
                    {
                        this.BackToMainForm.Visible = true;
                        this.Close();
                    }
                    else
                    {
                        this.Visible = true;
                        timer.Start();
                    }
                    break;

                // move shape left
                case Keys.Left:
                    verticalMove--;
                    break;

                // move shape right
                case Keys.Right:
                    verticalMove++;
                    break;

                // move shape down faster
                case Keys.Down:
                    pressing_down=true;
                    horizontalMove++;
                    break;

                // rotate the shape clockwise
                case Keys.Up:
                    currentShape.turn();
                    break;
                default:
                    break;
            }

            var isMoveSuccess = moveShapeIfPossible(horizontalMove, verticalMove);

            // if the player was trying to rotate the shape, but
            // that move was not possible - rollback the shape
            if (!isMoveSuccess && keyData == Keys.Up)
                currentShape.rollback();

            return base.ProcessCmdKey(ref msg, keyData);
        }

        int score;
        public void clearFilledRowsAndUpdateScore()
        {
            // check through each rows
            for (int i = 0; i < canvasHeight; i++)
            {
                int j;
                for (j = canvasWidth - 1; j >= 0; j--)
                {
                    if (canvasDotArray[j, i] == 0)
                        break;
                }

                if (j == -1)
                {
                    // update score and level values and labels
                    score = score + 100; 
                    label1.Text = "Score: " + score;
                    label2.Text = "Level: " + score / 1000;
                    // increase the speed 
                    timer.Interval -= 10;

                    // update the dot array based on the check
                    for (j = 0; j < canvasWidth; j++)
                    {
                        for (int k = i; k > 0; k--)
                        {
                            canvasDotArray[j, k] = canvasDotArray[j, k - 1];
                            canvasBrushArray[j, k] = canvasBrushArray[j, k - 1];
                        }

                        canvasDotArray[j, 0] = 0;
                        canvasBrushArray[j, 0] = Brushes.LightGray;
                    }
                }
            }

            // Draw panel based on the updated array values
            for (int i = 0; i < canvasWidth; i++)
            {
                for (int j = 0; j < canvasHeight; j++)
                {
                    canvasGraphics = Graphics.FromImage(canvasBitmap);
                    canvasGraphics.FillRectangle(
                        canvasDotArray[i, j] == 1 ? canvasBrushArray[i, j] : Brushes.LightGray,
                        i * dotSize, j * dotSize, dotSize, dotSize);
                }
            }

            #region Draw Grid
            for (int i = 0; i < canvasHeight; i++)
            {
                canvasGraphics.DrawLine(gridColor, i * dotSize, 0, i * dotSize, canvasHeight * dotSize);
            }

            for (int i = 0; i < canvasHeight; i++)
            {
                canvasGraphics.DrawLine(gridColor, 0, i * dotSize, canvasHeight * dotSize, i * dotSize);
            }
            #endregion

            pictureBox1.Image = canvasBitmap;
        }

        Bitmap nextShapeBitmap;
        Graphics nextShapeGraphics;
        private Shape getNextShape()
        {
            var shape = getRandomShapeWithCenterAligned();

            // Codes to show the next shape in the side panel
            nextShapeBitmap = new Bitmap(6 * dotSize, 6 * dotSize);
            nextShapeGraphics = Graphics.FromImage(nextShapeBitmap);

            nextShapeGraphics.FillRectangle(Brushes.LightGray, 0, 0, nextShapeBitmap.Width, nextShapeBitmap.Height);

            // Find the ideal position for the shape in the side panel
            var startX = (6 - shape.Width) / 2;
            var startY = (6 - shape.Height) / 2;

            for (int i = 0; i < shape.Height; i++)
            {
                for (int j = 0; j < shape.Width; j++)
                {
                    nextShapeGraphics.FillRectangle(
                        shape.Dots[i, j] == 1 ? shape.Color : Brushes.LightGray,
                        (startX + j) * dotSize, (startY + i) * dotSize, dotSize, dotSize);
                }
            }

            pictureBox2.Size = nextShapeBitmap.Size;
            pictureBox2.Image = nextShapeBitmap;

            return shape;
        }

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down)
            {
                pressing_down = false;
            }
        }
    }
}
