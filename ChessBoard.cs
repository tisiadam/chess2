using System;
using System.Collections.Generic;
using System.Text;

namespace chess
{
     public class ChessBoard
    {
        private string[,] chessBoard;
        public const int Dimension = 8;
        private Pawn pawn;
        private Move move;
     public ChessBoard()
        {
            move = new Move();
            chessBoard = new string[Dimension, Dimension];
            ChessBoardhorizontalsymbol = "+---";
            ChessBoardverticalsymbol = "| ";
        }
        public string ChessBoardhorizontalsymbol { get; set; }
        public string ChessBoardverticalsymbol { get; set; }

        public void displaychessboard()
        {
       while (!move.EXIT)
            {
                Console.Clear();
                Console.WriteLine("    0   1   2   3   4   5   6   7");
                for (int r= 0; r< Dimension; r++)
                {
                    Console.Write("  ");
                    for (int c =0; c < Dimension; c++)
                    {
                        Console.Write(ChessBoardhorizontalsymbol);
                    }
                    Console.Write("+\n");
                    for (int c= 0; c< Dimension; c++)
                    {
                        if (c == 0)
                        {
                            Console.Write(r + " ");
                        }

                        Console.Write(ChessBoardverticalsymbol + Pawn.pawns[r, c] + " ");
                    }
                    Console.Write("|\n");
                }
                Console.Write("  ");
                for(int c=0;c<Dimension;c++)
                {
                    Console.Write(ChessBoardhorizontalsymbol);
                }
                Console.Write("+\n\n");
                move.makemove();
            }
        }
    }
}
