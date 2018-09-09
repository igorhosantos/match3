using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSession  {

    private int line;
    private int column;
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
            Debug.Log("Valid Classic Movement");
            SwapPieces(firstPiece, secondPiece);
        }


       
    }

    private void SwapPieces(Piece ft, Piece sc)
    {
        Piece saveDestiny = sc;
        board[sc.tupplePosition.line, sc.tupplePosition.column] = ft;
        board[ft.tupplePosition.line, ft.tupplePosition.column] = saveDestiny;
        sc.tupplePosition = ft.tupplePosition;
        ft.tupplePosition = saveDestiny.tupplePosition;

    }
	
}
