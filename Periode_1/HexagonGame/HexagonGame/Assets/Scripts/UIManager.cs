using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {

	public static UIManager instance;

	[SerializeField] private TextMeshProUGUI m_NameText, m_MoveText, m_CitynameText;
	[SerializeField] private Image m_AvatarImage, m_HealthImage;
	[SerializeField] private GameObject m_InfoObject, m_CityInfoObject, m_MoreSpaceObject;

	void Start () {
		instance = this;
		SelectionManager.instance.OnUnitSelected += UpdateUI;
		SelectionManager.instance.OnUnitDeselected += HideInfoObject;

		SelectionManager.instance.OnCitySelected += UpdateCityUI;
		SelectionManager.instance.OnCityDeselected += HideCityInfoObject;
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
	
	public void UpdateCityUI(City city)
	{
		if (!m_CityInfoObject.activeSelf)
			m_CityInfoObject.SetActive(true);

		m_CitynameText.text = city.GetCityName();
	}

	public void HideCityInfoObject()
	{
		m_CityInfoObject.SetActive(false);
	}

	private void HideInfoObject()
	{
		m_InfoObject.SetActive(false);
	}

	public void ToggleMoreSpaceObject()
	{
		m_MoreSpaceObject.SetActive(true);
	}

	private void OnDestroy()
	{
		SelectionManager.instance.OnUnitSelected -= UpdateUI;
		SelectionManager.instance.OnUnitDeselected -= HideInfoObject;

		SelectionManager.instance.OnCitySelected -= UpdateCityUI;
		SelectionManager.instance.OnCityDeselected -= HideCityInfoObject;
	}
}
