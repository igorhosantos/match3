using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSession  {

    private int line;
    private int column;
    private Piece[,] board;


    public override string ToString()
    {
        //string str = "";
        return "[" +  board + "]";
            
    }
    public MatchSession(int line, int column)
    {
        this.line = line;
        this.column = column;
        board = new Piece[line, column];

    }
	
}
