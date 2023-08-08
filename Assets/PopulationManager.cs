using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public static PopulationManager instance;
    
    public GameObject village;
    public GameObject human;
    
    // public string humanTag = "human";
   
    private void Awake()
    {
        instance = this;
    }
    
    public void SpawnHumans(int humansCount, GameObject hex)
    {
        var _village = CreateVillage(hex);
        
        for (int i = 0; i < humansCount; i++)
        {
            SpawnHuman(_village);
        }
    }

    public GameObject CreateVillage(GameObject hex)
    {
        //TODO check if village already exists
        
        GameObject _village = Instantiate(village, hex.transform.position, Quaternion.identity, hex.transform);
        _village.name = "village";
        hex.GetComponent<OwnedHex>().village = _village;
        return _village;
    }

    public GameObject SpawnHuman(GameObject village)
    {
        var position = ConstructionManager.instance.PositionOnHex(village.transform.position) + new Vector3(0, 1.25f, 0);
        GameObject humanGameObject = Instantiate(human, position, Quaternion.identity, village.transform);
        GameStats.Population++;

        string name = NameGenerator.CreateHumanName();
        humanGameObject.name = name;

        Human humanComponent = humanGameObject.GetComponent<Human>();
        humanComponent.Name = name;
        OwnedHex homeHex = village.GetComponentInParent<OwnedHex>();
        SettleHumanInHex(homeHex, humanComponent);

        return humanGameObject;
    }
    
    public void SettleHumanInHex(OwnedHex newHomeHex, Human humanComponent)
    {
        if (humanComponent.homeHex != null)
        {
            Village homeVillage = humanComponent.homeHex.gameObject.GetComponentInChildren<Village>();
            homeVillage.humans.Remove(humanComponent);
            humanComponent.homeHex.HexPopulation--;

            GameObject newHomeVillage = newHomeHex.gameObject.GetComponentInChildren<Village>().gameObject;
            humanComponent.gameObject.transform.SetParent(newHomeVillage.transform);
        }
        
        newHomeHex.village.GetComponent<Village>().humans.Add(humanComponent);
        humanComponent.homeHex = newHomeHex;
        newHomeHex.HexPopulation++;
    }

}
