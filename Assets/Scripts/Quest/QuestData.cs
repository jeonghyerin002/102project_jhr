using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Queat", menuName = "Quest System/Quest")]
public class QuestData : ScriptableObject
{
    [Header("기본정보")]
    public string questTitle = "새로운 퀘스트";

    [TextArea(2, 4)]
    public string desctiption = "퀘스트 설명을 입력하세요";
    public Sprite questIcon;

    [Header("퀘스트 설정")]
    public QuestType questType;
    public int targetAmount = 1;

    [Header("배달 퀘스트용 (Delivery")]
    public Vector3 deliveryPosition;
    public float deliveryRedius = 3f;

    [Header("수집/상호작용 퀘스트용")]
    public string targetTag = "";

    [Header("보상")]
    public int experienceReward = 100;
    public string rewardMessage = "퀘스트 완료";

    [Header("퀘스트 연결")]
    public QuestData nextQuest;

    //런타임 데이터 (저장되지 않음)
    [System.NonSerialized] public int currentProgress = 0;
    [System.NonSerialized] public bool isActive = false;
    [System.NonSerialized] public bool isCompleted = false;

    //퀘스트 최기화
    public void Initalize()
    {
        currentProgress = 0;
        isActive = false;
        isCompleted = false;
    }

    //퀘스트 완료 체트
    public bool isComplete()
    {
        switch (questType)
        {
            case QuestType.Delivery:
                return currentProgress >= 1;
            case QuestType.Collect:
            case QuestType.Interact:
                return currentProgress >= targetAmount;
            default:
                return false;
        }
    }

    //진행률 계산
    public float GetProgressPersentage()
    {
        if (targetAmount <= 0) return 0;
        return Mathf.Clamp01((float)currentProgress / targetAmount);
    }

    //진행 상황 텍스트
    public string GetProgressText()
    {
        switch (questType)
        {
            case QuestType.Delivery:
                return isCompleted ? "배달 완료!" : "목적지로 이동하세요.";
            case QuestType.Collect:
                return $"{currentProgress}/{targetAmount}";
            case QuestType.Interact:
                return $"{currentProgress} / {targetAmount}";
            default:
                return "";
        }
    }
}

