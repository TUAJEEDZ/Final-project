using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    public GameObject[] dropItems; // ��������ͧ�����������ö��ͻ��
    public float dropChance = 1.0f; // �͡��㹡�ô�ͻ����
    public int minDropCount = 1; // �ӹǹ����ش����ͻ
    public int maxDropCount = 5; // �ӹǹ�٧�ش����ͻ

    public void DropItem()
    {
        if (dropItems.Length > 0)
        {
            // ��Ǩ�ͺ�����Ҩд�ͻ����������� ���� dropChance
            if (Random.value <= dropChance)
            {
                // �����ӹǹ��������д�ͻ
                int dropCount = GetRandomDropCount();

                // �����������д�ͻ����ӹǹ���������
                for (int i = 0; i < dropCount; i++)
                {
                    int randomIndex = Random.Range(0, dropItems.Length);
                    GameObject dropItem = dropItems[randomIndex];

                    // ���ҧ����㹵��˹觢ͧ�ѵ��
                    Instantiate(dropItem, transform.position, Quaternion.identity);

                    Debug.Log("Dropped item: " + dropItem.name); // �ʴ���ͤ��� Debug ����ʹ�ͻ����
                }
            }
            else
            {
                Debug.Log("Item did not drop due to drop chance."); // �ʴ���ͤ������������ͻ
            }
        }
        else
        {
            Debug.LogWarning("No items to drop."); // �ʴ���ͤ�����͹�����������������������
        }
    }

    private int GetRandomDropCount()
    {
        // �����ӹǹ��������д�ͻ
        return Random.Range(minDropCount, maxDropCount + 1); // +1 �����������ӹǹ maxDropCount ����
    }
}
