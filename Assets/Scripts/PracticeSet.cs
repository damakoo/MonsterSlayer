using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Text.RegularExpressions;
using System.Linq;


public class PracticeSet: MonoBehaviourPunCallbacks
{
    private List<Vector3> FieldCardPotential = new List<Vector3>();
    private List<(int, int, int)> MyCardPotential;
    BlackJackManager _BlackJackManager { get; set; }
    private PhotonView _PhotonView;
    public int MySelectedCard { get; set; }
    public int YourSelectedCard { get; set; }
    public List<float> MySelectedTime { get; set; }
    public List<float> YourSelectedTime { get; set; }
    public int MySelectedBet { get; set; }
    public int YourSelectedBet { get; set; }
    public void SetMySelectedBet(int bet)
    {
        MySelectedBet = bet;
        _PhotonView.RPC("UpdateMySelectedBetOnAllClients", RpcTarget.Others, bet);
    }
    [PunRPC]
    void UpdateMySelectedBetOnAllClients(int bet)
    {
        // ここでカードデータを再構築
        MySelectedBet = bet;
    }
    public void SetYourSelectedBet(int bet)
    {
        YourSelectedBet = bet;
        _PhotonView.RPC("UpdateYourSelectedBetOnAllClients", RpcTarget.Others, bet);
    }
    [PunRPC]
    void UpdateYourSelectedBetOnAllClients(int bet)
    {
        // ここでカードデータを再構築
        YourSelectedBet = bet;
    }
    public void SetMySelectedTime(float time, int trial)
    {
        MySelectedTime[trial] = time;
        _PhotonView.RPC("UpdateMySelectedTimeOnAllClients", RpcTarget.Others, time,trial);
    }
    [PunRPC]
    void UpdateMySelectedTimeOnAllClients(float time, int trial)
    {
        // ここでカードデータを再構築
        MySelectedTime[trial] = time;
    }
    public void SetYourSelectedTime(float time, int trial)
    {
        YourSelectedTime[trial] = time;
        _PhotonView.RPC("UpdateYourSelectedTimeOnAllClients", RpcTarget.Others, time, trial);
    }
    [PunRPC]
    void UpdateYourSelectedTimeOnAllClients(float time, int trial)
    {
        // ここでカードデータを再構築
        YourSelectedTime[trial] = time;
    }
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
    public List<List<Vector3>> MyCardsPracticeList { get; set; } = new List<List<Vector3>>();
    public List<Vector3> FieldCardsPracticeList /*{ get; set; }*/ = new List<Vector3>();
    public void SetMyCardsPracticeList(List<List<Vector3>> _MyCardsPracticeList)
    {
        List<List<Vector3>> temp = _MyCardsPracticeList;
        MyCardsPracticeList = temp;
        _PhotonView.RPC("UpdateMyCardsPracticeListOnAllClients", RpcTarget.Others, SerializeCardList(_MyCardsPracticeList));
    }
    [PunRPC]
    void UpdateMyCardsPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        MyCardsPracticeList = DeserializeCardList(serializeCards);
    }
    public void SetFieldCardsList(List<Vector3> _FieldCardsPracticeList)
    {
        List<Vector3> temp = FieldCardsPracticeList;
        FieldCardsPracticeList = temp;
        _PhotonView.RPC("UpdateFieldCardsPracticeListOnAllClients", RpcTarget.Others, SerializeFieldCard(_FieldCardsPracticeList));
    }
    [PunRPC]
    void UpdateFieldCardsPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        FieldCardsPracticeList = DeserializeFieldCard(serializeCards);
    }

    private string SerializeCardList(List<List<Vector3>> cards)
    {

        string cards_json = "";
        for (int set = 0; set < NumberofSet; set++)
        {
            for(int card = 0;card < NumberofCards; card++)
            {
                cards_json += SerializeVector3(cards[set][card]) + ",";
            }
        }
        cards_json = cards_json.Remove(cards_json.Length - 1);
        //cards_json += "]";
        return cards_json;
    }
    private string SerializeVector3(Vector3 cards)
    {
        return "[" + cards.x.ToString() + "," + cards.y.ToString() + "," + cards.z.ToString() + "]";
    }
    private Vector3 DeSerializeVector3(string cards)
    {
        // 角括弧を取り除く
        cards = cards.TrimStart('[').TrimEnd(']');

        // コンマで分割
        string[] values = cards.Split(',');

        // floatに変換してVector3を作成
        float x = float.Parse(values[0]);
        float y = float.Parse(values[1]);
        float z = float.Parse(values[2]);

        return new Vector3(x, y, z);
    }

    private List<List<Vector3>> DeserializeCardList(string json)
    {
        Regex regex = new Regex(@"\d+");

        List<int> numbers = new List<int>();
        foreach (Match match in regex.Matches(json))
        {
            numbers.Add(int.Parse(match.Value));
        }

        List<List<Vector3>> cardList = new List<List<Vector3>>();

        // JSON 文字列を Vector3[] の配列に変換
        for (int i = 0; i < NumberofSet; i++)
        {
            List<Vector3> element = new List<Vector3>();
            for (int j = 0; j < NumberofCards; j++)
            {
                // ここで3つの数値を取り出してVector3に変換
                int index = (i * NumberofCards + j) * 3; // Vector3ごとに3つの数値が必要
                if (index + 2 < numbers.Count) // インデックスが範囲内であることを確認
                {
                    Vector3 vector = new Vector3(numbers[index], numbers[index + 1], numbers[index + 2]);
                    element.Add(vector);
                }
            }
            cardList.Add(element);
        }
        return cardList;
    }

    private string SerializeFieldCard(List<Vector3> cards)
    {
        string cards_json = "";
        for (int set = 0; set < NumberofSet; set++)
        {
            cards_json += SerializeVector3(cards[set]) + ",";
        }
        cards_json = cards_json.Remove(cards_json.Length - 1);
        return cards_json;
    }

    private List<Vector3> DeserializeFieldCard(string serializedCards)
    {
        Regex regex = new Regex(@"\d+");

        List<int> numbers = new List<int>();
        foreach (Match match in regex.Matches(serializedCards))
        {
            numbers.Add(int.Parse(match.Value));
        }

        List<Vector3> vectorList = new List<Vector3>();

        // 3つの連続する数値を取り出してVector3に変換
        for (int i = 0; i < numbers.Count; i += 3)
        {
            if (i + 2 < numbers.Count) // インデックスが範囲内であることを確認
            {
                Vector3 vector = new Vector3(numbers[i], numbers[i + 1], numbers[i + 2]);
                vectorList.Add(vector);
            }
        }

        return vectorList;
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

    
    public enum BlackJackStateList
    {
        BeforeStart,
        WaitForNextTrial,
        ShowMyCards,
        SelectCards,
        SelectBet,
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


    public int NumberofSet = 5;
    Vector3 FieldCards = Vector3.zero;

    List<Vector3> MyCards;
    private void Start()
    {
        _PhotonView = GetComponent<PhotonView>();
        _BlackJackManager = GameObject.FindWithTag("Manager").GetComponent<BlackJackManager>();
    }
    public void UpdateParameter()
    {
        //GenerateFieldSet();
        //MyCardPotential = FindCombinations(1, 4);
        for (int i = 0; i < NumberofSet; i++)
        {
            DecidingCards();
            FieldCardsPracticeList.Add(FieldCards);
            MyCardsPracticeList.Add(MyCards);
        }
        ShareInit();
    }
    public void ShareInit()
    {
        SetMyCardsPracticeList(MyCardsPracticeList);
        SetFieldCardsList(FieldCardsPracticeList);
        InitializeCard();
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

    void DecidingCards()
    {
        DecideRandomCards();
        while (CheckDoubleCard() || CheckContainSuccess())// || CheckContainAnotherPair())
        {
            DecideRandomCards();
        }
    }
    void DecideRandomCardsSum()
    {
        MyCards = new List<Vector3>();
        HashSet<int> cardnum = PickNumbers(4, 2);
        List<int> sortedList = new List<int>(cardnum);
        sortedList.Sort();
        FieldCards = new Vector3(sortedList[0] + 2, sortedList[1] - sortedList[0] + 1, 7 - sortedList[1]);
        for (int i = 0; i < NumberofCards; i++)
        {
            cardnum = PickNumbers(5, 2);
            sortedList = new List<int>(cardnum);
            sortedList.Sort();
            MyCards.Add(new Vector3(sortedList[0] + 1, sortedList[1] - sortedList[0], 5 - sortedList[1]));
        }
        ShuffleCards();
    }
    void DecideRandomCards()
    {
        MyCards = new List<Vector3>();
        FieldCards = new Vector3(Random.Range(2, 10), Random.Range(2, 10), Random.Range(2, 10));
        while (FieldCards.x + FieldCards.y + FieldCards.z < 15 || FieldCards.x + FieldCards.y + FieldCards.z > 21)
        {
            FieldCards = SortVector(new Vector3(Random.Range(2, 10), Random.Range(2, 10), Random.Range(2, 10)));
        }
        /*for (int i = 0; i < NumberofCards; i++)
        {
            Vector3 card = new Vector3(Random.Range(1, 6), Random.Range(1, 6), Random.Range(1, 6));
            while (card.x + card.y + card.z < 8 || card.x + card.y + card.z > 12 || CalculateVariance(card) < 1.3f)
            {
                card = new Vector3(Random.Range(1, 6), Random.Range(1, 6), Random.Range(1, 6));
            }
            MyCards.Add(card);
        }
            ShuffleCards();*/
        MyCards.Add(new Vector3(5, 3, 1));
        MyCards.Add(new Vector3(5, 1, 3));
        MyCards.Add(new Vector3(3, 5, 1));
        MyCards.Add(new Vector3(1, 5, 3));
        MyCards.Add(new Vector3(3, 1, 5));
        MyCards.Add(new Vector3(1, 3, 5));
        //MyCards.Add(new Vector3(4, 3, 2));
        //MyCards.Add(new Vector3(3, 2, 4));
        //MyCards.Add(new Vector3(2, 4, 3));
    }
    // この関数は外部からVector3を受け取り、並べ替えたVector3を返します
    public Vector3 SortVector(Vector3 originalVector)
    {
        float[] components = new float[3] { originalVector.x, originalVector.y, originalVector.z };
        System.Array.Sort(components);
        System.Array.Reverse(components);
        return new Vector3(components[0], components[1], components[2]);
    }
    float CalculateVariance(Vector3 vector)
    {
        // XYZの平均値を計算
        float mean = (vector.x + vector.y + vector.z) / 3;

        // 分散を計算
        float variance = ((vector.x - mean) * (vector.x - mean) +
                          (vector.y - mean) * (vector.y - mean) +
                          (vector.z - mean) * (vector.z - mean)) / 3;

        return variance;
    }
    List<(int, int, int)> FindCombinations(int _minnum, int _maxnum)
    {
        List<(int, int, int)> validCombinations = new List<(int, int, int)>();

        for (int x = _minnum; x <= _maxnum; x++)
        {
            for (int y = _minnum; y <= _maxnum; y++)
            {
                for (int z = _minnum; z <= _maxnum; z++)
                {
                    if (x + y + z == _maxnum)
                    {
                        validCombinations.Add((x, y, z));
                    }
                }
            }
        }

        return validCombinations;
    }

    void GenerateFieldSet()
    {
        List<Vector3> validCombinations = new List<Vector3>();

        // 2以上6以下の整数で合計が10になる組み合わせを探す
        for (int x = 2; x <= 6; x++)
        {
            for (int y = 2; y <= 6; y++)
            {
                for (int z = 2; z <= 6; z++)
                {
                    if (x + y + z == 10)
                    {
                        FieldCardPotential.Add(new Vector3(x, y, z));
                    }
                }
            }
        }

    }
    HashSet<int> PickNumbers(int n, int i)
    {
        HashSet<int> pickedNumbers = new HashSet<int>();
        while (pickedNumbers.Count < i)
        {
            int randomNumber = Random.Range(0, n);
            pickedNumbers.Add(randomNumber);
        }
        return pickedNumbers;
    }
    private bool CheckDoubleCard()
    {
        // HashSetを使用して重複をチェック
        HashSet<Vector3> seen = new HashSet<Vector3>();

        foreach (Vector3 card in MyCards)
        {
            // もし既に同じVector3が存在したら、重複があると判定
            if (seen.Contains(card))
            {
                return true;
            }
            seen.Add(card);
        }

        // 重複が見つからなければfalseを返す
        return false;
    }
    private bool CheckContainSuccess()
    {
        for (int i = 0; i < MyCards.Count - 1; i++)
        {
            for (int j = i + 1; j < MyCards.Count; j++)
            {
                if (CalculateResult(i, j)) return false;
            }
        }
        return true;
    }
    private bool CheckContainAnotherPair()
    {
        for (int i = 0; i < MyCards.Count; i++)
        {
            if (CalculateResult(i, i)) return true;
        }
        return false;
    }
    private bool CalculateResult(int i, int j)
    {
        bool result = true;
        if (MyCards[i].x + MyCards[j].x < FieldCards.x) result = false;
        if (MyCards[i].y + MyCards[j].y < FieldCards.y) result = false;
        if (MyCards[i].z + MyCards[j].z < FieldCards.z) result = false;
        return result;
    }
    void ShuffleCards()
    {
        for (int i = 0; i < MyCards.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, MyCards.Count);
            Vector3 temp = MyCards[i];
            MyCards[i] = MyCards[randomIndex];
            MyCards[randomIndex] = temp;
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

    public void MoveToShowMyCards()
    {
        _BlackJackManager.MoveToShowMyCards();
        _PhotonView.RPC("RPCMoveToShowMyCards", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMoveToShowMyCards()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MoveToShowMyCards();
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
    public void MoveToSelectBet()
    {
        _BlackJackManager.MoveToSelectBet();
        _PhotonView.RPC("RPCMoveToSelectBet", RpcTarget.Others);
    }
    [PunRPC]
    void RPCMoveToSelectBet()
    {
        // ここでカードデータを再構築
        _BlackJackManager.MoveToSelectBet();
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
