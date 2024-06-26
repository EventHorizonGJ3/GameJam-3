using System;
using System.Collections;
using UnityEngine;

public class MeshHandler : MonoBehaviour
{
	[Header("Parameters")]
	[SerializeField] float rotationSpeed;
	[SerializeField] float scaleMult = 1.3f;
	[SerializeField] float animationDuration =1.0f;

	Vector3 startScale;

	private void Start()
	{
		startScale = transform.localScale;
	}

	private void FixedUpdate()
	{
		if (InputManager.IsMoving(out Vector3 direction))
		{
			Quaternion rotation = Quaternion.LookRotation(direction);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * rotationSpeed);
		}
	}

	private void OnEnable()
	{
		RageBar.OnBerserkExtraDmg += Embiggened;
	}
	private void OnDisable()
	{
		RageBar.OnBerserkExtraDmg -= Embiggened;
	}

    public void Embiggened(float obj)
    {
        if (obj == 0)
        {
            StopAllCoroutines();
            StartCoroutine(ScaleTo(startScale, animationDuration));
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(ScaleTo(transform.localScale * scaleMult, animationDuration));
        }
    }

     IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = transform.localScale;
        float time = 0;

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }


}








