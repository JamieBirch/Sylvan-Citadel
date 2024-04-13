using UnityEngine;

public class Fruit : MonoBehaviour
{
    public OwnedHex hex;
    // public Woodland woodland;
    public bool isClaimed = false;
    //TODO code duplication
    public GameObject treePrefab;
    
    private TerrainManager _terrainManager;
    private int _chanceToGrowTree = 5;

    private void Start()
    {
        Calendar.NewDay += StartDay;
        GameStats.Food++;
        _terrainManager = TerrainManager.instance;
    }
    
    void StartDay()
    {
        double chance = Utils.GenerateRandomChance();
        if (chance < _chanceToGrowTree)
        {
            _terrainManager.SpawnTreeAt(hex, treePrefab, transform.position);
            // _terrainManager.SpawnTreeAt(hex, woodland.transform, transform.position);
        }
        Destroy(gameObject);
        
        // hex.FruitsAvailable--;
        GameStats.Food--;
    }
    
    public void OnDestroy()
    {
        Calendar.NewDay -= StartDay;
    }
}
