using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Lobby : MonoBehaviour
{
    // 네트워크 플레이 클릭
    public void ClickNetworkPlay()
    {
        gameObject.SetActive(true);
    }

    // 방 생성 클릭
    public void ClickCreate()
    {
        transform.GetChild(3).gameObject.SetActive(true);
    }
}
