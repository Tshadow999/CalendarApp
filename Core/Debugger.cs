using Godot;

public partial class Debugger : Node
{
    public static bool IsDebugging = false;

    private static Label _debugLabel;

    public static Label GetLabel() => _debugLabel;

    public static void SetLabel(Label label)
    {
        _debugLabel = label;
    }
}
