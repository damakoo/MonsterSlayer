using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonNetWorkStarter: MonoBehaviourPunCallbacks
{
    [SerializeField] DecideHostorClient _DecideHostorClient;
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        GameObject practiceset = PhotonNetwork.Instantiate("PracticeSet", Vector3.zero, Quaternion.identity);
        _DecideHostorClient._practiceSet = practiceset.GetComponent<PracticeSet>();
        _DecideHostorClient.isConnecting = true;
    }
}