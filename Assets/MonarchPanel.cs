using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonarchPanel : MonoBehaviour
{
    public Text MonarchNameText;
    public Text MonarchBoonText;
    public GameObject missions;
    public GameObject missionPrefab;

    public List<MissionPrefab> SetNewMonarch(Monarch monarch)
    {
        Boon monarchBoon = monarch.boon;
        MonarchNameText.text = monarch.GetNamePlusNickname();

        MonarchBoonText.text = monarchBoon.GetDescription();

        //destroy previous monarch's missions
        foreach(Transform child in missions.transform)
        {
            Destroy(child.gameObject);
        }
        
        List<MissionPrefab> newMissions = new List<MissionPrefab>();
        foreach (Mission mission in monarch.missions)
        {
            MissionPrefab msn = Instantiate(missionPrefab, missions.transform).GetComponent<MissionPrefab>();
            msn.SetMission(mission);
            newMissions.Add(msn);
        }

        return newMissions;
    }
}
