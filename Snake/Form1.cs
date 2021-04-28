using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams CP = base.CreateParams;
                CP.ExStyle = CP.ExStyle | 0x02000000; // WS_EX_COMPOSITED
                return CP;
            }
        }
        bool[,] gameVecs;
        List<Vector2> snake = new List<Vector2>();
        Vector2 apple;
        private Color SnakeColor;
        private bool loose = false;
        private bool pause = false;
        int dir = 0;
        int actualDir = 0;

        Vector2 gameSize = new Vector2(22, 11);

        public Form1()
        {
            InitializeComponent();
            SnakeColor = Color.FromArgb(56, 182, 255);
            gameVecs = new bool[gameSize.x, gameSize.y];
            Restart();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Updater()
        {
            if (loose) Restart();
            Movement();
        }

        private void Movement()
        {
            if (pause) return;

            actualDir = dir;

            for (int i = snake.Count - 1; i >= 0; i--)
            {
                if (i != 0)
                {
                    if(snake[i] != snake[i - 1])
                    {
                        snake[i].Set(snake[i - 1]);
                    }
                }
                else
                {

                    if (dir == 0)
                        snake[0].Set(new Vector2(snake[i].x - 1, snake[i].y));
                    else if (dir == 1)
                        snake[0].Set(new Vector2(snake[i].x, snake[i].y - 1));
                    else if (dir == 2)
                        snake[0].Set(new Vector2(snake[i].x + 1, snake[i].y));
                    else if (dir == 3)
                        snake[0].Set(new Vector2(snake[i].x, snake[i].y + 1));
                    if (snake[0].x < 0)
                    {
                        loose = true;
                        return;
                        //snake[0].x = gameSize.x - 1;
                    }
                    else if (snake[0].x >= gameSize.x)
                    {
                        loose = true;
                        return;
                        //snake[0].x = 0;
                    }

                    if (snake[0].y < 0)
                    {
                        loose = true;
                        return;
                        //snake[0].y = gameSize.y - 1;
                    }
                    else if (snake[0].y >= gameSize.y)
                    {
                        loose = true;
                        return;
                        //snake[0].y = 0;
                    }

                    for (int x = 1; x < snake.Count; x++)
                    {
                        if (snake[0] == snake[x])
                        {
                            loose = true;
                            return;
                        }
                    }
                }
            }

            bool newApple = false;
            if (snake[0] == apple)
            {
                newApple = GenApple();
            }

            Refresh();

            if (newApple)
            {
                snake.Add(new Vector2(snake[snake.Count - 1]));
            }
        }

        private void Restart()
        {
            dir = 2;

            snake.Clear();

            snake.Add(new Vector2(4, (int)((float)gameSize.y / 2)));
            snake.Add(new Vector2(3, (int)((float)gameSize.y / 2)));
            snake.Add(new Vector2(2, (int)((float)gameSize.y / 2)));
            snake.Add(new Vector2(1, (int)((float)gameSize.y / 2)));

            apple = new Vector2((int)((float)gameSize.x / 2), (int)((float)gameSize.y / 2));
            loose = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var path = new GraphicsPath();
            Color red = Color.FromArgb(255, 87, 87);
            Color blue = Color.FromArgb(56, 182, 255);
            Color gray = Color.FromArgb(240, 245, 240);

            for (int x = 0; x < gameSize.x; x++)
            {
                for (int y = 0; y < gameSize.y; y++)
                {
                    if(x % 2 == 0 && y % 2 == 0)
                        CreateRecObject(e, path, x * 40, y * 40, 40, 40, 0, gray, false);
                    else if (x % 2 == 1 && y % 2 == 1)
                        CreateRecObject(e, path, x * 40, y * 40, 40, 40, 0, gray, false);
                }
            }
            Brush _fillColor = new SolidBrush(gray);

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.FillPath(_fillColor, path);


            path = new GraphicsPath();
            int ind = 0;
            foreach (var item in snake)
            {
                if(ind == 0)
                {
                    /*
                    if(dir == 0 || dir == 2)
                    {
                        CreateRecObject(e, path, item.x * 40, item.y * 40 + 2, 41, 36, 10);
                    }
                    else
                    {
                        CreateRecObject(e, path, item.x * 40 + 2, item.y * 40, 36, 41, 10);
                    }
                    */
                    CreateRecObject(e, path, item.x * 40 + 1, item.y * 40 + 1, 38, 38, 10);
                }
                else
                {
                    /*
                    if(snake[ind - 1].x > item.x)
                    {
                        CreateRecObject(e, path, item.x * 40, item.y * 40 + 2, 41, 36, 10);
                    }
                    else if (snake[ind - 1].y < item.y)
                    {
                        CreateRecObject(e, path, item.x * 40 + 2, item.y * 40, 36, 41, 10);
                    }
                    else if (snake[ind - 1].x < item.x)
                    {
                        CreateRecObject(e, path, item.x * 40, item.y * 40 + 2, 41, 36, 10);
                    }
                    else if (snake[ind - 1].y > item.y)
                    {
                        CreateRecObject(e, path, item.x * 40 + 2, item.y * 40, 36, 41, 10);
                    }
                    */
                    if(ind < snake.Count - 5)
                    {
                        CreateRecObject(e, path, item.x * 40 + 2, item.y * 40 + 2 + 1, 36, 36, 10);
                    }
                    else
                    {
                        int val = 6 - (snake.Count - ind);

                        CreateRecObject(e, path, item.x * 40 + val / 2 + 2, item.y * 40 + val / 2 + 2, 36 - val, 36 - val, 10);
                    }
                }
                ind++;
            }

            _fillColor = new SolidBrush(blue);
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.FillPath(_fillColor, path);


            path = new GraphicsPath();

            CreateRecObject(e, path, apple.x * 40 + 2, apple.y * 40 + 2, 36, 36, 36, red);
        }
        public void CreateRecObject(PaintEventArgs e, GraphicsPath path, int x, int y, int width, int height, int radius, Color color = new Color(), bool fill = true)
        {
            if (height <= 1 || width <= 1)
            {
                return;
            }

            int borderR = radius;
            RectangleF circleRect = new RectangleF(x, y, borderR, borderR);
            RectangleF circleRect2 = new RectangleF(x + width - (borderR + 1), y, borderR, borderR);
            RectangleF circleRect3 = new RectangleF(x + width - (borderR + 1), y + height - (borderR + 1), borderR, borderR);
            RectangleF circleRect4 = new RectangleF(x, y + height - (borderR + 1), borderR, borderR);

            if (borderR > 0)
            {
                path.AddArc(circleRect, 180, 90);
                path.AddArc(circleRect2, 270, 90);
                path.AddArc(circleRect3, 0, 90);
                path.AddArc(circleRect4, 90, 90);
            }
            else
            {
                path.AddLine(new Point(x, y),
                             new Point(x + width, y));

                path.AddLine(new Point(x + width, y),
                             new Point(x + width, y + height));

                path.AddLine(new Point(x + width, y + height),
                             new Point(x, y + height));

                path.AddLine(new Point(x, y + height),
                             new Point(x, y));
            }
            path.CloseFigure();


            if (fill)
            {
                Brush _fillColor = new SolidBrush(color);
                //Pen _borderColor = new Pen(borderColor);

                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.FillPath(_fillColor, path);
                //e.Graphics.DrawPath(_borderColor, path);
            }
        }


        private bool GenApple()
        {
            Random rdm = new Random();
            int newPos = rdm.Next(0, (gameSize.x * gameSize.y) - (snake.Count + 1));
            if (newPos == 0) MessageBox.Show("done");
            for (int x = 0; x < gameSize.x; x++)
            {
                for (int y = 0; y < gameSize.y; y++)
                {
                    if (!GetSnakePos(new Vector2(x, y)))
                    {
                        if (newPos == 0)
                        {
                            apple = new Vector2(x, y);
                            return true;
                        }
                        newPos--;
                    }
                }
            }
            return false;
        }
        private bool GetSnakePos(Vector2 vec)
        {
            foreach (var item in snake)
            {
                if(item == vec)
                {
                    return true;
                }
            }
            return false;
        }

        private void time_Tick(object sender, EventArgs e)
        {
            Updater();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'q' && actualDir != 2)
            {
                dir = 0;
            }
            else if (e.KeyChar == 'z' && actualDir != 3)
            {
                dir = 1;
            }
            else if (e.KeyChar == 'd' && actualDir != 0)
            {
                dir = 2;
            }
            else if (e.KeyChar == 's' && actualDir != 1)
            {
                dir = 3;
            }
            else if (e.KeyChar == ' ')
            {
                pause = !pause;
            }
        }
    }

    public class Vector2
    {
        public int x;
        public int y;
        public Vector2(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }
        public void Set(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public void Set(Vector2 v)
        {
            this.x = v.x;
            this.y = v.y;
        }
        public Vector2(Vector2 v)
        {
            this.x = v.x;
            this.y = v.y;
        }

        public static bool operator !=(Vector2 a, Vector2 b)
        {
            if(a is null && b is null)
            {
                return true;
            }
            else if (a is null || b is null)
            {
                return false;
            }
            else if(a.x == b.x && a.y == b.y)
            {
                return false;
            }
            return true;
        }
        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return !(a != b);
        }
    }
}
