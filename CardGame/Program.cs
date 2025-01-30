using System;
using System.IO;


Console.WriteLine("Üdvözlünk a Higher or Lower Játékban!");
bool keepPlaying = true;

while (keepPlaying)
{
    Console.Clear();
    Console.WriteLine("--------------------------------------------------------------------------------------");
    Console.WriteLine($"Üdvözlünk a Higher or Lower játékban!");
    Console.WriteLine("Kérlek add meg a neved!");
    Console.WriteLine("--------------------------------------------------------------------------------------");
    Console.Write("Név: ");
    string playerName = Console.ReadLine();
    Console.Clear();

    if (playerName == "Admin")
    {
        Console.WriteLine("--------------------------------------------------------------------------------------");
        Console.WriteLine("\t\t\tADMIN MÓD - Bejelentkezés");
        Console.WriteLine("--------------------------------------------------------------------------------------");
        Console.Write("Admin jelszó: ");
        string password = Console.ReadLine();

        if (password == "Admin123")
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("\t\t\tADMIN MÓD - Aktív");
            Console.WriteLine("--------------------------------------------------------------------------------------");
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

    while (playing)
    {
        Console.Clear();
        Console.WriteLine("--------------------------------------------------------------------------------------");
        Console.WriteLine("Találd ki, hogy a következő szám 'kisebb' vagy 'nagyobb' lesz!");
        Console.WriteLine("Kilépéshez írd: 'kilépés'");
        Console.WriteLine("--------------------------------------------------------------------------------------");
        Console.WriteLine("Jelenlegi számkártya:");
        PrintCard(currentNumber);
        Console.WriteLine();

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
            Console.Clear();
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("TALÁLT!");
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("Következő számkártya:");
            PrintCard(nextNumber);
            Console.WriteLine("Helyes, eltaláltad a számot!");
            score++;
        }
        else
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("\t\tNEM TALÁLT! JÁTÉK VÉGE!");
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("A következő számkártya:");
            PrintCard(nextNumber);
            Console.WriteLine();
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

    Console.WriteLine("--------------------------------------------------------------------------------------");
    Console.WriteLine("Ranglista:");
    Console.WriteLine();

    using (StreamReader reader = new StreamReader("leaderboard.txt"))
    {
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            Console.WriteLine(line);
        }
    }

    Console.WriteLine("--------------------------------------------------------------------------------------");
    Console.WriteLine("");
    Console.Write("Akarsz újra játszani? (igen/nem): ");
    string response = Console.ReadLine().ToLower();
    keepPlaying = response == "igen" || response == "i";
}

Console.WriteLine("Ügyes voltál! Szia!");

static void PrintCard(int number)
{
    string numStr = number.ToString().PadLeft(3);
    Console.WriteLine("\t┌─────┐");
    Console.WriteLine("\t|     |");
    Console.WriteLine($"\t| {numStr} |");
    Console.WriteLine("\t|     |");
    Console.WriteLine("\t└─────┘");
}