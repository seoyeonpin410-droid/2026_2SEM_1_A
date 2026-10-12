using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("아이템 정보")]
    [SerializeField] private string itemName = "아이템";

    [SerializeField] private int amount = 1;

    [Header("화면 연출")]
    [SerializeField] private float rotationSpeed = 60f;
    [SerializeField] private float floatingHeight = 0.15f;
    [SerializeField] private float floatingSpeed = 2f;

    private Vector3 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //아이템을 천천히 회전 시킨다.
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        //아이템을 위아래로 움직인다.
        float y = Mathf.Sin(Time.time * floatingSpeed) * floatingHeight;
        transform.position = startPosition + Vector3.up * y;
    }

    public void Collect()
    {
        Debug.Log(itemName + " " + amount + "개 획득");
        Destroy(gameObject);
    }

    public string GetliemName()
    {
        return itemName;
    }
}