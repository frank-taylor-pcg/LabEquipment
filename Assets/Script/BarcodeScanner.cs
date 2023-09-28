using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarcodeScanner : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}
	
	// OnTriggerEnter is called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other)
	{
		Barcode barcode = other.gameObject.GetComponent<Barcode>();
		if (barcode == null) return;
		
		print($"Scanned barcode => {barcode.Value}");
	}
}
