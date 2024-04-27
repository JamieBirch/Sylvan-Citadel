using UnityEngine;

public class Pine : MonoBehaviour
{
    public OwnedHex hex;
    public GameObject treePrefab;
    public int _chanceToGrowTree;

    private TerrainManager _terrainManager;
    
    private void Start()
    {
        Calendar.NewDay += StartDay;
        _terrainManager = TerrainManager.instance;
    }
    
    void StartDay()
    {
        if (hex.GetWoodland().getCount() < 100)
        {
            double chance = Utils.GenerateRandomChance();
            if (chance < _chanceToGrowTree)
            {
                _terrainManager.SpawnTreeAt(hex, treePrefab, transform.position);
            }
        }
        Destroy(gameObject);
    }
    
    public void OnDestroy()
    {
        Calendar.NewDay -= StartDay;
    }
}
