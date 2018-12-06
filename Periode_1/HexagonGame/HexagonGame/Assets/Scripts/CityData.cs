using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "City", menuName = "Custom/City", order = 0)]
public class CityData : ScriptableObject {

	[SerializeField] private string m_Name;

	public string cityName
	{
		get { return m_Name; }
	}
}
