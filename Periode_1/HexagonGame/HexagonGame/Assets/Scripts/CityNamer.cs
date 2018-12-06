using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CityNamer : MonoBehaviour {

	public static CityNamer instance;

	[SerializeField] private GameObject m_InputObject;
	[SerializeField] private TMP_InputField m_InputField;

	private City m_City;

	private void Start()
	{
		instance = this;
	}

	public void ChangeCity(City city)
	{
		m_City = city;
	}

	public void SetCityName()
	{
		m_City.SetCityName(m_InputField.text);
		m_City = null;
		m_InputObject.SetActive(false);
	}

	public void SetInputObject(bool state)
	{
		m_InputObject.SetActive(state);
	}
}
