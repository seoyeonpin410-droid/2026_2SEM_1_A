using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 3f;

    private bool isOpen;

    [SerializeField] private Quaternion closedRotation;
    [SerializeField] private Quaternion openRoatation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closedRotation = transform.localRotation;
        openRoatation = closedRotation * Quaternion.Euler(0, openAngle, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRotation;

        if (isOpen)
        {
            targetRotation = openRoatation;
        }
        else
        {
            targetRotation = closedRotation;
        }

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, openSpeed * Time.deltaTime);
    }

    public void Interact()
    {
        isOpen = !isOpen;                   //bool 값을 뒤집는다.
    }

    public string GetGuideText()
    {
        if (isOpen)
        {
            return "[E] 문 닫기";
        }
        return "[E] 문 열기";
    }
}