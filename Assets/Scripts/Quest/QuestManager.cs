using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    [Header("UI ���")]
    public GameObject questUI;
    public Text questTitleText;
    public Text questDescriptionText;
    public Text questProgressText;
    public Button completeButton;

    [Header("����Ʈ ���")]
    public QuestData[] availableQuests;

    private QuestData currentQuest;
    private int currentQuestIndex = 0;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if(availableQuests.Length > 0)
        {
            StartQuest(availableQuests[0]);
        }
        if(completeButton != null)
        {
            completeButton.onClick.AddListener(CompleteCurrentQuest);
        }
    }


    void Update()
    {
        if(currentQuest != null && currentQuest.isActive)
        {
            CheckDeliveryProgess();
            UpdateQuestUI();
        }
    }

    void UpdateQuestUI()
    {
        if (currentQuest == null) return;

        if(questTitleText != null)
        {
            questTitleText.text = currentQuest.questTitle;
        }
        if (questDescriptionText != null)
        {
            questDescriptionText.text = currentQuest.desctiption;
        }
        if(questProgressText != null)
        {
            questProgressText.text = currentQuest.GetProgressText();
        }
        {
            
        }
    }

    public void StartQuest(QuestData quest)
    {
        if(quest == null) return;

        currentQuest = quest;
        currentQuest.Initalize();
        currentQuest.isActive = true;

        Debug.Log("����Ʈ ���� : " + questTitleText);
        UpdateQuestUI();
        if(questUI != null)
        {
            questUI.SetActive(true);
        }
    }

    //��� ����Ʈ ���� üũ
    void CheckDeliveryProgess()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) return;

        float distance = Vector3.Distance(player.position, currentQuest.deliveryPosition);

        if(distance <= currentQuest.deliveryRedius)
        {
            if(currentQuest.currentProgress == 0)
            {
                currentQuest.currentProgress = 1;
            }
        }
        else
        {
            currentQuest.currentProgress = 0;
        }
    }

    //���� ����Ʈ ����(�ܺο��� ȣ��)
    public void AddCollectProgress(string itemTag)
    {
        if(currentQuest == null || !currentQuest.isActive) return;

        if(currentQuest.questType == QuestType.Collect && currentQuest.targetTag == itemTag)
        {
            currentQuest.currentProgress++;
            Debug.Log("������ ���� :" + itemTag);
        }
    }
    
    public void AddInteractProgress(string objectTag)
    {
        if(currentQuest == null || !currentQuest.isActive) return;

        if(currentQuest.questType == QuestType.Interact && currentQuest.targetTag == objectTag)
        {
            currentQuest.currentProgress++;
            Debug.Log("������ ���� :" + objectTag);
        }
    }

    //���� ����Ʈ �Ϸ�
    public void CompleteCurrentQuest()
    {
        if(currentQuest == null || !currentQuest.isCompleted) return;

        Debug.Log("����Ʈ �Ϸ�!" + currentQuest.rewardMessage);

        //�Ϸ� ��ư ��Ȱ��ȭ
        currentQuestIndex++;
        if(currentQuestIndex < availableQuests.Length)
        {
            StartQuest(availableQuests[currentQuestIndex]);
        }
        else
        {
            //��� ����Ʈ �Ϸ�
            currentQuest = null;
            if(questUI != null)
            {
                questUI.gameObject.SetActive(false);
            }
        }
    }

    //����Ʈ ���� üũ
    void CheckQuestProgress()
    {
        if(currentQuest.questType == QuestType.Delivery)
        {
            CheckDeliveryProgess();
        }
        if(currentQuest.isComplete() && !currentQuest.isCompleted)
        {
            currentQuest.isCompleted = true;

            //�Ϸ� ��ư Ȱ��ȭ
            if(completeButton != null)
            {
                completeButton.gameObject.SetActive(true);
            }
        }
    }
}
