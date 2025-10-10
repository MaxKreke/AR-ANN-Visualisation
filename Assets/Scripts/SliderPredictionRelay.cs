using UnityEngine;
using UnityEngine.EventSystems;

public class SliderPredictionRelay : MonoBehaviour, IPointerUpHandler
{

    public ANNContainer ann;

    public void OnPointerUp(PointerEventData eventData)
    {
        ann.PredictInput();
    }

}
