using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.view.services;
using UnityEngine;

public class MatchController : Singleton<MatchController> {

    public MatchSession match { get; private set; }
	
    public void StartSession(IGameServices services,int line , int collumn)
    {
        match = new MatchSession(services, line, collumn);
        //LogGame(match.board,5);
        
    }
    

    public void LogGame(Piece[,] rawData, int breakLine)
    {
 
        string str = "Log Engine" + '\n';
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

    public void RequestMovement(Piece first , Piece second)=> match.RequestMovement(first,second);
    public void RequestDropPieces()=> match.RequestDropPieces();
    public void RequestOtherMatches()=> match.RequestOtherMatches();

    public List<Piece> ExecutePowerup(List<Piece> pieces)
    {
        return match.ExecutePowerup(pieces);
    }

}
