using UnityEngine;
using TMPro;

public class StartPauseButton : MonoBehaviour
{
    //Mode 0 = Hasnt Started Training
    //Mode 1 = Is Training
    //Mode 2 = Is Paused
    private int mode = 0;
    public TMP_Text buttonText;
    public ANNContainer ann;

    public void OnReset()
    {
        mode = 0;
        buttonText.text = "Start";
    }

    public void Toggle()
    {
        buttonText.text = "Pause Training";

        if (mode == 0) ann.StartProcess();
        if (mode == 1) buttonText.text = "Resume Training";

        //Branchless way of mapping 0 to 1, 1 to 2 and 2 to 1
        mode = (mode % 2) + 1;
        ann.SetFinished(mode == 2);
    }
}
