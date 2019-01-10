using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.engine.model.piece
{
    public class PowerPiece:PieceType
    {
        public override List<string> types { get; protected set; } //=  
        public override PieceBehaviour behaviour { get; protected set; }

        public PowerPiece()
        {
            types = new List<string>() { "OneLine", "OneCollumn", "OneColor", "Diagonal" };
            behaviour = PieceBehaviour.RELATIVE;
        }

    }
}
