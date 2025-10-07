using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LayerList : MonoBehaviour
{
    public TMP_InputField input;
    public List<int> layers;
    private CanvasController cc;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        layers = new List<int> ();
        cc = GameObject.Find("MainCanvas").GetComponent<CanvasController>();
    }

    public bool AddLayer()
    {
        //Abort if layer count would exceed 5
        if (layers.Count == 5)
        {
            cc.StatusPrint(0, "Maximale Schichtenzahl Erreicht.");
            return false;
        }

        string digit = input.text;
        int nodeCount;

        //Abort if Input isnt an integer
        if(!int.TryParse(digit, out nodeCount))
        {
            cc.StatusPrint(0, "Neronenzahl muss Ganzzahl sein.");
            Debug.LogWarning("Invalid input");
            return false;
        }

        //Abort of layer has invalid node count
        if (nodeCount < 1)
        {
            cc.StatusPrint(0, "Neuronenzahl muss zwischen 1 und 20 liegen.");
            return false;
        }
        if (nodeCount > 20)
        {
            cc.StatusPrint(0, "Neuronenzahl muss zwischen 1 und 20 liegen.");
            return false;
        }

        layers.Add(nodeCount);
        cc.StatusPrint(0, " ");
        return true;
    }

    public bool RemoveLayer()
    {
        if (layers.Count == 0) return false;
        layers.RemoveAt(layers.Count - 1);
        return true;
    }
}
