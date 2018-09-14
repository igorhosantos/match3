using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceView : MonoBehaviour
{
    private Image image;
    private Text text;
    public RectTransform piecePosition;
    public Button button;

    public Piece currentPiece { get; private set; }

    void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        piecePosition = GetComponent<RectTransform>();
    }
    
    public void Initate(Piece p)
    {
        currentPiece = p;
        switch (p.type)
        {
            case PieceType.R:
                image.sprite = Resources.Load<Sprite>("Sprites/jewel1");
                break;
            case PieceType.G:
                image.sprite = Resources.Load<Sprite>("Sprites/jewel2");
                break;
            case PieceType.B:
                image.sprite = Resources.Load<Sprite>("Sprites/jewel3");
                break;
            default:
                Debug.LogError("NEED TO IMPLEMENT TYPE: " +  p.type);
                break;
        }

        UpdateText(p);
    }

    public void UpdateText(Piece p)
    {
        text.text = p.tupplePosition.ToString();
    }

}
