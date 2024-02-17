using System;
using System.Collections.Generic;

namespace SnakeGame
{
	class Program
	{
		public const int maxTimeToReact = 500;
		public const int screenWidth = 32;
		public const int screenHeight = 16;

		static Pixel snakeHead = new Pixel
		{
			xPos = screenWidth / 2,
			yPos = screenHeight / 2,
			color = ConsoleColor.Red
		};
		static Berry snakeBerry = new Berry();
		static List<Pixel> snakeBody = new List<Pixel>();

		static void Main(string[] args)
		{
			int score = 5;
			bool gameOver = false;
			string movement = "RIGHT";

			prepareConsoleWindow(screenHeight, screenWidth);
			snakeBerry.generateNewBerry(screenWidth - 1, screenHeight - 1);

			while (!gameOver)
			{
				Console.Clear();
				drawBorder();
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
					drawVerticalPixel(bodyPart.xPos, bodyPart.yPos);

				}

				if (gameOver)
					break;

				renderGame(movement);

				if (snakeHead.xPos == screenWidth - 1 || snakeHead.xPos == 0 ||
					snakeHead.yPos == screenHeight - 1 || snakeHead.yPos == 0)
				{
					gameOver = true;
					break;
				}

				movement = waitForInput(movement);
				snakeBody.Add(new Pixel { xPos = snakeHead.xPos, yPos = snakeHead.yPos });
				moveSnake(movement);
				checkSnakeLenght(score, snakeBody);
			}
			drawGameOver(score);
		}

		private static void renderGame(string movement)
		{
			Console.SetCursorPosition(snakeHead.xPos, snakeHead.yPos);
			Console.ForegroundColor = snakeHead.color;
			drawVerticalPixel(snakeHead.xPos, snakeHead.yPos);


			Console.SetCursorPosition(snakeBerry.xPos, snakeBerry.yPos);
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write("■");
		}

		private static void moveSnake(string movement)
		{
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
		}

		private static string waitForInput(string movement)
		{
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

			return movement;
		}

		private static void setForegroundColorToCyen()
		{

			Console.ForegroundColor = ConsoleColor.Cyan;
		}

		private static int checkIfBerryIsEaten(int score, Pixel snakeHead, Berry snakeBerry)
		{
			if (snakeBerry.checkIfBerryIsEaten(snakeHead))
			{
				score++;
				snakeBerry.generateNewBerry(screenWidth - 1, screenHeight - 1);
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

		public static void drawSquarePixel(int x, int y)
		{
			Console.SetCursorPosition(x, y);
			Console.Write("▄");
		}

		public static void drawVerticalPixel(int x, int y)
		{
			Console.SetCursorPosition(x, y);
			Console.Write("█");
		}

		private static void drawBorder()
		{
			for (int i = 0; i < screenWidth; i++)
			{
				drawSquarePixel(i, 0);
				drawSquarePixel(i, screenHeight - 1);
			}

			for (int i = 1; i < screenHeight; i++)
			{
				drawVerticalPixel(0, i);
				drawVerticalPixel(screenWidth - 1, i);
			}
		}

		private static void drawGameOver(int score)
		{
			int x = screenWidth / 5;
			int y = screenHeight / 2;

			Console.SetCursorPosition(x, y);
			Console.WriteLine($"Game over, Score: {score - 5}");
			Console.SetCursorPosition(x, y + 1);
			// Další vylepšení můžete přidat tady...
			//toz asi bych smazal zbytecny text pod tim a dal to na stred nejak?
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

		public void generateNewBerry(int screenWidth, int screenHeight)
		{
			Random randomnum = new Random();
			xPos = randomnum.Next(1, screenWidth);
			yPos = randomnum.Next(1, screenHeight);
		}

		public bool checkIfBerryIsEaten(Pixel snakeHead)
		{
			return xPos == snakeHead.xPos && yPos == snakeHead.yPos;
		}
	}
}