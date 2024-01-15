using Godot;

public partial class DayEvent : Control
{
	[Export] private int CornerRadius = 5;

	[Export] private Panel LeftRect;
	[Export] private Panel RightRect;
	
	private StyleBoxFlat _boxFlatLeft;
	private StyleBoxFlat _boxFlatRight;
	
	private DateEventData _eventData;

	public override void _Ready()
	{
		_boxFlatLeft = new StyleBoxFlat
		{
			CornerRadiusBottomLeft = CornerRadius,
			CornerRadiusTopLeft = CornerRadius,
		};
		
		_boxFlatRight = new StyleBoxFlat
		{
			CornerRadiusTopRight = CornerRadius,
			CornerRadiusBottomRight = CornerRadius,
		};
	}

	public DateEventData GetEvent() => _eventData;
	public void SetEvent(DateEventData dateEventData) => _eventData = dateEventData;
	
	public void SetColor(Color color, bool left)
	{
		if (left)
		{
			SetColorLeft(color);
		}
		else
		{
			SetColorRight(color);
		}
	}
	
	public void SetColor(Color colorLeft, Color colorRight)
	{
		SetColorLeft(colorLeft);
		SetColorRight(colorRight);
	}
	
	public void SetColor(Color colorBoth)
	{
		SetColorLeft(colorBoth);
		SetColorRight(colorBoth);
	}
	
	private void SetColorLeft(Color c)
	{
		_boxFlatLeft.BgColor = c;
		LeftRect.AddThemeStyleboxOverride("panel", _boxFlatLeft);
	}
	
	private void SetColorRight(Color c)
	{
		_boxFlatRight.BgColor = c;
		RightRect.AddThemeStyleboxOverride("panel", _boxFlatRight);
	}
}
