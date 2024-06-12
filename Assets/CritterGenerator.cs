using UnityEngine;

public class CritterGenerator : MonoBehaviour
{
    public GameObject[] mouths;
    public GameObject[] moustache;
    public GameObject[] cheeks;
    public Renderer skinRend;
    public Color[] colors;
    
    public void RandomizeAppearance()
    {
        int mouthIndex = Utils.GenerateRandomIntNumberWhereMaxIs(mouths.Length - 1);
        mouths[mouthIndex].SetActive(true);

        int moustacheChance = 15;
        if (Utils.GenerateRandomChance() < moustacheChance)
        {
            int moustacheIndex = Utils.GenerateRandomIntNumberWhereMaxIs(moustache.Length - 1);
            moustache[moustacheIndex].SetActive(true);
        }
        
        int cheeksChance = 15;
        if (Utils.GenerateRandomChance() < cheeksChance)
        {
            int cheeksIndex = Utils.GenerateRandomIntNumberWhereMaxIs(cheeks.Length - 1);
            moustache[cheeksIndex].SetActive(true);
        }
        
        skinRend.material.color = colors[Utils.GenerateRandomIntNumberWhereMaxIs(colors.Length - 1)];
    }
}
