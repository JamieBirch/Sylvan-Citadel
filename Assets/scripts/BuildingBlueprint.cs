using UnityEngine;

public abstract class BuildingBlueprint : MonoBehaviour
{
    public bool locked;
    public string name;
    public int woodPrice;
    public string description;
    public GameObject buildingPrefab;
    
    /*public OwnedTile tile;
    
    private bool selected;*/

    public bool IsBuildable()
    {
        return TileManager.instance.activeTile != null && 
               GameStats.GetWood() >= woodPrice;
    }

    public bool IsShowable()
    {
        return !locked;
    }

}
