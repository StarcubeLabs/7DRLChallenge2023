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

public interface IMenuInteractable
{
    void StartHighlightMenuItem(MenuItem menuItem);
    void StopHighlightMenuItem(MenuItem menuItem);
}

public class MenuItem : MonoBehaviour, IHighlightable
{
    public EventHandler<EventArgs> onSelect;

    List<IMenuInteractable> menuItemListeners = new List<IMenuInteractable>();

    public void AttachMenuListener(IMenuInteractable menuInteractable)
    {
        menuItemListeners.Add(menuInteractable);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        menuItemListeners.ForEach((menuItemListener) =>
        {
            menuItemListener.StopHighlightMenuItem(this);
        });
    }

    public void OnSelect(BaseEventData eventData)
    {
        menuItemListeners.ForEach((menuItemListener) =>
        {
            menuItemListener.StartHighlightMenuItem(this);
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        menuItemListeners.ForEach((menuItemListener) =>
        {
            menuItemListener.StartHighlightMenuItem(this);
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        menuItemListeners.ForEach((menuItemListener) =>
        {
            menuItemListener.StopHighlightMenuItem(this);
        });
    }


    public void OnClick(BaseEventData eventData)
    {
        menuItemListeners.ForEach((menuItemListener) =>
        {
            menuItemListener.StartHighlightMenuItem(this);
        });
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