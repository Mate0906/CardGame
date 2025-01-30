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
            Console.WriteLine("3 - Kilépés");
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
                    keepPlaying = false;
                    Console.WriteLine("Viszlát! Köszönjük, hogy játszottál!");
                    break;
                default:
                    Console.WriteLine("Érvénytelen választás. Próbáld újra!");
                    break;
            }
        }
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
                return;
            }
            else
            {
                Console.WriteLine("Helytelen jelszó. Visszatérés a főmenübe.");
                return;
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

        Console.WriteLine("--------------------------------------------------------------------------------------");
        Console.WriteLine("Játék vége! Elért pontszám: " + score);
        Console.Write("Nyomj Entert a folytatáshoz...");
        Console.ReadLine();
    }

    static void ShowInfo()
    {
        Console.Clear();
        string fileName = "info.txt";

        if (!File.Exists(fileName))
        {
            using (StreamWriter writer = new StreamWriter(fileName))
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

        Console.WriteLine(File.ReadAllText(fileName));
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
}