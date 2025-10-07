using UnityEngine;

public class OutputNodeRef : NodeRef
{

    private string[] classNames =
    {
        "Kieferngewächse",
        "Weidegewächse",
        "Krummholz"
    };
    private string className;

    public void SetClass(int idx)
    {
        className = classNames[idx];
    }

    public override void collectWeightRefs()
    {
        //Adds All Weightrefs into wrs List
        for(int i = 1; i < transform.childCount; i++) 
        {
            Transform t = transform.GetChild(i);
            wrs.Add(t.gameObject.GetComponent<WeightRef>());
        }
    }

    public override void Highlight(float thickness)
    {
        Utils.HighlightSelf(transform.GetChild(0).gameObject, thickness);
        if (thickness > 0.01f) HighlightChildren(1.0f);
        else HighlightChildren(0.0f);
    }

    public override string GetString()
    {
        return "Klasse: " + className + "\nBias: " + GetBias().ToString();
    }
}
