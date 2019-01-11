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
    
    private List<Piece> CheckHorizontalMatches() => behaviour.CheckHorizontalMatches();
    private List<Piece> CheckVerticalMatches()=> behaviour.CheckVerticalMatches();

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

    public List<Piece> ExecutePowerup(List<Piece> list)
    {
        DestroyMatches(list);

        return list;
    }


}
