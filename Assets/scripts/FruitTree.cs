using UnityEngine;

public class FruitTree : Tree
{
    public override void DropSeed()
    {
        GameObject _fruit = Instantiate(seedPrefab, transform.position + FruitPositionOffset(), Quaternion.identity);
        Fruit fruitComponent = _fruit.GetComponent<Fruit>();
        fruitComponent.tile = tile;
    }

    private static Vector3 FruitPositionOffset()
    {
        return new Vector3(Utils.GenerateRandom(-0.5f, 0.5f), 0f, Utils.GenerateRandom(-0.5f, 0.5f));
    }
}
