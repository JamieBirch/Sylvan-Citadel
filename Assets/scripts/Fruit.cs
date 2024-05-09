using UnityEngine;

public class Fruit : MonoBehaviour
{
    public OwnedHex tile;
    public bool isClaimed = false;
    public GameObject treePrefab;

    private TerrainManager _terrainManager;

    private void Start()
    {
        Calendar.NewDay += StartDay;
        GameStats.instance.AddFood();
        _terrainManager = TerrainManager.instance;
    }
    
    void StartDay()
    {
        int treesOnTile = tile.GetWoodland().getCount();
        if (treesOnTile < 100)
        {
            double chance = Utils.GenerateRandomChance();
            if (chance < 100 - treesOnTile)
            {
                _terrainManager.SpawnTreeAt(tile, treePrefab, transform.position);
            }
        }

        GameStats.instance.RemoveFood();

        Destroy(gameObject);
    }
    
    public void OnDestroy()
    {
        Calendar.NewDay -= StartDay;
    }
}
