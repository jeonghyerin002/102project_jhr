using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : InteractableObject
{
    [Header("���� ����")]
    public int CoinValue = 10;

    protected override void Start()
    {
        base.Start();
        objectName = "����";
        interacrtionText = "[E] ���� ȹ��";
        interactionType = InteractionType.Item;
    }

    protected override void CollectItem()
    {
        transform.Rotate(Vector3.up * 360f);
        Destroy(gameObject, 0.5f);
    }
}
