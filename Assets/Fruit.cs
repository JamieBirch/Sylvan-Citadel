using UnityEngine;

public class Fruit : MonoBehaviour
{
    public bool isClaimed = false;
    private TerrainManager _terrainManager;

    private void Start()
    {
        Calendar.NewDay += StartDay;
        _terrainManager = TerrainManager.instance;
    }
    
    void StartDay()
    {
        double chance = Utils.GenerateRandomChance();
        if (chance < 80)
        {
            _terrainManager.SpawnTreeAt(gameObject.GetComponentInParent<Woodland>().transform, transform.position);
        }
        Destroy(gameObject);
        GameStats.FruitsAvailable--;
    }
    
    public void OnDestroy()
    {
        Calendar.NewDay -= StartDay;
    }
}
