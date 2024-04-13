using UnityEngine;
using UnityEngine.UI;

public class TileStat : MonoBehaviour
{
    public Text tileStatNameText;
    public Text tileStatCountText;
    
    public void SetName(string name)
    {
        tileStatNameText.text = name;
    }
    
    public void SetCount(int count)
    {
        tileStatCountText.text = count.ToString();
    }
}
