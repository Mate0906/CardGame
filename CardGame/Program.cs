﻿using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Üdvözlünk a Higher or Lower Játékban!");
        bool keepPlaying = true;

        while (keepPlaying)
        {
            string playerName = GetPlayerName();
            int score = PlayGame();
            SaveScoreToFile(playerName, score);
            Console.WriteLine($"{playerName}, a pontszámod: {score}");
            keepPlaying = false; // Ideiglenes érték a teszteléshez
        }

        Console.WriteLine("Köszönjük a játékot! Viszontlátásra!");
    }

    static string GetPlayerName()
    {
        Console.Write("Kérem, add meg a neved: ");
        return Console.ReadLine();
    }

    static int PlayGame()
    {
        Random random = new Random();
        int currentNumber = random.Next(1, 101);
        int score = 0;
        bool playing = true;

        Console.WriteLine("Találd ki, hogy a következő szám nagyobb vagy kisebb lesz!");
        Console.WriteLine("Írd, hogy 'nagyobb', 'kisebb', vagy 'kilépés' a kilépéshez.");

        while (playing)
        {
            Console.WriteLine($"Current number: {currentNumber}");
            string guess;

            do
            {
                Console.Write("Your guess: ");
                guess = Console.ReadLine().ToLower();

                if (guess != "nagyobb" && guess != "kisebb" && guess != "kilépés")
                {
                    Console.WriteLine("Érvénytelen bemenet. Kérlek írd, hogy 'nagyobb', 'kisebb', vagy 'kilépés'.");
                }

            } while (guess != "nagyobb" && guess != "kisebb" && guess != "kilépés");

            if (guess == "kilépés")
            {
                playing = false;
                break;
            }

            int nextNumber = random.Next(1, 101);

            if ((guess == "nagyobb" && nextNumber > currentNumber) ||
                (guess == "kisebb" && nextNumber < currentNumber))
            {
                Console.WriteLine($"Következő szám: {nextNumber}");
                Console.WriteLine("Helyes, eltaláltad a számot!");
                score++;
            }
            else
            {
                Console.WriteLine($"Következő szám: {nextNumber}");
                Console.WriteLine("Nem talált! Játék vége.");
                playing = false;
            }

            currentNumber = nextNumber;
        }

        return score;
    }

    static void SaveScoreToFile(string playerName, int score)
    {
        string filePath = "leaderboard.txt";

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }

        string[] lines = File.ReadAllLines(filePath);
        bool found = false;

        for (int i = 0; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(':');
            if (parts[0] == playerName)
            {
                int existingScore = int.Parse(parts[1]);
                if (score > existingScore)
                {
                    lines[i] = $"{playerName}:{score}";
                }
                found = true;
                break;
            }
        }

        if (!found)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{playerName}:{score}");
            }
        }
        else
        {
            File.WriteAllLines(filePath, lines);
        }
    }
}
