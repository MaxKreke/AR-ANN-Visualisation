using UnityEngine;
using TMPro;

public class ToggleMode : MonoBehaviour
{
    private bool training = true;
    public TMP_Text buttonText;
    public ANNManager ann;
    public InputValues iv;

    public bool GetIsTraining()
    {
        return training;
    }

    public void SetMode()
    {
        training = !training;
        transform.GetChild(0).gameObject.SetActive(training);
        transform.GetChild(1).gameObject.SetActive(!training);
        if (training)
        {
            buttonText.text = "Vorhersagemodus";
            ann.ResetColors();
            return;
        }
        buttonText.text = "Trainingsmodus";

        //Paint Selected Node as soon as Vorhersagemodus is enabled
        iv.UpdateSelection();
    }

}
