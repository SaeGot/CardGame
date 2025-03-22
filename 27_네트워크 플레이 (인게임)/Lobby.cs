using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Lobby : MonoBehaviour
{
    public Text userCount;
    public Text roomsCount;
    public InputField roomName;
    public GameStart gameStart;
    public Transform content;
    public GameObject room;
    public GameObject lobby;
    private bool inLobby;
    private float currentTime = 0f;
    private float updateDelay = 2f;

    // Update is called once per frame
    void Update()
    {
        if (inLobby)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= updateDelay)
            {
                UpdateLobby();
                currentTime = 0.0f;
            }
        }
        userCount.text = NetworkManager.instance.GetUserCount().ToString();
        roomsCount.text = NetworkManager.instance.GetRoomsCount().ToString();
    }

    // 네트워크 플레이 클릭
    public void ClickNetworkPlay()
    {
        StartCoroutine(JoinLobby());
    }

    private IEnumerator JoinLobby()
    {
        yield return StartCoroutine(NetworkManager.instance.JoinLobby());
        inLobby = NetworkManager.instance.CheckInLobby();
        Debug.Log(inLobby);
        if (inLobby)
        {
            lobby.SetActive(true);
        }
        else
        {
            GameManager.instance.FailedToConnect();
        }
    }

    public void ClickCloseMenu(GameObject game_Object)
    {
        game_Object.SetActive(false);
        roomName.text = "";
        if (game_Object.name == "Lobby")
        {
            inLobby = false;
            game_Object.transform.Find("CreateOption").gameObject.SetActive(false);
        }
    }

    // 방 생성 클릭
    public void ClickCreate()
    {
        lobby.transform.GetChild(3).gameObject.SetActive(true);
    }

    // 방 최종 생성 클릭
    public void ClickCreateRoom()
    {
        NetworkManager.instance.CreateRoom(roomName.text);
        gameStart.StartGame(2);
    }

    // 랜덤 참가
    public void ClickJoin()
    {
        bool joined = NetworkManager.instance.JoinRoom();
        if (joined)
            gameStart.StartGame(2);
        else
            GameManager.instance.JoinFailed();
    }

    // 방 클릭
    public void ClickRoom()
    {
        GameObject room_object = EventSystem.current.currentSelectedGameObject;
        string room_name = room_object.transform.GetChild(0).GetComponent<Text>().text;
        string[] room_count = room_object.transform.GetChild(1).GetComponent<Text>().text.Split('/');
        string player_count_str = room_count[0].Trim();
        string max_players_str = room_count[1].Trim();
        int player_count = int.Parse(player_count_str);
        int max_players = int.Parse(max_players_str);
        if (player_count >= max_players)
            GameManager.instance.RoomIsFull();
        else
        {
            bool joined = NetworkManager.instance.JoinRoom(room_name);
            if (joined)
                gameStart.StartGame(2);
            else
                GameManager.instance.JoinFailed();
        }

    }

    // 로비 업데이트
    private void UpdateLobby()
    {
        // 방 목록 삭제
        foreach (Transform room_object in content)
        {
            Destroy(room_object.gameObject);
        }
        List<LobbyRoom> room_list = NetworkManager.instance.GetRoomList();
        roomsCount.text = room_list.Count.ToString();
        // 방 목록 생성
        foreach (LobbyRoom lobby_room in room_list)
        {
            GameObject temp = Instantiate(room, content);
            temp.transform.GetChild(0).GetComponent<Text>().text = lobby_room.roomName;
            string count = string.Format("{0} / {1}", lobby_room.playerCount, lobby_room.maxPlayers);
            temp.transform.GetChild(1).GetComponent<Text>().text = count;
            temp.GetComponent<Button>().onClick.AddListener(delegate { ClickRoom(); });
        }
    }
}
