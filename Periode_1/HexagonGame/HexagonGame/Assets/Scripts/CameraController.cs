using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	#region Variables
	[SerializeField] private Transform m_CameraParent, m_PivotParent, m_LowPivot, m_HighPivot;

	[SerializeField] private float m_Scrollspeed, m_Panspeed;

	private Vector3 m_CameraTargetPosition;
	private Vector3 m_CameraTargetRotation;

	private float m_Percentage;

	private bool test = false;
	#endregion
	
	void Update () {

		UpdateScrollPercentage();

		m_CameraTargetPosition = GetPointBetweenTwoVectors(m_LowPivot.position, m_HighPivot.position);
		m_CameraTargetRotation = GetPointBetweenTwoVectors(m_LowPivot.rotation.eulerAngles, m_HighPivot.rotation.eulerAngles);
	}

	private void LateUpdate()
	{
		m_CameraParent.position = Vector3.Lerp(m_CameraParent.position, m_CameraTargetPosition, m_Scrollspeed * Time.deltaTime);
		m_CameraParent.rotation = Quaternion.Euler(Vector3.Lerp(m_CameraParent.rotation.eulerAngles, m_CameraTargetRotation, m_Scrollspeed * Time.deltaTime));

		if (Input.GetMouseButton(2))
		{
			Vector3 direction = new Vector3(-Input.GetAxis("Mouse X"), 0, -Input.GetAxis("Mouse Y")) * m_Panspeed * Time.deltaTime;
			m_CameraParent.Translate(direction, Space.World);
			m_PivotParent.Translate(direction, Space.World);
		}
	}

	private Vector3 GetPointBetweenTwoVectors(Vector3 vectorA, Vector3 vectorB)
	{
		float distance = Vector3.Distance(vectorA, vectorB);

		distance = distance * m_Percentage;

		Vector3 difference = vectorB - vectorA;

		difference = difference.normalized * distance;

		return vectorA + difference;
	}

	private void UpdateScrollPercentage()
	{
		float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

		m_Percentage -= scrollDelta;

		m_Percentage = Mathf.Clamp(m_Percentage, 0, 1);
	}

	public void ChangeCameraParent(Vector3 pos)
	{
		Vector3 tempPos = new Vector3(pos.x, m_CameraParent.position.y, pos.z);

		m_CameraTargetPosition = tempPos;
	}
}
