using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3_match
{
    public static class GameManager
    {
        private static int x1 = 0;
        private static int y1 = 0;

        public static Game game;

        public static bool IsButtonChoosed = false;

        public static void SecondClick(Button btn)
        {
            int x2 = btn.Location.X / 50 - 1;
            int y2 = btn.Location.Y / 50 - 1;
            if (x1 == x2)
            {
                if(y1 == y2 + 1 | y1 == y2 - 1)
                {
                    game.ChangePlace(x1, y1, x2, y2);
                    IsButtonChoosed = false;
                }
            }
            else
            {
                IsButtonChoosed = false;
            }
            if (y1 == y2)
            {
                if (x1 == x2 + 1 | x1 == x2 - 1)
                {
                    game.ChangePlace(x1, y1, x2, y2);
                    IsButtonChoosed = false;
                }
            }
            else
            {
                IsButtonChoosed = false;
            }
        }
        public static void FirstClick(Button btn)
        {
            x1 = btn.Location.X / 50 - 1;
            y1 = btn.Location.Y / 50 - 1;
            IsButtonChoosed = true;
        }


        static bool clearedBTN = false;
        static Button currentBTN;
        static List<Button> collector = new List<Button>();
        static Button[,] toTest = new Button[8, 8];
        public static bool CanMatch = true;

        static void CopyBoard(Button[,] source)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    toTest[x, y] = source[x, y];
                }
            }
        }
        public static int Mathes = 0;
        public static void MatchAndClear(Button[,] board)
        {
            CanMatch = true;
            clearedBTN = false;
            // Создаём копию поля для проверки
            CopyBoard(board);
            //_________
            currentBTN = null;
            collector.Clear();

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    TestButton(x, y);
                    if (collector.Count >= 3)
                    {
                        game.SetScore(collector.Count);
                        foreach (Button btn in collector)
                        {
                            Mathes++;
                            game.DestroyBTN(btn);
                            clearedBTN = true;
                            //Soundboard.PlayClear();
                        }
                    }
                    else
                    {
                        CanMatch = false;
                    }
                    currentBTN = null;
                    collector.Clear();
                }
            }
            if (clearedBTN)
            {
                //SettleBlocks(board)
                game.RespawnBTN();
            }
        }

        private static void TestButton(int x, int y)
        {
            List<Button> HorizontalList = new List<Button>();
            List<Button> VerticalList = new List<Button>();

            HorizontalList.Add(toTest[x, y]);
            VerticalList.Add(toTest[x, y]);


            for (int j=x+1; j < 8; j++)
            {
                if (toTest[x, y].Text == toTest[j, y].Text)
                {
                    HorizontalList.Add(toTest[j,y]);
                }
                else
                {
                    break;
                }
            }
            if (HorizontalList.Count < 3)
            {
                HorizontalList.Clear();
            }
            collector.Add(toTest[x, y]);
            for (int k = y+1; k < 8; k++)
            {
                if (toTest[x, y].Text == toTest[x, k].Text)
                {
                    VerticalList.Add(toTest[x, k]);
                }
                else
                {
                    break;
                }
            }
            if (VerticalList.Count < 3)
            {
                VerticalList.Clear();
            }
            
            for(int j = 1; j < HorizontalList.Count; j++)
            {
                collector.Add(HorizontalList[j]);
            }
            for (int j = 1; j < VerticalList.Count; j++)
            {
                collector.Add(VerticalList[j]);
            }

            // Тайл уже проверен, пропускаем
            /*if (toTest[x, y] == null)
            {
                return;
            }
            // Начинаем проверять блок
            if (currentBTN == null)
            {
                currentBTN = toTest[x, y];
                toTest[x, y] = null;
                collector.Add(currentBTN);
            }
            else if (currentBTN.Text != toTest[x, y].Text)
            {
                return;
            }
            else
            {
                collector.Add(toTest[x, y]);
                toTest[x, y] = null;
            }

            // Если мы обрабатываем этот тайл, то проверяем все тайлы вокруг него
            if (x > 0)
                 TestButton(x - 1, y);
             if (y > 0)
                 TestButton(x, y - 1);
             if (x < 8 - 1)
                 TestButton(x + 1, y);
             if (y < 8 - 1)
                 TestButton(x, y + 1);
              */

        }
    }
}
