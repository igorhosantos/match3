using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece {


    public int id { get; private set; }
    public PieceType type{ get; private set; }
    public Tupple tupplePosition{ get; set; }

    public Piece(int id,PieceType type,Tupple tupplePosition)
    {
        this.id = id;
        this.type = type;
        this.tupplePosition = tupplePosition;
    }

}
