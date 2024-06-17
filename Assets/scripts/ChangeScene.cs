using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void LoadIslandScene()
    {
        SceneManager.LoadScene("IslandLobby");
    }

    public void LoadMountainScene()
    {
        SceneManager.LoadScene("MountainLobby");
    }
}

