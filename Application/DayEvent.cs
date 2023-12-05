using Godot;

public partial class DayEvent : Control
{
	[Export] private ColorRect colorRect;
	private DateEvent _event;

	public DateEvent GetEvent() => _event;
	public void SetEvent(DateEvent dateEvent) => _event = dateEvent;
	public void SetColor(Color color) => colorRect.Color = color;
	
}
