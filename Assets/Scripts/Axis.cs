using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Axis : MonoBehaviour
{
	// Default values are something simple.  The MotionRange maps the values to use for (PositionA, PositionB) as scalars.
	// For instance, moving to position 0.0 with the defaults should move to PositionA.  Moving to position 1.0 should move
	// to PositionB.  The MotionRange isn't restricted to [0, 1] and can be any range as long as the two values are not equal.

	public AxisId Id = AxisId.None;
	
	public Transform[] TeachPoints;
	private List<float> teachPointPositions = new List<float>();
	
	private Vector3 PositionA = new Vector3(0f, 0f, 0f);
	private Vector3 PositionB = new Vector3(1f, 0f, 0f);
	
	public Vector2 MotionRange = new Vector2(0f, 1f);
	
	// When the stage is close to the target position
	public float CoarseTolerance = 0.05f;
	
	// When the stage is close enough to the target position to stop
	public float FineTolerance = 0.01f;
	
	// These are values between 0f and 1f
	private float currentPosition = 0.5f;
	private float targetPosition = 0f;
	// The speed of movement, with an implicit direction based on whether this is positive or negative
	private float velocity = 0f;
	
	// This is a percentage of the range moved
	public float Speed = .01f;
	
	private bool isMoving = false;
	
	public bool IsMoving() => isMoving;
	
	public Transform Mover;
	
  // Start is called before the first frame update
  void Start()
	{
		if (Id.Equals(AxisId.None))
		{
			throw new System.InvalidOperationException($"An axis cannot be created of type 'None'. Make sure all axes have been assigned a type Id");
		}
		
		if (MotionRange.x == MotionRange.y)
		{
			throw new	System.InvalidOperationException($"Axis {Id} initialized with invalid range of motion");
		}
		
		// TODO: Should I make the min and max extents separate?  That might make more sense and be less fragile.
		if (TeachPoints.Length < 2)
		{
			throw new System.InvalidOperationException($"Axis {Id} the minimum and maximum movement positions must be the first and last elements of the TeachPoints array");
		}
		
		// Initialize positions A and B to the start and end of the movement array
		PositionA = TeachPoints[0].localPosition;
		PositionB = TeachPoints[^1].localPosition;
		
		float fullRange = Vector3.Distance(PositionA, PositionB);
		foreach (Transform t in TeachPoints)
		{
			float distance = Vector3.Distance(PositionA, t.localPosition);
			teachPointPositions.Add(distance / fullRange);
		}
	}
  
	bool MoveCompleted()
	{
		return Mathf.Abs(currentPosition - targetPosition) < FineTolerance;
	}

  // Update is called once per frame
  void Update()
  {
	  if (isMoving)
	  {
	  	if (MoveCompleted())
	  	{
	  		isMoving = false;
	  		velocity = 0f;
	  		return;
	  	}
	  	
	  	currentPosition += velocity * Time.deltaTime;
	  	Mover.localPosition = Vector3.Lerp(PositionA, PositionB, currentPosition);
	  }
  }
  
	void StartMovement(float target)
	{
		targetPosition = target;
		
		// If the target position is less than the current position, we need to subtract our speed, otherwise we need to add it
		velocity = (targetPosition < currentPosition) ? -Speed : Speed;
		isMoving = true;
	}
  
	public void MoveTo(float position)
	{
		if (position < MotionRange.x) print($"Axis [{Id}] ordered to move beyond it's lower limit. Movement will stop when lower limit is reached.");
		if (position > MotionRange.y) print($"Axis [{Id}] ordered to move beyond it's upper limit. Movement will stop when lower limit is reached.");
		
		StartMovement(position);
	}
	
	public void MoveToTeachPoint(int index)
	{
		if (isMoving)
		{
			print("Axis commanded to move before previous move was completed");
			return;
		}
		if (index < 0 || index >= teachPointPositions.Count)
		{
			print($"Axis {Id} order to move to invalid teach point with index {index}");
			return;
		}
		
		StartMovement(teachPointPositions[index]);
	}
}
