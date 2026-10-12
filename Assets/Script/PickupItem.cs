using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerPickup : MonoBehaviour
{
    [Header("직접 연결")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;

    [Header("아이템 탐색")]
    [SerializeField] private float detectionRadius = 2f;

    [Header("줍기 모션")]
    [SerializeField] private float pickupDuration = 1.2f;
    [SerializeField] private float pickupMoment = 0.55f;

    [Header("아이템 정보 및 연출")]
    [SerializeField] private string itemName = "아이템";
    [SerializeField] private int amount = 1;
    [SerializeField] private float rotationSpeed = 60f;
    [SerializeField] private float floatingHeight = 0.15f;
    [SerializeField] private float floatingSpeed = 2f;

    private Transform targetItemTransform;
    private bool isPickingUp;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (isPickingUp) return;

        // 1. 주변 아이템 탐색
        targetItemTransform = FindClosestItemTransform();

        // 2. 키 입력 및 줍기 실행
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null && targetItemTransform != null)
        {
            if (keyboard.eKey.wasPressedThisFrame)
            {
                StartCoroutine(PickupAnimation());
            }
        }

        // 3. 아이템 회전 및 연출
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        float y = Mathf.Sin(Time.time * floatingSpeed) * floatingHeight;
        transform.position = startPosition + Vector3.up * y;
    }

    public void Collect()
    {
        Debug.Log(itemName + " " + amount + "개 획득");

        // 감지된 타겟 아이템이 있으면 타겟을 지우고, 없으면 감지된 오브젝트 삭제
        if (targetItemTransform != null)
        {
            Destroy(targetItemTransform.gameObject);
        }
    }

    private Transform FindClosestItemTransform()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, ~0, QueryTriggerInteraction.Collide);
        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider itemCollider in colliders)
        {
            // 자기 자신(플레이어)은 탐색 제외
            if (itemCollider.gameObject == this.gameObject) continue;

            float distance = Vector3.Distance(transform.position, itemCollider.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = itemCollider.transform;
            }
        }
        return closest;
    }

    private IEnumerator PickupAnimation()
    {
        isPickingUp = true;

        Transform itemToCollect = targetItemTransform;

        if (itemToCollect != null)
        {
            // 아이템 방향으로 캐릭터 회전
            Vector3 itemDirection = itemToCollect.position - transform.position;
            itemDirection.y = 0;

            if (itemDirection.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(itemDirection);
            }
        }

        // 줍기 상태로 변경하여 이동 차단
        if (playerController != null)
        {
            playerController.ChangeState(PlayerState.Pickup);
        }

        if (animator != null)
        {
            animator.CrossFade("PickUp01", 0.1f);
        }

        // 손이 닿는 타이밍까지 대기
        yield return new WaitForSeconds(pickupMoment);

        // 아이템 파괴 처리
        Collect();

        // 남은 애니메이션 시간 대기
        yield return new WaitForSeconds(pickupDuration - pickupMoment);

        if (animator != null)
        {
            animator.CrossFade("Blend Tree", 0.1f);
        }

        if (playerController != null)
        {
            playerController.ChangeState(PlayerState.Normal);
        }

        targetItemTransform = null;
        isPickingUp = false;
    }
}