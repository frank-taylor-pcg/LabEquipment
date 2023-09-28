using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageMonitor : MonoBehaviour
{
	[SerializeField] Axis[] AxesToMonitor;
	[SerializeField] Image[] MovementIndicators;
	[SerializeField] Material MovingMaterial;
	[SerializeField] Material StoppedMaterial;
	
	// Start is called before the first frame update
	void Start()
	{
		if (AxesToMonitor.Length != MovementIndicators.Length)
			print("The number of axes being monitored should match the number of movement indicators specified");
	}
	
	// Update is called once per frame
	void Update()
	{
		for (int i = 0; i < AxesToMonitor.Length; i++)
		{
			if (AxesToMonitor[i].IsMoving())
			{
				MovementIndicators[i].material = MovingMaterial;
			}
			else
			{
				MovementIndicators[i].material = StoppedMaterial;
			}
		}
	}
}
