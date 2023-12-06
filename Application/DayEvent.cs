using Godot;

public partial class DayEvent : Control
{
	[Export] private ColorRect colorRect;
	private DateEventData _eventData;

	public DateEventData GetEvent() => _eventData;
	public void SetEvent(DateEventData dateEventData) => _eventData = dateEventData;
	public void SetColor(Color color) => colorRect.Color = color;
	
}
