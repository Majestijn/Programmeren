using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Allegiance
{
	Ally,
	Enemy
};

[CreateAssetMenu(fileName ="Unit", menuName ="Custom/Unit", order =1)]
public class UnitData : ScriptableObject {
	
	[SerializeField] private int m_Health;
	[SerializeField] private int m_AttackDamage;
	[SerializeField] private int m_Movespeed;

	[SerializeField] private int m_MoveAmount;

	[SerializeField] private GameObject m_Prefab;

	[SerializeField] private Allegiance m_Allegiance;

	[SerializeField] private string m_UnitName;
	[SerializeField] private Sprite m_UnitImage;

	public int health
	{
		get { return m_Health;}
	}

	public int attackDamage
	{
		get { return m_AttackDamage;}
	}

	public int moveSpeed
	{
		get { return m_Movespeed;}
	}

	public int moveAmount
	{
		get { return m_MoveAmount;}
	}

	public GameObject prefab
	{
		get { return m_Prefab;}
	}

	public Allegiance alligiance
	{
		get { return m_Allegiance;}
	}

	public Sprite avatarImage
	{
		get { return m_UnitImage;}
	}

	public string unitName
	{
		get { return m_UnitName;}
	}

}
