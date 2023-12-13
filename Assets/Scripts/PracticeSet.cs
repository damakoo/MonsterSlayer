using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Text.RegularExpressions;
using System.Linq;


public class PracticeSet: MonoBehaviourPunCallbacks
{
    BlackJackManager _BlackJackManager { get; set; }
    private PhotonView _PhotonView;
    public int MySelectedCard { get; set; }
    public int YourSelectedCard { get; set; }
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
                int index = i * NumberofCards + j * 3; // Vector3ごとに3つの数値が必要
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
        for (int i = 0; i < NumberofSet; i++)
        {
            DecidingCards();
            FieldCardsPracticeList.Add(FieldCards);
            MyCardsPracticeList.Add(MyCards);
        }
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
        while (CheckDoubleCard())
        {
            DecideRandomCards();
        }
    }
    void DecideRandomCards()
    {
        MyCards = new List<Vector3>();
        FieldCards = new Vector3(Random.Range(6, 11), Random.Range(6, 11), Random.Range(6, 11));
        for (int i = 0; i < NumberofCards; i++)
        {
            MyCards.Add(new Vector3(Random.Range(1, 6), Random.Range(1, 6), Random.Range(1, 6)));
        }
        ShuffleCards();
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
