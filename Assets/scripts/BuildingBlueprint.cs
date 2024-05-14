using UnityEngine;

public abstract class BuildingBlueprint : MonoBehaviour
{
    public string name;
    public int woodPrice;
    public string description;
    public GameObject buildingPrefab;
    
    /*public OwnedTile tile;
    
    private bool selected;*/

    public bool IsBuildable()
    {
        return GameStats.GetWood() >= woodPrice;
    }

    public abstract bool IsShowable();

    
}
