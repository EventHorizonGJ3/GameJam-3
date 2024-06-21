using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] float elementSizeMultiplier = 1.2f;
    [SerializeField] float animationDuration = 0.2f;
    private RectTransform rectTransform;
    private Coroutine currentCoroutine;
    private Vector3 originalElementScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalElementScale = rectTransform.localScale;

        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        AddEventTrigger(trigger, EventTriggerType.PointerEnter, Animation_Expand);
        AddEventTrigger(trigger, EventTriggerType.Select, Animation_Expand);
        AddEventTrigger(trigger, EventTriggerType.PointerExit, Animation_Shrink);
        AddEventTrigger(trigger, EventTriggerType.Deselect, Animation_Shrink);
    }

    private void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    private void Animation_Expand(BaseEventData eventData)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(AnimateScale(originalElementScale * elementSizeMultiplier));
    }

    private void Animation_Shrink(BaseEventData eventData)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(AnimateScale(originalElementScale));
    }

    private IEnumerator AnimateScale(Vector3 targetScale)
    {
        Vector3 initialScale = rectTransform.localScale;
        float time = 0;

        while (time < animationDuration)
        {
            rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, time / animationDuration);
            time += Time.deltaTime;
            yield return null;
        }
        rectTransform.localScale = targetScale;
    }

    public void BackToOriginalScale()
    {
        rectTransform.localScale = originalElementScale;
    }

    private void OnDisable()
    {
        BackToOriginalScale();
    }


}
