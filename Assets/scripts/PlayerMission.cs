using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerMission : MonoBehaviourPunCallbacks
{
    public bool missionComplete = false;
    public TMP_Text missionCompleteUI; // �̼� �Ϸ� UI
    public TMP_Text survivalTextUI; // ���� �ؽ�Ʈ UI
    public GameObject lobbyButtonUI; // �κ� ��ư UI
    public GameObject healthBarUI; // �ｺ �� UI
    private multiEscapeGate escapeGate; // Ż�� ����Ʈ ���� ��ũ��Ʈ
    private static bool escapeGateActivated = false; // Ż�� ����Ʈ Ȱ��ȭ ����

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

        escapeGate = FindObjectOfType<multiEscapeGate>(); // Ż�� ����Ʈ ���� ��ũ��Ʈ ã��
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
                Invoke("CheckAllMissions", 0.5f); // 0.5�� ���� �� Ȯ��
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
            yield return new WaitForSeconds(3); // 3�� ���
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
            escapeGateActivated = true; // Ż�� ����Ʈ Ȱ��ȭ ���� ����
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
