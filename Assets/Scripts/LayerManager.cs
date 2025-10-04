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

}
