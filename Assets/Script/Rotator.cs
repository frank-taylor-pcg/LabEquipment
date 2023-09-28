using UnityEngine;

public class Rotator : MonoBehaviour
{
	[SerializeField] float RotationSpeed = 5f;

  void Update()
	{
		transform.Rotate(new Vector3(RotationSpeed * Time.deltaTime, 0, 0), Space.World);
  }
}
