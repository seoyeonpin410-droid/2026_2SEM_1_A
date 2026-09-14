using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform cameraTransform;

    [Header("이동설정")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("바닥설정")]
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        // Animator 자동 할당 (없으면 자식 오브젝트까지 탐색)
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        // CameraTransform 미할당 시 메인 카메라 자동 연결
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        Vector2 input = Vector2.zero;

        if (keyboard.aKey.isPressed) input.x -= 1f;
        if (keyboard.dKey.isPressed) input.x += 1f;
        if (keyboard.sKey.isPressed) input.y -= 1f;
        if (keyboard.wKey.isPressed) input.y += 1f; // 버그 수정: input.x -> input.y

        input = Vector2.ClampMagnitude(input, 1f);

        // 2. 카메라 방향 참조 (cameraTransform이 null일 경우 디폴트 정면 사용)
        Vector3 cameraForward = cameraTransform != null ? cameraTransform.forward : Vector3.forward;
        Vector3 cameraRight = cameraTransform != null ? cameraTransform.right : Vector3.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // 3. 카메라 기준 이동 방향
        Vector3 moveDirection = cameraForward * input.y + cameraRight * input.x;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        // 4. Shift 달리기
        bool isRunning = keyboard.leftShiftKey.isPressed;
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // 5. 수평 이동
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        // 6. 이동 방향으로 회전
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 7. 중력 설정
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);

        // 8. 애니메이션 전달 (animator 예외 처리)
        if (animator != null)
        {
            float animationSpeed = 0f;

            if (moveDirection.sqrMagnitude > 0.001f)
            {
                animationSpeed = isRunning ? 1f : 0.5f;
            }

            animator.SetFloat("speed", animationSpeed, 0.1f, Time.deltaTime);
        }
    }
}