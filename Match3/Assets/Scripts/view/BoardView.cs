using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BoardView : MonoBehaviour
{

    public List<PieceView> pieces { get; private set; }
    private GameObject pieceContainer;
    private int countClick;
    private PieceView first;
    private PieceView second;
    private List<Piece> piecesToDestroy;
    private float speedSwap = 0.3f;
    void Awake()
    {
        pieceContainer = transform.Find("Pieces").gameObject;
        DrawSession();
    }

    private void DrawSession()
    {
        pieces = new List<PieceView>();

        float initalX = 0;
        float initalY = 800f;
        
        for (int i = 0; i < MatchController.ME.match.board.GetLength(0); i++)
        {
            for (int j = 0; j < MatchController.ME.match.board.GetLength(1); j++)
            {
                GameObject gb = (GameObject) Instantiate(Resources.Load("Prefab/Piece"), pieceContainer.transform);
                PieceView pc = gb.AddComponent<PieceView>();
                pc.Initate(MatchController.ME.match.board[i,j]);
                pc.piecePosition.anchoredPosition = new Vector2(initalX,initalY);
                pieces.Add(pc);
                initalX += 265f;
                pc.button.onClick.AddListener(()=>PieceChosen(pc));

            }
            initalX = 0;
            initalY -= 256;
        }
        
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

        piecesToDestroy = MatchController.ME.ExecuteClassicMovement(first.currentPiece, second.currentPiece);
        SwapPieces();
        
    }
    
    private void CheckResult()
    {
        if (piecesToDestroy != null && piecesToDestroy.Count > 0)
        {
            OnFinishSwap();
            DestroyPieces(piecesToDestroy);
        }
        else
            SwapPieces(false);
    }
    
    private void SwapPieces(bool withCallback = true)
    {
        foreach (PieceView t in pieces)
        {
            t.piecePhysics.bodyType = RigidbodyType2D.Static;
        }

        Vector2 saveFirst = first.piecePosition.anchoredPosition;
        Vector2 saveSecond = second.piecePosition.anchoredPosition;

        first.UpdateText(second.currentPiece);
        second.UpdateText(first.currentPiece);
        
        first.piecePosition.DOAnchorPos(saveSecond, speedSwap);
        second.piecePosition.DOAnchorPos(saveFirst, speedSwap).OnComplete(()=>
        {
            if (withCallback) CheckResult();
            else OnFinishSwap();
        });
    }

    private void OnFinishSwap()
    {
        foreach (PieceView t in pieces)
        {
            t.piecePhysics.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void DestroyPieces(List<Piece> piecesToDestroy)
    {
        foreach (Piece pc in piecesToDestroy)
        {
            foreach (PieceView t in pieces)
            {
                if (pc == t.currentPiece)
                {
                    t.DestroyPiece();
                    //Destroy(t.gameObject);
                }
            }
        }
    }
}
