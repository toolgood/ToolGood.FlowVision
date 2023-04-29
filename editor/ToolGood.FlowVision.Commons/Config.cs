using Microsoft.Extensions.Configuration;
using ToolGood.ReadyGo3;

namespace ToolGood.FlowVision.Commons
{
    public static class Config
    {
        public static IConfiguration Configuration;

        [ThreadStatic]
        private static SqlHelper _dbHelper;
        [ThreadStatic]
        private static SqlHelper _readHelper;


        public static SqlHelper WriteHelper {
            get
            {
                if (_dbHelper == null || _dbHelper._IsDisposed) {
                    lock (typeof(Config)) {
                        if (_dbHelper == null || _dbHelper._IsDisposed) {
							var connectionString = "Data Source=App_Data/data.sav;Mode=ReadWrite;Password=77EF7DA7-FD74-4F58-BD39-1B6953D8487B";
							var helper = SqlHelperFactory.OpenDatabase(connectionString, SqlType.SQLite);
//#if Sqlite
//                            var connectionString = "Data Source=App_Data/data.sav;Mode=ReadWrite;Password=77EF7DA7-FD74-4F58-BD39-1B6953D8487B";
//							var helper = SqlHelperFactory.OpenDatabase(connectionString, SqlType.SQLite);
//#else
//							var connStr = Configuration.GetValue<string>(DatabaseSetting.WriterConnectionString);
//                            var providerName = Configuration.GetValue<string>(DatabaseSetting.WriterProviderName);
//                            var helper = SqlHelperFactory.OpenDatabase(connStr, providerName);
//#endif
                            _dbHelper = helper;
                        }
                    }
                }
                return _dbHelper;
            }
        }

        public static SqlHelper ReadHelper {
            get
            {
                if (_readHelper == null || _readHelper._IsDisposed) {
                    lock (typeof(Config)) {
                        if (_readHelper == null || _readHelper._IsDisposed) {
							var connectionString = "Data Source=App_Data/data.sav;Mode=ReadWrite;Password=77EF7DA7-FD74-4F58-BD39-1B6953D8487B";
							var helper = SqlHelperFactory.OpenDatabase(connectionString, SqlType.SQLite);
//#if Sqlite
//                            var connectionString = "Data Source=App_Data/data.sav;Mode=ReadWrite;Password=77EF7DA7-FD74-4F58-BD39-1B6953D8487B";
//                            var helper = SqlHelperFactory.OpenDatabase(connectionString, SqlType.SQLite);
//#else
//							var connStr = Configuration.GetValue<string>(DatabaseSetting.ReaderConnectionString);
//                            var providerName = Configuration.GetValue<string>(DatabaseSetting.ReaderProviderName);
//                            var helper = SqlHelperFactory.OpenDatabase(connStr, providerName);
//#endif
                            _readHelper = helper;
                        }
                    }
                }
                return _readHelper;
            }
        }

   

        public static void Dispose()
        {
            if (_dbHelper != null) {
                _dbHelper.Dispose();
                _dbHelper = null;
            }
            if (_readHelper != null) {
                _readHelper.Dispose();
                _readHelper = null;
            }
        }
    }
}