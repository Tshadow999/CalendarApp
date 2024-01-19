using Godot;
using System;

public partial class TimeSelector : HBoxContainer
{
	[Export] private SpinBox HourSpinBox;
	[Export] private SpinBox MinSpinBox;

	private EditPopup _editPopup;
	private bool _startTime;

	public void Init(EditPopup popup, bool startTime)
	{
		_editPopup = popup;
		_startTime = startTime;
		HourSpinBox.Value = DateTime.Now.Hour;
		Visible = true;
	}

	private void OnHourSpinBoxValueChanged_Signal(float value)
	{
		if (value > HourSpinBox.MaxValue) HourSpinBox.Value = HourSpinBox.MinValue;
		if (value < HourSpinBox.MinValue) HourSpinBox.Value = HourSpinBox.MaxValue;
		
		_editPopup.SetTime(new Vector2I((int)HourSpinBox.Value, (int)MinSpinBox.Value), _startTime);
	}
	
	private void OnMinSpinBoxValueChanged_Signal(float value)
	{
		if (value > MinSpinBox.MaxValue) MinSpinBox.Value = MinSpinBox.MinValue;
		if (value < MinSpinBox.MinValue) MinSpinBox.Value = MinSpinBox.MaxValue;
		_editPopup.SetTime(new Vector2I((int)HourSpinBox.Value, (int)MinSpinBox.Value), _startTime);
	}
}
