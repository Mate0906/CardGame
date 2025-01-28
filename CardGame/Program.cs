﻿using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Üdvözlünk a Higher or Lower Játékban!");
        bool keepPlaying = true;

        while (keepPlaying)
        {
            string playerName = GetPlayerName();

            if (playerName.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                if (AuthenticateAdmin())
                {
                    HandleAdminTasks();
                    continue;
                }
                else
                {
                    Console.WriteLine("Helytelen jelszó. Visszatérés a főmenübe.");
                    continue;
                }
            }

            int score = PlayGame();
            SaveScoreToFile(playerName, score);
            DisplayLeaderboardFromFile();
            keepPlaying = AskToPlayAgain();
        }

        Console.WriteLine("Ügyes voltál! Szia!");
    }

    static string GetPlayerName()
    {
        Console.Write("Kérem, add meg a neved: ");
        return Console.ReadLine();
    }

    static bool AuthenticateAdmin()
    {
        Console.Write("Admin jelszó: ");
        string password = Console.ReadLine();
        return password == "Admin123";
    }

    static void HandleAdminTasks()
    {
        Console.WriteLine("Admin mód aktiválva.");
        Console.WriteLine("1: Leaderboard törlése\n2: Vissza a főmenübe");
        Console.Write("Választás: ");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            ClearLeaderboard();
        }
        else
        {
            Console.WriteLine("Visszatérés a főmenübe.");
        }
    }

    static void ClearLeaderboard()
    {
        string filePath = "leaderboard.txt";

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Console.WriteLine("Leaderboard sikeresen törölve.");
        }
        else
        {
            Console.WriteLine("A leaderboard fájl nem létezik.");
        }
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
            Console.WriteLine($"Jelenlegi szám: {currentNumber}");
            string guess;

            do
            {
                Console.Write("Tipp: ");
                guess = Console.ReadLine().ToLower();

                if (guess != "nagyobb" && guess != "kisebb" && guess != "kilépés")
                {
                    Console.WriteLine("Érvénytelen bemenet. Kérlek írd, hogy 'nagyobb', 'kisebb' vagy 'kilépés'.");
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

    static void DisplayLeaderboardFromFile()
    {
        string filePath = "leaderboard.txt";

        if (File.Exists(filePath))
        {
            Console.WriteLine("\nLeaderboard:");

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
        else
        {
            Console.WriteLine("No leaderboard data found.");
        }
    }

    static bool AskToPlayAgain()
    {
        Console.Write("Akarsz újra játszani? (igen/nem): ");
        string response = Console.ReadLine().ToLower();
        return response == "igen" || response == "i";
    }
}