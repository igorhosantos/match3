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
    public Rigidbody2D piecePhysics;
    public Piece currentPiece { get; private set; }

    void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        piecePhysics = GetComponent<Rigidbody2D>();
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
            case PieceType.P:
                image.sprite = Resources.Load<Sprite>("Sprites/jewel4");
                break;
            default:
                Debug.LogError("NEED TO IMPLEMENT TYPE: " +  p.type);
                break;
        }

        UpdateText();
    }

    public void UpdateText()
    {
        text.text = currentPiece.tupplePosition.ToString();
    }

    public void DestroyPiece()
    {
        //image.color = ColorHEX(0xffffff,0.3f);
        Destroy(gameObject);
    }

    public Color ColorHEX(int hexadecimal, float alpha = 1f)
    {
        var rgbColor = new Color
        {
            r = ((hexadecimal >> 16) & 0xFF) / 255.0f,
            g = ((hexadecimal >> 8) & 0xFF) / 255.0f,
            b = (hexadecimal & 0xFF) / 255.0f,
            a = alpha
        };

        return rgbColor;
    }

    public void SetOld()
    {
        image.color = ColorHEX(0xffffff,0.3f);
    }


}
