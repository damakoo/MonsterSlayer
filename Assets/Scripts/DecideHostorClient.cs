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
    bool tryConnetcion = false;
    public bool isConnecting = false;
    public PracticeSet _practiceSet { get; set; }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            if (hit)
            {

                if (hit.collider.gameObject.name == "Host")
                {
                    _BlackJackManager._hostorclient = BlackJackManager.HostorClient.Host;
                    WaitforAnother.SetActive(true);
                    tryConnetcion = true;
                }
                else if (hit.collider.gameObject.name == "Client")
                {
                    _BlackJackManager._hostorclient = BlackJackManager.HostorClient.Client;
                    WaitforAnother.SetActive(true);
                    tryConnetcion = true;
                }
            }

        }
        if (tryConnetcion)
        {
            if (isConnecting)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
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
                        _BlackJackManager.PhotonMakeReadyHost();
                    }
                    else if (_BlackJackManager._hostorclient == BlackJackManager.HostorClient.Client)
                    {
                        _BlackJackManager.PhotonMakeReadyClient();
                    }
                    if (HostReady && ClientReady)
                    {
                        if (_BlackJackManager._hostorclient == BlackJackManager.HostorClient.Host)
                        {
                            _BlackJackManager.UpdateParameter();
                            _BlackJackManager.GameStartUI();
                        }
                        this.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
