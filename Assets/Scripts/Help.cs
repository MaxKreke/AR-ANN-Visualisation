using UnityEngine;

public class Help : MonoBehaviour
{

    private bool helpVisible = false;
    public GameObject helpfield;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleHelp()
    {
        helpVisible = !helpVisible;
        helpfield.SetActive(helpVisible);
    }
}
