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

    public List<Piece> ExecuteClassicMovement(Piece first, Piece second)
    {
        firstPiece = first;
        secondPiece = second;
        
        if (firstPiece == secondPiece) throw new System.Exception("COMPARE EQUAL PIECES" + firstPiece +  " | " + secondPiece);
        
        //CHECK TUPPLE
        if(firstPiece.tupplePosition.line == secondPiece.tupplePosition.line+1 ||
           firstPiece.tupplePosition.line == secondPiece.tupplePosition.line-1 ||
           firstPiece.tupplePosition.column == secondPiece.tupplePosition.column + 1 ||
           firstPiece.tupplePosition.column == secondPiece.tupplePosition.column - 1)
        {
            
            SwapPieces(firstPiece, secondPiece);
            
            List<Piece> verticalPieces = CheckVerticalMatches();
            List<Piece> horizontalPieces = CheckHorizontalMatches();

            //Debug.Log("Matches Vertical: " + verticalPieces.Count);
            //Debug.Log("Matches Horizontal: " + horizontalPieces.Count);
            
            if (verticalPieces.Count > MINIMUM_MATCH || horizontalPieces.Count > MINIMUM_MATCH)
            {
                //Debug.Log("Apply destroy : " + verticalPieces.Count +  " | " + horizontalPieces.Count);
                //return list of matches
                /*string str = "Vertical: " + "\n";
                for (int i = 0; i < verticalPieces.Count; i++)
                {
                    str += verticalPieces[i].type + " | " + verticalPieces[i].tupplePosition + "\n";
                }

                str += "Horizontal: " + "\n";
                for (int i = 0; i < horizontalPieces.Count; i++)
                {
                    str += horizontalPieces[i].type + " | " + horizontalPieces[i].tupplePosition + "\n";
                }

                Debug.Log(str);*/

                List<Piece> totalMatches = new List<Piece>();
                totalMatches.AddRange(horizontalPieces);
                totalMatches.AddRange(verticalPieces);

                DestroyMatches(totalMatches);

               
                return totalMatches;
            }
            else
            {
                //Debug.Log("Turn back pieces");
                SwapPieces(firstPiece, secondPiece);
                return null;
            }

        }

        return null;

    }

    private void SwapPieces(Piece ft, Piece sc)
    {
        board[sc.tupplePosition.line, sc.tupplePosition.column] = firstPiece;
        board[ft.tupplePosition.line, ft.tupplePosition.column] = secondPiece;

        Tupple savet = ft.tupplePosition;
        firstPiece.tupplePosition = sc.tupplePosition;
        secondPiece.tupplePosition = savet;
    }
    
    private List<Piece> CheckHorizontalMatches()
    {
        List<Piece> horizontalPieces = new List<Piece>();
        int countLine = 0;

        //Debug.Log("Matches Per Line: " + current.type + " | " + criteria.Count + " | " + current.tupplePosition);
        
        while (countLine < line)
        {
            for (int i = 0; i < column; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    Piece current = board[countLine, j];

                    List<Piece> criteria = LineCriteria(current);
//                    Debug.Log("CURRENT PIECE: " + current.type +  " " +  current.tupplePosition +  " LINE: " + countLine +  " | EQUAL PIECES: " +  criteria.Count);
                    if (criteria.Count >= MINIMUM_MATCH)
                    {
                        horizontalPieces.AddRange(criteria);
                    }
                }
            }

            countLine++;
        }

        
        return horizontalPieces;

    }

    private List<Piece> CheckVerticalMatches()
    {

        List<Piece> verticalPieces = new List<Piece>();

        int countCollumn = 0;

        //Debug.Log("Matches Per Line: " + current.type + " | " + criteria.Count + " | " + current.tupplePosition);
        
        while (countCollumn < column)
        {
            for (int i = 0; i < line; i++)
            {
                for (int j = 0; j < line; j++)
                {
                    Piece current = board[j, countCollumn];

                    List<Piece> criteria = CollumnCriteria(current);
                    //Debug.Log("CURRENT PIECE: " + current.type +  " " +  current.tupplePosition +  " LINE: " + countCollumn +  " | EQUAL PIECES: " +  criteria.Count);
                    if (criteria.Count >= MINIMUM_MATCH)
                    {
                        verticalPieces.AddRange(criteria);
                        /*string str = "FIND VERTICAL MATCH: " + '\n';
                        for (int k = 0; k < verticalPieces.Count; k++)
                        {
                            str += verticalPieces[k].type + " | ";
                        }

                        Debug.Log(str);*/
                    }
                }
            }

            countCollumn++;
        }


        return verticalPieces;
    }

    private void DestroyMatches(List<Piece> pieces)
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            Tupple reference = pieces[i].tupplePosition;
            board[reference.line, reference.column] = null;
        }

        Reposition();

    }

    private void Reposition()
    {
        //TODO logic initial pieces here
        for (int i = board.GetLength(0)-1; i >= 0 ; i--)
        {
            for (int j = board.GetLength(1)-1; j >= 0; j--)
            {
                if(board[i, j] == null)
                {
                    int index = i-1;

                    while(index >= 0 && board[i, j] == null)
                    {
                        if (board[index, j] != null)
                        {
                            board[i,j] = board[index, j];
                            board[i,j].tupplePosition = new Tupple(index, j);
                            board[index, j] = null;
                        }

                        index--;
                    }

                }
            }
        }
    }

    public List<List<Piece>> NewPieces()
    {
        List<List<Piece>> newpieces = new List<List<Piece>>();

        System.Array types = System.Enum.GetValues(typeof(PieceType));
        System.Random random = new System.Random();

       for (int j = 0; j < column; j++)
       {
            int currentLine = 0;
            newpieces.Add(new List<Piece>());

            while(currentLine<line)
            {
                if (board[currentLine, j] == null)
                {
                    PieceType randomType = (PieceType)types.GetValue(random.Next(types.Length));
                    board[currentLine, j] = new Piece(currentLine, randomType, new Tupple(currentLine, j));
                    newpieces[j].Add(board[currentLine, j]);
                }
                currentLine++;
            }

        }


        return newpieces;
    }


    private List<Piece> LineCriteria(Piece reference)
    {
        List<Piece> countPieces = new List<Piece>();
        Tupple tpIndex = reference.tupplePosition;

        //piece to make a match
        countPieces.Add(reference);
        //left reference
        for (int i = tpIndex.column; i > 0; i--)
        {
            if (board[tpIndex.line, i] == reference)
            {

            }
            else if (board[tpIndex.line, i].type == reference.type)
                countPieces.Add(board[tpIndex.line, i]);
            else break;
        }
        //right reference
        for (int i = tpIndex.column; i < column; i++)
        {
            if (board[tpIndex.line, i] == reference)
            {

            }
            else if (board[tpIndex.line, i].type == reference.type)
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
            else if (board[i, tpIndex.column].type == reference.type)
                countPieces.Add(board[i, tpIndex.column]);
            else break;
        }
        //down reference
        for (int i = tpIndex.line; i < line; i++)
        {
            if (board[i, tpIndex.column] == reference)
            {

            }
            else if (board[i, tpIndex.column].type == reference.type)
                countPieces.Add(board[i, tpIndex.column]);
            else break;
        }


        return countPieces;
    }


}
