

using System;
using System.Collections.Generic;
using Assets.Scripts.engine.model.piece;

public abstract class PieceType
{
    public abstract List<string> types { get; protected set; }
    public abstract PieceBehaviour behaviour { get; protected set; }

    public string type { get; set; }
}
