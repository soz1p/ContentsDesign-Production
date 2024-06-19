using UnityEngine;
using Photon.Pun;

public class cshDungeonCamera : MonoBehaviourPunCallbacks
{
    public GameObject target; // 타겟을 수동으로 설정할 필요 없음
    public float damping = 1;
    private Vector3 offset;

    void Start()
    {
        // PhotonView를 통해 로컬 플레이어의 카메라만 타겟 설정
        if (photonView.IsMine)
        {
            target = GameObject.FindWithTag("Player"); // 로컬 플레이어를 타겟으로 설정
            offset = transform.position - target.transform.position;
        }
    }

    void LateUpdate()
    {
        if (photonView.IsMine)
        {
            Vector3 desiredPosition = target.transform.position + offset;
            Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
            transform.position = position;

            transform.LookAt(target.transform.position);
        }
    }
}
