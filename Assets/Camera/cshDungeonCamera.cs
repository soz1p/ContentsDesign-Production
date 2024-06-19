using UnityEngine;
using Photon.Pun;

public class cshDungeonCamera : MonoBehaviourPunCallbacks
{
    public GameObject target; // Ÿ���� �������� ������ �ʿ� ����
    public float damping = 1;
    private Vector3 offset;

    void Start()
    {
        // PhotonView�� ���� ���� �÷��̾��� ī�޶� Ÿ�� ����
        if (photonView.IsMine)
        {
            target = GameObject.FindWithTag("Player"); // ���� �÷��̾ Ÿ������ ����
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
