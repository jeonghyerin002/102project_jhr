using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : InteractableObject
{
    [Header("���� ����")]
    public int CoinValue = 10;
    public string questTag = "Coin";

    protected override void Start()
    {
        base.Start();
        objectName = "����";
        interacrtionText = "[E] ���� ȹ��";
        interactionType = InteractionType.Item;
    }

    protected override void CollectItem()
    {

        if(QuestManager.instance != null)
        {
            QuestManager.instance.AddCollectProgress(questTag);
        }
        transform.Rotate(Vector3.up * 180f);
        Destroy(gameObject, 0.5f);
    }
}
