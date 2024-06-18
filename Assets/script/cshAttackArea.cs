using System.Collections.Generic;
using UnityEngine;

public class cshAttackArea : MonoBehaviour
{
    public List<Collider> colliders
    {
        get
        {
            if (0 < colliderList.Count)
            {
                //  ���� colliders ����Ʈ�� ��ü�� null�� ���� �����Ͽ� colliderList�� ���� �� ��ȯ
                colliderList.RemoveAll(c => c == null);
            }
            return colliderList;
        }
    }
    private List<Collider> colliderList = new List<Collider>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BreakableObject") || other.CompareTag("Enemy"))
        {
            colliders.Add(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BreakableObject") || other.CompareTag("Enemy"))
        {
            colliders.Remove(other);
        }
    }
}