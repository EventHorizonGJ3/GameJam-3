using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] float buttonSizeMultiplier = 1.2f;
    [SerializeField] float animationDuration = 0.2f;
    Button button;
    Coroutine currentCoroutine;
    Vector3 originalButtonScale;

    private void Awake()
    {
        button = GetComponent<Button>();

        
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((eventData) => { Animation_Expand((BaseEventData)eventData); });
        trigger.triggers.Add(entryEnter);

        EventTrigger.Entry entryEnter2 = new EventTrigger.Entry();
        entryEnter2.eventID = EventTriggerType.Select;
        entryEnter2.callback.AddListener((eventData) => { Animation_Expand((BaseEventData)eventData); });
        trigger.triggers.Add(entryEnter2);


        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((eventData) => { Animation_Shrink((BaseEventData)eventData); });
        trigger.triggers.Add(entryExit);

        EventTrigger.Entry entryExit2 = new EventTrigger.Entry();
        entryExit2.eventID = EventTriggerType.Deselect;
        entryExit2.callback.AddListener((eventData) => { Animation_Shrink((BaseEventData)eventData); });
        trigger.triggers.Add(entryExit2);
    }

    private void Start()
    {
        originalButtonScale = button.transform.localScale;
    }

    public void Animation_Expand(BaseEventData eventData)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(Expand());
    }

    public void Animation_Shrink(BaseEventData eventData)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(Shrink());
    }

    IEnumerator Expand()
    {

        Vector3 targetScale = originalButtonScale * buttonSizeMultiplier;

        float time = 0;
        while (time < animationDuration)
        {
            button.transform.localScale = Vector3.Lerp(originalButtonScale, targetScale, time / animationDuration);
            time += Time.deltaTime;
            yield return null;
        }
        button.transform.localScale = targetScale;
    }

    IEnumerator Shrink()
    {
        Vector3 originalScale = button.transform.localScale;
        Vector3 targetScale = originalButtonScale;

        float time = 0;
        while (time < animationDuration)
        {
            button.transform.localScale = Vector3.Lerp(originalScale, targetScale, time / animationDuration);
            time += Time.deltaTime;
            yield return null;
        }
        button.transform.localScale = targetScale;
    }

    public void BackToOriginalSize()
    {
        button.transform.localScale = originalButtonScale;
    }

    
}
