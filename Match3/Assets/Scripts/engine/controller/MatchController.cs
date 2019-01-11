using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.view.services;
using UnityEngine;

public class MatchController : Singleton<MatchController> {

    public MatchSession session { get; private set; }
	
    public void StartSession(IGameServices services,int line , int collumn)
    {
        session = new MatchSession(services, line, collumn);
        
    }
    
    public void RequestMovement(Piece first , Piece second)=> session.RequestMovement(first,second);
    public void RequestDropPieces()=> session.RequestDropPieces();
    public void RequestOtherMatches()=> session.RequestOtherMatches();

    public List<Piece> ExecutePowerup(List<Piece> pieces)
    {
        return session.ExecutePowerup(pieces);
    }

}
