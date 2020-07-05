using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Media;

namespace _3_match
{
    public static class GameManager
    {
        private static int x1 = 0;
        private static int y1 = 0;

        public static Game game;

        public static bool IsButtonChoosed = false;
        //Обработчик 2-ого нажатия
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
        //Обработчик 2-ого нажатия
        public static void FirstClick(Button btn)
        {
            x1 = btn.Location.X / 50 - 1;
            y1 = btn.Location.Y / 50 - 1;
            IsButtonChoosed = true;
        }


        static bool clearedBTN = false;
        static List<Button> collector = new List<Button>();
        static Button[,] toTest = new Button[8, 8];
        public static List<Button> RememberList = new List<Button>();
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
                    collector.Clear();
                }
            }
            if (clearedBTN)
            {
                //SettleBlocks(board)
                game.RespawnBTN();
            }
            RememberList.Clear();
        }

        private static void TestButton(int x, int y)
        {
            //Листы куда мы будем записывать совпадения по горизонтали и по вертикале.
            List<Button> HorizontalList = new List<Button>();
            List<Button> VerticalList = new List<Button>();

            HorizontalList.Add(toTest[x, y]);
            VerticalList.Add(toTest[x, y]);

            //Проверяем совпадения по горизонтали
            for (int j=x+1; j < 8; j++)
            {
                if (toTest[x, y].BackColor == toTest[j, y].BackColor)
                {
                    HorizontalList.Add(toTest[j,y]);
                }
                else
                {
                    break;//Если попали на не совпадающие элементы, то выходим из цикла
                }
            }
            if (HorizontalList.Count < 3)//Если кол-во элементов меншьше 3-х то очищаем лист
            {
                HorizontalList.Clear();
            }

            collector.Add(toTest[x, y]);
            //То же самое по вертикали
            for (int k = y+1; k < 8; k++)
            {
                if (toTest[x, y].BackColor == toTest[x, k].BackColor)
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
            
            //Запись в основоной коллектор(лист) совпадений из горизонтального листа
            for(int j = 0; j < HorizontalList.Count; j++)
            {
                //Если у всего по горизонтали 4 элемента и это уже не является каким-то бонусом то мы респванем горизонтальный рузрушитель
                if (j == 3 && HorizontalList.Count == 4 && !RememberList.Contains(HorizontalList[j]) && HorizontalList[j].Text!="-" && HorizontalList[j].Text != "|" && HorizontalList[j].Text != "(")
                {
                    game.RespawnSpecialBTN(HorizontalList[j], "horizontal");//Респавн на месте 4-ого элемента горизонтального разрушителя
                    RememberList.Add(HorizontalList[j]);//Запоминаем что мы в этом месте заспванили разрушитель, чтобы он не удалился или перезаписался при дальнейшей проверки доски
                    game.SetScore(1);
                }
                //Аналогично с бомбой.
                if (j == HorizontalList.Count-1 && HorizontalList.Count >= 5)
                {
                    game.RespawnSpecialBTN(HorizontalList[j], "bomb");
                    RememberList.Add(HorizontalList[j]);
                    game.SetScore(1);
                }
                //Далее если мы наткнулись не на спецальное поле которое мы в этом ходу должны зареспавнить то...
                else if (!RememberList.Contains(HorizontalList[j]))
                {
                    //если это горизонтальнй бонус то добваляем в коллектор всю строку
                    if (HorizontalList[j].Text == "-")
                    {
                        addAllHorizontal(HorizontalList[j]);
                    }
                    //Если вертикальной то весь столбец
                    else if(HorizontalList[j].Text == "|")
                    {
                        addAllVertical(HorizontalList[j]);
                    }
                    //Если это бомба то квадрат вокруг этой бомбы
                    else if(HorizontalList[j].Text == "(")
                    {
                        BombActivation(HorizontalList[j]);
                    }
                    //если это обычное поле то просто добовляем эту ячейку в коллектор
                    else
                    {
                        collector.Add(HorizontalList[j]);
                    }
                }
            }
            //Аналогчино для вертикального списка
            for (int j = 0; j < VerticalList.Count; j++)
            {
                if (j == 3 && VerticalList.Count == 4 && !RememberList.Contains(VerticalList[j]) && VerticalList[j].Text!="|" && VerticalList[j].Text != "-" && VerticalList[j].Text != "(")
                {
                    game.RespawnSpecialBTN(VerticalList[j], "vertical");
                    RememberList.Add(VerticalList[j]);
                    game.SetScore(1);
                }
                if (j == VerticalList.Count-1 && VerticalList.Count >= 5)
                {
                    game.RespawnSpecialBTN(VerticalList[j], "bomb");
                    RememberList.Add(VerticalList[j]);
                    game.SetScore(1);
                }
                else if (!RememberList.Contains(VerticalList[j]))
                {
                    if (VerticalList[j].Text == "-")
                    {
                        addAllHorizontal(VerticalList[j]);
                    }
                    else if (VerticalList[j].Text == "|")
                    {
                        addAllVertical(VerticalList[j]);
                    }
                    else if(VerticalList[j].Text == "(")
                    {
                        BombActivation(VerticalList[j]);
                    }
                    else
                    {
                        collector.Add(VerticalList[j]);
                    }
                }
            }
        }
        private static void addAllHorizontal(Button btn)
        {
            int y = btn.Location.Y / 50 - 1;
            for(int i = 0; i < 8; i++)
            {
                if (!collector.Contains(toTest[i, y]))
                {
                    if (toTest[i, y].Text == "|")
                    {
                        addAllVertical(toTest[i, y]);
                    }
                    else if (toTest[i, y].Text == "(")
                    {
                        BombActivation(toTest[i, y]);
                    }
                    else
                    {
                        collector.Add(toTest[i, y]);
                    }
                }
                   
            }
        }
        private static void addAllVertical(Button btn)
        {
            int x = btn.Location.X / 50 - 1;
            for (int i = 0; i < 8; i++)
            {
                if (!collector.Contains(toTest[x, i]))
                {
                    if (toTest[x, i].Text == "-")
                    {
                        addAllHorizontal(toTest[x, i]);
                    }
                    else if (toTest[x, i].Text == "(")
                    {
                        BombActivation(toTest[x, i]);
                    }
                    else
                    {
                        collector.Add(toTest[x, i]);
                    }
                }
            }
        }
        private static void BombActivation(Button btn)
        {
            int x = btn.Location.X / 50 - 1;
            int y = btn.Location.Y / 50 - 1;
            for(int j = x - 1; j <= x + 1; j++)
            {
                for(int k = y - 1; k <= y + 1; k++)
                {
                    try
                    {
                        if (!collector.Contains(toTest[j, k]))
                        {
                            if (toTest[j, k].Text == "-")
                            {
                                addAllHorizontal(toTest[j, k]);
                            }
                            else if (toTest[j, k].Text == "|")
                            {
                                addAllHorizontal(toTest[j, k]);
                            }
                            else if (toTest[j, k].Text == "(" && j != x && k!= y)
                            {
                                BombActivation(toTest[j, k]);
                            }
                            else
                            {
                                collector.Add(toTest[j, k]);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}
