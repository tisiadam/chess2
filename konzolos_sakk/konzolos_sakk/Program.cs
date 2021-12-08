using System;

namespace konzolos_sakk
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Grafika kinezet = new Grafika();
            jatek = new jatekmenet();

            do
            {
                jatek.rajzol(kinezet);
                kinezet.csere();
                jatek.frissit();
            } while (jatek.fut);

            Console.Read();
        }
    }
}
