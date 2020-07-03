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
        public Game()
        {
            InitializeComponent();
        }

        private Button[,] buttons = new Button[8,8];
        private Label[,] Labels = new Label[8, 8];
        private SoundPlayer DefoultBoom = new SoundPlayer(@".../Sounds/DefoultBoom.wav");


        private void Game_Load(object sender, EventArgs e)
        {
            GameManager.game = this;
            for(int y = 0; y < 8; y++)
            {
                for(int x=0; x<8; x++)
                {
                    buttons[x, y] = new Button();
                    buttons[x, y].Height = 50;
                    buttons[x, y].Width = 50;
                    buttons[x, y].Location = new Point((x+1)* 50, (y+1)* 50);
                    Random random = new Random();
                    Thread.Sleep(15);//Задержка потока т.к рандомизатор берёт как сид время и если не будет хоть какой-то задержки то будут одни и те же числа
                    buttons[x, y].Text = random.Next(1, 5).ToString();
                    if(buttons[x, y].Text == "1")
                    {
                        buttons[x, y].BackColor = Color.Brown;
                    }
                    if (buttons[x, y].Text == "2")
                    {
                        buttons[x, y].BackColor = Color.Red;
                    }
                    if (buttons[x, y].Text == "3")
                    {
                        buttons[x, y].BackColor = Color.Aquamarine;
                    }
                    if (buttons[x, y].Text == "4")
                    {
                        buttons[x, y].BackColor = Color.DarkBlue;
                    }
                    buttons[x, y].Click += ButtonClick;
                    this.Controls.Add(buttons[x, y]);
                }
            }
            for(int y = 0; y < 8; y++)
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
            }
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
            GameManager.MatchAndClear(buttons);
            if (GameManager.Mathes==0)
            {
                buttons[x1, y1] = buffer1;
                buttons[x2, y2] = buffer2;
                Animate(x2, y2, x1, y1);
                LabelText();
            }
        }
        public void LabelText()
        {
            for (int y = 0; y < 8; y++)
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
            }
        }
        public void DestroyBTN(Button btn) 
        {
            int x = btn.Location.X / 50 - 1;
            int y = btn.Location.Y / 50 - 1;
            buttons[x, y] = null;
            Controls.Remove(btn);
            DefoultBoom.Play();
        }
        public void RespawnBTN()
        {
            for(int x = 0; x < 8; x++)
            {
                for(int y=0; y < 8; y++)
                {
                   
                    if (buttons[x, y]==null)
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
                            Thread.Sleep(50);
                            buttons[x, y] = new Button();
                            this.Controls.Add(buttons[x, y]);
                            buttons[x, y].Height = 50;
                            buttons[x, y].Width = 50;
                            buttons[x, y].Text = random.Next(1, 5).ToString();
                            buttons[x, y].Click += ButtonClick;
                            if (buttons[x, y].Text == "1")
                            {
                                buttons[x, y].BackColor = Color.Brown;
                            }
                            if (buttons[x, y].Text == "2")
                            {
                                buttons[x, y].BackColor = Color.Red;
                            }
                            if (buttons[x, y].Text == "3")
                            {
                                buttons[x, y].BackColor = Color.Aquamarine;
                            }
                            if (buttons[x, y].Text == "4")
                            {
                                buttons[x, y].BackColor = Color.DarkBlue;
                            }
                            buttons[x, y].Location = new Point((x + 1) * 50, (y + 1) * 50);
                        }
                        this.Update();
                    }
                }
            }
            LabelText();
            GameManager.MatchAndClear(buttons);
        }

        private async void Game_Shown(object sender, EventArgs e)
        {
            Thread.Sleep(1500);
            CheckMatchesAfterRespawn();
            CheckForIllegalCrossThreadCalls = false;
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
            int speed = 10;
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



        private async void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        //System.Timers.Timer timer = new System.Timers.Timer();
        string time = "5:00";
        int i=300;
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
            TimeText.Text = time;
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
            }));
            };
            timer.Start();
        }
        private void TimePassed(object source, System.Timers.ElapsedEventArgs e)
        {
            BeginInvoke((Action)(() =>
            {
                tk = --i;
                TimeSpan span = TimeSpan.FromMinutes(tk);
                string label = span.ToString(@"hh\:mm");
                TimeText.Text = label.ToString();
                this.Controls.Add(TimeText);
            }));
        }
        private string SetTimerText()
        {
            //TimeText.Text = string.Format("{0:HH:mm:ss:ff}");
            tk = --i;
            TimeSpan span = TimeSpan.FromMinutes(tk);
            string label = span.ToString(@"hh\:mm");
            return label.ToString();
            //if (i < 0)
               // timer1.Stop();
        }

    }
}
