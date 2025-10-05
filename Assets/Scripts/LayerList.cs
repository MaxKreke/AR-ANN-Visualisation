using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LayerList : MonoBehaviour
{
    public TMP_InputField input;
    public List<int> layers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        layers = new List<int> ();
    }

    public bool AddLayer()
    {
        //Abort if layer count would exceed 5
        if (layers.Count == 5) return false;

        string digit = input.text;
        int nodeCount;

        //Abort if Input isnt an integer
        if(!int.TryParse(digit, out nodeCount))
        {
            Debug.LogWarning("Invalid input");
            return false;
        }

        //Abort of layer has invalid node count
        if (nodeCount < 1) return false;
        if (nodeCount > 20) return false;

        layers.Add(nodeCount);
        return true;
    }

    public bool RemoveLayer()
    {
        if (layers.Count == 0) return false;
        layers.RemoveAt(layers.Count - 1);
        return true;
    }
}
