using Godot;

public partial class PopupManager : Control
{
	private DateEventData _dataToDisplay;
	
	public override void _Ready()
	{
		GetTree().Root.CallDeferred(MethodName.MoveChild,this, -1);
		Visible = true;
	}

	public void Popup(DateEventData dateEventData)
	{
		_dataToDisplay = dateEventData;
	}
}
