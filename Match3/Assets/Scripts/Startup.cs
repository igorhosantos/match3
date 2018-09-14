using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Startup : MonoBehaviour
{

    public Button logButton;

    public int lineOrigin;
    public int collumOrigin;
    public int lineDestiny;
    public int collumnDestiny;

    private Canvas mainCanvas;
    private GameView view;

    private void Awake()
    {
        MatchController.ME.StartSession();

        logButton.onClick.AddListener(Teste);

        mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        view = mainCanvas.transform.Find("Game").gameObject.AddComponent<GameView>();
    }

    private void Teste()
    {
        MatchController.ME.LogButton(lineOrigin, collumOrigin, lineDestiny, collumnDestiny);
    }
}
