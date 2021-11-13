using System;
using System.Collections.Generic;
using System.Text;

namespace chess
{
    public class Move:Pawn
    {
        private int targetx ;
        private int targety;
        private int destinationx;
        private int destinationy;
        public Move()
        {
            targetx = 0;
            targety = 0;
            destinationx = 0;
            destinationy = 0;
            EXIT = false;
        }
        public bool EXIT { get; set; }
        public void makemove()
        {
            getinput();
            if(!EXIT)
            rearangepawns();
        }
        private void getinput()
        {
            Console.WriteLine("Enter X axis");
            EXIT = validateinput(int.TryParse(Console.ReadLine(), out targetx));
            if (!EXIT)
            {
                Console.WriteLine("Enter Y axis");
                EXIT = validateinput(int.TryParse(Console.ReadLine(), out targety));
            }
            if (!EXIT)
            {
                Console.WriteLine("Enter Destination X axis");
                EXIT = validateinput(int.TryParse(Console.ReadLine(), out destinationx));
            }
            if (!EXIT)
            {
                Console.WriteLine("Enter Destination Y axis");
                EXIT = validateinput(int.TryParse(Console.ReadLine(), out destinationy));
            }
        }
        private bool validateinput(bool parsed)
        {
            bool error = false;

            if (!parsed)
            {
                error = true;
            }
            else if (targetx < 0 || targety < 0 || destinationx < 0 || destinationy < 0)
            {
                error = true;
            }else if (targetx>ChessBoard.Dimension-1||targety>ChessBoard.Dimension-1||destinationx>ChessBoard.Dimension-1 || destinationy > ChessBoard.Dimension - 1)
            {
                error = true;
            }
            if (error)
            {
                Console.WriteLine("invalid input. program exit...");
            }


            return error;
        }
        private void rearangepawns()
        {
            pawns[destinationx, destinationy] = pawns[targetx, targety];
            pawns[targetx, targety] = SPACE;
        }
    }
}
