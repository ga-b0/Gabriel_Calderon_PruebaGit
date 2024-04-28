using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;


public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text RoomListName;
    public RoomInfo roomInfo;
    public void SetUp(RoomInfo _info)
    {
        roomInfo = _info;
        RoomListName.text = _info.Name;
    }

    public void OnClick()
    {
        Launcher.Instance.JoinRoom(roomInfo);
    }

}
