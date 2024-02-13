using System;
using System.Collections.Generic;

namespace SnakeGame
{
    class Program
    {
        public const int MaxTimeToReact = 500;
        public const int ScreenWidth = 32;
        public const int ScreenHeight = 16;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(32, 16);
            Console.SetBufferSize(32, 16);
            Console.WindowHeight = ScreenHeight;
            Console.WindowWidth = ScreenWidth;

            Random random = new Random();
            int score = 5;
            bool gameOver = false;

            Pixel snakeHead = new Pixel
            {
                xPos = ScreenWidth / 2,
                yPos = ScreenHeight / 2,
                Color = ConsoleColor.Red
            };

            Berry snakeBerry = new Berry();
            snakeBerry.GenerateNewBerry(ScreenWidth - 1, ScreenHeight - 1);

            List<Pixel> snakeBody = new List<Pixel>();

            string movement = "RIGHT";

            while (!gameOver)
            {
                Console.Clear();
                DrawBorder();
                Console.ForegroundColor = ConsoleColor.Green;

                if (snakeBerry.IsBerryEaten(snakeHead))
                {
                    score++;
                    snakeBerry.GenerateNewBerry(ScreenWidth - 1, ScreenHeight - 1);
                }

                foreach (Pixel bodyPart in snakeBody)
                {
                    if (bodyPart.xPos == snakeHead.xPos && bodyPart.yPos == snakeHead.yPos)
                    {
                        gameOver = true;
                        break;
                    }

                    DrawPixel(bodyPart.xPos, bodyPart.yPos);

                }

                if (gameOver)
                    break;

                Console.SetCursorPosition(snakeHead.xPos, snakeHead.yPos);
                Console.ForegroundColor = snakeHead.Color;
                Console.Write("■");


                Console.SetCursorPosition(snakeBerry.xPos, snakeBerry.yPos);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("■");


                if (snakeHead.xPos == ScreenWidth - 1 || snakeHead.xPos == 0 ||
                    snakeHead.yPos == ScreenHeight - 1 || snakeHead.yPos == 0)
                {
                    gameOver = true;
                    break;
                }

                DateTime startTime = DateTime.Now;

                while (true)
                {
                    DateTime currentTime = DateTime.Now;
                    if (currentTime.Subtract(startTime).TotalMilliseconds > MaxTimeToReact)
                        break;

                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        if (keyInfo.Key == ConsoleKey.UpArrow && movement != "DOWN")
                            movement = "UP";
                        if (keyInfo.Key == ConsoleKey.DownArrow && movement != "UP")
                            movement = "DOWN";
                        if (keyInfo.Key == ConsoleKey.LeftArrow && movement != "RIGHT")
                            movement = "LEFT";
                        if (keyInfo.Key == ConsoleKey.RightArrow && movement != "LEFT")
                            movement = "RIGHT";
                    }
                }

                snakeBody.Add(new Pixel { xPos = snakeHead.xPos, yPos = snakeHead.yPos });

                switch (movement)
                {
                    case "UP":
                        snakeHead.yPos--;
                        break;
                    case "DOWN":
                        snakeHead.yPos++;
                        break;
                    case "LEFT":
                        snakeHead.xPos--;
                        break;
                    case "RIGHT":
                        snakeHead.xPos++;
                        break;
                }

                checkSnakeLenght(score, snakeBody);
            }

            Console.SetCursorPosition(ScreenWidth / 5, ScreenHeight / 2);
            Console.WriteLine("Game over, Score: " + (score - 5));
            Console.SetCursorPosition(ScreenWidth / 5, ScreenHeight / 2 + 1);
        }

        private static void checkSnakeLenght(int score, List<Pixel> snakeBody)
        {
            if (snakeBody.Count > score)
                snakeBody.RemoveAt(0);
        }

        public static void DrawPixel(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("■");
        }

        private static void DrawBorder()
        {
            for (int i = 0; i < ScreenWidth; i++)
            {
                DrawPixel(i, 0);
                DrawPixel(i, ScreenHeight - 1);
            }

            for (int i = 0; i < ScreenHeight; i++)
            {
                DrawPixel(0, i);
                DrawPixel(ScreenWidth - 1, i);
            }
        }
    }



    class Pixel
    {
        public int xPos { get; set; }
        public int yPos { get; set; }
        public ConsoleColor Color { get; set; }

    }

    class Berry
    {
        public int xPos { get; set; }
        public int yPos { get; set; }

        public void GenerateNewBerry(int screenWidth, int screenHeight)
        {
            Random randomnum = new Random();
            xPos = randomnum.Next(1, screenWidth);
            yPos = randomnum.Next(1, screenHeight);
        }

        public bool IsBerryEaten(Pixel snakeHead)
        {
            return xPos == snakeHead.xPos && yPos == snakeHead.yPos;
        }
    }
}