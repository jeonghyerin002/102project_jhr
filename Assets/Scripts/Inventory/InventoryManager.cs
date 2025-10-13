using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Inventory Setting")]
    public int inventorySize = 20;
    public GameObject inventoryUI;
    public Transform itemSlotParnent;
    public GameObject itemSlotPrefab;

    [Header("Input")]
    public KeyCode inventoryKey = KeyCode.I;
    private List<InventorySlot> slots = new List<InventorySlot>();
    private bool isInventoryOpen = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy (gameObject);
    }
    void Start()
    {
        CreateInventorySlots();
        inventoryUI.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(inventoryKey))
        {
            ToggleInventory();
        }
    }

    //인벤토리 슬롯들을 생성하는 함수
    void CreateInventorySlots()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            //프리팹으로 슬롯 생성
            GameObject slotObj = Instantiate(itemSlotPrefab, itemSlotParnent);
            InventorySlot slot = slotObj.GetComponent<InventorySlot>();
            slots.Add(slot);
        }
    }
    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);

        if(isInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    //아이템을 인벤토리에 추가하는 함수
    public bool AddItem(ItemData item, int amount = 1)
    {
        foreach(InventorySlot slot in slots)
        {
            if(slot.item == item && slot.amount < item.maxStack)
            {
                int spaceLeft = item.maxStack - slot.amount;
                int amountToAdd = Mathf.Min(amount, spaceLeft);
                slot.AddAmount(amountToAdd);
                amount -= amountToAdd;
                
                if(amount <= 0)
                {
                    return true;
                }
            }
        }
        foreach (InventorySlot slot in slots)
        {
            if(slot.item == null)
            {
                slot.SetItem(item, amount);
                return true;
            }
        }
        Debug.Log("인벤토리가 가득 참");
        return false;
    }

    //아이템을 인벤토리에서 제거 함수
    public void RemoveItem(ItemData item, int amount = 1)
    {
        foreach(InventorySlot slot in slots)
        {
            if(slot.item == item)
            {
                slot.RemoveAmount(amount);
                return;
            }
        }
    }
    public int GetItemCount(ItemData item)
    {
        int count = 0;
        foreach (InventorySlot slot in slots)
        {
            if(slot.item == item)
            {
                count += slot.amount;
            }
        }
        return count;
    }
}
