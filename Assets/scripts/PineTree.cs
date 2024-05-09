using UnityEngine;

public class PineTree : Tree
{
    private static Vector3 PinePositionOffset()
    {
        return new Vector3(Utils.GenerateRandom(-0.5f, 0.5f), 0f, Utils.GenerateRandom(-0.5f, 0.5f));
    }

    public override void DropSeed()
    {
        GameObject _pine = Instantiate(seedPrefab, transform.position + PinePositionOffset(), Quaternion.identity);
        Pine pineComponent = _pine.GetComponent<Pine>();
        pineComponent.tile = tile;
    }
}
