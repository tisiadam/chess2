using System;
using System.Linq;

namespace ChessConsole
{
    public enum JatekosSzin
    {
        White, Black
    }

    public enum JatekosStatusz
    {
        Tetlen, Var, MasikraVar, JatekVege
    }

    public enum Opciok
    {
        Kiralyno = 0, Bastya = 1, Futo = 2, Lo = 3
    }

    public class ChessGame
    {
        public bool Futas
        {
            private set;
            get;
        }

        private JatekosStatusz jatekosstatusz;
        private Opciok Opcio;
        private JatekosSzin currentPlayer;

        /// <summary>
        /// Coordinates for the virtual cursor on the board
        /// </summary>
        private int cursorX, cursorY;

        /// <summary>
        /// The actual chess board
        /// </summary>
        private ChessBoard board;

        /// <summary>
        /// Currently holded piece's parent cell
        /// </summary>
        private ChessBoard.Cell holdedNode = null;

        /// <summary>
        /// Where to move
        /// </summary>
        private ChessBoard.Cell moveTo = null;

        public ChessGame()
        {
            Futas = true;
            board = new ChessBoard();
            currentPlayer = JatekosSzin.White;
            turnStart();
        }

        #region PublicInterfaceCommands
        public void Update()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.LeftArrow && cursorX > 0 && JatekosStatusz != JatekosStatusz.MasikraVar)
                    cursorX--;
                else if (keyInfo.Key == ConsoleKey.RightArrow && cursorX < 7 && JatekosStatusz != JatekosStatusz.MasikraVar)
                    cursorX++;
                else if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (JatekosStatusz != JatekosStatusz.MasikraVar && cursorY < 7)
                        cursorY++;
                    else if ((int)Opcio > 0)
                        Opcio--;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (JatekosStatusz != JatekosStatusz.MasikraVar && cursorY > 0)
                        cursorY--;
                    else if ((int)Opcio < 3)
                        Opcio++;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                    interact();
                else if (keyInfo.Key == ConsoleKey.D)
                    debugInteract();
                else if (keyInfo.Key == ConsoleKey.Escape)
                    cancel();
            }
        }

        /// <summary>
        /// Draws the game
        /// </summary>
        /// <param name="g">ConsoleGraphics object to draw with/to</param>
        public void Draw(ConsoleGraphics g)
        {
            g.FillArea(new CChar(' ', ConsoleColor.Black, ConsoleColor.DarkGray), 10, 5, 8, 8);

            //7-j everywhere cuz it's reversed in chess
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //Draw the symbol
                    ChessBoard.Cell cell = board.GetCell(i, j);
                    if (cell.Piece != null)
                    {
                        g.DrawTransparent(cell.Piece.Char, (cell.Piece.Color == JatekosSzin.White) ? ConsoleColor.White : ConsoleColor.Black, 10 + i, 5 + (7 - j));
                        if (cell.Piece.LegalMoves.Count == 0)
                        {
                            g.SetBackground(ConsoleColor.DarkRed, 10 + i, 5 + (7 - j));
                        }
                    }

                    if (cell.HitBy.Contains(debugPiece))
                        g.SetBackground(ConsoleColor.DarkMagenta, 10 + i, 5 + (7 - j));
                }
            }

            if (holdedNode != null && JatekosStatusz == JatekosStatusz.Var)
            {
                //Highlight legal moves
                foreach (ChessBoard.Cell move in holdedNode.Piece.LegalMoves)
                {
                    g.SetBackground(ConsoleColor.DarkGreen, 10 + move.X, 5 + (7 - move.Y));
                }
            }

            //Sets the cursor color -> yellow
            g.SetBackground(ConsoleColor.DarkYellow, 10 + cursorX, 5 + (7 - cursorY));

            //TODO: Remove en passant testing
            /*if (board.KeresztLepes != null)
                g.SetBackground(ConsoleColor.DarkCyan, 10 + board.KeresztLepes.X, 5 + (7 - board.KeresztLepes.Y));

            if (board.KeresztUtes != null)
                g.SetBackground(ConsoleColor.DarkMagenta, 10 + board.KeresztUtes.X, 5 + (7 - board.KeresztUtes.Y));*/

            //Lighten for checkerboard pattern
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 1) g.LightenBackground(10 + i, 5 + j);
                }
            }

            //Promotion option menu

            if (JatekosStatusz == JatekosStatusz.MasikraVar)
            {
                g.DrawTextTrasparent("Kiralyno", Opcio == Opciok.Kiralyno ? ConsoleColor.Yellow : ConsoleColor.White, 22, 7);
                g.DrawTextTrasparent("Bastya", Opcio == Opciok.Bastya ? ConsoleColor.Yellow : ConsoleColor.White, 22, 9);
                g.DrawTextTrasparent("Futo", Opcio == Opciok.Futo ? ConsoleColor.Yellow : ConsoleColor.White, 22, 11);
                g.DrawTextTrasparent("Lo", Opcio == Opciok.Lo ? ConsoleColor.Yellow : ConsoleColor.White, 22, 13);
            }
            else
            {
                g.ClearArea(22, 7, 6, 7);
            }
        }

        #endregion

        #region EventHandlerLikeMethods

        /// <summary>
        /// Happens when the user presses the enter key
        /// </summary>
        private void interact()
        {
            switch (JatekosStatusz)
            {
                case JatekosStatusz.Tetlen:
                    holdedNode = board.GetCell(cursorX, cursorY);

                    if (holdedNode.Piece == null || holdedNode.Piece.Color != currentPlayer || holdedNode.Piece.LegalMoves.Count == 0)
                    {
                        holdedNode = null;
                        return;
                    }
                    else JatekosStatusz = JatekosStatusz.Var;


                    break;
                case JatekosStatusz.Var:
                    JatekosStatusz = JatekosStatusz.Var;

                    moveTo = board.GetCell(cursorX, cursorY);

                    if (!holdedNode.Piece.LegalMoves.Contains(moveTo))
                    {
                        moveTo = null;
                        return;
                    }

                    if (board.Elmozdithato(holdedNode, moveTo))
                        showPromote();
                    else
                        turnOver();

                    break;
                case JatekosStatusz.MasikraVar:
                    turnOver();
                    break;
                case JatekosStatusz.JatekVege:
                    Futas = false;
                    break;
            }
        }


        private Piece debugPiece;
        private void debugInteract()
        {
            debugPiece = board.GetCell(cursorX, cursorY).Piece;
        }

        /// <summary>
        /// Happens when the user presses the escape key
        /// </summary>
        private void cancel()
        {
            JatekosStatusz = JatekosStatusz.Tetlen;
            holdedNode = null;
        }

        #endregion

        #region EventLikeMethods
        /// <summary>
        /// Called on every turn start
        /// </summary>
        private void turnStart()
        {
            board.TurnStart(currentPlayer);
        }

        /// <summary>
        /// Shows promotion dialog (set's the state)
        /// </summary>
        private void showPromote()
        {
            JatekosStatusz = JatekosStatusz.MasikraVar;
            Opcio = Opciok.Kiralyno; //reset the menu
        }

        /// <summary>
        /// Called when the turn is passed to the other player
        /// </summary>
        private void turnOver()
        {
            board.Move(holdedNode, moveTo, Opcio);
            holdedNode = null;
            moveTo = null;
            JatekosStatusz = JatekosStatusz.Tetlen;
            currentPlayer = currentPlayer == JatekosSzin.White ? JatekosSzin.Black : JatekosSzin.White;
            turnStart();
        }
        #endregion
    }
}
