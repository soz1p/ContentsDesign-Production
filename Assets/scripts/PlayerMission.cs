using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerMission : MonoBehaviourPunCallbacks
{
    public bool missionComplete = false;
    public TMP_Text missionCompleteUI; // 미션 완료 UI
    public TMP_Text survivalTextUI; // 생존 텍스트 UI
    public GameObject lobbyButtonUI; // 로비 버튼 UI
    public GameObject healthBarUI; // 헬스 바 UI
    private multiEscapeGate escapeGate; // 탈출 게이트 관리 스크립트
    private static bool escapeGateActivated = false; // 탈출 게이트 활성화 여부

    void Start()
    {
        if (missionCompleteUI != null)
        {
            missionCompleteUI.gameObject.SetActive(false);
        }
        if (survivalTextUI != null)
        {
            survivalTextUI.gameObject.SetActive(false);
        }
        if (lobbyButtonUI != null)
        {
            lobbyButtonUI.SetActive(false);
        }
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(true);
        }

        escapeGate = FindObjectOfType<multiEscapeGate>(); // 탈출 게이트 관리 스크립트 찾기
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Compass") && !missionComplete)
        {
            SetMissionComplete();
        }
    }

    public void SetMissionComplete()
    {
        if (!missionComplete)
        {
            missionComplete = true;
            ShowLocalMissionCompleteUI();
            photonView.RPC("ShowMissionCompleteUI", RpcTarget.All, photonView.ViewID);
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
            {
                { "MissionComplete", true }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            Debug.Log("MissionComplete property set for player: " + PhotonNetwork.LocalPlayer.NickName);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Starting mission check as MasterClient.");
                Invoke("CheckAllMissions", 0.5f); // 0.5초 지연 후 확인
            }
        }
    }

    void ShowLocalMissionCompleteUI()
    {
        if (missionCompleteUI != null)
        {
            StartCoroutine(ShowMissionCompleteUICoroutine());
        }
    }

    [PunRPC]
    public void ShowMissionCompleteUI(int viewID)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null && targetView.IsMine)
        {
            StartCoroutine(ShowMissionCompleteUICoroutine());
        }
    }

    private IEnumerator ShowMissionCompleteUICoroutine()
    {
        if (missionCompleteUI != null)
        {
            missionCompleteUI.gameObject.SetActive(true);
            Debug.Log("Mission complete UI activated.");
            yield return new WaitForSeconds(3); // 3초 대기
            missionCompleteUI.gameObject.SetActive(false);
            Debug.Log("Mission complete UI deactivated.");
        }
        else
        {
            Debug.LogError("missionCompleteUI is not assigned.");
        }
    }

    void CheckAllMissions()
    {
        bool allMissionsComplete = true;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("MissionComplete"))
            {
                if (!(bool)player.CustomProperties["MissionComplete"])
                {
                    allMissionsComplete = false;
                    Debug.Log("Player " + player.NickName + " has not completed the mission.");
                    break;
                }
            }
            else
            {
                allMissionsComplete = false;
                Debug.Log("Player " + player.NickName + " does not have the MissionComplete property.");
                break;
            }
        }

        if (allMissionsComplete && !escapeGateActivated)
        {
            Debug.Log("All missions complete! Activating escape gate.");
            photonView.RPC("ActivateEscapeGate", RpcTarget.AllBuffered);
            escapeGateActivated = true; // 탈출 게이트 활성화 여부 설정
        }
        else
        {
            Debug.Log("Not all missions complete or escape gate already activated.");
        }
    }

    [PunRPC]
    void ActivateEscapeGate()
    {
        Debug.Log("ActivateEscapeGate RPC called.");
        if (escapeGate != null)
        {
            escapeGate.EnableEscapeGate();
        }
        else
        {
            Debug.LogError("Escape gate script not found!");
        }
    }
}
