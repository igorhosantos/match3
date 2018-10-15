using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    private BoardView board;

    void Awake()
    {
        board = transform.Find("Board").gameObject.AddComponent<BoardView>();


        //TODO MOCK
        Button pwOneLine = GameObject.Find("PwDestroyLine").GetComponent<Button>();
        pwOneLine.onClick.AddListener(()=>board.UsePowerup(PowerupController.POWERUP_TYPE.ONE_LINE_PW, MatchController.ME.match.board[2, 0]));

        //PwDestroyCollumn
        Button pwOneCollumn = GameObject.Find("PwDestroyCollumn").GetComponent<Button>();
        pwOneCollumn.onClick.AddListener(()=>board.UsePowerup(PowerupController.POWERUP_TYPE.ONE_COLLUMN_PW, MatchController.ME.match.board[0, 2]));
    
        //PwDestroyColor
        Button pwOneType = GameObject.Find("PwDestroyColor").GetComponent<Button>();
        pwOneType.onClick.AddListener(()=>board.UsePowerup(PowerupController.POWERUP_TYPE.ONE_TYPE_PW, MatchController.ME.match.board[0, 0]));
    
    }

    public void ExecutePowerup()
    {
        
    }

 
}
