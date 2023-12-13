using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Renderer))]
public class CardState : MonoBehaviour
{
    //public bool isOpen;// { get; set; }
    public int Number;// { get; set; }
    public bool MyCard;
    public int ID { get; set; }
    public Suit suit { get; set; }
    private SuitPNG suitPNG;
    GameObject thisgameobject;
    Renderer thisrenderer;
    TextMeshProUGUI thistextMeshPro;
    public CardState Initialize(GameObject _thisgameobject, bool _myCard, int _ID)
    {
        MyCard = _myCard;
        thisrenderer = GetComponent<Renderer>();
        //isOpen = false;
        thisgameobject = _thisgameobject;
        thistextMeshPro = thisgameobject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        suitPNG = _thisgameobject.transform.GetChild(0).GetChild(1).GetComponent<SuitPNG>();
        ID = _ID;
        Close();
        return this;
    }

    public void Open()
    {
        //isOpen = true;
        thistextMeshPro.text = Number.ToString();
        thisrenderer.material.color = Color.white;
        suitPNG.SetSprite(DecideSuit(suit));
    }
    public void Close()
    {
        //isOpen = false;
        thistextMeshPro.text = "";
        thisrenderer.material.color = Color.gray;
        suitPNG.SetSprite(suitPNG.TransParent);
    }

    public void Clicked()
    {
        thisrenderer.material.color = Color.yellow;
    }
    public void UnClicked()
    {
        thisrenderer.material.color = Color.white;
    }
    private Sprite DecideSuit(Suit _suit)
    {
        switch (_suit)
        {
            case Suit.Spades:
                return suitPNG.Spade;

            case Suit.Diamonds:
                return suitPNG.Diamond;

            case Suit.Hearts:
                return suitPNG.Heart;

            case Suit.Clubs:
                return suitPNG.Club;

            default:
                return null;
        }
    }
}
