using UnityEngine;

public class Fruit : MonoBehaviour
{
    public OwnedHex hex;
    public bool isClaimed = false;
    public GameObject treePrefab;
    public int _chanceToGrowTree;

    private TerrainManager _terrainManager;

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
        }
        Destroy(gameObject);
        
        GameStats.Food--;
    }
    
    public void OnDestroy()
    {
        Calendar.NewDay -= StartDay;
    }
}
