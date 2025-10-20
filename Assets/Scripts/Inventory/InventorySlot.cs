using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public ItemData item;
    public int amount;

    [Header("UI References")]
    public Image itemIcon;
    public Text amountText;
    public GameObject emptySlotImage;
    void Start()
    {
        
    }

    //슬롯에 아이템 설정하는 함수
    public void SetItem(ItemData newItem, int newAmount)
    {
        item = newItem;
        amount = newAmount;
    }
    //아이템 개수 추가하는 함수
    public void AddAmount (int value)
    {
        amount += value;
        UpdateSlotUI();
    }
    public void RemoveAmount(int value)
    {
        amount -= value;

        if (amount <= 0 )
        {
            ClearSlot();
        }
        else
        {
            UpdateSlotUI();
        }
    }
    public void ClearSlot()
    {
        item = null;
        amount = 0;
        UpdateSlotUI();
    }
    //UI를 업데이트하는 함수
    void UpdateSlotUI()
    {
        if (item != null)
        {
            itemIcon.sprite = item.itemIcon;
            itemIcon.enabled = true;

            amountText.text = amount > 1 ? amount.ToString() : "";
            if (emptySlotImage != null)
            {
                emptySlotImage.SetActive(false);
            }
        }
        else
        {
            itemIcon.enabled = false;
            amountText.text = "";
            if(emptySlotImage != null)
            {
                emptySlotImage.SetActive(true);
            }
        }
    }
}
