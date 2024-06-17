using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangePlayMode : MonoBehaviour
{
    public void LoadIslandPlayScene()
    {
        SceneManager.LoadScene("IslandPlay");
    }

    public void LoadIslandMultiPlayScene()
    {
       // SceneManager.LoadScene("MultiIslandPlay");
    }

    public void LoadMountainPlayScene()
    {
        SceneManager.LoadScene("MountainPlay");
    }

    public void LoadMountainMultiPlayScene()
    {
        // SceneManager.LoadScene("MultiIslandPlay");
    }
}