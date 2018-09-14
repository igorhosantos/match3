using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardView : MonoBehaviour
{

    public List<PieceView> pieces { get; private set; }
    private GameObject pieceContainer;
    private int countClick;
    private PieceView first;
    private PieceView second;
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

        CheckResult(MatchController.ME.ExecuteClassicMovement(first.currentPiece, second.currentPiece));
        
    }
    

    private void CheckResult(List<Piece> piecesToDestroy)
    {
        
        if(piecesToDestroy!=null && piecesToDestroy.Count>0) DestroyPieces(piecesToDestroy);
    }

    private void DestroyPieces(List<Piece> piecesToDestroy)
    {
//        for (int i = 0; i < pieces.Count; i++)
//        {
//            for (int i = 0; i < piecesToDestroy.Count; i++)
//            {
//
//            }
//        }
       
    }
}
