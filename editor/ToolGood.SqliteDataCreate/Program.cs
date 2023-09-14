using ToolGood.DataCreate;
using ToolGood.ReadyGo3;

namespace ToolGood.SqliteDataCreate
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var path = Path.GetFullPath("data.sav");
			if (File.Exists(path)) {
				File.Delete(path);
			}
            //var connectionString = "Data Source=data.sav;Mode=ReadWriteCreate;";
            var connectionString = "Data Source=data.sav;Mode=ReadWriteCreate;Password=77EF7DA7-FD74-4F58-BD39-1B6953D8487B";
            var srcHelper = SqlHelperFactory.OpenDatabase(connectionString, SqlType.SQLite);

			MemberTypes.CreateTable(srcHelper);
			MemberMenus.CreateMenu(srcHelper);
			MemberDatas.CreateData(srcHelper);

			ProjectTypes.CreateTable(srcHelper);
			ProjectMenus.CreateMenu(srcHelper);
			ProjectDatas.CreateData(srcHelper);

		}
	}
}