using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour {

	private string m_CityName;
	
	public Hex m_Hex;

	void Start () {
		m_Hex = GetComponent<Hex>();
	}
	
	void Update () {
		
	}

	public void SetCityName(string name)
	{
		m_CityName = name;
	}

	public string GetCityName()
	{
		return m_CityName;
	}
}
