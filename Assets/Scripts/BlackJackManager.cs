using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlackJackManager : MonoBehaviour
{
    [SerializeField] CardsList _cardslist;
    [SerializeField] int TaskLimitTime = 7;
    [SerializeField] int ShowMyCardsTime = 5;
    [SerializeField] int BetTime = 4;
    [SerializeField] int ResultsTime = 5;
    [SerializeField] int WaitingTime = 3;
    [SerializeField] int NumberofSet = 10;
    [SerializeField] TextMeshProUGUI FinishUI;
    [SerializeField] BlackJackRecorder _blackJackRecorder;
    [SerializeField] TextMeshProUGUI MyScoreUI;
    [SerializeField] DecideHostorClient _decideHostorClient;
    [SerializeField] GameObject StartingUi;
    [SerializeField] GameObject WaitforStartUi;
    [SerializeField] GameObject ClientUi;
    [SerializeField] GameObject BetUi;
    [SerializeField] GameObject CardListObject;
    [SerializeField] GameObject MonsterIconObject;
    [SerializeField] GameObject _SceneReloaderHost;
    [SerializeField] GameObject _SceneReloaderClient;
    [SerializeField] List<TextMeshProUGUI> BetUiChild;
    [SerializeField] GameObject TimeLimitObj;
    [SerializeField] GameObject TimeLimit_Bet;
    [SerializeField] GameObject TimeLimit_notBet;
    [SerializeField] GameObject AllTrialFinishedUI;
    [SerializeField] TextMeshProUGUI TimeLimitObj_str;
    public PracticeSet _PracticeSet { get; set; }
    private List<bool> ScoreList = new List<bool>();
    private List<int> MyScorePointList = new List<int>();
    private List<int> YourScorePointList = new List<int>();
    private int NotSelectedNumber = 100;

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
    private int MyScorePoint = 0;
    private int YourScorePoint = 0;
    public bool hasPracticeSet { get; set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        FinishUI.text = "";
        TimeLimitObj_str.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (hasPracticeSet)
        {
            if (_hostorclient == HostorClient.Host)
            {
                if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.BeforeStart)
                {
                    StartingGame();
                    if (_PracticeSet.HostPressed)
                    {
                        PhotonMoveToWaitForNextTrial(nowTrial);
                        _PracticeSet.SetHostPressed(false);
                        _PracticeSet.SetClientPressed(false);
                    }
                    //if (Input.GetKeyDown(KeyCode.Space)) PhotonMoveToWaitForNextTrial(nowTrial);
                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.WaitForNextTrial)
                {
                    //if (Input.GetKeyDown(KeyCode.Space)) MoveToShowMyCards();
                    nowTime += Time.deltaTime;
                    _PracticeSet.SetTimeLeft(WaitingTime - nowTime);
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
                        _PracticeSet.SetTimeLeft(ShowMyCardsTime - nowTime);
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
                    _PracticeSet.SetTimeLeft(TaskLimitTime - nowTime);
                    BlackJacking();
                    if (nowTime > TaskLimitTime) PhotonMoveToSelectBet();
                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectBet)
                {
                    nowTime += Time.deltaTime;
                    _PracticeSet.SetTimeLeft(BetTime - nowTime);
                    SelectBetting();
                    if (nowTime > BetTime) PhotonMoveToShowResult();
                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.ShowResult)
                {
                    //if (Input.GetKeyDown(KeyCode.Space)) MoveToWaitForNextTrial();
                    nowTime += Time.deltaTime;
                    _PracticeSet.SetTimeLeft(ResultsTime - nowTime);
                    if (nowTime > ResultsTime)
                    {
                        nowTime = 0;
                        PhotonMoveToWaitForNextTrial(nowTrial);
                    }
                }
                else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.Finished)
                {
                    if (_PracticeSet.HostPressed)
                    {
                        PhotonRestart();
                    }
                }

            }
            else if (_hostorclient == HostorClient.Client && _PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.BeforeStart)
            {
                StartingGame();
            }
            else if (_hostorclient == HostorClient.Client && _PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectCards)
            {
                nowTime += Time.deltaTime;
                BlackJacking();
            }
            else if (_hostorclient == HostorClient.Client && _PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectBet)
            {
                SelectBetting();
            }
            else if (_hostorclient == HostorClient.Client)
            {
                nowTime = 0;
            }
            if (_PracticeSet.BlackJackState != PracticeSet.BlackJackStateList.BeforeStart) TimeLimitObj_str.text = "Time: " + Mathf.CeilToInt(_PracticeSet.TimeLeft).ToString();
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
    public void ReUpdateParameter()
    {
        _PracticeSet.ReUpdateParameter();
    }
    public void InitializeCard()
    {
        _cardslist.InitializeCards();
    }
    public void ReInitializeCard()
    {
        _cardslist.ReInitializeCards();
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
                        if (_hostorclient == HostorClient.Host)
                        {
                            _cardslist.MyCardsOpen();
                            _PracticeSet.SetMySelectedCard(thisCard.ID);
                            _PracticeSet.SetMySelectedTime(nowTime, nowTrial);
                            thisCard.HostClicked();
                        }
                        else if (_hostorclient == HostorClient.Client)
                        {
                            _cardslist.MyCardsOpen();
                            _PracticeSet.SetYourSelectedCard(thisCard.ID);
                            _PracticeSet.SetYourSelectedTime(nowTime, nowTrial);
                            thisCard.ClientClicked();
                        }
                    }
                }
            }
        }
    }
    void SelectBetting()
    {
        // �}�E�X�{�^�����N���b�N���ꂽ���m�F
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            // ���C�L���X�g���g�p���ăI�u�W�F�N�g�����o
            if (hit && hit.collider.gameObject.CompareTag("Bet"))
            {
                TextMeshProUGUI textMesh;
                if (hit.collider.gameObject.TryGetComponent<TextMeshProUGUI>(out textMesh))
                {
                    string text = textMesh.text;

                    // 文字列から数字を抽出してint型に変換
                    int number;
                    if (int.TryParse(text, out number))
                    {
                        foreach (TextMeshProUGUI child in BetUiChild) child.color = Color.white;
                        textMesh.color = Color.yellow;
                        if (_hostorclient == HostorClient.Host)
                        {
                            _PracticeSet.SetMySelectedBet(number);
                        }
                        else if (_hostorclient == HostorClient.Client)
                        {
                            _PracticeSet.SetYourSelectedBet(number);
                        }
                    }
                    else
                    {
                        Debug.LogError("文字列に数字が含まれていません。");
                    }
                }
                else
                {
                    Debug.LogError("TextMeshProUGUIコンポーネントが見つかりませんでした。");
                }
            }
        }
    }
    public void GameStartUI()
    {
        StartingUi.SetActive(true);
    }
    public void PhotonGameStartUI()
    {
        _PracticeSet.GameStartUi();
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
                if (_hostorclient == HostorClient.Host)
                {
                    _PracticeSet.SetHostPressed(true);
                }
                else if (_hostorclient == HostorClient.Client)
                {
                    _PracticeSet.SetClientPressed(true);
                }
                WaitforStartUi.SetActive(true);
                StartingUi.SetActive(false);
            }
        }
    }

    public void MoveToShowMyCards()
    {
        _cardslist.AllOpen();
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.ShowMyCards;
        TimeLimitObj.transform.position = TimeLimit_notBet.transform.position;
    }
    public void PhotonMoveToShowMyCards()
    {
        _PracticeSet.MoveToShowMyCards();
    }
    public void PhotonMoveToSelectBet()
    {
        _PracticeSet.MoveToSelectBet();
    }
    public void MoveToSelectBet()
    {
        _PracticeSet.MySelectedBet = 0;
        _PracticeSet.YourSelectedBet = 0;
        CardListObject.SetActive(false);
        MonsterIconObject.SetActive(false);
        BetUi.SetActive(true);
        foreach (TextMeshProUGUI child in BetUiChild) child.color = Color.white;
        nowTime = 0;
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.SelectBet;
        TimeLimitObj.transform.position = TimeLimit_Bet.transform.position;
    }
    public void MoveToSelectCards()
    {
        _cardslist.FieldCardsOpen();
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.SelectCards;
        TimeLimitObj.transform.position = TimeLimit_notBet.transform.position;
    }
    public void PhotonMoveToSelectCards()
    {
        _PracticeSet.MoveToSelectCards();
    }
    public void MoveToShowResult()
    {
        _PracticeSet.YourSelectedCard = Random.Range(0, 6);
        CardListObject.SetActive(true);
        MonsterIconObject.SetActive(true);
        BetUi.SetActive(false);
        if (_PracticeSet.MySelectedCard != NotSelectedNumber) _cardslist.MyCardsList[_PracticeSet.MySelectedCard].HostClicked();
        if (_PracticeSet.YourSelectedCard != NotSelectedNumber) _cardslist.MyCardsList[_PracticeSet.YourSelectedCard].ClientClicked();
        if (_PracticeSet.MySelectedCard == _PracticeSet.YourSelectedCard && _PracticeSet.MySelectedCard != NotSelectedNumber) _cardslist.MyCardsList[_PracticeSet.MySelectedCard].Clicked_deep();

        _cardslist.MyResultCard.Number = ((_PracticeSet.MySelectedCard == NotSelectedNumber) ? Vector3.zero : _cardslist.MyCardsList[_PracticeSet.MySelectedCard].Number) + ((_PracticeSet.YourSelectedCard == NotSelectedNumber) ? Vector3.zero : _cardslist.MyCardsList[_PracticeSet.YourSelectedCard].Number);
        _cardslist.MyResultCard.Open();
        Score = CalculateResult();
        MyScorePoint = _PracticeSet.MySelectedBet * (Score ? 1 : -1);
        YourScorePoint = _PracticeSet.YourSelectedBet * (Score ? 1 : -1);
        _blackJackRecorder.RecordResult(_PracticeSet.MySelectedCard + 1, _PracticeSet.YourSelectedCard + 1, (_PracticeSet.MySelectedCard == NotSelectedNumber) ? Vector3.zero : _cardslist.MyCardsList[_PracticeSet.MySelectedCard].Number, (_PracticeSet.YourSelectedCard == NotSelectedNumber) ? Vector3.zero : _cardslist.MyCardsList[_PracticeSet.YourSelectedCard].Number, _PracticeSet.MySelectedBet, _PracticeSet.YourSelectedBet, Score, MyScorePoint, YourScorePoint);
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.ShowResult;
        //MyScoreUI.text = (Score ? "Win!" : "Lose!") + "\nHost:" + MyScorePoint.ToString() + "\nClient:" + YourScorePoint.ToString();
        //MyScoreUI.text = (Score ? "Win!" : "Lose!") + ((_hostorclient == HostorClient.Host) ? ("\nHost:" + MyScorePoint.ToString()) : ("\nClient:" + YourScorePoint.ToString()));
        MyScoreUI.text = (Score ? "Win!" : "Lose!");// + ((_hostorclient == HostorClient.Host) ? ("\nHost:" + MyScorePoint.ToString()) : ("\nClient:" + YourScorePoint.ToString()));
        ScoreList.Add(Score);
        MyScorePointList.Add(MyScorePoint);
        YourScorePointList.Add(YourScorePoint);
        //YourScoreUI.text = Score.ToString();
        nowTime = 0;
        nowTrial += 1;
        TimeLimitObj.transform.position = TimeLimit_notBet.transform.position;
        if (nowTrial == _PracticeSet.TrialAll)
        {
            _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.Finished;
            FinishUI.text = "Finished! \n Win:" + ReturnSum(ScoreList).ToString() + "/5" + "\n" + "Trial: " + _blackJackRecorder.Trial.ToString() + "/" + NumberofSet.ToString();// + "Point:" + (ReturnSumPoint(MyScorePointList) + ReturnSumPoint(YourScorePointList)).ToString();
            //_blackJackRecorder.WriteResult();
            _blackJackRecorder.ExportCsv();
            if (_blackJackRecorder.Trial == NumberofSet)
            {
                AllTrialFinishedUI.SetActive(true);
            }
            else
            {
                _SceneReloaderHost.SetActive(true);
            }
            TimeLimitObj_str.text = "";
        }
    }
    public void PhotonMoveToShowResult()
    {
        _PracticeSet.MoveToShowResult();
    }
    public void MoveToWaitForNextTrial(int _nowTrial)
    {
        WaitforStartUi.SetActive(false);
        _cardslist.AllClose();
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.WaitForNextTrial;
        nowTrial = _nowTrial;
        _cardslist.SetCards(_nowTrial);
        _cardslist.MyCardsOpen();
        MyScoreUI.text = "";
        //YourScoreUI.text = "";
        _PracticeSet.MySelectedCard = NotSelectedNumber;
        _PracticeSet.YourSelectedCard = NotSelectedNumber;
        SetClientUI(false);
        TimeLimitObj.transform.position = TimeLimit_notBet.transform.position;
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
        foreach (var element in _list)
        {
            if (element) result += 1;
        }
        return result;
    }
    private int ReturnSumPoint(List<int> _list)
    {
        int result = 0;
        foreach (var element in _list)
        {
            result += element;
        }
        return result;
    }
    public void SetClientUI(bool setactive)
    {
        ClientUi.SetActive(setactive);
    }

    public void PhotonRestart()
    {
        _PracticeSet.SetHostPressed(false);
        _PracticeSet.SetClientPressed(false);
        ReUpdateParameter();
        _PracticeSet.Restart();
    }
    public void Restart()
    {
        _SceneReloaderClient.SetActive(false);
        TimeLimitObj_str.text = "";
        _blackJackRecorder.Trial += 1;
        FinishUI.text = "";
        _cardslist.AllClose();
        ScoreList = new List<bool>();
        MyScorePointList = new List<int>();
        YourScorePointList = new List<int>();
        nowTrial = 0;
        nowTime = 0;
        _blackJackRecorder.Initialize();
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.BeforeStart;
        MyScoreUI.text = "";
        GameStartUI();
    }
    public void PressedReload()
    {
        _SceneReloaderHost.SetActive(false);
        if (_hostorclient == HostorClient.Host)
        {
            _PracticeSet.SetHostPressed(true);
            _SceneReloaderClient.SetActive(true);
        }
        else if (_hostorclient == HostorClient.Client)
        {
            _PracticeSet.SetClientPressed(true);
            _SceneReloaderClient.SetActive(true);
        }
    }
}
