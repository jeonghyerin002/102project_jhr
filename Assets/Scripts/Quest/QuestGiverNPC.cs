using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiverNPC : InteractableObject
{
    [Header("NPC Quest Setting")]
    public QuestData questToGive;
    public string npcName = "NPC";
    public string questStartMeesage = "새로운 퀘스트가 있습니다.";
    public string noQuestMessage = "퀘스트가 없습니다.";
    public string QuestAlreadyQctiveMessage = "이미 진행중인 퀘스트가 있습니다.";

    public QuestManager questManager;

    protected override void Start()
    {
        base.Start();
        questManager = FindObjectOfType<QuestManager>();

        if (questManager == null)
        {
            Debug.Log("QuestManager가 없습니다.");
        }
        interacrtionText = "E" + npcName + "와 대화하기";
    }

    public override void Interact()
    {
        base.Interact();

        questManager.StartQuest(questToGive);
    }

    private void Update()
    {
        if (questToGive != null && questManager != null && questManager.currentQuest == null)
        {
            interacrtionText = "[E]" + npcName + "와 대화하기";
        }
        else if (questManager != null && questManager.currentQuest != null)
        {
            interacrtionText = "[E]" + npcName;
        }
    }
}
