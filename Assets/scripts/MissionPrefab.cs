using UnityEngine;
using UnityEngine.UI;

public class MissionPrefab : MonoBehaviour
{
    public Text missionText;
    public GameObject checkmark;
    public Mission mission; 

    private void Start()
    {
        missionText.text = mission.GiveWording();
    }
    
    void Update()
    {
        if (!mission.finished && mission.CheckFinished())
        {
            mission.finished = true;
            checkmark.SetActive(true);
            SoundManager.PlaySound(SoundManager.Sound.applause);
            SoundManager.PlaySound(SoundManager.Sound.pencil);
            GetComponent<Animator>().Play("pop");
        }
    }

    public void SetMission(Mission mission)
    {
        this.mission = mission;
    }
    
}
