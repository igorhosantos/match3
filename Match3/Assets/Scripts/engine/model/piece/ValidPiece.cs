using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.engine.model.piece
{
    public class ValidPiece:PieceType
    {
        public override List<string> types { get; protected set; } //=  
        public override PieceBehaviour behaviour { get; protected set; }
       
        public ValidPiece()
        {
            types = new List<string>(){"Red", "Green", "Blue", "Purple" };
            behaviour = PieceBehaviour.RELATIVE;
        }
    }
}
