using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MakePracticeSet : MonoBehaviour
{
    [SerializeField] int NumberofSet = 1000;
    [SerializeField] int NumberofMyCard = 5;
    [SerializeField] string filename;
    private string PracticeSet;
    private int FieldCards = 0;
    private List<int> MyCards;
    private List<int> YourCards;

    // Start is called before the first frame update
    void Start()
    {
        PracticeSet = "FieldCards";
        for (int i = 1; i < NumberofMyCard + 1; i++) PracticeSet += ",MyCards"+i.ToString();
        for (int i = 1; i < NumberofMyCard + 1; i++) PracticeSet += ",YourCards" + i.ToString();
        for (int i = 1; i < NumberofMyCard + 1; i++) WriteCSV(PracticeSet,filename+"_"+i.ToString());

        for (int i = 0; i < NumberofSet; i++)
        {
            for (int j = 1; j < NumberofMyCard + 1; j++)
            {
                DecidingCards(j);
                WriteContent(j);
                /*cardPool = new List<int>();
                InitializeCardPool();
                SelectFieldCard();
                DistributeCards(j);*/
            }
        }
    }
    void WriteContent(int j)
    {
        PracticeSet = FieldCards.ToString();
        for (int i = 0; i < NumberofMyCard; i++) PracticeSet += "," + MyCards[i].ToString();
        for (int i = 0; i < NumberofMyCard; i++) PracticeSet += "," + YourCards[i].ToString();
        WriteCSV(PracticeSet, filename + "_" + j.ToString());
    }
    void DecidingCards(int _j)
    {
        DecideCards(_j);
        while(CheckmorethanfourCards())
        {
            DecideCards(_j);
        }
    }

    void DecideCards(int _j)
    {
        MyCards = new List<int>();
        YourCards = new List<int>();
        FieldCards = UnityEngine.Random.Range(1, 14);
        int _targetSum = 21 - FieldCards;
        for (int i = 0; i < _j; i++)
        {
            int card = UnityEngine.Random.Range(1, 14);
            while (ValidityCheck(_targetSum, card, MyCards))
            {
                card = UnityEngine.Random.Range(1, 14);
            }
            MyCards.Add(card);
            YourCards.Add(_targetSum - card);
        }
        if(_j < NumberofMyCard)
        {
            for (int i = 0; i < NumberofMyCard - _j; i++)
            {
                int mycard = UnityEngine.Random.Range(1, 14);
                int yourcard = UnityEngine.Random.Range(1, 14);
                while (ValidityCheck_remaining(_targetSum,mycard, yourcard, MyCards,YourCards))
                {
                    mycard = UnityEngine.Random.Range(1, 14);
                    yourcard = UnityEngine.Random.Range(1, 14);
                }
                MyCards.Add(mycard);
                YourCards.Add(yourcard);
            }
        }
        ShuffleCards();
    }
    bool CheckmorethanfourCards()
    {
        bool Result = false;
        for(int k = 1; k < 14; k++)
        {
            int number = 0;
            if (FieldCards == k) number++;
            foreach (var i in MyCards) if (i == k) number++;
            foreach (var i in YourCards) if (i == k) number++;
            if (number > 4) Result = true;
        }
        return Result;
    }
    bool ValidityCheck(int _targetSum, int card, List<int> _MyCard)
    {
        bool Result = false;
        if (_targetSum <= card) Result = true;
        if (_targetSum - card > 13) Result = true;
        foreach (var eachcard in _MyCard) if(eachcard == card) Result = true;
        return Result;
    }
    bool ValidityCheck_remaining(int _targetSum, int mycard, int yourcard, List<int> _MyCard, List<int> _YourCard)
    {
        bool Result = false;
        if (mycard + yourcard == _targetSum) Result = true;
        //foreach (var eachcard in _MyCard) if (eachcard == mycard) Result = true;
        foreach (var eachcard in _MyCard) if (yourcard + eachcard == _targetSum) Result = true;
        return Result;
    }
    void ShuffleCards()
    {
        for (int i = 0; i < MyCards.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, MyCards.Count);
            int temp = MyCards[i];
            MyCards[i] = MyCards[randomIndex];
            MyCards[randomIndex] = temp;
        }
        for (int i = 0; i < YourCards.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, YourCards.Count);
            int temp = YourCards[i];
            YourCards[i] = YourCards[randomIndex];
            YourCards[randomIndex] = temp;
        }
    }
    
private void WriteCSV(string line, string _filename)
    {
        StreamWriter streamWriter;
        FileInfo fileInfo;
        fileInfo = new FileInfo(Application.dataPath + "/StreamingAssets/" + _filename + ".csv");
        streamWriter = fileInfo.AppendText();
        streamWriter.WriteLine(line);
        streamWriter.Flush();
        streamWriter.Close();
    }

}
