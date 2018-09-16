using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Startup : MonoBehaviour
{
    
    public int lineOrigin;
    public int collumOrigin;
    public int lineDestiny;
    public int collumnDestiny;

    private Canvas mainCanvas;
    private GameView view;

    private void Awake()
    {
        MatchController.ME.StartSession();

    
        mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        view = mainCanvas.transform.Find("Game").gameObject.AddComponent<GameView>();
    }
    
}
