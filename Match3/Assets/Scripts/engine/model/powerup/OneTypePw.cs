using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTypePw : Powerup {

    public override List<Piece> ExecutePowerup(Piece p)
    {
        List<Piece> lp = new List<Piece>();
        PieceType pref = p.type;


        for (int i = 0; i < MatchController.ME.match.board.GetLength(0); i++)
        {
            for (int j = 0; j < MatchController.ME.match.board.GetLength(1); j++)
            {
                if(MatchController.ME.match.board[i,j].type == pref) lp.Add(MatchController.ME.match.board[i,j]);
            }

        }

        return lp;
    }
}
