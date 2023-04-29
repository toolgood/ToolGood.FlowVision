using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolGood.Datas;
using ToolGood.FlowVision.Commons.Utils;
using ToolGood.FlowVision.Datas;
using ToolGood.ReadyGo3;

namespace ToolGood.DataCreate
{
	class MemberDatas
	{
		public static void CreateData(SqlHelper helper)
		{
			#region 角色
			helper.Insert(new DbSysMemberGroup() {
				Name = "主账号",
				Comment = "",
				OrderNum = 1,
				IsSystem = true
			});
			helper.Insert(new DbSysMemberGroup() {
				Name = "开发者",
				Comment = "指定项目可编辑，生成导出",
				OrderNum = 2,
				IsSystem = true
			});
			helper.Insert(new DbSysMemberGroup() {
				Name = "只读子账号",
				Comment = "指定项目可读取，不能导出",
				OrderNum = 3,
				IsSystem = true
			});
			#endregion

			#region 账号
			var salt = Guid.NewGuid().ToString().Replace("-", "");
			helper.Insert(new DbSysMember() {
				Name = "admin",
				Salt = salt,
				Password = DataUtil.CreatePassword("admin", salt, "admin"),
				Phone = "13958600000",
				Email = "toolgood@qq.com",
				TrueName = "管理员",

				CreateTime = DateTime.Now,
				GroupId = 1,
				MainMemberId = 1,
			});

			helper.Insert(new DbSysMember() {
				Name = "developer",
				Salt = salt,
				Password = DataUtil.CreatePassword("developer", salt, "developer"),
				Phone = "13958600000",
				Email = "toolgood@qq.com",
				TrueName = "开发员",
				ProjectIds = "1",

				CreateTime = DateTime.Now,
				GroupId = 2,
				MainMemberId = 1,
			});


			helper.Insert(new DbSysMember() {
				Name = "reader",
				Salt = salt,
				Password = DataUtil.CreatePassword("reader", salt, "reader"),
				Phone = "13958600000",
				Email = "toolgood@qq.com",
				TrueName = "只读账号",
				ProjectIds = "1",
				CreateTime = DateTime.Now,
				GroupId = 3,
				MainMemberId = 1,
			});

			#endregion

			#region AdminMenuPass
			{
				var menus = helper.Select<DbSysMemberMenu>();
				var btns = helper.Select<DbSysMemberMenuButton>();

				foreach (var item in menus) {
					if (string.IsNullOrEmpty(item.MenuCode)) { continue; }
					var acs = item.Buttons.Split('|');
					foreach (var ac in acs) {
						helper.Insert(new DbSysMemberGroup_Menu() {
							GroupId = 1,
							MenuId = item.Id,
							MenuCode = item.MenuCode,
							ButtonCode = ac,
							ButtonId = btns.Where(q => q.ButtonCode == ac.ToLower()).First().Id
						});
					}
				}
			}

			{
				var menuid = helper.FirstOrDefault<DbSysMemberMenu>("where MenuCode='ProjectTopDesktop'");
				var menus = helper.Select<DbSysMemberMenu>("where Id>=@0", menuid.Id);
				var btns = helper.Select<DbSysMemberMenuButton>();

				foreach (var item in menus) {
					if (string.IsNullOrEmpty(item.MenuCode)) { continue; }
					var acs = item.Buttons.Split('|');
					foreach (var ac in acs) {
						helper.Insert(new DbSysMemberGroup_Menu() {
							GroupId = 2,
							MenuId = item.Id,
							MenuCode = item.MenuCode,
							ButtonCode = ac,
							ButtonId = btns.Where(q => q.ButtonCode == ac.ToLower()).First().Id
						});
					}
				}
			}

			{
				var menuid = helper.FirstOrDefault<DbSysMemberMenu>("where MenuCode='ProjectTopDesktop'");
				var menus = helper.Select<DbSysMemberMenu>("where Id>=@0", menuid.Id);
				var btns = helper.FirstOrDefault<DbSysMemberMenuButton>("where ButtonCode='show'");
				var btns2 = helper.FirstOrDefault<DbSysMemberMenuButton>("where ButtonCode='navbar'");

				foreach (var item in menus) {
					if (string.IsNullOrEmpty(item.MenuCode)) { continue; }
					if (item.Buttons.Contains("navbar")) {
						helper.Insert(new DbSysMemberGroup_Menu() {
							GroupId = 3,
							MenuId = item.Id,
							MenuCode = item.MenuCode,
							ButtonCode = btns2.ButtonCode,
							ButtonId = btns2.Id
						});
					}
					helper.Insert(new DbSysMemberGroup_Menu() {
						GroupId = 3,
						MenuId = item.Id,
						MenuCode = item.MenuCode,
						ButtonCode = btns.ButtonCode,
						ButtonId = btns.Id
					});
				}
			}

			#endregion

			{
				helper.Insert(new DbSysSettingValue() {
					Code = "time",
					Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
					CategoryName = "系统",
					SettingName = "时间",
					CreateTime = DateTime.Now,
					ModifyTime = DateTime.Now,
					Comment = "时间"
				});
				helper.Insert(new DbSysSettingValue() {
					CategoryName = "账号",
					Code = "LoginPassword",
					SettingName = "默认登录密码",
					Value = "a123456",
					Comment = "密码长度必须大于3位，小于20位",
					CreateTime = DateTime.Now,
					ModifyTime = DateTime.Now,
				});
				helper.Insert(new DbSysSettingValue() {
					CategoryName = "账号",
					Code = "ManagerPassword",
					SettingName = "默认管理密码",
					Value = "a123456789",
					Comment = "密码长度必须大于3位，小于20位",
					CreateTime = DateTime.Now,
					ModifyTime = DateTime.Now,
				});
				helper.Insert(new DbSysSettingValue() {
					CategoryName = "账号",
					Code = "MemberLoginPassword",
					SettingName = "默认会员强制密码",
					Value = "abc888888",
					Comment = "密码长度必须大于3位，小于20位",
					CreateTime = DateTime.Now,
					ModifyTime = DateTime.Now,
				});

				helper.Insert(new DbSysSettingValue() {
					CategoryName = "账号",
					Code = "CookieTimes",
					SettingName = "Cookie保存时间",
					Value = "10800",
					Comment = "数字，单位：秒,最小10分钟，默认180分钟",
					CreateTime = DateTime.Now,
					ModifyTime = DateTime.Now,
				});
			}

		}
	}
}
