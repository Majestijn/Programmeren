using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour {

	[SerializeField] private CanvasGroup m_Text;

	private void OnEnable()
	{
		m_Text.alpha = 0;
		StartCoroutine(Fade());
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private IEnumerator Fade()
	{
		while (m_Text.alpha < 1)
		{
			m_Text.alpha = m_Text.alpha += Time.deltaTime;
			yield return null;
		}

		yield return new WaitForSeconds(3f);

		gameObject.SetActive(false);
		yield return null;
	}
}
