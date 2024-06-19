using UnityEngine;
using Photon.Pun;
using System.Collections;

public class DungeonCamera : MonoBehaviourPunCallbacks
{
    private GameObject target;
    public float damping = 1;
    private Vector3 offset;

    void Start()
    {
        StartCoroutine(FindLocalPlayer());
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
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                PhotonView photonView = player.GetComponent<PhotonView>();
                if (photonView != null && photonView.IsMine)
                {
                    target = player;
                    offset = transform.position - target.transform.position;
                    break;
                }
            }
            yield return new WaitForSeconds(0.5f); // 잠시 기다린 후 다시 시도
        }
    }
}
