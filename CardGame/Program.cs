using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Üdvözlünk a Higher or Lower Játékban!");
        bool keepPlaying = true;

        while (keepPlaying)
        {
            // A játékmenet és más funkciók később kerülnek ide
            keepPlaying = false; // Ideiglenes érték a teszteléshez
        }

        Console.WriteLine("Köszönjük a játékot! Viszontlátásra!");

        static string GetPlayerName()
        {
            Console.Write("Kérem, add meg a neved: ");
            return Console.ReadLine();
        }
    }
}
