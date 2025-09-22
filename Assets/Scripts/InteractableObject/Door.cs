using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    [Header("�� ����")]
    public bool isOpen = false;
    public Vector3 openPosition;
    public float openSpeed = 2f;

    private Vector3 closePosition;

    protected override void Start()
    {
        base.Start();                                                 //���� ��ӹ��� ��ŸƮ �Լ��� �� �� �����Ų��.
        objectName = "��";
        interacrtionText = "[E] ������";
        interactionType = InteractionType.Building;

        closePosition = transform.position;
        openPosition = closePosition + Vector3.right * 3f;           //���������� 3m �̵�
    }

    protected override void AccessBuilding()
    {
        isOpen = !isOpen;
        if(isOpen)
        {
            interacrtionText = "[E] �� �ݱ�";
            StartCoroutine(MoveDoor(openPosition));
        }
        else
        {
            interacrtionText = "[E] �� ����";
            StartCoroutine(MoveDoor(closePosition));
        }
    }

    IEnumerator MoveDoor(Vector3 targetPosition)
    {
        while(Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, openSpeed *  Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
    }
}
