using System;

public class Tupple
{
    public int line;
    public int column;


    public override string ToString()
    {
        return "[" + line +  ", " +  column +  "]";
    }

    public Tupple(int line, int column)
    {
        this.line = line;
        this.column = column;
    }
}

