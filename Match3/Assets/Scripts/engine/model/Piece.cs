using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece {


    private int id;
    private PieceType type;
    private Vector2 tupplePosition;

    public Piece(int id,PieceType type,Vector2 tupplePosition)
    {
        this.id = id;
        this.type = type;
        this.tupplePosition = tupplePosition;
    }

}
