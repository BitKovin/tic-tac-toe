namespace _3inRow
{
    public partial class Form1 : Form
    {
        int[,] CurentData = new int[5, 5];

        int moves = 0;

        int curentPlayer = 2;

        bool simulation;

        bool finished;

        Random rand = new Random();

        Dictionary<Point, Button> map = new Dictionary<Point, Button>();

        public Form1()
        {
            InitializeComponent();
            BuildMap();
            
        }


        void Start()
        {
            DialogResult dialogResult = MessageBox.Show("Ви хочете щоб комп'ютер зробив перший хід?", "Початок гри", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                curentPlayer = 2;
                AfterMove();
            }
            else if (dialogResult == DialogResult.No)
            {
                curentPlayer = 1;
            }
        }

        #region input
        private void button11_Click(object sender, EventArgs e)
        {
            MakeMove(4, 0, sender as Button);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MakeMove(1, 1, sender as Button);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MakeMove(2, 1, sender as Button);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MakeMove(3, 1, sender as Button);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MakeMove(4, 1, sender as Button);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MakeMove(1, 2, sender as Button);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MakeMove(2, 2, sender as Button);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MakeMove(3, 2, sender as Button);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            MakeMove(4, 2, sender as Button);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MakeMove(1, 3, sender as Button);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MakeMove(2, 3, sender as Button);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MakeMove(3, 3, sender as Button);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            MakeMove(4, 3, sender as Button);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            MakeMove(2, 4, sender as Button);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            MakeMove(3, 4, sender as Button);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            MakeMove(4, 4, sender as Button);
        }

        #endregion

        void BuildMap()
        {
            map.Add(new Point(1, 1), button1);
            map.Add(new Point(2, 1), button2);
            map.Add(new Point(3, 1), button3);
            map.Add(new Point(1, 2), button4);
            map.Add(new Point(2, 2), button5);
            map.Add(new Point(3, 2), button6);
            map.Add(new Point(1, 3), button7);
            map.Add(new Point(2, 3), button8);
            map.Add(new Point(3, 3), button9);
            map.Add(new Point(4, 0), button11);
            map.Add(new Point(4, 1), button10);
            map.Add(new Point(4, 2), button12);
            map.Add(new Point(4, 3), button13);
            map.Add(new Point(4, 4), button14);
            map.Add(new Point(3, 4), button15);
            map.Add(new Point(2, 4), button16);
        }

        void MakeMove(int x, int y, Button button)
        {
            if (CurentData[x, y] == 0)
            {
                CurentData[x, y] = curentPlayer;

                button.Text = SymbolFromPlayer(curentPlayer);

                if (PerformWinCheck()) return;

                

                curentPlayer = NotCurentPlayer();
                moves++;
                if (!finished)
                {
                    AfterMove();
                }
                else { finished = false; }
            }

        }

        string SymbolFromPlayer(int player)
        {
            if (player == 1)
            {
                return "X";
            }
            return "O";
        }

        void AfterMove()
        {
            label1.Text = "moves: " + moves.ToString();
            label2.Text = "curent player: " + SymbolFromPlayer(curentPlayer);

            if (moves == 7)
            {
                button16.Enabled = button15.Enabled = button14.Enabled = true;
            }
            if (moves == 10)
            {
                button10.Enabled = button11.Enabled = button12.Enabled = button13.Enabled = true;
            }
            if (moves == 16)
            {
                MessageBox.Show("Нічия");
            }

            if(curentPlayer==2)MakeAiMove();

        }

        void MakeAiMove()
        {
            //Thread.Sleep(1000);
            List<Point> points = GetAvalibleToMovePoints();

            Console.WriteLine(curentPlayer);

            if (TryFindWinMove(points)) return;
            if (AiTryBlock(points)) return;
            AiMakeRandomMove(points);

        }

        int[,] CopyCurentData()
        {
            int[,] TempData = new int[5, 5];

            for(int x = 0; x<5;x++)
                for (int y = 0; y < 5; y++)
                {
                    TempData[x,y] = CurentData[x, y];
                }

        return TempData;
        }

        bool AiTryBlock(List<Point> points)
        {
            simulation = true;
            foreach (Point point in points)
            {
                int[,] TempData = CopyCurentData();
                TempData[point.X, point.Y] = NotCurentPlayer();

                
                if(CheckWinAi(NotCurentPlayer(), TempData))
                {
                    simulation = false;
                    MakeMove(point.X, point.Y, map[point]);
                    Console.WriteLine("block move");
                    return true;
                }
            }
            simulation = false;
            return false;
        }

        bool CheckWinAi(int player, int[,] data)
        {
            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                {
                    if (CheckWin(x, y, player, data))
                    {
                        return true;
                    }
                }
            return false;
        }

        int NotCurentPlayer()
        {
            if (curentPlayer == 1)
                return 2;
            return 1;
        }

        bool TryFindWinMove(List<Point> points)
        {

            simulation = true;
            foreach (Point point in points)
            {
                int[,] TempData = CopyCurentData();
                TempData[point.X, point.Y] = curentPlayer;

                if (CheckWinAi(curentPlayer, TempData))
                {
                    simulation = false;
                    MakeMove(point.X, point.Y, map[point]);
                    Console.WriteLine("win move");
                    return true;
                }
            }
            simulation = false;
            return false;
        }

        void AiMakeRandomMove(List<Point> points)
        {
            int i = rand.Next(points.Count);

            MakeMove(points[i].X, points[i].Y, map[points[i]]);
            Console.WriteLine("random move");
        }

        List<Point> GetAvalibleToMovePoints()
        {
            List<Point> points = new List<Point>();

            for (int y = 1; y < 4; y++)

                for (int x = 1; x < 4; x++)
                {
                    points.Add(new Point(x, y));
                }

            if (moves >= 8)
            {
                points.Add(new Point(2, 4));
                points.Add(new Point(3, 4));
                points.Add(new Point(4, 4));
            }
            if (moves >= 11)
            {
                points.Add(new Point(4, 0));
                points.Add(new Point(4, 1));
                points.Add(new Point(4, 2));
                points.Add(new Point(4, 3));
            }

            for (int y = 0; y < 5; y++)

                for (int x = 0; x < 5; x++)
                {
                    if (points.Contains(new Point(x, y)) && CurentData[x, y] != 0)
                    {
                        points.Remove(new Point(x, y));
                    }
                }

            return points;
        }

        bool PerformWinCheck()
        {
            for (int y = 0; y < 3; y++)
                for (int x = 0; x < 3; x++)
                    if (CheckWin(x, y, curentPlayer))
                    {
                        if(curentPlayer==1)
                            MessageBox.Show("Ви перемогли!!!");
                        if (curentPlayer == 2)
                            MessageBox.Show("Комп'ютер переміг");
                        ResetGame();
                        return true;
                    }
            return false;
        }

        bool CheckWin(int oX, int oY, int player, int[,] data = null)
        {
            if (data == null)
            {
                data = CurentData;
            }
            if (data[oX, oY] == player && data[oX + 1, oY] == player && data[oX + 2, oY] == player)
            {
                ShowWinResult(new Point(oX, oY), new Point(oX + 1, oY), new Point(oX + 2, oY));
                return true;
            }
            else if (data[oX, oY + 1] == player && data[oX + 1, oY + 1] == player && data[oX + 2, oY + 1] == player)
            {
                ShowWinResult(new Point(oX, oY + 1), new Point(oX + 1, oY + 1), new Point(oX + 2, oY + 1));
                return true;
            }
            else if (data[oX, oY + 2] == player && data[oX + 1, oY + 2] == player && data[oX + 2, oY + 2] == player)
            {
                ShowWinResult(new Point(oX, oY + 2), new Point(oX + 1, oY + 2), new Point(oX + 2, oY + 2));
                return true;
            }
            else if (data[oX, oY] == player && data[oX, oY + 1] == player && data[oX, oY + 2] == player)
            {
                ShowWinResult(new Point(oX, oY), new Point(oX, oY + 1), new Point(oX, oY + 2));
                return true;
            }
            else if (data[oX + 1, oY] == player && data[oX + 1, oY + 1] == player && data[oX + 1, oY + 2] == player)
            {
                ShowWinResult(new Point(oX + 1, oY), new Point(oX + 1, oY + 1), new Point(oX + 1, oY + 2));
                return true;
            }
            else if (data[oX + 2, oY] == player && data[oX + 2, oY + 1] == player && data[oX + 2, oY + 2] == player)
            {
                ShowWinResult(new Point(oX + 2, oY), new Point(oX + 2, oY + 1), new Point(oX + 2, oY + 2));
                return true;
            }
            else if (data[oX, oY] == player && data[oX + 1, oY + 1] == player && data[oX + 2, oY + 2] == player)
            {
                ShowWinResult(new Point(oX, oY), new Point(oX + 1, oY + 1), new Point(oX + 2, oY + 2));
                return true;
            }
            else if (data[oX + 2, oY] == player && data[oX + 1, oY + 1] == player && data[oX, oY + 2] == player)
            {
                ShowWinResult(new Point(oX + 2, oY), new Point(oX + 1, oY + 1), new Point(oX, oY + 2));
                return true;
            }


            return false;
        }

        void ShowWinResult(Point p1, Point p2, Point p3)
        {
            if (simulation) return;
            map[p1].BackColor = Color.Green;
            map[p2].BackColor = Color.Green;
            map[p3].BackColor = Color.Green;
        }

        void ResetResult(Point p1)
        {
            try
            {
                map[p1].BackColor = Color.White;
                map[p1].Text = ".";
                moves = 0;
            }
            catch (Exception ex) { }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            moves++;
            AfterMove();
        }

        void ShowDebugData()
        {
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    Console.Write(CurentData[x, y]);
                }
                Console.WriteLine();
            }
        }

        void ResetGame()
        {
            for (int y = 0; y < 5; y++)

                for (int x = 0; x < 5; x++)
                {
                    CurentData[x, y] = 0;
                    ResetResult(new Point(x, y));
                }
            moves = 0;
            finished = true;
            Start();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Гра в хрестики-нолики. 17 Варіант. На початку користувач повинен вибрати чи буде комп'ютер ходити першим. Після 7 та 10 кроків відкриваються додаткові клітинки. \n Южда Богдан ІПЗ 20-1", "about");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Start();
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }
    }
}