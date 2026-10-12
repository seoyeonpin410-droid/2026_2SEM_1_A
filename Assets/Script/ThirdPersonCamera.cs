using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("따라갈 대상")]
    [SerializeField] private Transform target;

    [Header("카메라 위치")]
    [SerializeField] private float distance = 7f;
    [SerializeField] private float height = 1.5f;

    [Header("마우스 회전")]
    [SerializeField] private float mouseSensitivity = 0.12f;
    [SerializeField] private float minVerticalAngle = -30f;
    [SerializeField] private float maxVerticalAngle = 60;

    [Header("마우스 스크롤")]
    [SerializeField] private float zoomSpeed = 1;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 10f;

    private float yaw;
    private float pitch = 15f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;       //시작시에 커서를 Lock 한다.
        Cursor.visible = false;                         //커서를 안보이게 한다.
    }

    void Update()
    {
        Mouse mouse = Mouse.current;

        if (mouse == null)
        {
            return;
        }

        //마우스 회전
        Vector2 mouseDelta = mouse.delta.ReadValue();

        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minVerticalAngle, maxVerticalAngle);

        //마우스 휠 확대와 축소
        float scroll = mouse.scroll.ReadValue().y;

        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }
    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        //카메라가 바라볼 위치
        Vector3 lookPoint = target.position + Vector3.up * height;

        //Yaw와 pitch를 실제 회전 값으로 변환
        Quaternion orbitRotation = Quaternion.Euler(pitch, yaw, 0f);

        //회전 방향을 기준으로 Player 뒤쪽 위치 계산
        Vector3 cameraOffset = orbitRotation * new Vector3(0f, 0f, -distance);

        //Player 주변의 계산된 위치로 이동
        transform.position = lookPoint + cameraOffset;

        //Player 중심을 바라본다.
        transform.LookAt(lookPoint);
    }
}
