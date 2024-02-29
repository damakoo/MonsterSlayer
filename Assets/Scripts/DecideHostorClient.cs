using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DecideHostorClient : MonoBehaviour
{
    public bool HostReady { get; set; } = false;
    public bool ClientReady { get; set; } = false;
    [SerializeField] BlackJackManager _BlackJackManager;
    [SerializeField] GameObject WaitforAnother;
    bool _DecideHostorClient = false;
    public bool isConnecting { get; set; } = false;
    public PracticeSet _practiceSet { get; set; }
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    // Update is called once per frame
    void Update()
    {

        if (isConnecting)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1 && !_DecideHostorClient)
            {
                _BlackJackManager._hostorclient = BlackJackManager.HostorClient.Host;
                _DecideHostorClient = true;
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && !_DecideHostorClient)
            {
                _BlackJackManager._hostorclient = BlackJackManager.HostorClient.Client;
                _DecideHostorClient = true;
            }

            if (PhotonNetwork.CurrentRoom.PlayerCount > 1 && _DecideHostorClient)
            {
                if (_BlackJackManager._hostorclient == BlackJackManager.HostorClient.Client)
                {
                    PhotonView[] photonviews = FindObjectsOfType<PhotonView>();
                    foreach (var _photonview in photonviews)
                    {
                        if (!_photonview.IsMine) _practiceSet = _photonview.gameObject.GetComponent<PracticeSet>();
                    }
                }
                _BlackJackManager.SetPracticeSet(_practiceSet);
                if (_BlackJackManager._hostorclient == BlackJackManager.HostorClient.Host)
                {
                    _BlackJackManager.UpdateParameter();
                    _BlackJackManager.PhotonGameStartUI();
                }
                WaitforAnother.SetActive(false);
                this.gameObject.SetActive(false);
            }
        }
    }
}

