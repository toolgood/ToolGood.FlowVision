using ToolGood.FlowVision.Datas.Projects;
using ToolGood.ReadyGo3;

namespace ToolGood.DataCreate
{
    class ProjectDatas
    {
        public static void CreateData(SqlHelper helper)
        {
            var db = new DbProject() {
                CreateTime = DateTime.Now,
                ModifyTime = DateTime.Now,
                Name = "第一个项目",
                MainMemberId = 1,
                Code = "02048942-EACB-4B78-BCAE-0F39FDC12AE2",
                ExcelIndex = 0,
                NumberRequired = 0,
                Distance = 4,
                Area = 14,
                Volume = 24,
                Mass = 32,
                CreateUserId = 1,
                ModifyUserId = 1,
                Comment="",
            };

            helper.Insert(db);
            // 增加默认厂区
            DbFactory factory = new DbFactory() {
                Code = "default",
                Comment = "",
                Category = "默认",
                SimplifyName = "默认",
                MainMemberId = db.MainMemberId,
                ProjectId = db.Id,
                CreateTime = DateTime.Now,
                CreateUserId = db.CreateUserId,
            };
            helper.Insert_Async(factory);

            //var txt = File.ReadAllText(@"File\空白箱.txt");
            //helper.Insert(new DbAppFlow() {
            //	CreateTime = DateTime.Now,
            //	ProjectId = 1,
            //	CreateUserId = 1,
            //	MainMemberId = 1,
            //	AppCode = "BlankBox",
            //	FlowString = txt
            //}); ;
        }

    }
}
