using UnityEngine;
using System.Collections.Generic;

public class InputContainer : MonoBehaviour
{
    private List<InputDescription> inputDescs;

    //Awake gets called as soon as Class is created as opposed to Start which gets called only at the beginning of the next frame
    public void Awake()
    {
        inputDescs = new List<InputDescription>();
    }

    public void CollectInputRefs()
    {
        //Adds All Input Descriptions into inputDescs List
        foreach (Transform t in transform) inputDescs.Add(t.gameObject.GetComponent<InputDescription>());
    }

    public void ColorBySelection(int selection)
    {
        ResetColors();
        inputDescs[selection].ColorNode(true);
    }

    public void ResetColors()
    {
        foreach (InputDescription indsc in inputDescs)
        {
            indsc.ColorNode(false);
        }
    }
}
