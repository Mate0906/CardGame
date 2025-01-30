using System;
using System.IO;

class Program
{
    static void Main()
    {
        Console.Title = "Higher or Lower Játék";
        bool keepPlaying = true;

        while (keepPlaying)
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("\t\tÜdvözlünk a Higher or Lower Játékban!");
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("1 - Játék");
            Console.WriteLine("2 - Információ, készítők");
            Console.WriteLine("3 - Leaderboard");
            Console.WriteLine("4 - Admin mód");
            Console.WriteLine("5 - Kilépés");
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.Write("Válassz egy opciót: ");

            string menuChoice = Console.ReadLine();

            switch (menuChoice)
            {
                case "1":
                    StartGame();
                    break;
                case "2":
                    ShowInfo();
                    break;
                case "3":
                    ShowLeaderboard();
                    break;
                case "4":
                    AdminMode();
                    break;
                case "5":
                    keepPlaying = false;
                    Console.WriteLine("Viszlát! Köszönjük, hogy játszottál!");
                    break;
                default:
                    Console.WriteLine("Érvénytelen választás. Próbáld újra!");
                    break;
            }
        }
    }

    static void AdminMode()
    {
        Console.Clear();
        Console.WriteLine("--------------------------------------------------------------------------------------");
        Console.WriteLine("\t\t\tAdmin Mód - Belépés");
        Console.WriteLine("--------------------------------------------------------------------------------------");
        Console.Write("Add meg az admin jelszót: ");
        string password = Console.ReadLine();

        if (password == "Admin123")
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("\t\t\tAdmin Mód - Aktív");
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("1: Leaderboard törlése\n2: Vissza a főmenübe");
            Console.Write("Választás: ");
            string adminChoice = Console.ReadLine();

            if (adminChoice == "1")
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
        }
        else
        {
            Console.WriteLine("Helytelen jelszó. Visszatérés a főmenübe.");
        }

        Console.WriteLine("\nNyomj Entert a visszatéréshez...");
        Console.ReadLine();
    }

    static void StartGame()
    {
        Console.Clear();
        Console.WriteLine("--------------------------------------------------------------------------------------");
        Console.WriteLine("Kérlek add meg a neved!");
        Console.WriteLine("--------------------------------------------------------------------------------------");
        Console.Write("Név: ");
        string playerName = Console.ReadLine();
        Console.Clear();

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

        Console.WriteLine("--------------------------------------------------------------------------------------");
        Console.WriteLine("Játék vége! Elért pontszám: " + score);
        Console.Write("Nyomj Entert a folytatáshoz...");
        Console.ReadLine();

        string tempFile = "leaderboard_temp.txt";
        bool found = false;

        if (!File.Exists("leaderboard.txt"))
        {
            File.Create("leaderboard.txt").Close();
        }

        using (StreamReader reader = new StreamReader("leaderboard.txt"))
        using (StreamWriter writer = new StreamWriter(tempFile))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 2 && parts[0] == playerName && int.TryParse(parts[1], out int savedScore))
                {
                    if (score > savedScore)
                    {
                        writer.WriteLine(playerName + ":" + score);
                    }
                    else
                    {
                        writer.WriteLine(line);
                    }
                    found = true;
                }
                else
                {
                    writer.WriteLine(line);
                }
            }

            if (!found)
            {
                writer.WriteLine(playerName + ":" + score);
            }
        }

        File.Delete("leaderboard.txt");
        File.Move(tempFile, "leaderboard.txt");
    }

    static void ShowInfo()
    {
        Console.Clear();

        if (!File.Exists("info.txt"))
        {
            using (StreamWriter writer = new StreamWriter("info.txt"))
            {
                writer.WriteLine("Higher or Lower Játék");
                writer.WriteLine("---------------------");
                writer.WriteLine("A játék lényege: Egy számot kapsz, és ki kell találnod, hogy a következő szám nagyobb vagy kisebb lesz.");
                writer.WriteLine("Ha helyesen tippelsz, pontot kapsz. Ha tévedsz, a játék véget ér.");
                writer.WriteLine();
                writer.WriteLine("Szabályok:");
                writer.WriteLine("1. A játék mindig egy véletlenszerű számmal kezdődik 1 és 100 között.");
                writer.WriteLine("2. Minden körben meg kell tippelned, hogy a következő szám nagyobb vagy kisebb lesz.");
                writer.WriteLine("3. Ha helyesen tippelsz, a játék folytatódik és növeled a pontjaidat.");
                writer.WriteLine("4. Ha rosszul tippelsz, a játék véget ér.");
                writer.WriteLine();
                writer.WriteLine("Készítők: Kovács Lajos, Varga Máté");
            }
        }

        Console.WriteLine(File.ReadAllText("info.txt"));
        Console.WriteLine("\nNyomj Entert a visszatéréshez...");
        Console.ReadLine();
    }

    static void PrintCard(int number)
    {
        string numStr = number.ToString().PadLeft(3);
        Console.WriteLine("\t┌─────┐");
        Console.WriteLine("\t|     |");
        Console.WriteLine($"\t| {numStr} |");
        Console.WriteLine("\t|     |");
        Console.WriteLine("\t└─────┘");
    }

    static void ShowLeaderboard()
    {
        Console.Clear();
        Console.WriteLine("--------------------------------------------------------------------------------------");
        Console.WriteLine("\t\t\tLeaderboard");
        Console.WriteLine("--------------------------------------------------------------------------------------");

        if (!File.Exists("leaderboard.txt"))
        {
            Console.WriteLine("Még nincs elérhető toplista.");
        }
        else
        {
            using (StreamReader reader = new StreamReader("leaderboard.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }

        Console.WriteLine("\nNyomj Entert a visszatéréshez...");
        Console.ReadLine();
    }
}
