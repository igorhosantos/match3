using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneLinePw : Powerup {

    public override List<Piece> ExecutePowerup(Piece p)
    {
        List<Piece> lp = new List<Piece>();
        Tupple lineRef = p.tupplePosition;


        for (int j = 0; j < MatchController.ME.session.board.GetLength(1); j++)
        {
            lp.Add(MatchController.ME.session.board[lineRef.line, j]);
        }

        return lp;
    }
}
