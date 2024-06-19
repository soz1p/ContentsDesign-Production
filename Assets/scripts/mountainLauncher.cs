using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class mountainLauncher : MonoBehaviourPunCallbacks
{
    public PhotonView playerPrefab;

    // 원하는 위치를 Vector3로 설정합니다.
    private Vector3 spawnPosition = new Vector3(50f, 46.7f, 50f);

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room.");
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
        Debug.Log("Player instantiated at: " + spawnPosition);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a random room: " + message);
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Creating a new room.");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created a room successfully.");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to create room: " + message);
    }
}

