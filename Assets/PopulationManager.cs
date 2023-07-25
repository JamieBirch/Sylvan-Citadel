using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public static PopulationManager instance;
    
    public GameObject human;
    
    public string humanTag = "human";
   
    private void Awake()
    {
        instance = this;
    }
    
    public void SpawnHumans(int humansCount, Vector3 hexCenter)
    {
        for (int i = 0; i < humansCount; i++)
        {
            SpawnHuman(hexCenter);
        }
    }

    public void SpawnHuman(Vector3 hexCenter)
    {
        var position = ConstructionManager.instance.PositionOnHex(hexCenter) + new Vector3(0, 1.25f, 0);
        GameObject humanGameObject = Instantiate(human, position, Quaternion.identity);
        GameStats.Population++;

        string name = NameGenerator.CreateHumanName();
        humanGameObject.name = name;

        humanGameObject.GetComponent<Human>().Name = name;
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
