using UnityEngine;
using Photon.Pun;
using System.Collections;

public class DungeonCamera : MonoBehaviourPunCallbacks
{
    private GameObject target; // 타겟을 수동으로 설정할 필요 없음
    public float damping = 1;
    private Vector3 offset;

    void Start()
    {
        Debug.Log("DungeonCamera Start");
        if (PhotonNetwork.IsConnected)
        {
            // Photon 네트워크에 연결되어 있으면 로컬 플레이어 찾기
            StartCoroutine(FindLocalPlayer());
        }
        else
        {
            // 로컬 모드일 경우
            target = GameObject.FindWithTag("Player");
            if (target != null)
            {
                offset = transform.position - target.transform.position;
                Debug.Log("Local mode: target found");
            }
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.transform.position + offset;
            Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
            transform.position = position;

            transform.LookAt(target.transform.position);
        }
    }

    IEnumerator FindLocalPlayer()
    {
        while (target == null)
        {
            Debug.Log("Finding local player...");
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            Debug.Log("Players found: " + players.Length);
            foreach (GameObject player in players)
            {
                PhotonView photonView = player.GetComponent<PhotonView>();
                if (photonView != null && photonView.IsMine)
                {
                    target = player;
                    offset = transform.position - target.transform.position;
                    Debug.Log("Local player found: " + target.name);
                    break;
                }
            }
            yield return new WaitForSeconds(0.5f); // 잠시 기다린 후 다시 시도
        }
    }
}
