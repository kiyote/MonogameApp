namespace App.DesktopGL;

public sealed class DisplayOptions {
	public const string Section = "Display";

	public int Width { get; set; } = 1280;

	public int Height { get; set; } = 720;

	public bool VSync { get; set; } = true;
}
