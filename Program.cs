using System;
using System.Collections.Generic;

namespace SnakeGame
{
    class Program
    {
        public const int maxTimeToReact = 500;
        public const int screenWidth = 32;
        public const int screenHeight = 16;

        static void Main(string[] args)
		{
			int score = 5;
			bool gameOver = false;
			string movement = "RIGHT";


			Pixel snakeHead = new Pixel
			{
				xPos = screenWidth / 2,
				yPos = screenHeight / 2,
				color = ConsoleColor.Red
			};
			Berry snakeBerry = new Berry();
			List<Pixel> snakeBody = new List<Pixel>();



			prepareConsoleWindow(screenHeight, screenWidth);
			snakeBerry.GenerateNewBerry(screenWidth - 1, screenHeight - 1);

			while (!gameOver)
			{
				Console.Clear();
				DrawBorder();
				setForegroundColorToCyen();
				score = checkIfBerryIsEaten(score, snakeHead, snakeBerry);

				foreach (Pixel bodyPart in snakeBody)
				{
					if (bodyPart.xPos == snakeHead.xPos && bodyPart.yPos == snakeHead.yPos)
					{
						gameOver = true;
						break;
					}
					//TODO: checkIfHEadBodyCollision() a nekde dal vykresleni hada vertikalne
					DrawPixel(bodyPart.xPos, bodyPart.yPos);

				}

				if (gameOver)
					break;

				Console.SetCursorPosition(snakeHead.xPos, snakeHead.yPos);
				Console.ForegroundColor = snakeHead.color;
				Console.Write("■");


				Console.SetCursorPosition(snakeBerry.xPos, snakeBerry.yPos);
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.Write("■");


				if (snakeHead.xPos == screenWidth - 1 || snakeHead.xPos == 0 ||
					snakeHead.yPos == screenHeight - 1 || snakeHead.yPos == 0)
				{
					gameOver = true;
					break;
				}

				DateTime startTime = DateTime.Now;

				while (true)
				{
					DateTime currentTime = DateTime.Now;
					if (currentTime.Subtract(startTime).TotalMilliseconds > maxTimeToReact)
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

			Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
			Console.WriteLine("Game over, Score: " + (score - 5));
			Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 + 1);
		}

		private static void setForegroundColorToCyen()
		{
			
			Console.ForegroundColor = ConsoleColor.Cyan;
		}

		private static int checkIfBerryIsEaten(int score, Pixel snakeHead, Berry snakeBerry)
		{
			if (snakeBerry.IsBerryEaten(snakeHead))
			{
				score++;
				snakeBerry.GenerateNewBerry(screenWidth - 1, screenHeight - 1);
			}

			return score;
		}

		private static void prepareConsoleWindow(int screenHeight, int screenWidth)
		{
			Console.CursorVisible = false;
			Console.SetWindowSize(screenWidth, screenHeight);
			Console.SetBufferSize(screenWidth, screenHeight);
			Console.WindowHeight = screenHeight;
			Console.WindowWidth = screenWidth;
		}

		private static void checkSnakeLenght(int score, List<Pixel> snakeBody)
        {
            if (snakeBody.Count > score)
                snakeBody.RemoveAt(0);
        }

        public static void DrawPixel(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("▄");
        }

		public static void DrawVerticalPixel(int x, int y)
		{
			Console.SetCursorPosition(x, y);
			Console.Write("█");
		}

		private static void DrawBorder()
        {
            for (int i = 0; i < screenWidth; i++)
            {
                DrawPixel(i, 0);
                DrawPixel(i, screenHeight - 1);
            }

			for (int i = 1; i < screenHeight; i++)
			{
				DrawVerticalPixel(0, i);
				DrawVerticalPixel(screenWidth - 1, i);
			}
		}
    }



    class Pixel
    {
        public int xPos { get; set; }
        public int yPos { get; set; }
        public ConsoleColor color { get; set; }

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