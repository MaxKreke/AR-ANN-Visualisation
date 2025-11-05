using UnityEngine;
using TMPro;

public class ToggleMode : MonoBehaviour
{
    private bool training = true;
    public TMP_Text buttonText;
    public ANNManager annM;
    public ANNContainer annC;
    public InputValues iv;
    private Transform helpField;

    private void Awake()
    {
        helpField = GameObject.Find("MainCanvas").transform.GetChild(2);
    }

    public bool GetIsTraining()
    {
        return training;
    }

    public void SetMode()
    {
        training = !training;
        UpdateTransform(transform);
        UpdateTransform(helpField);
        if (training)
        {
            buttonText.text = "Vorhersagemodus";
            annM.ResetColors();
            return;
        }
        buttonText.text = "Trainingsmodus";

        //Paint Selected Node as soon as Vorhersagemodus is enabled
        iv.UpdateSelection();

        annC.PredictInput();
    }

    private void UpdateTransform(Transform t)
    {
        t.GetChild(0).gameObject.SetActive(training);
        t.GetChild(1).gameObject.SetActive(!training);
    }

}
