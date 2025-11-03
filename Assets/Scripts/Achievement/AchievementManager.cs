using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    [Header("Achievement Setting")]
    public List<AchievementData> allAchievements = new List<AchievementData>();

    [Header("UI Referencces")]
    public GameObject achievementPopupPrefab;
    public Transform popupParent;
    public GameObject achievementPanel;
    public Transform achievementListContent;
    public GameObject achievementSlotPrefab;

    private Dictionary<AchievementType, int> progressData = new Dictionary<AchievementType, int>();           //통계 저장

    void Awake()
    {
       if(instance == null)               //싱글톤 화
       {
            instance = this;
            DontDestroyOnLoad(gameObject);
       }
       else
       {
            Destroy(gameObject);
       }
    }
    void Start()
    {
        ResetAllAchievements();                    //시작시에 리셋 강제로 (테스트용) 나중에 배포시에는 지운다.
        foreach(AchievementType type in System.Enum.GetValues(typeof(AchievementType)))
        {
            progressData[type] = 0;
        }
        LoadAchievements();
        UpdateAchievementUI();
    }


    void Update()
    {
        
    }
    public void UpdateAchievementUI()
    {
        if (achievementListContent == null || achievementSlotPrefab == null)
            return;
        foreach(Transform child in achievementListContent)
        {
            Destroy(child.gameObject);
        }
        foreach(AchievementData achievement in allAchievements)
        {
            GameObject slot = Instantiate(achievementSlotPrefab, achievementListContent);
            AchievementSlot slotScript = slot.GetComponent<AchievementSlot>();
            if(slotScript != null )
            {
                slotScript.SetAchievement(achievement, GetProgress(achievement));
            }
        }
    }
    public void UpdateProgress(AchievementType type, int amount = 1)
    {
        progressData[type] += amount;
        foreach(AchievementData achievement in allAchievements)
        {
            if(achievement.achievementType == type && !achievement.isUnlocked)
            {
                if (progressData[type] >= achievement.requiredAmount)
                {
                    UnlockAchievement(achievement);
                }
            }
        }
    }
    void UnlockAchievement(AchievementData achievement)
    {
        achievement.isUnlocked = true;

        ShowAchievementPopup(achievement);
        UpdateAchievementUI();
    }
    public float GetProgress(AchievementData achievement)
    {
        if (achievement.isUnlocked) return 1f;
        int current = progressData.ContainsKey(achievement.achievementType) ? progressData[achievement.achievementType] : 0;
        return Mathf.Min((float)current / achievement.requiredAmount, 1f);
    }
    void SaveAchievements()
    {
        foreach (var kvp in progressData)
        {
            PlayerPrefs.SetInt("Achievement_" +  kvp.Key, kvp.Value);
        }
        PlayerPrefs.Save();
    }
    void LoadAchievements()
    {
        foreach(AchievementType type in System.Enum.GetValues(typeof(AchievementType)))
        {
            progressData[type] = PlayerPrefs.GetInt("Achievement_" + type, 0);
        }
        foreach(AchievementData achievement in allAchievements)
        {
            achievement.isUnlocked = PlayerPrefs.GetInt("UnLocked_" + achievement.name, 0) == 1;
        }
    }
    void ShowAchievementPopup(AchievementData achievement)
    {
        if(achievementPopupPrefab != null && popupParent != null)
        {
            GameObject popup = Instantiate(achievementPopupPrefab, popupParent);

            Text titleText = popup.transform.Find("Title")?.GetComponent<Text>();
            Text descript = popup.transform.Find("Deacription")?.GetComponent<Text>();

            if (titleText != null) titleText.text = "업적 달성";
            if (descript != null) descript.text = achievement.achievementName;

            Destroy(popup, 3.0f);
        }
    }
    public void ResetAllAchievements()
    {
        foreach (AchievementType type in System.Enum.GetValues(typeof(AchievementType)))
        {
            progressData[type] = 0;
            PlayerPrefs.DeleteKey("Achievement_" + type);
        }
        foreach (AchievementData achievement in allAchievements)
        {
            achievement.isUnlocked = false;
            PlayerPrefs.DeleteKey("Unlocked_" + achievement.name);
        }

        PlayerPrefs.Save();
        UpdateAchievementUI();
    }
}

