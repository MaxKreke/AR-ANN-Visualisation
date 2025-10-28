using UnityEngine;

public class InputDescription : MonoBehaviour
{
    public string attributeName;

    public void SetAttributeName(int idx)
    {
        attributeName = Consts.names[idx];

        //Assign correct texture to the material that corresponds to the attribute
        Texture2D texture = Resources.Load<Texture2D>("Textures/" + attributeName);

        if(texture != null)GetTopMaterial().mainTexture = texture;
    }

    public string GetAttributeName()
    {
        return attributeName;
    }

    public Material GetTopMaterial()
    {
        return transform.GetChild(1).GetComponent<MeshRenderer>().material;
    }

    public void Highlight(float thickness)
    {
        Utils.HighlightSelf(transform.GetChild(0).gameObject, thickness);
    }

    public void ColorNode(bool selected)
    {
        GetTopMaterial().color = selected ? Color.yellow : Color.white;
    }

}
