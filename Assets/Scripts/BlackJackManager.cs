using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static BlackJackManager;

public class BlackJackManager : MonoBehaviour
{
    [SerializeField] bool useSuit = false;
    [SerializeField] CardsList _cardslist;
    [SerializeField] int TimeLimit;
    [SerializeField] int ShowMyCardsTime = 5;
    [SerializeField] int ResultsTime = 5;
    [SerializeField] int WaitingTime = 3;
    [SerializeField] TextMeshProUGUI FinishUI;
    [SerializeField] BlackJackRecorder _blackJackRecorder;
    [SerializeField] TextMeshProUGUI MyScoreUI;
    [SerializeField] DecideHostorClient _decideHostorClient;
    [SerializeField] GameObject StartingUi;
    //[SerializeField] TextMeshProUGUI YourScoreUI;
    public PracticeSet _PracticeSet { get; set; }
    private List<int> MaxScoreList = new List<int>();
    private List<int> ScoreList = new List<int>();

    public enum HostorClient
    {
        Host = 0,
        Client = 1
    }
    public HostorClient _hostorclient { get; set; }
    private enum HowShowCard
    {
        KeyBoard,
        Time
    }
    [SerializeField] HowShowCard _HowShowCard;
    int nowTrial = 0;
    float nowTime = 0;
    private int Score = 0;
    public bool hasPracticeSet { get; set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        FinishUI.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (hasPracticeSet)
        {
            if(_hostorclient == HostorClient.Host)
            {
                if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.BeforeStart)
                {
                    StartingGame();
                    //if (Input.GetKeyDown(KeyCode.Space)) PhotonMoveToWaitForNextTrial(nowTrial);
                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.WaitForNextTrial)
                {
                    //if (Input.GetKeyDown(KeyCode.Space)) MoveToShowMyCards();
                    nowTime += Time.deltaTime;
                    if (nowTime > WaitingTime)
                    {
                        nowTime = 0;
                        PhotonMoveToShowMyCards();
                    }
                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.ShowMyCards)
                {
                    if (_HowShowCard == HowShowCard.KeyBoard)
                    {
                        if (Input.GetKeyDown(KeyCode.Space)) PhotonMoveToSelectCards();
                    }
                    else if (_HowShowCard == HowShowCard.Time)
                    {
                        nowTime += Time.deltaTime;
                        if (nowTime > ShowMyCardsTime)
                        {
                            nowTime = 0;
                            PhotonMoveToSelectCards();
                        }
                    }

                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectCards)
                {
                    nowTime += Time.deltaTime;
                    BlackJacking();
                    if (nowTime > TimeLimit) PhotonMoveToShowResult();
                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.ShowResult)
                {
                    //if (Input.GetKeyDown(KeyCode.Space)) MoveToWaitForNextTrial();
                    nowTime += Time.deltaTime;
                    if (nowTime > ResultsTime)
                    {
                        nowTime = 0;
                        PhotonMoveToWaitForNextTrial(nowTrial);
                    }
                }
            }
            else if (_hostorclient == HostorClient.Client && _PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectCards)
            {
                BlackJacking();
            }
        }
    }
    public void SetPracticeSet(PracticeSet _practiceset)
    {
        _PracticeSet = _practiceset;
        _cardslist.SetPracticeSet(_practiceset);
        hasPracticeSet = true;
    }


    public void UpdateParameter()
    {
        _PracticeSet.UpdateParameter();
    }
    public void InitializeCard()
    {
        _cardslist.InitializeCards();
    }
    void BlackJacking()
    {
        // �}�E�X�{�^�����N���b�N���ꂽ���m�F
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            // ���C�L���X�g���g�p���ăI�u�W�F�N�g�����o
            if (hit && hit.collider.gameObject.CompareTag("Card"))
            {
                if (hit.collider.gameObject.TryGetComponent<CardState>(out CardState thisCard))
                {
                    if (thisCard.MyCard)
                    {
                        if(_hostorclient == HostorClient.Host)
                        {
                            _cardslist.MyCardsOpen();
                            _PracticeSet.SetMySelectedCard(thisCard.ID);
                            thisCard.Clicked();
                        }
                        else if(_hostorclient == HostorClient.Client)
                        {
                            _cardslist.YourCardsOpen();
                            _PracticeSet.SetYourSelectedCard(thisCard.ID);
                            thisCard.Clicked();
                        }
                    }
                }
            }
        }
    }
    public void GameStartUI()
    {
        StartingUi.SetActive(true);
    }
    void StartingGame()
    {
        // �}�E�X�{�^�����N���b�N���ꂽ���m�F
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            // ���C�L���X�g���g�p���ăI�u�W�F�N�g�����o
            if (hit && hit.collider.gameObject.CompareTag("Start"))
            {
                StartingUi.SetActive(false);
                PhotonMoveToWaitForNextTrial(nowTrial);
            }
        }
    }

    public void MoveToShowMyCards(int hostorClient)
    {
        if(hostorClient == (int)HostorClient.Host)
        {
            _cardslist.MyCardsOpen();
        }
        else if (hostorClient == (int)HostorClient.Client)
        {
            _cardslist.YourCardsOpen();
        }
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.ShowMyCards;
    }
    public void PhotonMoveToShowMyCards()
    {
        _PracticeSet.MoveToShowMyCards((int)_hostorclient);
    }
    public void MoveToSelectCards()
    {
        _cardslist.AllOpen();
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.SelectCards;
    }
    public void PhotonMoveToSelectCards()
    {
        _PracticeSet.MoveToSelectCards();
    }
    public void MoveToShowResult()
    {
        _cardslist.MyCardsList[_PracticeSet.MySelectedCard].Clicked();
        _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].Clicked();
        
        /*foreach (var card in _cardslist.MyCardsList_opponent)
        {
            if (card.Number == _PracticeSet.YourSelectedCard.Number) card.Clicked();
        }
        foreach (var card in _cardslist.YourCardsList_opponent)
        {
            if (card.Number == _PracticeSet.MySelectedCard.Number) card.Clicked();
        }*/
        Score = CalculateResult();
        _blackJackRecorder.RecordResult(_cardslist.MyCardsList[_PracticeSet.MySelectedCard].Number, _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].Number, (useSuit) ? CalculateSuitScore(): Score);
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.ShowResult;
        MyScoreUI.text = (useSuit)?CalculateScorewithSuit():Score.ToString();
        ScoreList.Add(Score);
        if (useSuit)
        {
            RecordMaxSuitScore();
        }
        else
        {
            RecordMaxScore();
        }
        //YourScoreUI.text = Score.ToString();
        nowTime = 0;
        nowTrial += 1;
        if(nowTrial == _PracticeSet.TrialAll)
        {
            _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.Finished;
            FinishUI.text = "Finished! \n Score:" + ReturnSum(ScoreList).ToString() + "/" + ReturnSum(MaxScoreList).ToString();
            //_blackJackRecorder.WriteResult();
            _blackJackRecorder.ExportCsv();
        }
    }
    public void PhotonMoveToShowResult()
    {
        _PracticeSet.MoveToShowResult();
    }
    public void MoveToWaitForNextTrial(int _nowTrial)
    {
        _cardslist.AllClose();
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.WaitForNextTrial;
        nowTrial = _nowTrial;
        _cardslist.SetCards(_nowTrial);
        MyScoreUI.text = "";
        //YourScoreUI.text = "";
        _PracticeSet.MySelectedCard = 0;
        _PracticeSet.YourSelectedCard = 0;
    }
    public void PhotonMoveToWaitForNextTrial(int _nowTrial)
    {
        _PracticeSet.MoveToWaitForNextTrial(_nowTrial);
    }
    private int CalculateResult()
    {
        return (_cardslist.MyCardsList[_PracticeSet.MySelectedCard].Number + _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].Number + _PracticeSet.FieldCardsPracticeList[nowTrial] > 21)?0: _cardslist.MyCardsList[_PracticeSet.MySelectedCard].Number + _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].Number + _PracticeSet.FieldCardsPracticeList[nowTrial];
    }
    public void MakeReadyHost()
    {
        _decideHostorClient.HostReady = true;
    }
    public void MakeReadyClient()
    {
        _decideHostorClient.ClientReady = true;
    }
    public void PhotonMakeReadyHost()
    {
        _PracticeSet.MakeReadyHost();
    }
    public void PhotonMakeReadyClient()
    {
        _PracticeSet.MakeReadyClient();
    }
    private string CalculateScorewithSuit()
    {
        string result = Score.ToString();
        if (_cardslist.MyCardsList[_PracticeSet.MySelectedCard].suit.GetColor() == _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].suit.GetColor())
        {
            if(_cardslist.MyCardsList[_PracticeSet.MySelectedCard].suit == _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].suit)
            {
                result += " x 1.2 = " + Mathf.Ceil(Score * 1.2f).ToString();
            }
            else
            {
                result += " x 1.1 = " + Mathf.Ceil(Score * 1.1f).ToString();
            }
        }
        else
        {
            result += " x 1.0 = " + Score.ToString();
        }
        return result;
    }
    private int CalculateSuitScore()
    {
        if (_cardslist.MyCardsList[_PracticeSet.MySelectedCard].suit.GetColor() == _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].suit.GetColor())
        {
            if (_cardslist.MyCardsList[_PracticeSet.MySelectedCard].suit == _cardslist.YourCardsList[_PracticeSet.YourSelectedCard].suit)
            {
                return (int)Mathf.Ceil(Score * 1.2f);
            }
            else
            {
                return (int)Mathf.Ceil(Score * 1.1f);
            }
        }
        else
        {
            return Score;
        }
    }
    private void RecordMaxScore()
    {
        int MaxScore = 0;
        for(int i = 0; i< _cardslist.MyCardsList.Count; i++)
        {
            for(int j = 0; j < _cardslist.YourCardsList.Count; j++)
            {
                int _score = (_cardslist.MyCardsList[i].Number + _cardslist.YourCardsList[j].Number + _PracticeSet.FieldCardsPracticeList[nowTrial] > 21) ? 0 : _cardslist.MyCardsList[i].Number + _cardslist.YourCardsList[j].Number + _PracticeSet.FieldCardsPracticeList[nowTrial];
                if (MaxScore < _score) MaxScore = _score;
            }
        }
        MaxScoreList.Add(MaxScore);
    }
    private void RecordMaxSuitScore()
    {
        int MaxScore = 0;
        for (int i = 0; i < _cardslist.MyCardsList.Count; i++)
        {
            for (int j = 0; j < _cardslist.YourCardsList.Count; j++)
            {
                int _score = (_cardslist.MyCardsList[i].Number + _cardslist.YourCardsList[j].Number + _PracticeSet.FieldCardsPracticeList[nowTrial] > 21) ? 0 : _cardslist.MyCardsList[i].Number + _cardslist.YourCardsList[j].Number + _PracticeSet.FieldCardsPracticeList[nowTrial];
                if (_cardslist.MyCardsList[i].suit.GetColor() == _cardslist.YourCardsList[j].suit.GetColor())
                {
                    if (_cardslist.MyCardsList[i].suit == _cardslist.YourCardsList[j].suit)
                    {
                        _score = (int)Mathf.Ceil(_score * 1.2f);
                    }
                    else
                    {
                        _score = (int)Mathf.Ceil(Score * 1.1f);
                    }
                }
                if (MaxScore < _score) MaxScore = _score;
            }
        }
        MaxScoreList.Add(MaxScore);
    }
    private int ReturnSum(List<int> _list)
    {
        int result = 0;
        foreach(var element in _list)
        {
            result += element;
        }
        return result;
    }
}
