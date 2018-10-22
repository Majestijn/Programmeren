using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {

	public static UIManager instance;

	[SerializeField] private TextMeshProUGUI m_NameText, m_MoveText;
	[SerializeField] private Image m_AvatarImage, m_HealthImage;
	[SerializeField] private GameObject m_InfoObject;

	void Start () {
		instance = this;
		SelectionManager.instance.OnUnitSelected += UpdateUI;
		SelectionManager.instance.OnUnitDeselected += HideInfoObject;
	}

	public void UpdateUI(Unit unit)
	{
		if (!m_InfoObject.activeSelf)
		{
			m_InfoObject.SetActive(true);
		}

		m_NameText.text = unit.unitData.unitName;
		m_MoveText.text = unit.m_MoveAmount.ToString();
		m_AvatarImage.sprite = unit.unitData.avatarImage;

		m_HealthImage.fillAmount = ((float)unit.healthScript.GetHealth() / (float)unit.unitData.health);
	}

	private void HideInfoObject()
	{
		m_InfoObject.SetActive(false);
	}

	private void OnDestroy()
	{
		SelectionManager.instance.OnUnitSelected -= UpdateUI;
	}
}
