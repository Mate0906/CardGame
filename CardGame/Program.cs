﻿Console.WriteLine("Üdvözlünk a Higher or Lower Játékban!");
bool keepPlaying = true;

while (keepPlaying)
{
    Console.Write("Kérem, add meg a neved: ");
    string playerName = Console.ReadLine();

    if (playerName == "Admin")
    {
        Console.Write("Admin jelszó: ");
        string password = Console.ReadLine();

        if (password == "Admin123")
        {
            Console.WriteLine("Admin mód aktiválva.");
            Console.WriteLine("1: Leaderboard törlése\n2: Vissza a főmenübe");
            Console.Write("Választás: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                if (File.Exists("leaderboard.txt"))
                {
                    File.Delete("leaderboard.txt");
                    Console.WriteLine("Leaderboard sikeresen törölve.");
                }
                else
                {
                    Console.WriteLine("A leaderboard fájl nem létezik.");
                }
            }
            else
            {
                Console.WriteLine("Visszatérés a főmenübe.");
            }
            continue;
        }
        else
        {
            Console.WriteLine("Helytelen jelszó. Visszatérés a főmenübe.");
            continue;
        }
    }

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


    if (!File.Exists("leaderboard.txt"))
    {
        File.Create("leaderboard.txt").Close();
    }

    string[] lines = File.ReadAllLines("leaderboard.txt");
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
        using (StreamWriter writer = new StreamWriter("leaderboard.txt", true))
        {
            writer.WriteLine($"{playerName}:{score}");
        }
    }
    else
    {
        File.WriteAllLines("leaderboard.txt", lines);
    }

    if (File.Exists("leaderboard.txt"))
    {
        Console.WriteLine("\nLeaderboard:");

        using (StreamReader reader = new StreamReader("leaderboard.txt"))
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

    Console.Write("Akarsz újra játszani? (igen/nem): ");
    string response = Console.ReadLine().ToLower();
    keepPlaying = response == "igen" || response == "i";
}

Console.WriteLine("Ügyes voltál! Szia!");