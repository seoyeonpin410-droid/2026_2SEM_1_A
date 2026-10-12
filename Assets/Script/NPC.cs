using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private string npcName = "마을 주민";

    [TextArea]
    [SerializeField] private string message = "마을에 온 것을 환영해";

    public string GetNPCName()
    {
        return npcName;
    }

    public string GetMessage()
    {
        return message;
    }

    public string GetGuideText()
    {
        return "[E] " + npcName + " 와 대화 하기 ";
    }
}