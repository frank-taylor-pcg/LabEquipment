using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;


// TODO: This is tainted by Bill's thinking.  I need to start simple.  An axis has a range of motion. I should define how an axis moves first.
public class TrayElevator : MonoBehaviour
{

	public float UpperLimit = 5f;
	public float Speed = 1f;
	public float VerticalTraySpacing = 0.5f;
	public float BottomTrayPosition = 1f;
	public int ScanDelayInMilliseconds = 250;
	public float YAxisHomePosition = 0f;
	public float ZAxisHomePosition = -2.5f;
	public float ZAxisBarcodeScanPosition = 0f;
	
	private int nextTrayIndex = 0;
	private float currentWaitTime = 0;
	private OperationState State =	OperationState.Idle;
	
	// Update is called once per frame
	void Update()
	{
		switch (State)
		{
			case OperationState.Initializing:
			{
				HomeTrayElevator();
				break;
			}
			case OperationState.Running:
			{
				ScanTrayBarcodes();
				break;
			}
			case OperationState.Idle:
			default:
			{
				// Just wait around doing nothing
				break;
			}
		}
	}
	
	private void MoveAxis(AxisId id, Vector3 position)
	{
		
	}
	
	private void HomeTrayElevator()
	{
		bool isHomed = true;
		if (transform.position.y > YAxisHomePosition)
		{
			isHomed = false;
			transform.position -= new Vector3(0, Speed * Time.deltaTime, 0);
		}
		
		if (transform.position.z > ZAxisHomePosition)
		{
			isHomed = false;
			transform.position -= new Vector3(0, 0, Speed * Time.deltaTime);
		}
		
		if (isHomed)
		{
			print("Tray Elevator Homed successfully");
			State = OperationState.Idle;
		}
	}
	
	private void ScanTrayBarcodes()
	{
		print("Moving tray elevator z-axis to barcode scan offset");
	}
	
	private int GetCurrentTrayPositionIndex()
	{		
		// Determine current tray index
		return (int)Mathf.Floor(2 * (transform.position.y - BottomTrayPosition));
	}
	
	private void MoveToNextTray()
	{
	}
}

