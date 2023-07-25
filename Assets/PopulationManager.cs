using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public static PopulationManager instance;
    
    public GameObject village;
    public GameObject human;
    
    public string humanTag = "human";
   
    private void Awake()
    {
        instance = this;
    }
    
    public void SpawnHumans(int humansCount, GameObject hex)
    {
        // GameObject village = hex.GetComponent<OwnedHex>().village;
        // if (village == null)
        // {
            GameObject _village = Instantiate(village, hex.transform.position, Quaternion.identity, hex.transform);
            _village.name = "village";
            hex.GetComponent<OwnedHex>().village = _village;
        // }
        
        for (int i = 0; i < humansCount; i++)
        {
            GameObject newHuman = SpawnHuman(_village);
            newHuman.transform.SetParent(_village.transform);
        }
    }

    public GameObject SpawnHuman(GameObject village)
    {
        var position = ConstructionManager.instance.PositionOnHex(village.transform.position) + new Vector3(0, 1.25f, 0);
        GameObject humanGameObject = Instantiate(human, position, Quaternion.identity, village.transform);
        GameStats.Population++;

        string name = NameGenerator.CreateHumanName();
        humanGameObject.name = name;

        humanGameObject.GetComponent<Human>().Name = name;

        return humanGameObject;
    }

    /*public Human FindAvailableHuman()
    {
        Human humanComponent = null;
        
        GameObject[] humans = GameObject.FindGameObjectsWithTag(humanTag);
        foreach (GameObject _human in humans)
        {
            Human _humanComponent = _human.GetComponent<Human>();
            if (_humanComponent.Satisfied())
            {
                humanComponent = _humanComponent;
                return humanComponent;
            }
        }

        return humanComponent;
    }*/
}
