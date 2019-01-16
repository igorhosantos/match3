using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.Scripts.view.services;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour, IGameServices
{
    private BoardView board;

    void Awake()
    {
        StartSession();

        //TODO MOCK
        Button pwOneLine = GameObject.Find("PwDestroyLine").GetComponent<Button>();
        pwOneLine.onClick.AddListener(()=>board.UsePowerup(PowerupController.POWERUP_TYPE.ONE_LINE_PW, MatchController.ME.session.board[2, 0]));

        //PwDestroyCollumn
        Button pwOneCollumn = GameObject.Find("PwDestroyCollumn").GetComponent<Button>();
        pwOneCollumn.onClick.AddListener(()=>board.UsePowerup(PowerupController.POWERUP_TYPE.ONE_COLLUMN_PW, MatchController.ME.session.board[0, 2]));
    
        //PwDestroyColor
        Button pwOneType = GameObject.Find("PwDestroyColor").GetComponent<Button>();
        pwOneType.onClick.AddListener(()=>board.UsePowerup(PowerupController.POWERUP_TYPE.ONE_TYPE_PW, MatchController.ME.session.board[0, 0]));
    
    }

    public void NotifyMovement(List<Piece> pieces) => board.NotifyMovement(pieces);
    public void NotifyDropPieces(List<List<Piece>> pieces) => board.NotifyDropPieces(pieces);
    public void NotifyOtherMatches(List<Piece> pieces)=> board.NotifyOtherMatches(pieces);

    private void StartSession()
    {
        board = transform.Find("Board").gameObject.AddComponent<BoardView>();
        MatchController.ME.StartSession(this, 8, 5, board.boardSize);

        lastEaseMatch = board.easeMatch;
        lastSpeedSwap = board.speedSwap;
        lastSpeedDraw = board.speedDraw;

        board.Initiate();
    }

    private Ease  lastEaseMatch;
    private float lastSpeedSwap;
    private float lastSpeedMatch;
    private float lastSpeedDraw;

    void Update()
    {
        if (board.easeMatch != lastEaseMatch || board.speedSwap != lastSpeedSwap || board.speedMatch != lastSpeedMatch || board.speedDraw != lastSpeedDraw)
        {
            lastEaseMatch = board.easeMatch;
            lastSpeedSwap = board.speedSwap;
            lastSpeedMatch = board.speedMatch;
            board.Initiate();
        }
    }

}
