using UnityEngine;

public class wobble : MonoBehaviour
{
    private float i = 0;
    public float speed = 100;
    public float range = 1000;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        i += 1.0f/ speed;
        transform.position += Vector3.forward * Mathf.Sin(i)/ range;
        transform.position += Vector3.left * Mathf.Cos(i) / range;
    }
}
