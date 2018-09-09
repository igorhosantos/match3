using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : Singleton<MatchController> {

    public MatchSession match { get; private set; }
	
    public void StartSession()
    {
        match = new MatchSession(5,5);
        LogGame(match.board,5);



        ExecuteClassicMovement(match.board[0,2]);
        ExecuteClassicMovement(match.board[0, 3]);

        LogGame(match.board, 5);
    }


    public static void LogGame(Piece[,] rawData, int breakLine)
    {
        string str = "";
        int countBreak = 0;

        for (int i = 0; i < rawData.GetLength(0); i++)
        {
            for (int j = 0; j < rawData.GetLength(1); j++)
            {
                countBreak++;

                if (countBreak == breakLine)
                {
                    str += rawData[i, j].type;
                    str += '\n';
                    countBreak = 0;
                }
                else
                {
                    str += rawData[i, j].type + ",";
                }
            }
        }

        Debug.Log(str);
    }

    private int countMovement = 0;
    public void ExecuteClassicMovement(Piece pc)
    {
        if (countMovement > 1) throw new System.Exception("ERROR COUNT MOVEMENT:  " +  countMovement);
        match.ExecuteClassicMovement(pc,countMovement);
        if(countMovement==0) countMovement++;
        else if(countMovement==1) countMovement=0;
        

    }

}
