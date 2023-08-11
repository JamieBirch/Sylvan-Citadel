using UnityEngine;
using UnityEngine.UI;

public class PlayerMessageService : MonoBehaviour
{
    public static PlayerMessageService instance;
    
    public GameObject messagePrefab;
    public Text messagePrefabText;
    
    public float showMessageFor = 3f;
    private float countdown;

    private void Awake()
    {
        instance = this;
    }
    
    private void Update()
    {
        //TODO >1 message
        if (messagePrefab.activeSelf)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0)
            {
                messagePrefab.SetActive(false);
                countdown = showMessageFor;
            }
        }
    }
    
    public void ShowMessage(string messageText)
    {
        messagePrefab.SetActive(true);
        messagePrefabText.text = messageText;
    }
}
