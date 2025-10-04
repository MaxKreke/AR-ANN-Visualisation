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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
