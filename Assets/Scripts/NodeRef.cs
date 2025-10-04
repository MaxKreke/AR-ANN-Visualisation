using UnityEngine;
using Accord.Neuro;
using System.Collections.Generic;

public class NodeRef : MonoBehaviour
{
    private ActivationNeuron neuron;
    public bool input = true;
    private List<WeightRef> wrs;

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

    public void collectWeightRefs()
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

}
