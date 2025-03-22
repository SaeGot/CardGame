using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Netplay : MonoBehaviourPunCallbacks
{
    public PhotonView pV;
    public SingleLaneGame game;
    private int readyCount;

    public void SendEndTurn(string selectedCard)
    {
        SetReady();
        pV.RPC("RPCSendEndTurn", RpcTarget.Others, selectedCard);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        NetworkPlayStart();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        NetworkPlayStart();
    }

    private void NetworkPlayStart()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            game.NetworkPlayStart();
        }
    }

    private void SetReady()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            readyCount += 1;
            // 둘다 EndTurn 일 경우 배틀 시작
            if (readyCount == 2)
            {
                readyCount = 0;
                pV.RPC("RPCStartBattle", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void RPCSendEndTurn(string selectedCard)
    {
        game.SetOpponentSelectedCard(selectedCard);
        SetReady();
    }

    [PunRPC]
    private void RPCStartBattle()
    {
        game.StartBattle();
    }
}
