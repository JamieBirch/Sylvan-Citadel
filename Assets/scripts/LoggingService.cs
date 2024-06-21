using UnityEngine;

public class LoggingService : MonoBehaviour
{
    public static LoggingService instance;

    public GameObject logsContainer;
    public GameObject logMessagePrefab;

    public float logMessageLifetime = 3f;

    private void Awake()
    {
        instance = this;
    }
    
    public void LogMessage(string messageText)
    {
        GameObject logGO = Instantiate(logMessagePrefab, logsContainer.transform);
        logGO.GetComponent<LogMessage>().text.text = messageText;
        Destroy(logGO, logMessageLifetime);
    }

}
