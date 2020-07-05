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
        public Game()
        {
            InitializeComponent();
        }

        private Button[,] buttons = new Button[8,8];
        private Label[,] Labels = new Label[8, 8];
        private SoundPlayer DefoultBoom = new SoundPlayer(@"C:\Users\Goba-PC\source\repos\3_match\Mach3\3_match\Sounds\DefoultBoom.wav");
        string dir = AppDomain.CurrentDomain.BaseDirectory;

        private void Game_Load(object sender, EventArgs e)
        {
            GameManager.game = this;
            /*for(int y = 0; y < 8; y++)
            {
                for(int x=0; x < 8; x++)
                {
                    Labels[x, y] = new Label();
                    Labels[x, y].Location = new Point((x + 10) * 50, (y+1) * 50);
                    Labels[x, y].Height = 50;
                    Labels[x, y].Width = 50;
                    Labels[x, y].BackColor = buttons[x,y].BackColor;
                    Labels[x, y].Text = buttons[x, y].Text;
                    Labels[x, y].Text += $" {buttons[x, y].Location.X.ToString()} {buttons[x, y].Location.Y.ToString()}";
                    this.Controls.Add(Labels[x, y]);
                }
            }*/
        }
        void ButtonClick(object sender, EventArgs e)
        {
            Button btn = null;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
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
            LabelText();
            GameManager.Mathes = 0;
            GameManager.RememberList.Clear();
            GameManager.MatchAndClear(buttons);
            /*if (GameManager.Mathes==0)
            {
                buttons[x1, y1] = buffer1;
                buttons[x2, y2] = buffer2;
                Animate(x2, y2, x1, y1);
                LabelText();
            }*/
        }
        public void LabelText()
        {
            /*for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    try
                    {
                        Labels[x, y].Text = buttons[x, y].Text;
                        Labels[x, y].Text += $" {buttons[x, y].Location.X.ToString()} {buttons[x, y].Location.Y.ToString()}";
                        Labels[x, y].BackColor = buttons[x, y].BackColor;
                    }
                    catch
                    {
                        Labels[x, y].Text = "-----";
                        Labels[x, y].BackColor = Color.White;
                    }
                }
            }*/
        }
        public void DestroyBTN(Button btn) 
        {
            int x = btn.Location.X / 50 - 1;
            int y = btn.Location.Y / 50 - 1;
            buttons[x, y] = null;
            Controls.Remove(btn);
            DefoultBoom.Play();
        }
        public void DestroyForSpecialBTN(Button btn)
        {
            Controls.Remove(btn);
        }
        public void RespawnSpecialBTN(Button btn, string type)
        {
            int x = btn.Location.X / 50 - 1;
            int y = btn.Location.Y / 50 - 1;
            if (type == "vertical")
            {
                buttons[x, y].Text = "|";
                buttons[x, y].BackColor = btn.BackColor;
            }
            if (type == "horizontal")
            {
                buttons[x, y].Text = "-";
                buttons[x, y].BackColor = btn.BackColor;
            }
            if (type == "bomb")
            {
                buttons[x, y].Text = "(%)";
                Controls.Add(buttons[x, y]);
            }
            //buttons[x, y].Click += ButtonClick;
        }
        public void RespawnBTN()
        {
            for(int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {

                    if (buttons[x, y] == null)
                    {
                        if (y > 0)
                        {
                            buttons[x, y] = buttons[x, y - 1];
                            buttons[x, y - 1] = null;
                            //buttons[x, y].Location = new Point((x + 1) * 50, (y + 1) * 50);
                            Animate(x, y);
                            RespawnBTN();
                        }
                        else
                        {
                            Random random = new Random();
                            Thread.Sleep(10);
                            buttons[x, y] = new Button();
                            this.Controls.Add(buttons[x, y]);
                            buttons[x, y].Height = 50;
                            buttons[x, y].Width = 50;
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
                            buttons[x, y].Location = new Point((x + 1) * 50, (y + 1) * 50);
                        }
                        this.Update();
                    }
                }
            }
            //LabelText();
            GameManager.MatchAndClear(buttons);
        }

        private void Game_Shown(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Thread.Sleep(1500);
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
        public void CheckMatchesAfterRespawn()
        {
            if (GameManager.CanMatch)
            {
                GameManager.RememberList.Clear();
                Thread.Sleep(100);
                GameManager.MatchAndClear(buttons);
            }
        }
        public void Animate(int x1,int y1, int x2, int y2)
        {
            int speed = 5;
            int XPos1 = (x1 + 1) * 50;
            int XPos2 = (x2 + 1) * 50;
            int YPos1 = (y1 + 1) * 50;
            int YPos2 = (y2 + 1) * 50;

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
        public void Animate(int x,int y)
        {
            int speed = 25;
            int XPosS = (x+1) * 50;
            int YPosS = y * 50;
            int XPosF = (x+1) * 50;
            int YPosF = (y+1) * 50;

            Point DeltaS = new Point(XPosS, YPosS);
            Point Finish = new Point(XPosF, YPosF);

            buttons[x, y].Location = DeltaS;


            do
            {
                Thread.Sleep(50);
                DeltaS = new Point(DeltaS.X + speed * GetSign(XPosF - XPosS), DeltaS.Y + speed * GetSign(YPosF - YPosS));
                buttons[x, y].Location = DeltaS;
                Update();
            }
            while (buttons[x, y].Location != Finish);
        }

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

        //System.Timers.Timer timer = new System.Timers.Timer();
        int i=10;
        int tk;
        private void StartTimer()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            //timer1?.Dispose();
            /* var Timer;
             var date = DateTime.Now;
             Timer.Tick += (o, args) =>
             {
                 TimeText.BeginInvoke((MethodInvoker)(() => TimeText.Text = (date - DateTime.UtcNow).ToString()));
             };
             Timer.Start();*/
            //TimeText.Text = time;
            timer.AutoReset = true; // Чтобы операции удаления не перекрывались
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
