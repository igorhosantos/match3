using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : Singleton<MatchController> {

    public MatchSession match { get; private set; }
	
    public void StartSession()
    {
        match = new MatchSession(5,5);
        LogGame(match.board,5);
        
    }

    public void LogButton(int lineOrigin, int collumOrigin, int lineDestiny, int collumnDestiny)
    {
        List<Piece> piecesToDestroy = ExecuteClassicMovement(match.board[lineOrigin, collumOrigin], match.board[lineDestiny, collumnDestiny]);
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
                    if (rawData[i, j] != null) str += rawData[i, j].type;
                    else str += "-1";

                    str += '\n';
                    countBreak = 0;
                }
                else
                {
                    if (rawData[i, j] != null) str += rawData[i, j].type + ",";
                    else str += "-1" + ",";
                }
            }
        }

        Debug.Log(str);
    }

    public List<Piece> ExecuteClassicMovement(Piece first , Piece second)
    {
        return match.ExecuteClassicMovement(first,second);
        
    }


}
