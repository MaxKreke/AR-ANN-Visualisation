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

    public Material GetTopMaterial()
    {
        return transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material;
    }

    public void SetClass(int idx)
    {
        className = classNames[idx];

        //Assign correct texture to the material that corresponds to the class
        Texture2D texture = Resources.Load<Texture2D>("Textures/"+idx.ToString());
        GetTopMaterial().mainTexture = texture;
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
        Utils.HighlightSelf(transform.GetChild(0).GetChild(0).gameObject, thickness);
        if (thickness > 0.01f) HighlightChildren(1.0f);
        else HighlightChildren(0.0f);
    }

    public override string GetString()
    {
        return "Klasse: " + className + "\nBias: " + GetBias().ToString();
    }

    public override void UpdateColor(float brightness)
    {
        GetTopMaterial().color = new Color(brightness, brightness, brightness);
    }
}
