using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public delegate void EndTurnAction();

	public event EndTurnAction OnEndTurn;

	public static GameManager instance;

	private void Start()
	{
		instance = this;
	}

	public void EndTurn()
	{
		OnEndTurn?.Invoke();
	}
}
