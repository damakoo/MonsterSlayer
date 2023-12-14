using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlackJackManager : MonoBehaviour
{
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
    public PracticeSet _PracticeSet { get; set; }
    private List<bool> ScoreList = new List<bool>();

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
    private bool Score = false;
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
                nowTime += Time.deltaTime;
                BlackJacking();
            }
            else if (_hostorclient == HostorClient.Client)
            {
                nowTime = 0;
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
                            _PracticeSet.SetMySelectedTime(nowTime,nowTrial);
                            thisCard.Clicked();
                        }
                        else if(_hostorclient == HostorClient.Client)
                        {
                            _cardslist.MyCardsOpen();
                            _PracticeSet.SetYourSelectedCard(thisCard.ID);
                            _PracticeSet.SetYourSelectedTime(nowTime, nowTrial);
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

    public void MoveToShowMyCards()
    {
        _cardslist.FieldCardsOpen();
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.ShowMyCards;
    }
    public void PhotonMoveToShowMyCards()
    {
        _PracticeSet.MoveToShowMyCards();
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
        _cardslist.MyCardsList[_PracticeSet.YourSelectedCard].Clicked();
        if(_PracticeSet.MySelectedCard == _PracticeSet.YourSelectedCard) _cardslist.MyCardsList[_PracticeSet.MySelectedCard].Clicked_deep();

        _cardslist.MyResultCard.Number = _cardslist.MyCardsList[_PracticeSet.MySelectedCard].Number + _cardslist.MyCardsList[_PracticeSet.YourSelectedCard].Number;
        _cardslist.MyResultCard.Open();
        Score = CalculateResult();
        _blackJackRecorder.RecordResult(_PracticeSet.MySelectedCard+1, _PracticeSet.YourSelectedCard+1, Score);
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.ShowResult;
        MyScoreUI.text = Score?"Win!":"Lose!";
        ScoreList.Add(Score);
        //YourScoreUI.text = Score.ToString();
        nowTime = 0;
        nowTrial += 1;
        if(nowTrial == _PracticeSet.TrialAll)
        {
            _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.Finished;
            FinishUI.text = "Finished! \n Score:" + ReturnSum(ScoreList).ToString() + "/5";
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
    private bool CalculateResult()
    {
        bool result = true;
        if (_cardslist.MyFieldCard.Number.x > _cardslist.MyResultCard.Number.x) result = false;
        if (_cardslist.MyFieldCard.Number.y > _cardslist.MyResultCard.Number.y) result = false;
        if (_cardslist.MyFieldCard.Number.z > _cardslist.MyResultCard.Number.z) result = false;
        return result;
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
    private int ReturnSum(List<bool> _list)
    {
        int result = 0;
        foreach(var element in _list)
        {
            if(element) result += 1;
        }
        return result;
    }
}
