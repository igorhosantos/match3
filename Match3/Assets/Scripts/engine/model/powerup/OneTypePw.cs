using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTypePw : Powerup {

    public override List<Piece> ExecutePowerup(Piece p)
    {
        List<Piece> lp = new List<Piece>();
        PieceType pref = p.type;


        for (int i = 0; i < MatchController.ME.session.board.GetLength(0); i++)
        {
            for (int j = 0; j < MatchController.ME.session.board.GetLength(1); j++)
            {
                if(MatchController.ME.session.board[i,j].type == pref) lp.Add(MatchController.ME.session.board[i,j]);
            }

        }

        return lp;
    }
}
