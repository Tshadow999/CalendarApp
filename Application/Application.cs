using System.IO;
using System.Threading.Tasks;
using Godot;
using HttpClient = System.Net.Http.HttpClient;

public partial class Application : Control
{	
	private const string QUIT_KEY = "QUIT";

	private const string utwenteRoosterICalLink =
		"https://rooster.utwente.nl/ical?6569c5eb&group=true&eu=czI1NTYxNTQ=&h=V5QsZ8N6rV__HKkSVTgWhYTi7Caox_owB_BHwVSbRCM=";

	public override void _Ready()
	{
		// cannot make this call await 
		ReadRoosterFileFromURI();
	}

	private async Task ReadRoosterFileFromURI()
	{
		using HttpClient client = new HttpClient();
		await using Stream webStream = await client.GetStreamAsync(utwenteRoosterICalLink);
		await using FileStream fileStream = new FileStream(ProjectSettings.GlobalizePath("res://ICS Files/utwenteRooster.ics"), FileMode.OpenOrCreate);
		await webStream.CopyToAsync(fileStream);
		
		fileStream.Close();
		webStream.Close();
		client.Dispose();
		
		ICalFileReader node = GetNode<ICalFileReader>("/root/IcalFileReader");
		node.ReadICalFiles();
	}


	public override void _Input(InputEvent @event)
	{
		if (@event.IsAction(QUIT_KEY)) GetTree().Quit();
	}
	
	
}
