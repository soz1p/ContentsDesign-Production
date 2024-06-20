using UnityEngine;
using TMPro;
using DoorScript; // DoorScript 네임스페이스를 사용

public class CameraOpenDoor : MonoBehaviour
{
    public float interactionDistance = 3f;
    public TMP_Text interactionText; // TMP_Text 컴포넌트로 변경

    private Door doorScript;

    void Start()
    {
        interactionText.gameObject.SetActive(false); // 텍스트 오브젝트를 비활성화합니다.
        doorScript = GetComponent<Door>();
        if (doorScript == null)
        {
            Debug.LogError("No Door script found on the escape gate object.");
        }
    }

    void Update()
    {
        if (doorScript == null) return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDistance))
        {
            if (hit.transform == transform)
            {
                interactionText.gameObject.SetActive(true); // 텍스트 오브젝트를 활성화합니다.
                if (Input.GetKeyDown(KeyCode.E))
                {
                    doorScript.OpenDoor();
                }
            }
            else
            {
                interactionText.gameObject.SetActive(false); // 텍스트 오브젝트를 비활성화합니다.
            }
        }
        else
        {
            interactionText.gameObject.SetActive(false); // 텍스트 오브젝트를 비활성화합니다.
        }
    }
}