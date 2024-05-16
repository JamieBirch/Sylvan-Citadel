using UnityEngine;

public abstract class BuildingBlueprint : MonoBehaviour
{
    public bool locked;
    public string name;
    public int woodPrice;
    public string description;
    public GameObject buildingPrefab;
    
    public abstract bool IsBuildable();

    public abstract bool IsShowable();
}
