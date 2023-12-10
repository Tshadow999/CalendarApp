using Godot;

public partial class Application : Control
{	
	private const string QUIT_KEY = "QUIT";

	[Export] private Label DebugLabel;

	public override void _Ready()
	{
		Debugger.SetLabel(DebugLabel);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsAction(QUIT_KEY)) GetTree().Quit();
	}
}
