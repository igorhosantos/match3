using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSession  {

    private int line;
    private int column;
    private const int MINIMUM_MATCH = 3;
    public Piece[,] board { get; private set; }


    public override string ToString()
    {
        //string str = "";
        return "[" +  board + "]";
            
    }
    public MatchSession(int line, int column)
    {
        this.line = line;
        this.column = column;
        board = new Piece[line, column];
        InitialPieces();
    }

    private void InitialPieces()
    {
        
        System.Array types  = System.Enum.GetValues(typeof(PieceType));
        System.Random random = new System.Random();

                                                                          //TODO logic initial pieces here
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                PieceType randomType = (PieceType)types.GetValue(random.Next(types.Length));
                board[i,j] = new Piece(i, randomType, new Tupple(i, j));
            }
        }

    }

    private Piece firstPiece;
    private Piece secondPiece;

    public void ExecuteClassicMovement(Piece current, int countMovements)
    {
        if (countMovements == 0) 
        {
            firstPiece = current;
            return;
        }

        if (countMovements == 1) secondPiece = current;
        if (firstPiece == secondPiece) throw new System.Exception("COMPARE EQUAL PIECES" + firstPiece +  " | " + secondPiece);


        //CHECK TUPPLE
        if(firstPiece.tupplePosition.line == secondPiece.tupplePosition.line+1 ||
           firstPiece.tupplePosition.line == secondPiece.tupplePosition.line-1 ||
           firstPiece.tupplePosition.column == secondPiece.tupplePosition.column + 1 ||
           firstPiece.tupplePosition.column == secondPiece.tupplePosition.column - 1)
        {

            Debug.Log("Try Classic Movement: " + firstPiece.tupplePosition + " | " + secondPiece.tupplePosition);

            SwapPieces(firstPiece, secondPiece);
            
            List<Piece> verticalPieces = CheckVerticalMatches(firstPiece);
            verticalPieces.AddRange(CheckVerticalMatches(secondPiece));

            List<Piece> horizontalPieces = CheckHorizontalMatches(firstPiece);
            horizontalPieces.AddRange(CheckVerticalMatches(secondPiece));

            if (verticalPieces.Count > MINIMUM_MATCH || horizontalPieces.Count > MINIMUM_MATCH)
            {
                Debug.Log("Apply destroy : " + verticalPieces.Count +  " | " + horizontalPieces.Count);
                //return list of matches
                string str = "Vertical: " + "\n";
                for (int i = 0; i < verticalPieces.Count; i++)
                {
                    str += verticalPieces[i].type + " | " + verticalPieces[i].tupplePosition + "\n";
                }

                str += "Horizontal: " + "\n";
                for (int i = 0; i < horizontalPieces.Count; i++)
                {
                    str += horizontalPieces[i].type + " | " + horizontalPieces[i].tupplePosition + "\n";
                }

                Debug.Log(str);

            }
            else
            {
                Debug.Log("Turn back pieces");
                SwapPieces(firstPiece, secondPiece);    
            }

        }
      
    }

    private void SwapPieces(Piece ft, Piece sc)
    {
        board[sc.tupplePosition.line, sc.tupplePosition.column] = firstPiece;
        board[ft.tupplePosition.line, ft.tupplePosition.column] = secondPiece;

        firstPiece = new Piece(sc.id, sc.type, sc.tupplePosition);
        secondPiece = new Piece(ft.id, ft.type, ft.tupplePosition);
    }


    private List<Piece> CheckVerticalMatches(Piece reference)
    {
        List<Piece> verticalPieces = new List<Piece>();
        Tupple tpIndex = reference.tupplePosition;

        //up reference
        for (int i = tpIndex.column; i > 0; i--)
        {
            if (board[tpIndex.line, i].type == reference.type)
                verticalPieces.Add(board[tpIndex.line, i]);
            else break;
        }
        //down reference
        for (int i = tpIndex.column; i < column; i++)
        {
            if (board[tpIndex.line, i].type == reference.type)
                verticalPieces.Add(board[tpIndex.line, i]);
            else break;
        }

        return verticalPieces;

    }

    private List<Piece> CheckHorizontalMatches(Piece reference )
    {
        List<Piece> horizontalPieces = new List<Piece>();

        Tupple tpIndex = reference.tupplePosition;

        //up reference
        for (int i = tpIndex.line; i > 0; i--)
        {
            if (board[i, tpIndex.column].type == reference.type)
                horizontalPieces.Add(board[i, tpIndex.column]);
            else break;
        }
        //down reference
        for (int i = tpIndex.line; i < line; i++)
        {
            if (board[i, tpIndex.column].type == reference.type)
                horizontalPieces.Add(board[i, tpIndex.column]);
            else break;
        }

        return horizontalPieces;
    }
	
}
