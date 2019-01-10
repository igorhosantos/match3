using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Startup : MonoBehaviour
{
    private Canvas mainCanvas;
    private GameView view;

    private void Awake()
    {
        mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        view = mainCanvas.transform.Find("Game").gameObject.AddComponent<GameView>();
    }

   
}
