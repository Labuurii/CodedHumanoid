

public static class GameVersionData {
	public const string SemVerVersion = "0.1";
#if !DEBUG_BUILD_NUMBER
	public const int BuildNumber = 8;
#else
	public const int BuildNumber = 0;
#endif
}