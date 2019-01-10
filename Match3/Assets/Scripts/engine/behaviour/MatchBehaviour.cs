using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.engine.model.piece;

namespace Assets.Scripts.engine.behaviour
{
    public class MatchBehaviour
    {
        public const int MINIMUM_MATCH = 3;
        private Piece[,] board;

        public MatchBehaviour(Piece[,] board)
        {
            this.board = board;
        }

        public void InitialPieces()
        {
            Random randomType = new Random();

            //TODO logic initial pieces here
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    PieceType piece = new ValidPiece();
                    piece.type = piece.types[randomType.Next(piece.types.Count)];
                    board[i, j] = new Piece(i, piece, new Tupple(i, j));
                }
            }
        }

        public List<List<Piece>> DropPieces()
        {
            List<List<Piece>> newpieces = new List<List<Piece>>();
            Random randomType = new Random();

            for (int j = 0; j < board.GetLength(1); j++)
            {
                int currentLine = 0;
                newpieces.Add(new List<Piece>());

                while (currentLine < board.GetLength(0))
                {
                    if (board[currentLine, j] == null)
                    {
                        PieceType piece = new ValidPiece();
                        piece.type = piece.types[randomType.Next(piece.types.Count)];
                        board[currentLine, j] = new Piece(currentLine, piece, new Tupple(currentLine, j));
                        newpieces[j].Add(board[currentLine, j]);
                    }
                    currentLine++;
                }
            }

            return newpieces;
        }

    }
}
