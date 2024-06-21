using System.Collections.Generic;
using UnityEngine;

public class TerraformingButtonsPanel : MonoBehaviour
{
    public List<TerraformingButton> buttons = new List<TerraformingButton>();

    // Update is called once per frame
    void Update()
    {
        foreach (TerraformingButton button in buttons)
        {
            if (button.IsShowable())
            {
                button.gameObject.SetActive(true);
            }
            else
            {
                button.gameObject.SetActive(false);
            }
        }
    }
}
