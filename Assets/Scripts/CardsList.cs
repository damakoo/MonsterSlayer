using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardsList : MonoBehaviour
{
    [SerializeField] BlackJackManager _blackJackManager; 
    public List<CardState> MyCardsList;
    public List<CardState> YourCardsList;
    //public List<CardState_opponent> MyCardsList_opponent;
    //public List<CardState_opponent> YourCardsList_opponent;
    public CardState MyFieldCard { get; set; }
    //public CardState YourFieldCard { get; set; }
    [SerializeField] GameObject CardPrefab;
    [SerializeField] GameObject CardsListParent;
    [SerializeField] Transform MyCards_upper;
    [SerializeField] Transform MyCards_lower;
    //[SerializeField] Transform MyCards_opponent_upper;
    //[SerializeField] Transform MyCards_opponent_lower;
    [SerializeField] Transform YourCards_upper;
    [SerializeField] Transform YourCards_lower;
    //[SerializeField] Transform YourCards_opponent_upper;
    //[SerializeField] Transform YourCards_opponent_lower;
    [SerializeField] Transform MyFieldCardtransform;
    //[SerializeField] Transform YourFieldCardtransform;
    PracticeSet _PracticeSet;
    public void SetPracticeSet(PracticeSet _practiceset)
    {
        _PracticeSet = _practiceset;
    }
    public void InitializeCards()
    {
        for(int i = 0; i < _PracticeSet.NumberofCards; i++)
        {
            if(_blackJackManager._hostorclient == BlackJackManager.HostorClient.Host)
            {
                GameObject mycard = Instantiate(CardPrefab, CardPos(i, _PracticeSet.NumberofCards, MyCards_upper.position, MyCards_lower.position), Quaternion.identity, CardsListParent.transform);
                mycard.name = "MyCard" + i.ToString();
                CardState mycardState = mycard.AddComponent<CardState>().Initialize(mycard, true, i);
                MyCardsList.Add(mycardState);


                GameObject yourcard = Instantiate(CardPrefab, CardPos(i, _PracticeSet.NumberofCards, YourCards_upper.position, YourCards_lower.position), Quaternion.identity, CardsListParent.transform);
                yourcard.name = "YourCard" + i.ToString();
                CardState yourcardState = yourcard.AddComponent<CardState>().Initialize(yourcard, false, i);
                YourCardsList.Add(yourcardState);

            }
            else if (_blackJackManager._hostorclient == BlackJackManager.HostorClient.Client)
            {
                GameObject mycard = Instantiate(CardPrefab, CardPos(i, _PracticeSet.NumberofCards, YourCards_upper.position, YourCards_lower.position), Quaternion.identity, CardsListParent.transform);
                mycard.name = "MyCard" + i.ToString();
                CardState mycardState = mycard.AddComponent<CardState>().Initialize(mycard, false,i);
                MyCardsList.Add(mycardState);


                GameObject yourcard = Instantiate(CardPrefab, CardPos(i, _PracticeSet.NumberofCards, MyCards_upper.position, MyCards_lower.position), Quaternion.identity, CardsListParent.transform);
                yourcard.name = "YourCard" + i.ToString();
                CardState yourcardState = yourcard.AddComponent<CardState>().Initialize(yourcard, true,i);
                YourCardsList.Add(yourcardState);

            }

            //GameObject mycard_opponent = Instantiate(CardPrefab, CardPos(i, _PracticeSet.NumberofCards, MyCards_opponent_upper.position, MyCards_opponent_lower.position), Quaternion.Euler(new Vector3(0, 0, -90)), CardsListParent.transform);
            //mycard_opponent.name = "MyCard_opponent" + i.ToString();
            //CardState_opponent mycardState_opponent = mycard_opponent.AddComponent<CardState_opponent>().Initialize(mycard_opponent, yourcardState);
            //MyCardsList_opponent.Add(mycardState_opponent);

            //GameObject yourcard_opponent = Instantiate(CardPrefab, CardPos(i, _PracticeSet.NumberofCards, YourCards_opponent_upper.position, YourCards_opponent_lower.position), Quaternion.Euler(new Vector3(0, 0, 90)), CardsListParent.transform);
            //yourcard_opponent.name = "YourCard_opponent" + i.ToString();
            //CardState_opponent yourcardState_opponent = yourcard_opponent.AddComponent<CardState_opponent>().Initialize(yourcard_opponent, mycardState);
            //YourCardsList_opponent.Add(yourcardState_opponent);

        }
        GameObject myfieldcard = Instantiate(CardPrefab, MyFieldCardtransform.position, Quaternion.identity, CardsListParent.transform);
        myfieldcard.name = "MyFieldCard";
        MyFieldCard = myfieldcard.AddComponent<CardState>().Initialize(myfieldcard, false, 0);

        //GameObject yourfieldcard = Instantiate(CardPrefab, YourFieldCardtransform.position, Quaternion.Euler(new Vector3(0, 0, 90)), CardsListParent.transform);
        //yourfieldcard.name = "YourFieldCard";
        //YourFieldCard = yourfieldcard.AddComponent<CardState>().Initialize(yourfieldcard, false);

    }
    private Vector3 CardPos(int i, int _numberofcards, Vector3 start, Vector3 end)
    {
        return Vector3.Lerp(start,end,(float)i/ ((float)_numberofcards-1f));
    }

    public void SetCards(int Trial)
    {
        for (int i = 0; i < _PracticeSet.NumberofCards; i++)
        {
            MyCardsList[i].Number = _PracticeSet.MyCardsPracticeList[Trial][i];
            YourCardsList[i].Number = _PracticeSet.YourCardsPracticeList[Trial][i];
            MyCardsList[i].suit = (Suit)Enum.ToObject(typeof(Suit), _PracticeSet.MyCardsSuitPracticeList[Trial][i]);
            YourCardsList[i].suit = (Suit)Enum.ToObject(typeof(Suit), _PracticeSet.YourCardsSuitPracticeList[Trial][i]);
        }
        MyFieldCard.Number = _PracticeSet.FieldCardsPracticeList[Trial];
        MyFieldCard.suit = (Suit)Enum.ToObject(typeof(Suit), _PracticeSet.FieldCardsSuitPracticeList[Trial]);
        //YourFieldCard.Number = _PracticeSet.FieldCardsPracticeList[Trial];
    }

    public void AllOpen()
    {
        for (int i = 0; i < _PracticeSet.NumberofCards; i++)
        {
            MyCardsList[i].Open();
            YourCardsList[i].Open();
            //MyCardsList_opponent[i].Open();
            //YourCardsList_opponent[i].Open();
        }
        MyFieldCard.Open();
        //YourFieldCard.Open();
    }
    public void AllClose()
    {
        for (int i = 0; i < _PracticeSet.NumberofCards; i++)
        {
            MyCardsList[i].Close();
            YourCardsList[i].Close();
            //MyCardsList_opponent[i].Close();
            //YourCardsList_opponent[i].Close();
        }
        MyFieldCard.Close();
        //YourFieldCard.Close();
    }
    public void MyCardsOpen()
    {
        for (int i = 0; i < _PracticeSet.NumberofCards; i++)
        {
            MyCardsList[i].Open();
            //YourCardsList[i].Open();
        }
        MyFieldCard.Open();
        //YourFieldCard.Open();
    }
    public void YourCardsOpen()
    {
        for (int i = 0; i < _PracticeSet.NumberofCards; i++)
        {
            //MyardsList[i].Open();
            YourCardsList[i].Open();
        }
        MyFieldCard.Open();
        //YourFieldCard.Open();
    }
}
