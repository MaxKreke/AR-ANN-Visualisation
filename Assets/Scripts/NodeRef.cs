using UnityEngine;
using Accord.Neuro;
using System.Collections.Generic;

public class NodeRef : MonoBehaviour
{
    protected ActivationNeuron neuron;
    protected List<WeightRef> wrs;
    public bool input = true;

    public void Awake()
    {
        wrs = new List<WeightRef>();
    }

    public void AssignNode(ActivationNeuron n)
    {
        neuron = n;
        input = false;
    }

    public Neuron GetNeuron() { return neuron; }

    public double GetBias() { return neuron.Threshold; }
    public float GetBiasF() { return (float)neuron.Threshold; }

    public virtual void collectWeightRefs()
    {
        //Adds All Weightrefs into wrs List
        foreach(Transform t in transform)wrs.Add(t.gameObject.GetComponent<WeightRef>());
    }

    public void UpdateWeights()
    {
        foreach (WeightRef wr in wrs)
        {
            wr.UpdateColorAndShape();
        }
    }

    public virtual void Highlight(float thickness)
    {
        Utils.HighlightSelf(this.gameObject, thickness);
        if (thickness > 0.01f) HighlightChildren(1.0f);
        else HighlightChildren(0.0f);
    }

    public void HighlightChildren(float thickness)
    {
        foreach (WeightRef wr in wrs)Utils.HighlightSelf(wr.gameObject, thickness);
    }

    public virtual string GetString()
    {
        return "Bias: " + GetBias().ToString();
    }

    public virtual void UpdateColor(float brightness)
    {
        Debug.Log("here");
        return;
    }

}
