using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public string name;
    public int woodPrice;
    public string description;

    public abstract bool IsBuildable();

    public abstract bool IsShowable();
}
