using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLight : MonoBehaviour
{
	public MeshRenderer WhiteLight;
	public MeshRenderer BlueLight;
	public MeshRenderer RedLight;
	public MeshRenderer AmberLight;
	public MeshRenderer GreenLight;
	
	private int cycleTime = 0;
	
	// Start is called before the first frame update
	void Start()
	{
	}
	
	void AllLightsOff()
	{
		WhiteLight.material.DisableKeyword("_EMISSION");
		BlueLight.material.DisableKeyword("_EMISSION");
		RedLight.material.DisableKeyword("_EMISSION");
		AmberLight.material.DisableKeyword("_EMISSION");
		GreenLight.material.DisableKeyword("_EMISSION");
	}
	
	// Update is called once per frame
	void Update()
	{
		if (cycleTime-- > 0) return;
		
		cycleTime = 250;
		int light = Random.Range(0, 5);
		
		AllLightsOff();
		
		switch (light)
		{
			case 0: WhiteLight.material.EnableKeyword("_EMISSION"); break;
			case 1: BlueLight.material.EnableKeyword("_EMISSION"); break;
			case 2: RedLight.material.EnableKeyword("_EMISSION"); break;
			case 3: AmberLight.material.EnableKeyword("_EMISSION"); break;
			case 4: GreenLight.material.EnableKeyword("_EMISSION"); break;
			default: AllLightsOff(); break;
		}
	}
}
