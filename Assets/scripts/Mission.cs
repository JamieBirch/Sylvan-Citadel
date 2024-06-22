using UnityEngine;
using UnityEngine.UI;

public abstract class Mission /*: MonoBehaviour*/
{
    // public string wording;
    public bool finished = false;
    // public Text missionText;
    // public GameObject checkmark;
    // public Animator animator;

    /*private void Start()
    {
        missionText.text = GiveWording();
    }*/

    // Update is called once per frame
    /*void Update()
    {
        if (!finished && CheckFinished())
        {
            finished = true;
            checkmark.SetActive(true);
            SoundManager.PlaySound(SoundManager.Sound.applause);
            SoundManager.PlaySound(SoundManager.Sound.pencil);
            GetComponent<Animator>().Play("pop");
        }
    }*/

    public abstract bool CheckFinished();

    public abstract string GiveWording();
    
    
}