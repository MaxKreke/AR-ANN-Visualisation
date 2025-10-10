using UnityEngine;

public class InputDescription : MonoBehaviour
{
    public string attributeName;

    public void SetAttributeName(int idx)
    {
        attributeName = Consts.names[idx];
    }

    public string GetAttributeName()
    {
        return attributeName;
    }

    public void Highlight(float thickness)
    {
        Utils.HighlightSelf(transform.GetChild(0).gameObject, thickness);
    }

    public void ColorNode(bool selected)
    {
        transform.GetChild(1).GetComponent<MeshRenderer>().material.color = selected ? Color.yellow : Color.white;
    }

}
