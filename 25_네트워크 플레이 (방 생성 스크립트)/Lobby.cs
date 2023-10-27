using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Lobby : MonoBehaviour
{
    public Text userCount;
    public Text roomsCount;
    public InputField roomName;
    public GameStart gameStart;

    // Update is called once per frame
    void Update()
    {
        userCount.text = NetworkManager.instance.GetUserCount().ToString();
        roomsCount.text = NetworkManager.instance.GetRoomsCount().ToString();
    }

    // 네트워크 플레이 클릭
    public void ClickNetworkPlay()
    {
        gameObject.SetActive(true);
    }

    public void ClickCloseMenu(GameObject game_Object)
    {
        game_Object.SetActive(false);
        roomName.text = "";
        if (game_Object.name == "Lobby")
            game_Object.transform.Find("CreateOption").gameObject.SetActive(false);
    }

    // 방 생성 클릭
    public void ClickCreate()
    {
        transform.GetChild(3).gameObject.SetActive(true);
    }

    // 방 최종 생성 클릭
    public void ClickCreateRoom()
    {
        NetworkManager.instance.CreateRoom(roomName.text);
        gameStart.StartGame(2);
    }
}
