using UnityEngine;
using TMPro;
using Photon.Pun;

public class MissionCompleteUI : MonoBehaviourPun
{
    public TMP_Text missionCompleteText; // 미션 완료 텍스트

    void Start()
    {
        if (missionCompleteText != null)
        {
            missionCompleteText.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    public void ShowMessage()
    {
        if (missionCompleteText != null)
        {
            missionCompleteText.gameObject.SetActive(true);
            Debug.Log("Mission complete message displayed.");
        }
    }
}
