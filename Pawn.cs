using System;
using System.Collections.Generic;
using System.Text;

namespace chess
{
    public class Pawn
    {
        public const char PAWN_SYMBOL = 'x';
        public const char SPACE = ' ';
        public static char[,] pawns;

        public Pawn()
        {
            pawns = new char[ChessBoard.Dimension, ChessBoard.Dimension];
            initpawm();

        }
        private void initpawm()
        {
            for(int r =0; r < ChessBoard.Dimension; r++)
            {
                for (int c =0; c<ChessBoard.Dimension; c++)
                {
                    if (r==0 || r==1 || r == ChessBoard.Dimension - 2 || r == ChessBoard.Dimension - 1)
                    {
                        pawns[r, c] = PAWN_SYMBOL;
                    }else
                    {
                        pawns[r, c] = SPACE;
                    }
                }
            }
        }
    }
}
