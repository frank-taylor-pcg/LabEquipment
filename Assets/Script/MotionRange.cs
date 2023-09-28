 public class MotionRange
{
	public float Start { get; set; }
	public float End { get; set; }
	private float _range;
	
	public MotionRange(float start, float end)
	{
		Start = start;
		End = end;
		_range = End - Start;
	}
	
	public float ParametricOffset(int offset)
	{
		return (offset - Start) / _range;
	}
	
	public float PositionFromParameter(float parameter)
	{
		return Start + _range * parameter;
	}
}
