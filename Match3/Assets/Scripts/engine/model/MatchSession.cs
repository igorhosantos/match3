using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.engine.behaviour;
using Assets.Scripts.engine.model.piece;
using Assets.Scripts.view.services;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class MatchSession  {

    private int line;
    private int column;
    public Piece[,] board { get; private set; }
    private MatchBehaviour behaviour;
    private IGameServices services;
    
    public override string ToString()
    {
        //string str = "";
        return "[" +  board + "]";
            
    }
    public MatchSession(IGameServices services, int line, int column)
    {
        this.services = services;
        this.line = line;
        this.column = column;

        board = new Piece[line, column];

        behaviour = new MatchBehaviour(board);
        behaviour.InitialPieces();
    }


    private Piece firstPiece;
    private Piece secondPiece;

    public void RequestMovement(Piece first, Piece second)
    {
        firstPiece = first;
        secondPiece = second;
        
        if (firstPiece == secondPiece) throw new System.Exception("COMPARE EQUAL PIECES" + firstPiece +  " | " + secondPiece);
        
        //CHECK TUPPLE
        if(CheckTupple(firstPiece, secondPiece))
        {
            SwapPieces(firstPiece, secondPiece);
            
            List<Piece> verticalPieces = CheckVerticalMatches();
            List<Piece> horizontalPieces = CheckHorizontalMatches();

            if (verticalPieces.Count > MatchBehaviour.MINIMUM_MATCH || horizontalPieces.Count > MatchBehaviour.MINIMUM_MATCH)
            {
                List<Piece> totalMatches = new List<Piece>();
                totalMatches.AddRange(horizontalPieces);
                totalMatches.AddRange(verticalPieces);

                DestroyMatches(totalMatches);

                services.NotifyMovement( totalMatches);
            }
            else SwapPieces(firstPiece, secondPiece);
           
        }

    }

    private bool CheckTupple(Piece firstPiece, Piece secondPiece)
    {
        return firstPiece.tupplePosition.line == secondPiece.tupplePosition.line + 1 ||
               firstPiece.tupplePosition.line == secondPiece.tupplePosition.line - 1 ||
               firstPiece.tupplePosition.column == secondPiece.tupplePosition.column + 1 ||
               firstPiece.tupplePosition.column == secondPiece.tupplePosition.column - 1;
    }

    public void RequestOtherMatches()
    {
        List<Piece> verticalPieces = CheckVerticalMatches();
        List<Piece> horizontalPieces = CheckHorizontalMatches();

        if (verticalPieces.Count > MatchBehaviour.MINIMUM_MATCH || horizontalPieces.Count > MatchBehaviour.MINIMUM_MATCH)
        {
            List<Piece> totalMatches = new List<Piece>();
            totalMatches.AddRange(horizontalPieces);
            totalMatches.AddRange(verticalPieces);
            DestroyMatches(totalMatches);

            services.NotifyOtherMatches(totalMatches);
        }
        

        services.NotifyOtherMatches(null);
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

        while (countLine < line)
        {
            for (int i = 0; i < column; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    Piece current = board[countLine, j];

                    List<Piece> criteria = LineCriteria(current);

                    if (criteria.Count >= MatchBehaviour.MINIMUM_MATCH)
                        horizontalPieces.AddRange(criteria);
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

        while (countCollumn < column)
        {
            for (int i = 0; i < line; i++)
            {
                for (int j = 0; j < line; j++)
                {
                    Piece current = board[j, countCollumn];

                    List<Piece> criteria = CollumnCriteria(current);

                    if (criteria.Count >= MatchBehaviour.MINIMUM_MATCH)
                        verticalPieces.AddRange(criteria);
                  
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

    public void RequestDropPieces()
    {
        List<List<Piece>> newpieces = behaviour.DropPieces();
        services.NotifyDropPieces(newpieces);
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
            else if (CheckValidPiece(board[tpIndex.line, i], reference))
                countPieces.Add(board[tpIndex.line, i]);
            else break;
        }
        //right reference
        for (int i = tpIndex.column; i < column; i++)
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
            else if (CheckValidPiece(board[i, tpIndex.column],reference))
                countPieces.Add(board[i, tpIndex.column]);
            else break;
        }
        //down reference
        for (int i = tpIndex.line; i < line; i++)
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

    public List<Piece> ExecutePowerup(List<Piece> list)
    {
        DestroyMatches(list);

        return list;
    }


}
