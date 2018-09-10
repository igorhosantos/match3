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

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Awake()
    {
        MatchController.ME.StartSession();

        logButton.onClick.AddListener(Teste);

    }

    private void Teste()
    {
        MatchController.ME.LogButton(lineOrigin, collumOrigin, lineDestiny, collumnDestiny);
    }
}
