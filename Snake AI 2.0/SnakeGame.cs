
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Snake_AI_2._0
{
    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    };

    public class SnakeGame : IComparable<SnakeGame>
    {
        private const int WIDTH = 20;
        private const int HEIGHT = 20;

        public static Random rand = new Random();

        public List<Point> snake = new List<Point>();
        private List<Point> possibleFoodPoints = new List<Point>();
        public Point food = new Point();

        private volatile bool gameOver;

        private Direction direction;

        private NeuralNetwork net;
        private int[] layers = new int[] { 4, 12, 12, 3 };
        private float[] inputs = new float[4];
        private float[] outputs = new float[3];

        private int time;

        List<int> pastDirs = new List<int>();

        private float dist;
        /*
         * 12 input layers
         * 4 direction - distance to food - distance to tail - distance to wall
         * */

        public SnakeGame()
        {
            dist = 999f;
            time = 100;
            pastDirs.Clear();

            gameOver = false;

            direction = Direction.Up;


            snake.Clear();
            snake.Add(new Point(10, 10));

            possibleFoodPoints.Clear();
            for (int i = 0; i < HEIGHT; i++)
                for (int j = 0; j < WIDTH; j++)
                    possibleFoodPoints.Add(new Point(j, i));
            possibleFoodPoints.Remove(snake[snake.Count - 1]);

            GenerateFood();

            net = new NeuralNetwork(layers);


        }

        public SnakeGame(SnakeGame copy)
        {
            dist = 999f;

            pastDirs.Clear();

            gameOver = false;

            direction = Direction.Up;
            time = 100;


            snake.Clear();
            snake.Add(new Point(10, 10));

            possibleFoodPoints.Clear();
            for (int i = 0; i < HEIGHT; i++)
                for (int j = 0; j < WIDTH; j++)
                    possibleFoodPoints.Add(new Point(j, i));
            possibleFoodPoints.Remove(snake[snake.Count - 1]);

            GenerateFood();

            net = new NeuralNetwork(copy.GetNet());
            net.Mutate();

        }

      

        public void Reset()
        {
            dist = 999f;

            gameOver = false;
            pastDirs.Clear();

            direction = Direction.Up;

            time = 100;

            snake.Clear();
            snake.Add(new Point(10, 10));

            possibleFoodPoints.Clear();
            for (int i = 0; i < HEIGHT; i++)
                for (int j = 0; j < WIDTH; j++)
                    possibleFoodPoints.Add(new Point(j, i));
            possibleFoodPoints.Remove(snake[snake.Count - 1]);

            GenerateFood();
            net = new NeuralNetwork(net);
        }

        public void ResetCopyNet(NeuralNetwork copyNet)
        {
            dist = 999f;

            gameOver = false;

            direction = Direction.Up;
            pastDirs.Clear();
            time = 100;


            snake.Clear();
            snake.Add(new Point(10, 10));

            possibleFoodPoints.Clear();
            for (int i = 0; i < HEIGHT; i++)
                for (int j = 0; j < WIDTH; j++)
                    possibleFoodPoints.Add(new Point(j, i));
            possibleFoodPoints.Remove(snake[snake.Count - 1]);

            GenerateFood();
            net = new NeuralNetwork(copyNet);

            net.Mutate();
        }

        public void Play()
        {
            while (true)
            {

                if (gameOver)
                {
                    return;
                }

                Simulation();
            }

        }

        public void Watch()
        {
            gameOver = false;
            direction = Direction.Up;
            time = 100;

            snake.Clear();
            snake.Add(new Point(10, 10));

            possibleFoodPoints.Clear();
            for (int i = 0; i < HEIGHT; i++)
                for (int j = 0; j < WIDTH; j++)
                    possibleFoodPoints.Add(new Point(j, i));
            possibleFoodPoints.Remove(snake[snake.Count -1]);

            GenerateFood();

        }


        private void GenerateFood()
        {
            Random test = new Random();
            var y = possibleFoodPoints.Count;
            int x = test.Next(possibleFoodPoints.Count);
            food = possibleFoodPoints[test.Next(possibleFoodPoints.Count)];
            dist = GetDistToFood();

        }

        public float GetAngleToFood()
        {

            //if (food.X < snake[snake.Count - 1].X)
                return (float)Math.Atan((((double)(food.Y - snake[snake.Count - 1].Y)) / (food.X - snake[snake.Count - 1].X)) + Math.PI / 2 * (int)direction);
            //else
                return (float)Math.Atan((((double)(food.Y - snake[snake.Count - 1].Y)) / (food.X - snake[snake.Count - 1].X)) /*+ Math.PI / 2*/);

        }
        public float GetDistToFood()
        {
            return (float)Math.Sqrt(Math.Pow((food.X - snake[^1].X), 2) + Math.Pow((food.Y - snake[^1].Y), 2));
        }
        public void Simulation()
        {

            //setting inputs and then recieving networks decision
            SetInputs();
            outputs = net.FeedForward(inputs);

            //picking move based on decision
            switch (Decision(outputs))
            {
                case 0:
                    direction = (Direction)(((int)direction - 1) % 4);
                    break;
                case 1:
                    break;
                case 2:
                    direction = (Direction)(((int)direction + 1) % 4);
                    break;
            }

            Point newHead;

            switch (direction)
            {
                case Direction.Right:
                    newHead = new Point(snake[snake.Count - 1].X + 1, snake[snake.Count - 1].Y);

                    if (food.Equals(newHead))
                    {
                        net.AddFitness(5);
                        time += 50;
                        snake.Add(newHead);
                        possibleFoodPoints.Remove(food);
                        GenerateFood();
                    }
                    else if (newHead.X >= WIDTH || snake.Contains(newHead))
                    {
                        Die();

                    }
                    else
                    {
                        snake.Add(newHead);
                        possibleFoodPoints.Add(snake[0]);
                        possibleFoodPoints.Remove(snake[snake.Count - 1]);
                        snake.RemoveAt(0);

                    }
                    break;
                case Direction.Left:
                    newHead = new Point(snake[snake.Count - 1].X - 1, snake[snake.Count - 1].Y);
                    if (food.Equals(newHead))
                    {
                        net.AddFitness(5);
                        time += 50;
                        snake.Add(newHead);
                        possibleFoodPoints.Remove(food);
                        GenerateFood();
                    }
                    else if (newHead.X < 0 || snake.Contains(newHead))
                    {
                        Die();

                    }
                    else
                    {
                        snake.Add(newHead);
                        possibleFoodPoints.Add(snake[0]);
                        possibleFoodPoints.Remove(snake[snake.Count - 1]);
                        snake.RemoveAt(0);

                    }
                    break;
                case Direction.Up:
                    newHead = new Point(snake[snake.Count - 1].X, snake[snake.Count - 1].Y - 1);
                    if (food.Equals(newHead))
                    {
                        net.AddFitness(5);
                        time += 50;
                        snake.Add(newHead);
                        GenerateFood();
                        possibleFoodPoints.Remove(food);

                    }
                    else if (newHead.Y < 0 || snake.Contains(newHead))
                    {
                        Die();

                    }
                    else
                    {
                        snake.Add(newHead);
                        possibleFoodPoints.Add(snake[0]);
                        possibleFoodPoints.Remove(snake[snake.Count - 1]);
                        snake.RemoveAt(0);

                    }
                    break;
                case Direction.Down:
                    newHead = new Point(snake[snake.Count - 1].X, snake[snake.Count - 1].Y + 1);
                    if (food.Equals(newHead))
                    {
                        net.AddFitness(5);
                        time += 50;
                        snake.Add(newHead);
                        possibleFoodPoints.Remove(food);
                        GenerateFood();
                    }
                    else if (newHead.Y >= HEIGHT || snake.Contains(newHead))
                    {
                        Die();

                    }
                    else
                    {
                        snake.Add(newHead);
                        possibleFoodPoints.Add(snake[0]);
                        possibleFoodPoints.Remove(snake[snake.Count - 1]);
                        snake.RemoveAt(0);

                    }
                    break;
            }
            if (dist > GetDistToFood())
            {
                net.AddFitness(3);
                dist = GetDistToFood();
            }
            net.AddFitness(1);

            time--;
            if (time <= 0)
            {
                Die();

            }


        }

        public void Move()
        {

            //setting inputs and then recieving networks decision
            SetInputs();
            outputs = net.FeedForward(inputs);

            //picking move based on decision
            switch (Decision(outputs))
            {
                case 0:
                    direction = (Direction)((((int)direction - 1) + 4) % 4);
                    pastDirs.Add(0);

                    break;
                case 1:

                    break;
                case 2:
                    direction = (Direction)(((int)direction + 1) % 4);
                    pastDirs.Add(1);

                    break;
            }

            Point newHead;

            switch (direction)
            {
                case Direction.Right:
                    newHead = new Point(snake[snake.Count - 1].X + 1, snake[snake.Count - 1].Y);

                    if (food.Equals(newHead))
                    {
                        net.AddFitness(50);
                        time += 50;
                        snake.Add(newHead);
                        possibleFoodPoints.Remove(food);
                        GenerateFood();
                    }
                    else if (newHead.X >= WIDTH || snake.Contains(newHead))
                    {
                        Die();

                    }
                    else
                    {
                        snake.Add(newHead);
                        possibleFoodPoints.Add(snake[0]);
                        possibleFoodPoints.Remove(snake[snake.Count - 1]);
                        snake.RemoveAt(0);

                    }
                    break;
                case Direction.Left:
                    newHead = new Point(snake[snake.Count - 1].X - 1, snake[snake.Count - 1].Y);
                    if (food.Equals(newHead))
                    {
                        net.AddFitness(50);
                        time += 50;
                        snake.Add(newHead);
                        possibleFoodPoints.Remove(food);
                        GenerateFood();
                    }
                    else if (newHead.X < 0 || snake.Contains(newHead))
                    {
                        Die();

                    }
                    else
                    {
                        snake.Add(newHead);
                        possibleFoodPoints.Add(snake[0]);
                        possibleFoodPoints.Remove(snake[snake.Count - 1]);
                        snake.RemoveAt(0);

                    }
                    break;
                case Direction.Up:
                    newHead = new Point(snake[snake.Count - 1].X, snake[snake.Count - 1].Y - 1);
                    if (food.Equals(newHead))
                    {
                        net.AddFitness(50);
                        time += 50;
                        snake.Add(newHead);
                        GenerateFood();
                        possibleFoodPoints.Remove(food);

                    }
                    else if (newHead.Y < 0 || snake.Contains(newHead))
                    {
                        Die();

                    }
                    else
                    {
                        snake.Add(newHead);
                        possibleFoodPoints.Add(snake[0]);
                        possibleFoodPoints.Remove(snake[snake.Count - 1]);
                        snake.RemoveAt(0);

                    }
                    break;
                case Direction.Down:
                    newHead = new Point(snake[snake.Count - 1].X, snake[snake.Count - 1].Y + 1);
                    if (food.Equals(newHead))
                    {
                        net.AddFitness(50);
                        time += 50;
                        snake.Add(newHead);
                        possibleFoodPoints.Remove(food);
                        GenerateFood();
                    }
                    else if (newHead.Y >= HEIGHT || snake.Contains(newHead))
                    {
                        Die();

                    }
                    else
                    {
                        snake.Add(newHead);
                        possibleFoodPoints.Add(snake[0]);
                        possibleFoodPoints.Remove(snake[snake.Count - 1]);
                        snake.RemoveAt(0);

                    }
                    break;
            }
            if (dist > GetDistToFood())
            {
                net.AddFitness(3);
                dist = GetDistToFood();
            }
            net.AddFitness(1);

            time--;
            if (time <= 0)
            {
                Die();

            }
            if (pastDirs.Count > 3)
                if (pastDirs[pastDirs.Count - 2] == pastDirs[^1] && pastDirs[pastDirs.Count - 3] == pastDirs[^1] && pastDirs[pastDirs.Count - 4] == pastDirs[^1])
                    Die();


        }


        private void SetInputs()
        {

            Point possibleLocation;
            for (int i = -1; i < 2; i++)
                switch ((Direction)(((int)direction + i) % 4))
                {
                    case Direction.Right:
                        possibleLocation = new Point(snake[snake.Count - 1].X + 1, snake[snake.Count - 1].Y);
                        if (possibleLocation.X >= WIDTH || snake.Contains(possibleLocation))
                            inputs[i + 1] = 4;
                        else
                            inputs[i + 1] = -4;
                        break;
                    case Direction.Left:
                        possibleLocation = new Point(snake[snake.Count - 1].X - 1, snake[snake.Count - 1].Y);
                        if (possibleLocation.X < 0 || snake.Contains(possibleLocation))
                            inputs[i + 1] = 4;
                        else
                            inputs[i + 1] = -4;
                        break;

                    case Direction.Up:
                        possibleLocation = new Point(snake[snake.Count - 1].X, snake[snake.Count - 1].Y - 1);
                        if (possibleLocation.Y < 0 || snake.Contains(possibleLocation))
                            inputs[i + 1] = 4;
                        else
                            inputs[i + 1] = -4;
                        break;

                    case Direction.Down:
                        possibleLocation = new Point(snake[snake.Count - 1].X, snake[snake.Count - 1].Y + 1);
                        if (possibleLocation.Y >= HEIGHT || snake.Contains(possibleLocation))
                            inputs[i + 1] = 4;
                        else
                            inputs[i + 1] = -4;
                        break;


                }

            inputs[3] = GetAngleToFood();

        }

        private int Decision(float[] outputData)
        {


            int temp = 0;
            for (int i = 0; i < outputData.Length; i++)
                if (outputData[i] > outputData[temp])
                    temp = i;
            return temp;

        }



        public void Die()
        {
            bool test;
            if (net.GetFitness() <= 3 || net.GetFitness() % 100 == 0 && net.GetFitness() != 100)
                 test = true;
            gameOver = true;


        }

        public bool GetGameOver()
        {
            return gameOver;
        }

        public NeuralNetwork GetNet()
        {

            return net;

        }
        public int GetLen()
        {

            return snake.Count;

        }

        public void Mutate()
        {

            net.Mutate();

        }

        public int CompareTo(SnakeGame obj)
        {
            return this.net.CompareTo(obj.net);

        }
    }
}