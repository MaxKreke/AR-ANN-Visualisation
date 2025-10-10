using UnityEngine;
using System.Collections.Generic;

public class LayerManager : MonoBehaviour
{

    private List<NodeRef> nrs;

    public void Awake()
    {
        nrs = new List<NodeRef>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectNodeRefs()
    {
        //Adds All NodeRef into nrs List
        foreach (Transform t in transform) nrs.Add(t.gameObject.GetComponent<NodeRef>());
    }

    public void UpdateNodes()
    {
        foreach (NodeRef nr in nrs)
        {
            nr.UpdateWeights();
        }
    }

    public void ColorByPrediction(double[] prediction)
    {
        if (nrs.Count != prediction.Length)
        {
            Debug.LogError("Array Size Mismatch!");
            return;
        }
        for(int i = 0; i < Consts.outputSize; i++)
        {
            nrs[i].UpdateColor((float)prediction[i]);
        }
    }

    public void ResetColors()
    {
        foreach (NodeRef nr in nrs) nr.UpdateColor(1.0f);
    }

}
