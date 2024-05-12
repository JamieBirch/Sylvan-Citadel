using UnityEngine;

public class Pine : MonoBehaviour
{
    public OwnedTile tile;
    // public GameObject treePrefab;

    private TerrainManager _terrainManager;
    
    private void Start()
    {
        Calendar.NewDay += StartDay;
        // _terrainManager = TerrainManager.instance;
    }
    
    void StartDay()
    {
        /*int treesOnTile = tile.GetWoodland().getCount();
        if (treesOnTile < 100)
        {
            double chance = Utils.GenerateRandomChance();
            if (chance < 100 - treesOnTile)
            {
                _terrainManager.SpawnTreeAt(tile, treePrefab, transform.position);
            }
        }*/
        Destroy(gameObject);
    }
    
    public void OnDestroy()
    {
        Calendar.NewDay -= StartDay;
    }
}
