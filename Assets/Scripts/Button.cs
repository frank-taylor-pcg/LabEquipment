using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
	public MeshRenderer Light;
	
	int counter = 0;
	bool lightState = false;
	public int FlashRate = 250;
	
	// Start is called before the first frame update
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
		if (counter-- > 0) return;
		counter = FlashRate;
		SetLight(lightState);
		lightState = !lightState;
	}
	
	void SetLight(bool on)
	{
		if (on)
		{	
			Light.material.EnableKeyword("_EMISSION");
			return;
		}
		Light.material.DisableKeyword("_EMISSION");
	}
}
