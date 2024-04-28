using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text ErrorMessage;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomItemPrefab;
    [SerializeField] Transform PlayerListContent;
    [SerializeField] GameObject PlayerItemPrefab;

    public static Launcher Instance;

    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        Debug.Log("Conectando");
        MenuManager.Instance.OpenMenuName("Loading");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Conectando al lobby"); 
        MenuManager.Instance.OpenMenuName("Home"); 
        PhotonNetwork.NickName = "player" + Random.Range(0, 1000).ToString("0000");
    }
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenuName("Room"); 
        roomName.text = PhotonNetwork.CurrentRoom.Name; 
        foreach (Transform playerT in PlayerListContent)
        { Destroy(playerT.gameObject); }

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerItemPrefab,
           PlayerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenuName("Loading");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ErrorMessage.text = "Error al crear la sala" + message;
        MenuManager.Instance.OpenMenuName("Error");
    }

    public void JoinRoom(RoomInfo _info)
    {
        PhotonNetwork.JoinRoom(_info.Name);
        MenuManager.Instance.OpenMenuName("Loading");

    }


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenuName("Loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenuName("Home");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform transfo in roomListContent)
        {
            Destroy(transfo.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            { continue; }
            Instantiate(roomItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerItemPrefab,
        PlayerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }



}
