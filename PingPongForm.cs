using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pong
{
    public partial class PingPongForm : Form
    {
        private System.Timers.Timer aTimer;
        PingPongGame g;

        public PingPongForm()
        {
            InitializeComponent();
            g = new PingPongGame((Size)this.ClientSize);

            // Создаем таймер с двухсекундным интервалом.
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 10;

            aTimer.Elapsed += OnTimedEvent; // Добавить обработчик событий
            aTimer.Enabled = true; // Запустить таймер
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (g.stopGame > 0)
            {
                // Снова запустить игру
                g.stopGame = 0;
                aTimer.Interval = 10;
                g.resetBall();
            }

            g.update_pos(); // Обновить позиции игрока и мяча

            if (g.stopGame > 0)
            {
                // Остановить игру и вывести счёт(гейм овер)
                aTimer.Enabled = false;
                aTimer.Interval = 2000;
                aTimer.Enabled = true;
            }

            this.Invalidate();
        }
        private void PingPongForm_KeyDown(object sender, KeyEventArgs e)
        {
            g.update_key(e, true);
        }

        private void PingPongForm_KeyUp(object sender, KeyEventArgs e)
        {
            g.update_key(e, false);
        }

        private void PingPongForm_Paint(object sender, PaintEventArgs e)
        {
            g.paint(e);
            scoreLabel.Text = String.Format("Game Over!");
            scoreLabel.Visible = (g.stopGame > 0);
        }
        private void PongForm_Load(object sender, EventArgs e)
        {

        }
    }


    class PingPongGame
    {
        private Player p1;
        private Ball b;
        public int stopGame = 0;

        public PingPongGame(Size clientSize)
        {
            int wall_offset = 25;
            Size playerSize = new Size(20, 100);
            Size ballSize = new Size(20, 20);

            p1 = new Player(
                new Rectangle(wall_offset, clientSize.Height / 2 - playerSize.Height / 2, playerSize.Width, playerSize.Height),
                clientSize, Keys.W, Keys.S);
            b = new Ball(
                new Rectangle(wall_offset, clientSize.Height - wall_offset, ballSize.Width, ballSize.Height),
                clientSize);
        }

        public void update_pos()
        {
            stopGame = b.update_pos(p1);
            p1.update_pos(b);
            //p2.update_pos(b);
        }

        public void resetBall()
        {
            b.reset(); // Поместите мяч в центр со случайной скоростью
        }

        public void update_key(KeyEventArgs e, bool down)
        {
            // Обновление направления движения игрока
            p1.update_key(e.KeyCode, down);
        }

        public void paint(PaintEventArgs e)
        {
            // цвет мяча и игрока
            e.Graphics.FillRectangle(Brushes.Green, p1.r);
            e.Graphics.FillRectangle(Brushes.Blue, b.r);
        }
    }


    class Player
    {
        public Rectangle r;
        public bool mov_up = false;
        public bool mov_down = false;
        Keys upKey;
        Keys downKey;
        Size enclosing;
        int speed = 3;

        public Player(Rectangle r, Size enclosing, Keys upKey, Keys downKey)
        {
            this.r = r;
            this.enclosing = enclosing;
            this.upKey = upKey;
            this.downKey = downKey;
        }

        public void update_pos(Ball b)
        {
            if (mov_up && mov_down)
            {
                return;
            }
            else if (mov_up)
            {
                if (r.Top - speed >= 0)
                {
                    r.Y -= speed;
                    if (this.r.IntersectsWith(b.r))
                    {
                        r.Y += speed; 
                    }
                }
                else
                {
                    r.Y = 0;
                }
            }
            else if (mov_down)
            {
                if (r.Bottom + speed <= enclosing.Height)
                {
                    r.Y += speed;
                    if (this.r.IntersectsWith(b.r))
                    {
                        r.Y -= speed; 
                    }
                }
                else
                {
                    r.Y = enclosing.Height - r.Height;
                }
            }
        }

        public void update_key(Keys key, bool down)
        {
            if (key == upKey)
            {
                mov_up = down;
            }
            else if (key == downKey)
            {
                mov_down = down;
            }
        }
    }


    class Ball
    {
        public Rectangle r;
        int dx;
        int dy;
        Size enclosing;
        Rectangle top_wall;
        Rectangle bot_wall;
        Rectangle right_wall;

        public Ball(Rectangle r, Size enclosing)
        {
            this.r = r;
            this.enclosing = enclosing;
            int wall_thickness = 100;
            top_wall = new Rectangle(0, -wall_thickness, enclosing.Width, wall_thickness);
            bot_wall = new Rectangle(0, enclosing.Height, enclosing.Width, wall_thickness);
            right_wall = new Rectangle(enclosing.Width-2, 0, wall_thickness, enclosing.Height);
            reset();
        }

        public void reset()
        {
            // Поместить мяч в центр со случайной скоростью
            Random rnd = new Random();
            r.X = enclosing.Width / 2;
            r.Y = enclosing.Height / 2;
            dx = rnd.Next(0, 7) - 3;
            dy = rnd.Next(0, 7) - 3;
            if (dx == 0)
                dx = 1;
            if (dy == 0)
                dy = -1;
        }

        public int update_pos (Player p1)
        {
            // Обновления позиции мяча
            r.X += dx;
            r.Y += dy;

            // Проверка, не проскользнул ли мяч через игрока
            if (r.Left < 0)
            {
                return 2;
            }

            // Проверка, касается ли мяч верхней или нижней стенки
            if (r.IntersectsWith(top_wall) || r.IntersectsWith(bot_wall))
            {
                dy = -dy;
            }
            if (r.IntersectsWith(right_wall))
            {
                dx = -dx;
            }
            // Проверка, касается ли мяч правой стенки
            if (r.IntersectsWith(p1.r)) // Отскок от игрока 
            {
                bounceWith(p1);
            }
   
            return 0;
        }

        private void bounceWith(Player p)
        {
            dx = -dx;

            if (dx > 0 && r.Left + dx < p.r.Right)
            {
                // Если отскакивает верх или низ игрок 1
                dx = -Math.Abs(dx);
                dy = -dy;
            }

            else
            {
                // Отскок с вертикальным краем игрока
                if (p.mov_down && !p.mov_up)
                {
                    if (dy > 0)
                    {
                        // Игрок вниз, мяч вниз
                        dy += 1;
                    }
                    else
                    {
                        // Игрок вниз, мяч вверх
                        dy += 1;
                        if (dy == 0)
                        {
                            dy = -1;
                        }
                        else
                        {
                            dx = Math.Sign(dx) * (Math.Abs(dx) + 1);
                        }
                    }
                }
                else if (p.mov_up && !p.mov_down)
                {
                    if (dy > 0)
                    {
                        // Игрок вверх, мяч вниз
                        dy += -1;
                        if (dy == 0)
                        {
                            dy = 1;
                        }
                        else
                        {
                            dx = Math.Sign(dx) * (Math.Abs(dx) + 1);
                        }
                    }
                    else
                    {
                        // Игрок вверх, мяч вверх
                        dy += -1;
                    }
                }
            }
        }
    }
}
