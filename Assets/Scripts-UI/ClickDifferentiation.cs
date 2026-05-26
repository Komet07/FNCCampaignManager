using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class ClickDifferentiation : MonoBehaviour, IPointerClickHandler
{
    public int _t = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (_t == 0)
            {
                MapManager.Instance.SwitchPlayer(1);
            }
        }
    }
}
