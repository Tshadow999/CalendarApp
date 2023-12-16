using Godot;

public partial class DayEvent : Control
{
	private DateEventData _eventData;

	public DateEventData GetEvent() => _eventData;
	public void SetEvent(DateEventData dateEventData) => _eventData = dateEventData;

	public void SetColor(Color color) => SelfModulate = color;
}
