using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 게임 시작 (0 : VS CPU, 1 : Hot Seat)
    public void StartGame(int play_Mode)
    {
        GameManager.instance.gameMode = play_Mode;
        SceneManager.LoadScene("SingleLane");
    }
}
