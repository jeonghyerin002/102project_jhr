using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUsePopup : MonoBehaviour
{
    public static ItemUsePopup instance;

    public GameObject popupPenel;
    public Text itemNameText;
    public Image itemIconImage;
    public Button useButton;
    public Button closeButton;

    private ItemData currentItem;
    private InventorySlot currentSlot;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        popupPenel.SetActive(false);
        useButton.onClick.AddListener(UseItem);
        closeButton.onClick.AddListener(ClosePopup);
    }

    public void ShowPopup(ItemData item, InventorySlot slot)
    {
        currentItem = item;
        currentSlot = slot;

        itemNameText.text = item.itemName;
        itemIconImage.sprite = item.itemIcon;

        useButton.interactable = item.isUsable;

        popupPenel.SetActive(true);
    }
    void ClosePopup()
    {
        popupPenel.SetActive(false);
    }
    void UseItem()
    {
        if(currentItem.isUsable)
        {
            PlayerStats player = FindObjectOfType<PlayerStats>();

            if(currentItem.healAmount > 0)
            {
                player.Heal(currentItem.healAmount);
                Debug.Log(currentItem.itemIcon + " 사용 : 체력 회복 " + currentItem.healAmount);
            }
            else if(currentItem.healAmount < 0)
            {
                player.TakeDamage(currentItem.healAmount);
                Debug.Log(currentItem.itemIcon + "사용 체력 감소" + currentItem.healAmount);
            }
            currentSlot.RemoveAmount(1);
        }
        ClosePopup();
    }
}
