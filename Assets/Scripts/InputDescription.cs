using UnityEngine;

public class InputDescription : MonoBehaviour
{

    private string[] attributeNames =
    {
        "Erhebung",
        "Entfernung zu Gew‰ssern",
        "Entfernung zu Straﬂen",
        "Hillshade Morgens",
        "Hillshade Mittags",
        "Hillshade Nachmittags",
        "Entfernung zu Brandstelle"
    };
    public string attributeName;

    public void SetAttributeName(int idx)
    {
        attributeName = attributeNames[idx];
    }

    public string GetAttributeName()
    {
        return attributeName;
    }

    public void Highlight(float thickness)
    {
        Utils.HighlightSelf(transform.GetChild(0).gameObject, thickness);
    }

}
