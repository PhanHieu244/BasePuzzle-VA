public class PlatformGameVersion
{
	public enum Platform
	{
		Android = 0,
		Ios = 1,
		Windowphone = 2,
		Blacberry = 3
	}

	public Platform PlatformType { get; set; }

	public string Version { get; set; }

	public string StoreUrl { get; set; }

	public bool ForceUpdate { get; set; }

	public string Message { get; set; }
}
