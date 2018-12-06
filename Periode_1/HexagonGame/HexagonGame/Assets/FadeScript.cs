using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeScript : MonoBehaviour {

	private TextMeshProUGUI m_Text;

	private void Start()
	{
		m_Text = GetComponent<TextMeshProUGUI>();
	}

	private void OnEnable()
	{
		m_Text.color = new Color(1, 1, 1, 0);
		StartCoroutine(Fade());
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private IEnumerator Fade()
	{
		while (m_Text.color.a < 1)
		{
			m_Text.color = new Color(1, 1, 1, m_Text.color.a + Time.deltaTime);
			yield return null;
		}

		gameObject.SetActive(false);
		yield return null;
	}
}
