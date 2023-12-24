using Godot;

public partial class Application : PanelContainer
{	
	private const string QUIT_KEY = "QUIT";

	[Export] private Label DebugLabel;
	
	[Export] private CarouselContainer Carousel;
	[Export] private CalendarMonthContent Content;
	
	[Export] private EditPopup PopupContainer;
	
	[Export] private float SwipeThreshold;
	
	private Vector2 _swipeStart;

	public override void _Ready()
	{
		Debugger.SetLabel(DebugLabel);
		ToggleEditPopup(false, DateEventData.Empty);
	}
	
	public override void _Input(InputEvent @event)
	{
		// Quick Exit if Esc is pressed;
		if (@event.IsAction(QUIT_KEY)) GetTree().Quit();
		
		// Listen for a screen swipe
		if (@event is InputEventScreenTouch { Pressed: true } eventScreenTouch)
		{
			_swipeStart = eventScreenTouch.Position;
		}
		else if (@event is InputEventScreenTouch { Pressed: false } eventScreenTouchRelease)
		{
			Vector2 swipeEnd = eventScreenTouchRelease.Position;
			float swipeDistance = _swipeStart.DistanceTo(swipeEnd);

			if (swipeDistance > SwipeThreshold)
			{
				Carousel.HandleButtonPress(Mathf.Sign(_swipeStart.X - swipeEnd.X));
				Content.HandleSwipe();
			}
		}
	}

	public void ToggleEditPopup(bool value, DateEventData data)
	{
		if (value) PopupContainer.Initialize(data);
		
		PopupContainer.Visible = value;
	}
}
