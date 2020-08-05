using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace _3_match
{
    public partial class Game : Form
    {
        public Form MenuForm;

        const int BoardSize = 8;
        const int ButtonSize = 50;
        public Game()
        {
            InitializeComponent();
        }

        private Button[,] buttons = new Button[BoardSize,BoardSize];
        //private SoundPlayer DefoultBoom = new SoundPlayer(@"C:\Users\Goba-PC\source\repos\3_match\Mach3\3_match\Sounds\DefoultBoom.wav");
        string dir = AppDomain.CurrentDomain.BaseDirectory;

        private void Game_Load(object sender, EventArgs e)
        {
            GameManager.game = this;
        }

        private void Game_Shown(object sender, EventArgs e)
        {
            //CheckForIllegalCrossThreadCalls = false;
            string time = "01:00";
            TimeText.Text = time;
            RespawnBTN();
            CheckMatchesAfterRespawn();
            Thread TimerThread = new Thread(() =>
            {
                StartTimer();
            });
            TimerThread.Start();
        }

        void ButtonClick(object sender, EventArgs e)
        {
            Button btn = null;
            for (int x = 0; x < BoardSize; x++)
            {
                for (int y = 0; y < BoardSize; y++)
                {
                    if (sender.Equals(buttons[x, y]))
                    {
                        btn = buttons[x, y];
                        break;
                    }
                }
            }
            if (!GameManager.IsButtonChoosed)
            {
                GameManager.FirstClick(btn);
            }
            else
            {
                GameManager.SecondClick(btn);
                this.ActiveControl = ScoreText;
            }
        }
        public void ChangePlace(int x1,int y1, int x2, int y2)
        {
            Button buffer1 = buttons[x1, y1];
            Button buffer2 = buttons[x2, y2];
            buttons[x1, y1] = buffer2;
            buttons[x2, y2] = buffer1;
            Animate(x1, y1, x2, y2);
            GameManager.Mathes = 0;
            GameManager.RememberList.Clear();
            GameManager.MatchAndClear(buttons);
            /*if (GameManager.Mathes==0)//Если не было ни одного совпадения то взоращаем на прежнии места.
            {
                buttons[x1, y1] = buffer1;
                buttons[x2, y2] = buffer2;
                Animate(x2, y2, x1, y1);
            }*/
        }
        public void DestroyBTN(Button btn) 
        {
            int x = btn.Location.X / ButtonSize - 1;
            int y = btn.Location.Y / ButtonSize - 1;
            buttons[x, y] = null;
            Controls.Remove(btn);
            //DefoultBoom.Play();
        }
        public void RespawnSpecialBTN(Button btn, string type)
        {
            int x = btn.Location.X / ButtonSize - 1;
            int y = btn.Location.Y / ButtonSize - 1;
            if (type == "vertical")
            {
                if (buttons[x, y].BackColor == Color.Transparent)
                {
                    buttons[x, y].BackgroundImage = new Bitmap(dir + "\\Images\\Vertical\\ТриугольникВ.png");
                }
                if (buttons[x, y].BackColor == Color.Red)
                {
                    buttons[x, y].BackgroundImage = new Bitmap(dir + "\\Images\\Vertical\\РомбВ.png");

                }
                if (buttons[x, y].BackColor == Color.Aquamarine)
                {
                    buttons[x, y].BackgroundImage = new Bitmap(dir + "\\Images\\Vertical\\КругВ.png");

                }
                if (buttons[x, y].BackColor == Color.DarkBlue)
                {
                    buttons[x, y].BackgroundImage = new Bitmap(dir + "\\Images\\Vertical\\КвадратВ.png");

                }
                if (buttons[x, y].BackColor == Color.AliceBlue)
                {
                    buttons[x, y].BackgroundImage = new Bitmap(dir + "\\Images\\Vertical\\ПятиугольникВ.png");
                }
                buttons[x, y].Text = "|";
                buttons[x, y].ForeColor = Color.Transparent;
            }
            if (type == "horizontal")
            {
                if (buttons[x, y].BackColor == Color.Transparent)
                {
                    buttons[x, y].BackgroundImage = new Bitmap(dir + "\\Images\\Horizontal\\ТриугольникГ.png");
                }
                if (buttons[x, y].BackColor == Color.Red)
                {
                    buttons[x, y].BackgroundImage = new Bitmap(dir + "\\Images\\Horizontal\\РомбГ.png");

                }
                if (buttons[x, y].BackColor == Color.Aquamarine)
                {
                    buttons[x, y].BackgroundImage = new Bitmap(dir + "\\Images\\Horizontal\\КругГ.png");

                }
                if (buttons[x, y].BackColor == Color.DarkBlue)
                {
                    buttons[x, y].BackgroundImage = new Bitmap(dir + "\\Images\\Horizontal\\КвадратГ.png");

                }
                if (buttons[x, y].BackColor == Color.AliceBlue)
                {
                    buttons[x, y].BackgroundImage = new Bitmap(dir + "\\Images\\Horizontal\\ПятиугольникГ.png");
                }
                buttons[x, y].Text = "-";
                buttons[x, y].ForeColor = Color.Transparent;
            }
            if (type == "bomb")
            {
                if (buttons[x, y].BackColor == Color.Transparent)
                {
                    buttons[x, y].BackgroundImage = new Bitmap(dir + "\\Images\\Bomb\\ТриугольникБ.png");
                }
                if (buttons[x, y].BackColor == Color.Red)
                {
                    buttons[x, y].BackgroundImage = new Bitmap(dir + "\\Images\\Bomb\\РомбБ.png");

                }
                if (buttons[x, y].BackColor == Color.Aquamarine)
                {
                    buttons[x, y].BackgroundImage = new Bitmap(dir + "\\Images\\Bomb\\КругБ.png");

                }
                if (buttons[x, y].BackColor == Color.DarkBlue)
                {
                    buttons[x, y].BackgroundImage = new Bitmap(dir + "\\Images\\Bomb\\КвадратБ.png");

                }
                if (buttons[x, y].BackColor == Color.AliceBlue)
                {
                    buttons[x, y].BackgroundImage = new Bitmap(dir + "\\Images\\Bomb\\ПятиугольникБ.png");
                }
                buttons[x, y].Text = "(";
                buttons[x, y].ForeColor = Color.Purple;
            }
        }
        public void RespawnBTN()
        {
            for(int x = 0; x < BoardSize; x++)
            {
                for (int y = 0; y < BoardSize; y++)
                {

                    if (buttons[x, y] == null)
                    {
                        if (y > 0)
                        {
                            buttons[x, y] = buttons[x, y - 1];
                            buttons[x, y - 1] = null;
                            //buttons[x, y].Location = new Point((x + 1) * ButtonSize, (y + 1) * ButtonSize);
                            Animate(x, y);
                            RespawnBTN();
                        }
                        else
                        {
                            Random random = new Random();
                            Thread.Sleep(10);
                            buttons[x, y] = new Button();
                            this.Controls.Add(buttons[x, y]);
                            buttons[x, y].Height = ButtonSize;
                            buttons[x, y].Width = ButtonSize;
                            int type = random.Next(1, 6);
                            buttons[x, y].Click += ButtonClick;
                            if (type == 1)
                            {
                                buttons[x, y].BackColor = Color.Transparent;
                                buttons[x, y].BackgroundImage = Image.FromFile(dir + "\\Images\\Триугольник.png");
                            }
                            if (type == 2)
                            {
                                buttons[x, y].BackColor = Color.Red;
                                buttons[x, y].BackgroundImage = new Bitmap(dir + "\\Images\\Ромб.png");

                            }
                            if (type == 3)
                            {
                                buttons[x, y].BackColor = Color.Aquamarine;
                                buttons[x, y].BackgroundImage = new Bitmap(dir + "\\Images\\Круг.png");

                            }
                            if (type == 4)
                            {
                                buttons[x, y].BackColor = Color.DarkBlue;
                                buttons[x, y].BackgroundImage = new Bitmap(dir + "\\Images\\Квадрат.png");

                            }
                            if(type == 5)
                            {
                                buttons[x, y].BackColor = Color.AliceBlue;
                                buttons[x, y].BackgroundImage = new Bitmap(dir + "\\Images\\Пятиугольник.png");
                            }
                            buttons[x, y].Location = new Point((x + 1) * ButtonSize, (y + 1) * ButtonSize);
                        }
                        this.Update();
                    }
                }
            }
            GameManager.MatchAndClear(buttons);
        }

        public void CheckMatchesAfterRespawn()
        {
            if (GameManager.CanMatch)
            {
                GameManager.RememberList.Clear();
                Thread.Sleep(100);
                GameManager.MatchAndClear(buttons);
            }
        }
        //Функция для перстановки 2-х элементов.
        public void Animate(int x1,int y1, int x2, int y2)
        {
            int speed = 10;
            int XPos1 = (x1 + 1) * ButtonSize;
            int XPos2 = (x2 + 1) * ButtonSize;
            int YPos1 = (y1 + 1) * ButtonSize;
            int YPos2 = (y2 + 1) * ButtonSize;

            Point Delta1 = new Point(XPos2, YPos2);
            Point Delta2 = new Point(XPos1, YPos1);


            do
            {
                Thread.Sleep(100);
                Delta1 = new Point(Delta1.X + speed * GetSign(x1 - x2), Delta1.Y + speed * GetSign(y1 - y2));
                Delta2 = new Point(Delta2.X + speed * GetSign(x2 - x1), Delta2.Y + speed * GetSign(y2 - y1));
                buttons[x1, y1].Location = Delta1;
                buttons[x2, y2].Location = Delta2;
                Update();
            }
            while (buttons[x1,y1].Location.X != XPos1 | buttons[x1,y1].Location.Y != YPos1 | buttons[x2,y2].Location.X != XPos2 | buttons[x2,y2].Location.Y != YPos2);
        }
        //Эта функция для падения элемента вниз
        public void Animate(int x,int y)
        {
            int speed = 25;
            int XPosS = (x+1) * ButtonSize;
            int YPosS = y * ButtonSize;
            int XPosF = (x+1) * ButtonSize;
            int YPosF = (y+1) * ButtonSize;

            Point DeltaS = new Point(XPosS, YPosS);
            Point Finish = new Point(XPosF, YPosF);

            buttons[x, y].Location = DeltaS;


            do
            {
                Thread.Sleep(ButtonSize);
                DeltaS = new Point(DeltaS.X + speed * GetSign(XPosF - XPosS), DeltaS.Y + speed * GetSign(YPosF - YPosS));
                buttons[x, y].Location = DeltaS;
                Update();
            }
            while (buttons[x, y].Location != Finish);
        }
        //Функция определяет является ли вектор движения одного элемента к другому положительным или отрицательным.
        private int GetSign(int x)
        {
            if (x < 0)
            {
                return (x / x * -1);
            }
            else if (x==0)
            {
                return 0;
            }
            else
            {
                return (x / x);
            }
        }
        public void SetScore(int add)
        {
            ScoreText.Text = (Convert.ToInt32(ScoreText.Text) + add).ToString();
        }

        int i=300;
        int tk;
        private void StartTimer()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.AutoReset = true; 
            timer.Interval = 1 * 1000;
            timer.Enabled = true;
            timer.Elapsed += (o,args) => {
            TimeText.BeginInvoke((MethodInvoker)(() =>
            {
                tk = --i;
                TimeSpan span = TimeSpan.FromMinutes(tk);
                string label = span.ToString(@"hh\:mm");
                TimeText.Text = label.ToString();
                TimeText.Refresh();
                if (TimeText.Text == "00:00")
                {
                    timer.Dispose();
                    MessageBox.Show($"Время закончилось ваш резултат:{ScoreText.Text}", "Время вышло", MessageBoxButtons.OK);
                    this.Close();
                }
            }));
            };
            timer.Start();
        }

        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            MenuForm.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ScoreText_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void TimeText_Click(object sender, EventArgs e)
        {

        }
    }
}
