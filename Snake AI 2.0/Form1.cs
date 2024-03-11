using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake_AI_2._0
{
    public partial class Form1 : Form
    {
        public const int POP = 70000;


        public SnakeGame[] snakes = new SnakeGame[POP];

        SnakeGame snake;
        SnakeGame snakeToWatch;


        public bool snakeIsDead = true;
        private static bool prep = true;
        private static bool isLearning = false;
        private static bool isPreping = false;

        List<string> lines = new List<string>();

        Task run;


        public int generation;
        public float bestFitness;

        List<Task> learning = new List<Task>();
        List<Task> breeding = new List<Task>();

        int wait = 0;

        public Form1()
        {
            InitializeComponent();

            //Set settings to default

            //Set game speed and start timer
            gameTimer.Interval = 1200 / 16;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

        }



        public async Task Gen()
        {
            learning.Clear();
            generation++;
            prep = false;
            isLearning = true;
            foreach (SnakeGame snake in snakes)
                learning.Add(Task.Run(() => snake.Play()));

            await Task.WhenAll(learning);
            
            isLearning = false;
        }
        public async Task GenPrep()
        {
            breeding.Clear();
            isPreping = true; ;
            //ExportNetworks(snakes);
            
            Array.Sort(snakes);
            ExportData();
            bestFitness = snakes[0].GetNet().GetFitness();

            for (int i = 0; i < POP / 2; i++)
            {
                int idx = i;
                breeding.Add(Task.Run(() => UpdateSnake(idx)));
            }

            await Task.WhenAll(breeding);
            prep = true;
            isPreping = false;
            snakeToWatch = new SnakeGame(snakes[0]);
        }

        private void UpdateSnake(int idx)
        {
            snakes[POP / 2 + idx].ResetCopyNet(snakes[idx].GetNet());
            snakes[idx].Reset();
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            pbCanvas.Invalidate();

        }

        public void ExportData()
        {
            lines.Clear();
            foreach (SnakeGame snake in snakes)
                lines.Add(snake.GetNet().GetFitness().ToString());
            File.WriteAllLines(string.Format("Generation{0}.txt", generation % 100), lines.ToArray());
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {

            bestFit.Text = bestFitness.ToString();
            genCount.Text = generation.ToString();
            Graphics canvas = e.Graphics;
            if (snakeToWatch != null)
            {
                if (snakeIsDead)
                {
                    snake = new SnakeGame(snakeToWatch);
                    snake.Watch();
                    snakeIsDead = false;
                }
                else if (!snake.GetGameOver())
                {
                    curFit.Text = String.Format("Dist: {0}\n Angle: {1}\nFit: {2}",snake.GetDistToFood().ToString(),snake.GetAngleToFood().ToString(), snake.GetNet().GetFitness().ToString());
                    for (int i = 0; i < snake.snake.Count; i++)
                    {
                        Brush snakeColour;
                        if (i == snake.snake.Count - 1)
                            snakeColour = Brushes.Black;     //Draw head
                        else
                            snakeColour = Brushes.Green;    //Rest of body

                        //Draw snake
                        canvas.FillEllipse(snakeColour,
                            new Rectangle(snake.snake[i].X * 20,
                                          snake.snake[i].Y * 20,
                                          20, 20));


                        //Draw Food
                        canvas.FillEllipse(Brushes.Red,
                            new Rectangle(snake.food.X * 20,
                                 snake.food.Y * 20, 20, 20));

                    }
                    snake.Move();
                }
                else
                {
                    snakeIsDead = true;
                }
            }


        }

        public void ExportNetworks(SnakeGame[] nets)
        {
            string netLocation = @"C:\Users\The Beast\Desktop\EVERYTHING\Programming\Visual Studio\Snake\Snake AI\DATA\NETWORKS";
            string filename;
            List<string> networkString = new List<string>();

            for (int i = 0; i < POP / 10; i++)
            {
                filename = "net" + i.ToString() + ".txt";
                filename = Path.Combine(netLocation, filename);

                for (int j = 0; j < nets[i].GetNet().weights.Length; j++)
                {
                    for (int k = 0; k < nets[i].GetNet().weights[j].Length; k++)
                    {
                        for (int l = 0; l < nets[i].GetNet().weights[j][k].Length; l++)
                        {
                            networkString.Add(nets[i].GetNet().weights[j][k][l].ToString());
                        }
                        networkString.Add(Environment.NewLine);
                    }
                    networkString.Add(Environment.NewLine);
                }

                File.WriteAllLines(filename, networkString);
                networkString.Clear();

            }

        }

        public void ImportNetwork()
        {

        }
        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {

            System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
            messageBoxCS.AppendFormat("{0} = {1}", "CloseReason", e.CloseReason);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Cancel", e.Cancel);
            messageBoxCS.AppendLine();
            MessageBox.Show(messageBoxCS.ToString(), "FormClosing Event");

        }


        private void genration_Click(object sender, EventArgs e)
        {

            if (!isLearning && !isPreping)
            {
                if (generation == 0)
                    Gen();
                else if (prep)
                    Gen();
                else
                    GenPrep();
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < POP; i++)
                snakes[i] = new SnakeGame();
            snake = new SnakeGame(snakes[0]);

        }

        private async void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            while (checkBox1.Checked)
            {
                if (!isLearning && !isPreping)
                {
                    if (generation == 0)
                        await Gen();
                    else if (prep)
                        await Gen();
                    else
                        await GenPrep();
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            snakeIsDead = true;
        }
    }
}