using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    public PieceView[,] pieces { get; private set; }
    private GameObject pieceContainer;
    private int countClick;
    private PieceView first;
    private PieceView second;
    private List<Piece> piecesToDestroy;
    private List<Piece> powerupPiecesToDestroy;
    private List<Piece> pendingPiecesToDestroy;
    private List<List<Piece>> newPieces;

    public float speedSwap = 0.3f;
    public float speedMatch = 0.5f;
    public float speedDraw = 1f;
    public Ease easeMatch = Ease.InOutExpo;

    public Vector2 boardSize { get; private set; }

    void Awake()
    {
        boardSize = GetComponent<RectTransform>().sizeDelta;
    }

    public void Initiate()
    {
        DestroySession();
        pieceContainer = transform.Find("Pieces").gameObject;
        //        LogEngine();

        DrawSession();
        LogView();
    }

    public void DestroySession()
    {
        CancelInvoke(nameof(CheckResult));
        CancelInvoke(nameof(DropNewPieces));

        if (pieces == null) return;

        for (var i = 0; i < pieces.GetLength(0); i++)
        for (int j = 0; j < pieces.GetLength(1); j++)
            if(pieces[i, j]!=null && pieces[i, j].gameObject!=null) Destroy(pieces[i, j].gameObject);
    }

    private void DrawSession()
    {
        pieces = new PieceView[MatchController.ME.session.board.GetLength(0),MatchController.ME.session.board.GetLength(1)];

        float initalX = 0;
        float initalY = boardSize.y;


        for (int i = 0; i < MatchController.ME.session.board.GetLength(0); i++)
        {
            for (int j = 0; j < MatchController.ME.session.board.GetLength(1); j++)
            {
                GameObject gb = (GameObject) Instantiate(Resources.Load("Prefab/Piece"), pieceContainer.transform);
                PieceView pc = gb.AddComponent<PieceView>();
                pc.Initate(MatchController.ME.session.board[i,j]);
                pc.piecePosition.anchoredPosition = new Vector2(initalX,initalY);
                pieces[i,j] = pc;
                initalX += 265f;
                pc.button.onClick.AddListener(()=>PieceChosen(pc));
                MovePiece(pc,true);
            }
            initalX = 0;
            initalY -= 256;
        }

        Invoke(nameof(CheckResult), 3);
    }
    
    private void PieceChosen(PieceView p)
    {
        if (countClick == 0)
        {
            first = p;
            countClick++;
            return;
        }

        second = p;
        countClick = 0;

        MatchController.ME.RequestMovement(first.currentPiece, second.currentPiece);
    }

    public void NotifyMovement(List<Piece> pieces)
    {
        piecesToDestroy = pieces;
        SwapPieces();
    }

    public void NotifyDropPieces(List<List<Piece>> pieces) => newPieces = pieces;

    public void NotifyOtherMatches(List<Piece> pieces)
    {
        pendingPiecesToDestroy = pieces;

        if (pendingPiecesToDestroy != null && pendingPiecesToDestroy.Count > 0)
        {
            DestroyPieces(pendingPiecesToDestroy);
            GetNewPieces();
            RePosition();
            DropNewPieces();
//            Invoke(nameof(DropNewPieces), 0.3f);
        }
        else Debug.Log("NO MATCHES");

    }


    public void UsePowerup(PowerupController.POWERUP_TYPE type, Piece p)
    {
        powerupPiecesToDestroy = PowerupController.ME.ExecutePowerup(type,p);
        CheckResult();
    }

    private void CheckResult()
    {
        if(powerupPiecesToDestroy != null && powerupPiecesToDestroy.Count > 0)
        {
            DestroyPieces(powerupPiecesToDestroy);
            GetNewPieces();
            RePosition();
            //            Invoke(nameof(DropNewPieces), 0.3f);
            DropNewPieces();
            powerupPiecesToDestroy.Clear();
        }
        else if (piecesToDestroy != null && piecesToDestroy.Count > 0)
        {
            ConfirmSwap(first, second);
            OnFinishSwap();
            LogView();
            DestroyPieces(piecesToDestroy);
            GetNewPieces();
            RePosition();
            //            Invoke(nameof(DropNewPieces), 0.3f);
            DropNewPieces();
            piecesToDestroy.Clear();

        }
        else if (first != null && second != null) SwapPieces(false);
        else MatchController.ME.RequestOtherMatches();
      
    }
  
    private void SwapPieces(bool withCallback = true)
    {
      
        Vector2 saveFirst = first.piecePosition.anchoredPosition;
        Vector2 saveSecond = second.piecePosition.anchoredPosition;
         
        first.piecePosition.DOAnchorPos(saveSecond, speedSwap).SetEase(easeMatch);
        second.piecePosition.DOAnchorPos(saveFirst, speedSwap).OnComplete(()=>
        {
            if (withCallback) CheckResult();
            else OnFinishSwap();
        }).SetEase(easeMatch);
    }

    private void ConfirmSwap(PieceView ft, PieceView sc)
    {
        pieces[sc.currentPiece.tupplePosition.line, sc.currentPiece.tupplePosition.column] = second;
        pieces[ft.currentPiece.tupplePosition.line, ft.currentPiece.tupplePosition.column] = first;
    }

//    private void RemovePhysics()
//    {
//        for (var i = 0; i < pieces.GetLength(0); i++)
//        for (int j = 0; j < pieces.GetLength(1); j++)
//            pieces[i, j].piecePhysics.bodyType = RigidbodyType2D.Static;
//    }
//
//    private void AddPhysics()
//    {
//        for (var i = 0; i < pieces.GetLength(0); i++)
//        for (int j = 0; j < pieces.GetLength(1); j++)
//            pieces[i, j].piecePhysics.bodyType = RigidbodyType2D.Dynamic;
//    }

    private void OnFinishSwap()
    {
        
    }

    private void DestroyPieces(List<Piece> piecesToDestroy)
    {
        foreach (Piece pc in piecesToDestroy)
        {
            Tupple pos = new Tupple(pc.tupplePosition.line, pc.tupplePosition.column);
            PieceView current = pieces[pos.line, pos.column];

            if (current != null)
            {
                current.DestroyPiece();
                pieces[pos.line, pos.column] = null;
 
            }
        }
    }

    private void RePosition()
    {
        //Order pieces
        for (int i = pieces.GetLength(0) - 1; i >= 0; i--)
        {
            for (int j = pieces.GetLength(1) - 1; j >= 0; j--)
            {
                if (pieces[i, j] == null)
                {
                    int index = i - 1;

                    while (index >= 0 && pieces[i, j] == null)
                    {
                        if (pieces[index, j] != null)
                        {
                            pieces[i, j] = pieces[index, j];
                            pieces[index, j] = null;
                        }

                        index--;
                    }
                }
            }
        }
    }

    private void GetNewPieces()=> MatchController.ME.RequestDropPieces();
    
    private void DropNewPieces()
    {
        float initalX = 0;
        float initalY = boardSize.y;


        List<PieceView> newPiecesView = new List<PieceView>();

        for (int i = 0; i < newPieces.Count; i++)
        {
            for (int j = 0; j < newPieces[i].Count; j++)
            {
                GameObject gb = (GameObject)Instantiate(Resources.Load("Prefab/Piece"), pieceContainer.transform);
                PieceView pc = gb.AddComponent<PieceView>();
                Piece p = newPieces[i][j];
                pc.Initate(p);
                pc.piecePosition.anchoredPosition = new Vector2(initalX, initalY);
                initalY -= 256f;
                pc.button.onClick.AddListener(() => PieceChosen(pc));
                newPiecesView.Add(pc);

                pieces[pc.currentPiece.tupplePosition.line, pc.currentPiece.tupplePosition.column] = pc;

                MovePiece(pc);

            }
            initalY = 800;
            initalX += 265;
        }

     
        //RETUPLE
        for (int i = pieces.GetLength(0) - 1; i >= 0; i--)
        {
            for (int j = pieces.GetLength(1) - 1; j >= 0; j--)
            {
                pieces[i, j].currentPiece.tupplePosition = new Tupple(i, j);
                pieces[i, j].UpdateText();
                MovePiece(pieces[i, j]);
            }
        }

        first = second = null;
        Invoke(nameof(CheckResult), 0.5f);
    }

    private void MovePiece(PieceView p, bool initSession = false)
    {
        p.piecePosition.DOAnchorPos(MatchController.ME.Destiny(p.currentPiece.tupplePosition.line, p.currentPiece.tupplePosition.column), initSession? speedDraw : speedMatch)
            .SetEase(easeMatch);
    }

    public void LogView()
    {
        string str = "Log View" + '\n';
        int countBreak = 0;
        int breakLine = 5;

        for (int i = 0; i < pieces.GetLength(0); i++)
        {
            for (int j = 0; j < pieces.GetLength(1); j++)
            {
                countBreak++;

                if (countBreak == breakLine)
                {
                    if (pieces[i,j] != null) str += pieces[i,j].currentPiece.type.type;
                    else str += "X";

                    str += '\n';
                    countBreak = 0;
                }
                else
                {
                    if (pieces[i,j] != null) str += pieces[i,j].currentPiece.type.type + ",";
                    else str += "X" + ",";
                }

            }
           
        }

        Debug.Log(str);
    }

    public void LogEngine()
    {

        string str = "Log Engine" + '\n';
        int countBreak = 0;
        int breakLine = 5;

        for (int i = 0; i < MatchController.ME.session.board.GetLength(0); i++)
        {
            for (int j = 0; j < MatchController.ME.session.board.GetLength(1); j++)
            {
                countBreak++;

                if (countBreak == breakLine)
                {
                    if (MatchController.ME.session.board[i, j] != null) str += MatchController.ME.session.board[i, j].type.type;
                    else str += "X";

                    str += '\n';
                    countBreak = 0;
                }
                else
                {
                    if (MatchController.ME.session.board[i, j] != null) str += MatchController.ME.session.board[i, j].type.type + ",";
                    else str += "X" + ",";
                }

            }

        }

        Debug.Log(str);
    }

}
