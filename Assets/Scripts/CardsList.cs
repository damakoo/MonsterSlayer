using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardsList : MonoBehaviour
{
    [SerializeField] GameObject MonsterIcon;
    [SerializeField] BlackJackManager _blackJackManager; 
    public List<CardState> MyCardsList;
    public CardState MyFieldCard { get; set; }
    public CardState MyResultCard { get; set; }
    [SerializeField] GameObject CardPrefab;
    [SerializeField] GameObject CardsListParent;
    [SerializeField] Transform MyCards_upper;
    [SerializeField] Transform MyCards_lower;
    [SerializeField] Transform MyFieldCardtransform;
    [SerializeField] Transform MyResultCardtransform;
    PracticeSet _PracticeSet;
    public void SetPracticeSet(PracticeSet _practiceset)
    {
        _PracticeSet = _practiceset;
    }
    public void InitializeCards()
    {
        MonsterIcon.SetActive(true);
        for(int i = 0; i < _PracticeSet.NumberofCards; i++)
        {
            GameObject mycard = Instantiate(CardPrefab, CardPos(i, _PracticeSet.NumberofCards, MyCards_upper.position, MyCards_lower.position), Quaternion.identity, CardsListParent.transform);
            mycard.name = "MyCard" + i.ToString();
            CardState mycardState = mycard.AddComponent<CardState>().Initialize(mycard, true,i);
            MyCardsList.Add(mycardState);
        }
        GameObject myfieldcard = Instantiate(CardPrefab, MyFieldCardtransform.position, Quaternion.identity, CardsListParent.transform);
        myfieldcard.name = "MyFieldCard";
        MyFieldCard = myfieldcard.AddComponent<CardState>().Initialize(myfieldcard, false, 0);
        Vector3 currentScale = myfieldcard.transform.localScale;
        myfieldcard.transform.localScale = new Vector3(currentScale.x * 2, currentScale.y * 2, currentScale.z);

        GameObject myresultcard = Instantiate(CardPrefab, MyResultCardtransform.position, Quaternion.identity, CardsListParent.transform);
        myfieldcard.name = "MyResultCard";
        MyResultCard = myresultcard.AddComponent<CardState>().Initialize(myresultcard, false, 0);
        currentScale = myresultcard.transform.localScale;
        myresultcard.transform.localScale = new Vector3(currentScale.x * 2, currentScale.y * 2, currentScale.z);
        _PracticeSet.MySelectedTime = new List<float>();
        _PracticeSet.YourSelectedTime = new List<float>();
        for (int i = 0; i < _PracticeSet.NumberofSet; i++)
        {
            _PracticeSet.MySelectedTime.Add(0);
            _PracticeSet.YourSelectedTime.Add(0);
        }
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
        }
        MyFieldCard.Number = _PracticeSet.FieldCardsPracticeList[Trial];
        //YourFieldCard.Number = _PracticeSet.FieldCardsPracticeList[Trial];
    }

    public void AllOpen()
    {
        MyCardsOpen();
    }
    public void AllClose()
    {
        for (int i = 0; i < _PracticeSet.NumberofCards; i++)
        {
            MyCardsList[i].Close();
        }
        MyFieldCard.Close();
        MyResultCard.Close();
    }
    public void MyCardsOpen()
    {
        for (int i = 0; i < _PracticeSet.NumberofCards; i++)
        {
            MyCardsList[i].Open();
        }
    }

    public void FieldCardsOpen()
    {
        MyFieldCard.Open();
    }
}
