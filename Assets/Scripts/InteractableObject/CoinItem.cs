using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : InteractableObject
{
    [Header("동전 설정")]
    public int CoinValue = 10;
    public string questTag = "Coin";

    protected override void Start()
    {
        base.Start();
        objectName = "동전";
        interacrtionText = "[E] 동전 획득";
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
