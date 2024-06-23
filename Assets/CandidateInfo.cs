using UnityEngine;
using UnityEngine.UI;

public class CandidateInfo : MonoBehaviour
{
    public Text name;
    public Text boon;
    public NewMissionInfo m1;
    public NewMissionInfo m2;
    // public NewMissionInfo m3;

    public Text buttonText;
    public Monarch monarch;

    public void SetStuff(Monarch monarch)
    {
        this.monarch = monarch;
        
        name.text = monarch.Name;
        boon.text = monarch.boon.GetDescription();

        m1.missionText.text = monarch.missions[0].GiveWording();
        m2.missionText.text = monarch.missions[1].GiveWording();
        // m3.missionText.text = monarch.missions[2].GiveWording();

        buttonText.text = "continue as " + monarch.Name;
    }

    public void ChooseMonarch()
    {
        GameManager.instance.ChooseMonarch(monarch);
    }
    
}
