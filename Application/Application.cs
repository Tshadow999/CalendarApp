using Godot;

public partial class Application : Control
{	
	private const string QUIT_KEY = "QUIT";

	public override void _Input(InputEvent @event)
	{
		if (@event.IsAction(QUIT_KEY)) GetTree().Quit();
	}
}
