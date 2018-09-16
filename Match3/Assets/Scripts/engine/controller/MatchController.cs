using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : Singleton<MatchController> {

    public MatchSession match { get; private set; }
	
    public void StartSession()
    {
        match = new MatchSession(8,5);
        LogGame(match.board,5);
        
    }
    

    public void LogGame(Piece[,] rawData, int breakLine)
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
                    else str += "X";

                    str += '\n';
                    countBreak = 0;
                }
                else
                {
                    if (rawData[i, j] != null) str += rawData[i, j].type + ",";
                    else str += "X" + ",";
                }
            }
        }

        Debug.Log(str);
    }

    public List<Piece> ExecuteClassicMovement(Piece first , Piece second)
    {
        return match.ExecuteClassicMovement(first,second);
        
    }

    public List<List<Piece>> NewPieces()
    {
        return match.NewPieces();
    }


}
