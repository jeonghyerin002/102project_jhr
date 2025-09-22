using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    [Header("문 설정")]
    public bool isOpen = false;
    public Vector3 openPosition;
    public float openSpeed = 2f;

    private Vector3 closePosition;

    protected override void Start()
    {
        base.Start();                                                 //기존 상속받은 스타트 함수를 한 번 실행시킨다.
        objectName = "문";
        interacrtionText = "[E] 문열기";
        interactionType = InteractionType.Building;

        closePosition = transform.position;
        openPosition = closePosition + Vector3.right * 3f;           //오른쪽으로 3m 이동
    }

    protected override void AccessBuilding()
    {
        isOpen = !isOpen;
        if(isOpen)
        {
            interacrtionText = "[E] 문 닫기";
            StartCoroutine(MoveDoor(openPosition));
        }
        else
        {
            interacrtionText = "[E] 문 열기";
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
