using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : InteractableObject
{
    [Header("µ¿Àü ¼³Á¤")]
    public int CoinValue = 10;
    public string questTag = "Coin";

    protected override void Start()
    {
        base.Start();
        objectName = "µ¿Àü";
        interacrtionText = "[E] µ¿Àü È¹µæ";
        interactionType = InteractionType.Item;
    }

    protected override void CollectItem()
    {

        if(QuestManager.instance != null)
        {
            QuestManager.instance.AddCollectProgress(questTag);
        }
        AchievementManager.instance?.UpdateProgress(AchievementType.CollectCoins, CoinValue);


        transform.Rotate(Vector3.up * 180f);
        Destroy(gameObject, 0.5f);
    }
}
