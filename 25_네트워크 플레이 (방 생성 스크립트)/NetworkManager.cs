using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 유저수 가져오기
    public int GetUserCount()
    {
        return PhotonNetwork.CountOfPlayers;
    }

    // 방수 가져오기
    public int GetRoomsCount()
    {
        return PhotonNetwork.CountOfRooms;
    }

    // 방 생성
    public void CreateRoom(string room_Name)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(room_Name, options);
    }
}
