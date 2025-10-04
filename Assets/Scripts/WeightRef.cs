using UnityEngine;

public class WeightRef : MonoBehaviour
{
    public int idx;
    public double[] weightsRef;

    public Transform prevNode;
    public Transform nextNode;

    public float thicknessFactor;
    private float maximumThickness = 2.0f;
    private MeshRenderer m_renderer;

    private void Awake()
    {
        m_renderer = GetComponent<MeshRenderer>();
    }

    public double GetWeight() { return weightsRef[idx]; }
    public float GetWeightF() { return (float)weightsRef[idx]; }

    public void AssignWeight(double[] weights, int index)
    {
        idx = index;
        weightsRef = weights;
    }

    public void AssignTransforms(Transform prev, Transform next)
    {
        prevNode = prev;
        nextNode = next;
    }

    public void AssignThickness(float thickness)
    {
        thicknessFactor = thickness;
    }

    public void UpdateColorAndShape()
    {
        Vector3 scale = transform.localScale;
        scale.x = thicknessFactor * Mathf.Min(Mathf.Abs(GetWeightF()), maximumThickness);
        scale.z = thicknessFactor * Mathf.Min(Mathf.Abs(GetWeightF()), maximumThickness);
        transform.localScale = scale;
        m_renderer.materials[0].color = GetColorFromWeight();
    }

    public Color GetColorFromWeight()
    {

        float weight = Mathf.Clamp(GetWeightF(), -4f,4f)/4f;

        //Returns Green if weight is large, Red if weight is negative and invibsible black if weight is small 
        if(weight>0)return new Color(0f, weight, 0f, weight*.95f);
        if(weight<0)return new Color(-weight, 0f, 0f, -weight*.95f);
        return new Color(0f,0f,0f,0f);
    }

}
