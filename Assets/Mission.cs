using UnityEngine;
using UnityEngine.UI;

public abstract class Mission : MonoBehaviour
{
    public string wording;
    public bool finished = false;
    public Text missionText;
    public GameObject checkmark;

    private void Start()
    {
        GiveWording();
        missionText.text = wording;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckFinished())
        {
            finished = true;
            checkmark.SetActive(true);
            //TODO add visual effect
            //TODO add sound
        }
    }

    public abstract bool CheckFinished();
    public abstract void GiveWording();
}