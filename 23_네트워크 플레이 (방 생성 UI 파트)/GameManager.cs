using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    // 0 : VS CPU, 1 : Hot Seat
    public int gameMode;

    void Awake()
    {
        instance = this;
        gameMode = 0;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
