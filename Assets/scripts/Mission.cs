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
        if (!finished && CheckFinished())
        {
            finished = true;
            checkmark.SetActive(true);
            SoundManager.PlaySound(SoundManager.Sound.mission_complete);
            //TODO add visual effect
        }
    }

    public abstract bool CheckFinished();
    public abstract void GiveWording();
}