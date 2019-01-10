using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.view.services
{
    public interface IGameServices
    {
        void NotifyMovement(List<Piece> pieces);
        void NotifyDropPieces(List<List<Piece>> pieces);
        void NotifyOtherMatches(List<Piece> pieces);
    }
}
