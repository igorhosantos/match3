using System.Collections.Generic;
using Assets.Scripts.engine.model.piece;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.engine.behaviour
{
    public class MatchBehaviour
    {
        public const int MINIMUM_MATCH = 3;
        private Piece[,] board;
        private Vector2 boardSize;
        private Vector2[,] boardPositions;

        public MatchBehaviour(Piece[,] board, Vector2 boardSize)
        {
            this.board = board;
            this.boardSize = boardSize;
            CreateBoardPosition();
        }

        private void CreateBoardPosition()
        {
            float pieceSize = 256;
            float initialX = boardSize.x;
            float initialY = -boardSize.y;

            Debug.Log("BOARD SIZE: " + boardSize);

            boardPositions = new Vector2[board.GetLength(0), board.GetLength(1)];

            for (int i = board.GetLength(0)-1; i >= 0; i--)
            {
                for (int j = board.GetLength(1)-1; j >= 0 ; j--)
                {
                    boardPositions[i,j] = new Vector2(initialX- pieceSize, initialY + pieceSize);
                    initialX -= pieceSize;

                }

                initialX = boardSize.x;
                initialY += pieceSize;
            }
        }

        public void InitialPieces()
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    PieceType piece = new ValidPiece();
//                    piece.type = piece.types[randomType.Next(piece.types.Count)];
                    piece.type = GetNonMatchType(i,j,piece.types);
                    board[i, j] = new Piece(i, piece, new Tupple(i, j));
                }
            }
        }

        public Vector2 Destiny(int line, int collumn)
        {
            //Debug.Log("POSITION: " + boardPositions[line, collumn]);
            return boardPositions[line, collumn];
        }

        private int countHorizontal = 0;
        private int countVertical = 0;

        private string GetNonMatchType(int line, int collumn, List<string> typeList)
        {
            string matchType = null;
            List<string> currentList = typeList;
           
            countHorizontal = 0;
            countVertical = 0;

            matchType = typeList[Random.Range(0, currentList.Count)];

            countHorizontal+=CheckHorizontalSides(line, collumn, 1, matchType);
            countHorizontal+=CheckHorizontalSides(line, collumn, 2, matchType);
           
            countVertical+=CheckVerticalSides(line,collumn,1,matchType); 
            countVertical+=CheckVerticalSides(line,collumn,2,matchType); 

            if (countHorizontal >= (MINIMUM_MATCH - 1) || countVertical >= (MINIMUM_MATCH - 1))
            {
                currentList.Remove(matchType);
                matchType = typeList[Random.Range(0, currentList.Count)];
            }
       
            return matchType;
        }

        private int CheckVerticalSides(int line, int collumn, int path,string matchType)
        {
            int totalPoints = 0;

            if (CheckBounds(line - path, collumn) && board[line - path, collumn]?.type?.type == matchType) totalPoints++;
            if (CheckBounds(line + path, collumn) && board[line + path, collumn]?.type?.type == matchType) totalPoints++;

            return totalPoints;
        }

        private int CheckHorizontalSides(int line, int collumn, int path, string matchType)
        {
            int totalPoints = 0;
            if (CheckBounds(line, collumn - path) && board[line, collumn - path]?.type?.type == matchType) totalPoints++;
            if (CheckBounds(line, collumn + path) && board[line, collumn + path]?.type?.type == matchType) totalPoints++;

            return totalPoints;
        }

        private bool CheckBounds(int currentLine , int currentCollumn)
        {
            return currentLine >= 0 && currentLine < board.GetLength(0) &&
                   currentCollumn >= 0 && currentCollumn < board.GetLength(1);
        }


        public List<List<Piece>> DropPieces()
        {
            List<List<Piece>> newpieces = new List<List<Piece>>();
         
            for (int j = 0; j < board.GetLength(1); j++)
            {
                int currentLine = 0;
                newpieces.Add(new List<Piece>());

                while (currentLine < board.GetLength(0))
                {
                    if (board[currentLine, j] == null)
                    {
                        PieceType piece = new ValidPiece();
                        piece.type = piece.types[Random.Range(0, piece.types.Count)];
                        board[currentLine, j] = new Piece(currentLine, piece, new Tupple(currentLine, j));
                        newpieces[j].Add(board[currentLine, j]);
                    }
                    currentLine++;
                }
            }

            return newpieces;
        }


        public List<Piece> CheckHorizontalMatches()
        {
            List<Piece> horizontalPieces = new List<Piece>();
            int countLine = 0;

            while (countLine < board.GetLength(0))
            {
                for (int i = 0; i < board.GetLength(1); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        Piece current = board[countLine, j];

                        List<Piece> criteria = LineCriteria(current);

                        if (criteria.Count >= MINIMUM_MATCH) horizontalPieces.AddRange(criteria);
                    }
                }

                countLine++;
            }


            return horizontalPieces;

        }

        public List<Piece> CheckVerticalMatches()
        {
            List<Piece> verticalPieces = new List<Piece>();

            int countCollumn = 0;

            while (countCollumn < board.GetLength(1))
            {
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(0); j++)
                    {
                        Piece current = board[j, countCollumn];

                        List<Piece> criteria = CollumnCriteria(current);

                        if (criteria.Count >= MINIMUM_MATCH) verticalPieces.AddRange(criteria);

                    }
                }

                countCollumn++;
            }


            return verticalPieces;
        }


        private List<Piece> LineCriteria(Piece reference)
        {
            List<Piece> countPieces = new List<Piece>();
            Tupple tpIndex = reference.tupplePosition;

            //piece to make a session
            countPieces.Add(reference);
            //left reference
            for (int i = tpIndex.column; i > 0; i--)
            {
                if (board[tpIndex.line, i] == reference)
                {

                }
                else if (CheckValidPiece(board[tpIndex.line, i], reference))
                    countPieces.Add(board[tpIndex.line, i]);
                else break;
            }
            //right reference
            for (int i = tpIndex.column; i < board.GetLength(1); i++)
            {
                if (board[tpIndex.line, i] == reference)
                {

                }
                else if (CheckValidPiece(board[tpIndex.line, i], reference))
                    countPieces.Add(board[tpIndex.line, i]);
                else break;
            }


            return countPieces;
        }

        private List<Piece> CollumnCriteria(Piece reference)
        {
            List<Piece> countPieces = new List<Piece>();

            Tupple tpIndex = reference.tupplePosition;
            countPieces.Add(reference);

            //        //up reference
            for (int i = tpIndex.line; i > 0; i--)
            {
                if (board[i, tpIndex.column] == reference)
                {

                }
                else if (CheckValidPiece(board[i, tpIndex.column], reference))
                    countPieces.Add(board[i, tpIndex.column]);
                else break;
            }
            //down reference
            for (int i = tpIndex.line; i < board.GetLength(0); i++)
            {
                if (board[i, tpIndex.column] == reference)
                {

                }
                else if (CheckValidPiece(board[i, tpIndex.column], reference))
                    countPieces.Add(board[i, tpIndex.column]);
                else break;
            }


            return countPieces;
        }

        private bool CheckValidPiece(Piece boardPiece, Piece refPiece)
        {
            return boardPiece.type is ValidPiece && refPiece.type is ValidPiece &&
                   boardPiece.type.type == refPiece.type.type;
        }


    }
}
