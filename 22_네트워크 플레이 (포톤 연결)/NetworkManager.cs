using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    public Text userCount;

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
        userCount.text = PhotonNetwork.CountOfPlayers.ToString();
    }
}
