using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasShower : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private TownhallCanvas _canvas;

    private void Awake()
    {
        _canvas.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData pointerEvent)
    {
        if(pointerEvent.button == PointerEventData.InputButton.Left)
        {
            _canvas.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData pointerEvent)
    {
        _canvas.gameObject.SetActive(false);
    }
}
