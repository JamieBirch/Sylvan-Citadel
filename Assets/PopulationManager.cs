using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public static PopulationManager instance;
    
    public GameObject human;
   
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

        string name = NameGenerator.CreateName();
        humanGameObject.name = name;

        humanGameObject.GetComponent<Human>().Name = name;
    }
}
