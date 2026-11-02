using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("아이템 정보")]
    [SerializeField] private string itemName = "아이템";
    [SerializeField] private int amount = 1;
    [SerializeField] private QuestManager questManager;

    [Header("화면 연출")]
    [SerializeField] private float rotationSpeed = 60f;
    [SerializeField] private float floatingHeight = 0.15f;
    [SerializeField] private float floatingSpeed = 2f;

    private Vector3 startPosition;

    private SimpleInventory inventory;

    // Awake를 Start보다 위에 배치
    public void Awake()
    {
        questManager = FindFirstObjectByType<QuestManager>();
        inventory = FindFirstObjectByType<SimpleInventory>();
    }

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // 아이템을 천천히 회전시킨다.
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        // 아이템을 위아래로 움직인다.
        float y = Mathf.Sin(Time.time * floatingSpeed) * floatingHeight;
        transform.position = startPosition + Vector3.up * y;
    }

    public void Collect()
    {
        if (inventory == null)
        {
            Debug.Log("SimpleInventory를 찾을 수 없습니다.");
            return;
        }
        if (inventory.AddItem(itemName,amount) == false)
        {
            return;
        }

        Debug.Log(itemName + " " + amount + "개 획득");
        if (questManager ! == null)
        {
            questManager.AddQuestItem(itemName, amount);
        }
        
        Destroy(gameObject);
    }

    // 오타 수정: GetliemName -> GetItemName
    public string GetItemName()
    {
        return itemName;
    }
}