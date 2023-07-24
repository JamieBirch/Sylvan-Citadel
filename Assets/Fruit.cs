using UnityEngine;

public class Fruit : MonoBehaviour
{
    public bool isClaimed = false;
    private TerrainManager _terrainManager;

    private void Start()
    {
        GameManager.NewDay += StartDay;
        _terrainManager = TerrainManager.instance;
    }
    
    void StartDay()
    {
        _terrainManager.SpawnTreeAt(transform.position);
        Destroy(gameObject);
    }
    
    public void OnDestroy()
    {
        GameManager.NewDay -= StartDay;
    }
}
