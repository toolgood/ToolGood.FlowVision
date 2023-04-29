namespace ToolGood.FlowVision.Commons
{
#if !Sqlite

	public static class DatabaseSetting
	{
		public const string WriterConnectionString = "configSections:Writer:connectionString";

		public const string WriterProviderName = "configSections:Writer:providerName";

		public const string ReaderConnectionString = "configSections:Reader:connectionString";

		public const string ReaderProviderName = "configSections:Reader:providerName";
	}

#endif
}