using System;
using System.IO;

        Console.Title = "Higher or Lower Játék";
        bool tovabbJatszik = true;

        while (tovabbJatszik)
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("\t\tÜdvözlünk a Higher or Lower Játékban!");
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("1 - Játék");
            Console.WriteLine("2 - Információ, készítők");
            Console.WriteLine("3 - Ranglista");
            Console.WriteLine("4 - Admin mód");
            Console.WriteLine("5 - Kilépés");
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.Write("Válassz egy opciót: ");

            string menu = Console.ReadLine()!;

            switch (menu)
            {
                case "1":
                    JatekElkezdese();
                    break;
                case "2":
                    Informaciok();
                    break;
                case "3":
                    Ranglista();
                    break;
                case "4":
                    AdminMod();
                    break;
                case "5":
                    tovabbJatszik = false;
                    Console.WriteLine("Viszlát! Köszönjük, hogy játszottál!");
                    break;
                default:
                    Console.WriteLine("Érvénytelen választás. Próbáld újra!");
                    break;
            }
        }

     void AdminMod()
    {
        Console.Clear();
        Console.WriteLine("--------------------------------------------------------------------------------------");
        Console.WriteLine("\t\t\tAdmin Mód - Belépés");
        Console.WriteLine("--------------------------------------------------------------------------------------");
        Console.Write("Add meg az admin jelszót: ");
        string jelszo = Console.ReadLine()!;

        if (jelszo == "Admin123")
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("\t\t\tAdmin Mód - Aktív");
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("1: Ranglista törlése\n2: Vissza a főmenübe");
            Console.Write("Választás: ");
            string adminValasztas = Console.ReadLine()!;

            if (adminValasztas == "1")
            {
                if (File.Exists("ranglista.txt"))
                {
                    File.Delete("ranglista.txt");
                    Console.WriteLine("ranglista sikeresen törölve.");
                }
                else
                {
                    Console.WriteLine("A ranglista fájl nem létezik.");
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

    void JatekElkezdese()
    {
        Console.Clear();
        Console.WriteLine("--------------------------------------------------------------------------------------");
        Console.WriteLine("Kérlek add meg a neved!");
        Console.WriteLine("--------------------------------------------------------------------------------------");
        Console.Write("Név: ");
        string jatekosnev = Console.ReadLine()!;
        Console.Clear();

        Random random = new Random();
        int jelenlegiSzam = random.Next(1, 101);
        int pontszam = 0;
        bool jatszik = true;

        while (jatszik)
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("Találd ki, hogy a következő szám 'kisebb' vagy 'nagyobb' lesz!");
            Console.WriteLine("Kilépéshez írd: 'kilépés'");
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("Jelenlegi számkártya:");
            Kartya(jelenlegiSzam);
            Console.WriteLine();

            string tipp;
            do
            {
                Console.Write("Tipp: ");
                tipp = Console.ReadLine()!.ToLower();

                if (tipp != "nagyobb" && tipp != "kisebb" && tipp != "kilépés")
                {
                    Console.WriteLine("Érvénytelen bemenet. Kérlek írd, hogy 'nagyobb', 'kisebb' vagy 'kilépés'.");
                }

            } while (tipp != "nagyobb" && tipp != "kisebb" && tipp != "kilépés");

            if (tipp == "kilépés")
            {
                jatszik = false;
                break;
            }

            int kovetkezoSzam = random.Next(1, 101);

            if ((tipp == "nagyobb" && kovetkezoSzam > jelenlegiSzam) ||
                (tipp == "kisebb" && kovetkezoSzam < jelenlegiSzam))
            {
                Console.Clear();
                Console.WriteLine("--------------------------------------------------------------------------------------");
                Console.WriteLine("TALÁLT!");
                Console.WriteLine("--------------------------------------------------------------------------------------");
                Console.WriteLine("Következő számkártya:");
                Kartya(kovetkezoSzam);
                Console.WriteLine("Helyes, eltaláltad a számot!");
                pontszam++;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("--------------------------------------------------------------------------------------");
                Console.WriteLine("\t\tNEM TALÁLT! JÁTÉK VÉGE!");
                Console.WriteLine("--------------------------------------------------------------------------------------");
                Console.WriteLine("A következő számkártya:");
                Kartya(kovetkezoSzam);
                Console.WriteLine();
                jatszik = false;
            }

            jelenlegiSzam = kovetkezoSzam;
        }

        Console.WriteLine("--------------------------------------------------------------------------------------");
        Console.WriteLine("Játék vége! Elért pontszám: " + pontszam);
        Console.Write("Nyomj Entert a folytatáshoz...");
        Console.ReadLine();

        string fajl = "ranglista_temp.txt";
        bool talalt = false;

        using (StreamReader olvas = new StreamReader("ranglista.txt"))
        using (StreamWriter w2 = new StreamWriter(fajl))
        {
            string sor;
            while ((sor = olvas.ReadLine()) != null)
            {
                string[] parts = sor.Split(':');
                if (parts.Length == 2 && parts[0] == jatekosnev && int.TryParse(parts[1], out int mentettPontszam))
                {
                    if (pontszam > mentettPontszam)
                    {
                        w2.WriteLine(jatekosnev + ":" + pontszam);
                    }
                    else
                    {
                        w2.WriteLine(sor);
                    }
                    talalt = true;
                }
                else
                {
                    w2.WriteLine(sor);
                }
            }

            if (!talalt)
            {
                w2.WriteLine(jatekosnev + ":" + pontszam);
            }
        }

        File.Delete("ranglista.txt");
        File.Move(fajl, "ranglista.txt");
}

    void Informaciok()
    {
        Console.Clear();

        if (!File.Exists("info.txt"))
        {
            using (StreamWriter w = new StreamWriter("info.txt"))
            {
                w.WriteLine("Higher or Lower Játék");
                w.WriteLine("---------------------");
                w.WriteLine("A játék lényege: Egy számot kapsz, és ki kell találnod, hogy a következő szám nagyobb vagy kisebb lesz.");
                w.WriteLine("Ha helyesen tippelsz, pontot kapsz. Ha tévedsz, a játék véget ér.");
                w.WriteLine();
                w.WriteLine("Szabályok:");
                w.WriteLine("1. A játék mindig egy véletlenszerű számmal kezdődik 1 és 100 között.");
                w.WriteLine("2. Minden körben meg kell tippelned, hogy a következő szám nagyobb vagy kisebb lesz.");
                w.WriteLine("3. Ha helyesen tippelsz, a játék folytatódik és növeled a pontjaidat.");
                w.WriteLine("4. Ha rosszul tippelsz, a játék véget ér.");
                w.WriteLine();
                w.WriteLine("Készítők: Kovács Lajos, Varga Máté");
            }
        }

        Console.WriteLine(File.ReadAllText("info.txt"));
        Console.WriteLine("\nNyomj Entert a visszatéréshez...");
        Console.ReadLine();
    }

    void Kartya(int szam)
    {
        string szamStr = szam.ToString().PadLeft(3);
        Console.WriteLine("\t┌─────┐");
        Console.WriteLine("\t|     |");
        Console.WriteLine($"\t| {szamStr} |");
        Console.WriteLine("\t|     |");
        Console.WriteLine("\t└─────┘");
    }

    void Ranglista()
    {
        Console.Clear();
        Console.WriteLine("--------------------------------------------------------------------------------------");
        Console.WriteLine("\t\t\tRanglista");
        Console.WriteLine("--------------------------------------------------------------------------------------");

        if (!File.Exists("ranglista.txt"))
        {
            Console.WriteLine("Még nincs elérhető toplista.");
        }
        else
        {
            using (StreamReader reader = new StreamReader("ranglista.txt"))
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