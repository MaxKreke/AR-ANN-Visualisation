using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

public class ANNManager : MonoBehaviour
{

    public LayerMask ANNLayers;
    public GameObject weightInfo;
    public List<LayerManager> layers;
    public GameObject selected;

    void Start() { WriteToInfo("Start"); }

    // Update is called once per frame
    void Update()
    {
        //Do one check for mouse clicks (for the editor) and one check for touch screen presses for the touch screen
        MouseCheck();
        TouchCheck();
    }

    private void MouseCheck()
    {
        if (Mouse.current == null) return;
        if (Mouse.current.leftButton.isPressed)
        {
            CheckRay(Mouse.current.position.ReadValue());
            return;
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame) Release();
    }

    private void TouchCheck()
    {
        if (Touchscreen.current == null)return;
        if (Touchscreen.current.press.isPressed)
        {
            CheckRay(Touchscreen.current.position.ReadValue());
            return;
        }
        if (Touchscreen.current.press.wasReleasedThisFrame) Release();

    }

    private void CheckRay(Vector2 point)
    {
        //Point is the position of the cursor or finger on the screen in 2d coordinates
        //Raycast from camera POV
        Ray ray = Camera.main.ScreenPointToRay(point);
        RaycastHit hit;
        //Cast the defined ray and check for collision on ANN layers
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ANNLayers))
        {
            WeightRef wr = hit.transform.gameObject.GetComponent<WeightRef>();
            NodeRef nr = hit.transform.gameObject.GetComponent<NodeRef>();
            InputDescription iDesc = hit.transform.gameObject.GetComponent<InputDescription>();
            if (wr || nr || iDesc)
            {
                Highlight(0.0f);
                selected = hit.transform.gameObject;

                if (wr)
                {
                    Highlight(1.0f);
                    WriteToInfo("Weight: " + wr.GetWeight().ToString());
                }
                if (nr)
                {
                    Highlight(1.25f);
                    WriteToInfo("Bias: " + nr.GetBias().ToString());
                }
                if (iDesc)
                {
                    Highlight(1.25f);
                    WriteToInfo("Attribute: " + iDesc.GetAttributeName());
                }
            }
        }
        else Release();
    }

    private void Release()
    {
        Highlight(0.0f);
        selected = null;
        weightInfo.SetActive(false);
    }

    private void WriteToInfo(string text)
    {
        weightInfo.GetComponent<TMP_Text>().text = text;
        weightInfo.SetActive(true);
    }

    public void CollectLayers()
    {
        layers = new List<LayerManager>();
        foreach (Transform t in transform.GetChild(0))
        {
            LayerManager lm = t.GetComponent<LayerManager>();
            if(lm)layers.Add(lm);
        }
    }

    public void UpdateLayers()
    {
        foreach (LayerManager lm in layers)
        {
            lm.UpdateNodes();
        }
    }

    public void Highlight(float thickness)
    {
        if (!selected) return;
        selected.GetComponent<MeshRenderer>().materials[1].SetFloat("_Scale", thickness);
    }

}
