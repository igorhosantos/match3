using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.view.services;
using UnityEngine;

public class MatchController : Singleton<MatchController> {

    public MatchSession session { get; private set; }
	
    public void StartSession(IGameServices services,int line , int collumn , Vector2 boardSize)
    {
        session = new MatchSession(services, line, collumn, boardSize);
        
    }
    
    public void RequestMovement(Piece first , Piece second)=> session.RequestMovement(first,second);
    public void RequestDropPieces()=> session.RequestDropPieces();
    public void RequestOtherMatches()=> session.RequestOtherMatches();

    public List<Piece> ExecutePowerup(List<Piece> pieces)
    {
        return session.ExecutePowerup(pieces);
    }

    public Vector2 Destiny(int line, int collumn)
    {
        return session.Destiny(line, collumn);
    }

}
