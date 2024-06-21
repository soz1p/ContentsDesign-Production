using UnityEngine;
using Photon.Pun;
using TMPro;

public class multiCompass : MonoBehaviourPun
{
    public GameObject missionCompleteUIPrefab; // �̼� �Ϸ� �޽��� ������

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMission playerMission = other.GetComponent<PlayerMission>();
            if (playerMission != null)
            {
                playerMission.SetMissionComplete();
            }
            ShowMissionCompleteMessage(other.transform);
            Destroy(gameObject); // ��ħ�� ��ü ����
        }
    }

    void ShowMissionCompleteMessage(Transform playerTransform)
    {
        if (missionCompleteUIPrefab != null)
        {
            GameObject missionCompleteUI = Instantiate(missionCompleteUIPrefab, playerTransform.position + Vector3.up * 2, Quaternion.identity);
            PhotonView photonView = missionCompleteUI.GetComponent<PhotonView>();
            if (photonView != null)
            {
                photonView.RPC("ShowMessage", RpcTarget.AllBuffered);
            }
            Destroy(missionCompleteUI, 3f); // 3�� �� �޽��� ����
        }
        else
        {
            Debug.LogError("Mission complete UI prefab is not assigned in the Compass script.");
        }
    }
}
