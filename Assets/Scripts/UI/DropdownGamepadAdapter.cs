using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropdownGamepadAdapter : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private ScrollRect scrollRect;
    private float scrollPosition = 1;
    // Coloring textmesh
    public Color highlightedColor = new Color(0.5568628f, 0.7490196f, 0.8941177f); // the same color of other UI elements
    public Color normalColor = Color.white;
    public Image imageComponent;

    void Start()
    {
        scrollRect = GetComponentInParent<ScrollRect>(true);
        int childCount = scrollRect.content.childCount - 1;
        int childIndex = transform.GetSiblingIndex();

        childIndex = childIndex < ((float)childCount / 2) ? childIndex - 1 : childIndex;
        scrollPosition = 1 - ((float)childIndex / childCount);

        imageComponent = GetComponentInChildren<Image>(true);

        // Ensure the currently selected item is highlighted and visible
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            HighlightAndScrollToCurrent();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        HighlightAndScrollToCurrent();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (imageComponent)
            imageComponent.color = normalColor;
    }

    private void HighlightAndScrollToCurrent()
    {
        if (scrollRect)
        {
            // Ensure scroll rect is updated after a short delay to account for dropdown opening
            StartCoroutine(DelayedScrollUpdate());
        }

        if (imageComponent)
            imageComponent.color = highlightedColor;
    }

    private IEnumerator DelayedScrollUpdate()
    {
        yield return new WaitForEndOfFrame();
        scrollRect.verticalScrollbar.value = scrollPosition;
    }
}
