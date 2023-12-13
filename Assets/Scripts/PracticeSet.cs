using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Text.RegularExpressions;
using System.Linq;


public class PracticeSet: MonoBehaviourPunCallbacks
{
    BlackJackManager _BlackJackManager;
    private PhotonView _PhotonView;
    public int MySelectedCard;// { get; set; }
    public int YourSelectedCard;// { get; set; }
    public void SetMySelectedCard(int card)
    {
        MySelectedCard = card;
        _PhotonView.RPC("UpdateMySelectedCardOnAllClients", RpcTarget.Others, card);
    }
    [PunRPC]
    void UpdateMySelectedCardOnAllClients(int _Number)
    {
        // ここでカードデータを再構築
        MySelectedCard = _Number;
    }
    public void SetYourSelectedCard(int card)
    {
        YourSelectedCard = card;
        _PhotonView.RPC("UpdateYourSelectedCardOnAllClients", RpcTarget.Others, card);
    }
    [PunRPC]
    void UpdateYourSelectedCardOnAllClients(int _Number)
    {
        // ここでカードデータを再構築
        YourSelectedCard = _Number;
    }
    public List<List<int>> MyCardsPracticeList { get; set; } = new List<List<int>>();
    public List<List<int>> YourCardsPracticeList { get; set; } = new List<List<int>>();
    public List<int> FieldCardsPracticeList /*{ get; set; }*/ = new List<int>();
    public void SetMyCardsPracticeList(List<List<int>> _MyCardsPracticeList)
    {
        List<List<int>> temp = _MyCardsPracticeList;
        MyCardsPracticeList = temp;
        _PhotonView.RPC("UpdateMyCardsPracticeListOnAllClients", RpcTarget.Others, SerializeCardList(_MyCardsPracticeList));
    }
    [PunRPC]
    void UpdateMyCardsPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        MyCardsPracticeList = DeserializeCardList(serializeCards);
    }
    public void SetYourCardsPracticeList(List<List<int>> _YourCardsPracticeList)
    {
        List<List<int>> temp = _YourCardsPracticeList;
        YourCardsPracticeList = temp;
        _PhotonView.RPC("UpdateYourCardsPracticeListOnAllClients", RpcTarget.Others, SerializeCardList(_YourCardsPracticeList));
    }
    [PunRPC]
    void UpdateYourCardsPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        YourCardsPracticeList = DeserializeCardList(serializeCards);
    }
    public void SetFieldCardsList(List<int> _FieldCardsPracticeList)
    {
        List<int> temp = FieldCardsPracticeList;
        FieldCardsPracticeList = temp;
        _PhotonView.RPC("UpdateFieldCardsPracticeListOnAllClients", RpcTarget.Others, SerializeFieldCard(_FieldCardsPracticeList));
    }
    [PunRPC]
    void UpdateFieldCardsPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        FieldCardsPracticeList = DeserializeFieldCard(serializeCards);
    }

    private string SerializeCardList(List<List<int>> cards)
    {

        string cards_json = "[";
        for (int i = 0; i < cards.Count; i++)
        {
            cards_json += JsonHelper.ToJson(cards[i]) + ",";
        }
        cards_json = cards_json.Remove(cards_json.Length - 1);
        cards_json += "]";
        return cards_json;
    }

    private List<List<int>> DeserializeCardList(string json)
    {
        Regex regex = new Regex(@"\d+");

        List<int> numbers = new List<int>();
        foreach (Match match in regex.Matches(json))
        {
            numbers.Add(int.Parse(match.Value));
        }
        List<List<int>> cardList = new List<List<int>>();
        // JSON 文字列を int[] の配列に変換
        for(int i = 0; i<NumberofSet; i++)
        {
            List<int> Element = new List<int>();
            for(int j = 0; j < NumberofCards; j++)
            {
                Element.Add(numbers[i*NumberofCards + j]);
            }
            cardList.Add(Element);
        }
        return cardList;
    }

    private string SerializeFieldCard(List<int> cards)
    {
        return JsonHelper.ToJson(cards);
    }

    private List<int> DeserializeFieldCard(string serializedCards)
    {
        Regex regex = new Regex(@"\d+");

        List<int> numbers = new List<int>();
        foreach (Match match in regex.Matches(serializedCards))
        {
            numbers.Add(int.Parse(match.Value));
        }        
        return numbers;
    }

    [System.Serializable]
    private class SerializationWrapper<T>
    {
        public T data;

        public SerializationWrapper(T data)
        {
            this.data = data;
        }
    }

    public List<List<int>> MyCardsSuitPracticeList = new List<List<int>>();
    public List<List<int>> YourCardsSuitPracticeList = new List<List<int>>();
    public List<int> FieldCardsSuitPracticeList = new List<int>();
    public void SetMyCardsSuitPracticeList(List<List<int>> _MyCardsSuitPracticeList)
    {
        List<List<int>> temp = _MyCardsSuitPracticeList;
        MyCardsSuitPracticeList = temp;
        _PhotonView.RPC("UpdateMyCardsSuitPracticeListOnAllClients", RpcTarget.Others, SerializeCardList(_MyCardsSuitPracticeList));
    }
    [PunRPC]
    void UpdateMyCardsSuitPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        MyCardsSuitPracticeList = DeserializeCardList(serializeCards);
    }
    public void SetYourCardsSuitPracticeList(List<List<int>> _YourCardsSuitPracticeList)
    {
        List<List<int>> temp = _YourCardsSuitPracticeList;
        YourCardsSuitPracticeList = temp;
        _PhotonView.RPC("UpdateYourCardsSuitPracticeListOnAllClients", RpcTarget.Others, SerializeCardList(_YourCardsSuitPracticeList));
    }
    [PunRPC]
    void UpdateYourCardsSuitPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        YourCardsSuitPracticeList = DeserializeCardList(serializeCards);
    }
    public void SetFieldCardsSuitList(List<int> _FieldCardsSuitPracticeList)
    {
        List<int> temp = FieldCardsSuitPracticeList;
        FieldCardsSuitPracticeList = temp;
        _PhotonView.RPC("UpdateFieldCardsSuitPracticeListOnAllClients", RpcTarget.Others, SerializeFieldCard(_FieldCardsSuitPracticeList));
    }
    [PunRPC]
    void UpdateFieldCardsSuitPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        FieldCardsSuitPracticeList = DeserializeFieldCard(serializeCards);
    }

    public enum BlackJackStateList
    {
        BeforeStart,
        WaitForNextTrial,
        ShowMyCards,
        SelectCards,
        ShowResult,
        Finished,
    }
    public BlackJackStateList BlackJackState = BlackJackStateList.BeforeStart;

    public void SetBlackJackState(BlackJackStateList _BlackJackState)
    {
        BlackJackState = _BlackJackState;
        _PhotonView.RPC("UpdateBlackJackStateListOnAllClients", RpcTarget.Others, SerializeBlackJackState(_BlackJackState));
    }
    [PunRPC]
    void UpdateBlackJackStateListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        BlackJackState = DeserializeBlackJackState(serializeCards);
    }

    private string SerializeBlackJackState(BlackJackStateList _BlackJackState)
    {
        return JsonUtility.ToJson(new SerializationWrapper<BlackJackStateList>(_BlackJackState));
    }

    private BlackJackStateList DeserializeBlackJackState(string serializedCards)
    {
        return JsonUtility.FromJson<SerializationWrapper<BlackJackStateList>>(serializedCards).data;
    }

    public int TrialAll;
    public int NumberofCards = 5;


    public int NumberofSet = 10;
    int FieldCards = 0;

    List<int> MyCards;
    List<int> YourCards;
    private static List<int> MyCardsSuit;
    private static List<int> YourCardsSuit;
    private static int FieldCardsSuit = 0;
    private void Start()
    {
        _PhotonView = GetComponent<PhotonView>();
        _BlackJackManager = GameObject.FindWithTag("Manager").GetComponent<BlackJackManager>();
    }
    public void UpdateParameter()
    {
        for (int i = 0; i < NumberofSet; i++)
        {
            //DecidingCards(Random.Range(0, NumberofCards));
            DecidingCards(RandomValue());
            FieldCardsPracticeList.Add(FieldCards);
            MyCardsPracticeList.Add(MyCards);
            YourCardsPracticeList.Add(YourCards);
            FieldCardsSuitPracticeList.Add(FieldCardsSuit);
            MyCardsSuitPracticeList.Add(MyCardsSuit);
            YourCardsSuitPracticeList.Add(YourCardsSuit);
        }
        SetMyCardsPracticeList(MyCardsPracticeList);
        SetYourCardsPracticeList(YourCardsPracticeList);
        SetFieldCardsList(FieldCardsPracticeList);
        SetMyCardsSuitPracticeList(MyCardsSuitPracticeList);
        SetYourCardsSuitPracticeList(YourCardsSuitPracticeList);
        SetFieldCardsSuitList(FieldCardsSuitPracticeList);
        InitializeCard();
    }
    private int RandomValue()
    {
        int result = Random.Range(0, 4);
        while(result == 1)
        {
            result = Random.Range(0, 4);
        }
        Debug.Log(result);
        return result;
    }
    public void InitializeCard()
    {
        _BlackJackManager.InitializeCard();
        _PhotonView.RPC("RPCInitializeCard", RpcTarget.Others);
    }
    [PunRPC]
    void RPCInitializeCard()
    {
        // ここでカードデータを再構築
        _BlackJackManager.InitializeCard();
    }

    void DecidingCards(int _j)
    {
        DecideRandomCards();
        while (CheckDoubleCard())
        {
            DecideRandomCards();
        }
    }

    void DecideCards(int _j)
    {
        MyCards = new List<int>();
        YourCards = new List<int>();
        MyCardsSuit = new List<int>();
        YourCardsSuit = new List<int>();
        FieldCards = UnityEngine.Random.Range(1, 14);
        FieldCardsSuit = UnityEngine.Random.Range(0, 4);
        int _targetSum = 21 - FieldCards;
        if (_j > 0)
        {
            for (int i = 0; i < _j; i++)
            {
                int card = UnityEngine.Random.Range(1, 14);
                while (ValidityCheck(_targetSum, card, MyCards))
                {
                    card = UnityEngine.Random.Range(1, 14);
                }
                MyCards.Add(card);
                YourCards.Add(_targetSum - card);
                MyCardsSuit.Add(UnityEngine.Random.Range(0, 4));
                YourCardsSuit.Add(UnityEngine.Random.Range(0, 4));
            }
        }
        if (_j < NumberofCards)
        {
            for (int i = 0; i < NumberofCards - _j; i++)
            {
                int mycard = UnityEngine.Random.Range(1, 14);
                int yourcard = UnityEngine.Random.Range(1, 14);
                while (ValidityCheck_remaining(_targetSum, mycard, yourcard, MyCards, YourCards))
                {
                    mycard = UnityEngine.Random.Range(1, 14);
                    yourcard = UnityEngine.Random.Range(1, 14);
                }
                MyCards.Add(mycard);
                YourCards.Add(yourcard);
                MyCardsSuit.Add(UnityEngine.Random.Range(0, 4));
                YourCardsSuit.Add(UnityEngine.Random.Range(0, 4));
            }
        }
        ShuffleCards();
    }
    void DecideRandomCards()
    {
        MyCards = new List<int>();
        YourCards = new List<int>();
        MyCardsSuit = new List<int>();
        YourCardsSuit = new List<int>();
        FieldCards = UnityEngine.Random.Range(1, 14);
        //FieldCards = 6;
        FieldCardsSuit = UnityEngine.Random.Range(0, 4);
        for (int i = 0; i < 5; i++)
        {
            MyCards.Add(UnityEngine.Random.Range(1, 14));
            YourCards.Add(UnityEngine.Random.Range(1, 14));
            //MyCards.Add(i + 6);
            //YourCards.Add(i + 5);
            MyCardsSuit.Add(UnityEngine.Random.Range(0, 4));
            YourCardsSuit.Add(UnityEngine.Random.Range(0, 4));
        }
        ShuffleCards();
    }
    private bool CheckDoubleCard()
    {
        var combinedList = new List<(int, int)>();

        // MyCards と MyCardsSuit の組み合わせを追加
        for (int i = 0; i < MyCards.Count; i++)
        {
            combinedList.Add((MyCards[i], MyCardsSuit[i]));
        }

        // YourCards と YourCardsSuit の組み合わせを追加
        for (int i = 0; i < YourCards.Count; i++)
        {
            combinedList.Add((YourCards[i], YourCardsSuit[i]));
        }

        // FieldCards と FieldCardsSuit の組み合わせを追加
        combinedList.Add((FieldCards, FieldCardsSuit));

        // 重複があるかチェック
        return combinedList.GroupBy(x => x).Any(g => g.Count() > 1);
    }
    bool CheckmorethanfourCards()
    {
        bool Result = false;
        for (int k = 1; k < 14; k++)
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
        foreach (var eachcard in _MyCard) if (eachcard == card) Result = true;
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

    public void MoveToWaitForNextTrial(int _nowTrial)
    {
        _BlackJackManager.MoveToWaitForNextTrial(_nowTrial);
        _PhotonView.RPC("RPCMoveToWaitForNextTrial", RpcTarget.Others, _nowTrial);
    }
    [PunRPC]
    void RPCMoveToWaitForNextTrial(int _nowTrial)
    {
        // ここでカードデータを再構築
        _BlackJackManager.MoveToWaitForNextTrial(_nowTrial);
    }

    public void MoveToShowMyCards(int hostorClient)
    {
        _BlackJackManager.MoveToShowMyCards(0);
        _PhotonView.RPC("RPCMoveToShowMyCards", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMoveToShowMyCards()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MoveToShowMyCards(1);
    }
    
    public void MoveToSelectCards()
    {
        _BlackJackManager.MoveToSelectCards();
        _PhotonView.RPC("RPCMoveToSelectCards", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMoveToSelectCards()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MoveToSelectCards();
    }
    public void MoveToShowResult()
    {
        _BlackJackManager.MoveToShowResult();
        _PhotonView.RPC("RPCMoveToShowResult", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMoveToShowResult()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MoveToShowResult();
    }
    public void MakeReadyHost()
    {
       _BlackJackManager.MakeReadyHost();
        _PhotonView.RPC("RPCMakeReadyHost", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMakeReadyHost()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MakeReadyHost();
    }
    public void MakeReadyClient()
    {
        _BlackJackManager.MakeReadyClient();
        _PhotonView.RPC("RPCMakeReadyClient", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMakeReadyClient()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MakeReadyClient();
    }
}
