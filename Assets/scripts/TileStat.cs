using UnityEngine;
using UnityEngine.UI;

public class TileStat : MonoBehaviour
{
    public Text tileStatNameText;
    public Text tileStatCountText;
    public int tileStatCount = 0;
    
    public void SetName(string name)
    {
        tileStatNameText.text = name;
    }
    
    public void SetCount(int count)
    {
        tileStatCount = count;
        tileStatCountText.text = count.ToString();
    }
    
    public void Increase()
    {
        tileStatCount++;
        tileStatCountText.text = tileStatCount.ToString();
    }

    public void Decrease()
    {
        tileStatCount--;
        tileStatCountText.text = tileStatCount.ToString();
    }
}
