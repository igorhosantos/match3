using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour
{
    private BoardView board;

    void Awake()
    {
        board = transform.Find("Board").gameObject.AddComponent<BoardView>();
        Initiate();
    }


    public void Initiate()
    {
        for (int i = 0; i < board.pieces.Count; i++)
        {
            
        }
    }

}
