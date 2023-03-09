using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IHighlightable : ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISubmitHandler
{

}

public class MenuItem : MonoBehaviour, IHighlightable
{
    public EventHandler<EventArgs> onSelect;

    ContextMenu contextMenu;

    public void Start()
    {
        contextMenu = FindObjectOfType<ContextMenu>();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        contextMenu.DeselectMenuItem(this);
    }

    public void OnSelect(BaseEventData eventData)
    {
        contextMenu = FindObjectOfType<ContextMenu>();
        contextMenu.SelectMenuItem(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        contextMenu.SelectMenuItem(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        contextMenu.DeselectMenuItem(this);
    }


    public void OnClick(BaseEventData eventData)
    {
        contextMenu.SelectMenuItem(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(onSelect != null)
        {
            onSelect(this, EventArgs.Empty);
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (onSelect != null)
        {
            onSelect(this, EventArgs.Empty);
        }
    }
}