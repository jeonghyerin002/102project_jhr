using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("��ȣ�ۿ� ����")]
    public string objectName = "������";
    public string interacrtionText = "[E] ��ȣ�ۿ�";
    public InteractionType interactionType = InteractionType.Item;

    [Header("���̶���Ʈ ����")]
    public Color highlightColor = Color.yellow;
    public float highlightIntensity = 1.5f;

    public Renderer objectRenderer;
    protected Color originalColor;
    protected bool isHighlighted = false;


    public enum InteractionType
    { 
        Item,
        Machine,
        Building,
        NPC,
        Collectible
    }
    protected virtual void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if(objectRenderer != null )
        {
            originalColor = objectRenderer.material.color;
        }

        gameObject.layer = 8;                  //(Layer 8 = interactable�� ����)
    }
    public virtual void OnPlyerEnter()
    {
        Debug.Log($"{objectName} ������");
        HighlightObject();
    }
    public virtual void OnPlayerExit()
    {
        Debug.Log($"{objectName} �������� ���");
        RemoveHighlight();
    }


    public virtual void Interact()
    {
        //��ȣ�ۿ� ���Կ� ���� �⺻ ����
        switch (interactionType)
        { 
            case InteractionType.Item:
                CollectItem();
                break;
            case InteractionType.Machine:
                OperateMachine();
                break;
            case InteractionType.Building:
                AccessBuilding();
                break;
            case InteractionType.Collectible:
                CollectItem();
                break;
        }

    }

    public string GetInteractionText()
    {
        return interacrtionText;
    }

    protected virtual void HighlightObject()
    {
        if(objectRenderer != null && !isHighlighted)
        {
            objectRenderer.material.color = highlightColor;
            objectRenderer.material.SetFloat("_Emission", highlightIntensity);
            isHighlighted = true;
        }

    }

    protected virtual void RemoveHighlight()
    {
        if (objectRenderer != null && isHighlighted)
        {
            objectRenderer.material.color = originalColor;
            objectRenderer.material.SetFloat("_Emission", 0f);
            isHighlighted = false;
        }
    }

    protected virtual void CollectItem()
    {
        Debug.Log($"{objectName}��(��)�� �����߽��ϴ�");
        Destroy(gameObject);
    }
    protected virtual void OperateMachine()
    {
        Debug.Log($"{objectName}��(��) �۵����׽��ϴ�.");
        if(objectRenderer != null)
        {
            objectRenderer.material.color = Color.green;
        }
    }
    protected virtual void AccessBuilding()
    {
        Debug.Log($"{objectName}��(��)�� �����߽��ϴ�.");
        transform.Rotate(Vector3.up * 90f);
    }
    protected virtual void TalkToNPC()
    {
        Debug.Log($"{objectName}�� ��ȭ�� �����մϴ�.");
    }
}
