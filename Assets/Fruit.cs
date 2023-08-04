using UnityEngine;

public class Fruit : MonoBehaviour
{
    public OwnedHex hex;
    public Woodland woodland;
    public bool isClaimed = false;
    
    private TerrainManager _terrainManager;
    private int _chanceToGrowTree = 80;

    private void Start()
    {
        Calendar.NewDay += StartDay;
        _terrainManager = TerrainManager.instance;
    }
    
    void StartDay()
    {
        double chance = Utils.GenerateRandomChance();
        if (chance < _chanceToGrowTree)
        {
            _terrainManager.SpawnTreeAt(hex, woodland.transform, transform.position);
        }
        Destroy(gameObject);
        
        hex.FruitsAvailable--;
    }
    
    public void OnDestroy()
    {
        Calendar.NewDay -= StartDay;
    }
}
