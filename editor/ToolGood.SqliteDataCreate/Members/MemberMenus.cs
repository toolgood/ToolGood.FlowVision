using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolGood.Datas;
using ToolGood.ReadyGo3;

namespace ToolGood.DataCreate
{
    class MemberMenus
    {
        public static void CreateMenu(SqlHelper helper)
        {
            var menuIndex = 1;
            var MemberDesktop = new DbSysMemberMenu() { MenuName = "基础设置", MenuCode = "MemberTopDesktop", Url = "", Buttons = "navbar|show", ParentId = 0, OrderNum = (menuIndex++), };
            helper.Insert(MemberDesktop);
            {
                var topdesktop = MemberDesktop;
                var desktop = new DbSysMemberMenu() { MenuName = "项目列表", MenuCode = "MemberDesktop", Buttons = "navbar|show", ParentId = topdesktop.Id, OrderNum = (menuIndex++), };
                helper.Insert(desktop);
                {
                    helper.Insert(new DbSysMemberMenu() { MenuName = "项目列表", MenuCode = "FlowProject", Url = "/FlowProject/Projects/Index", Buttons = "navbar|show|edit|delete|import", ParentId = desktop.Id, OrderNum = (menuIndex++), });
                    helper.Insert(new DbSysMemberMenu() { MenuName = "操作日志", MenuCode = "FlowProjectLog", Url = "/FlowProject/Projects/OperationList/Index", Buttons = "navbar|show|edit", ParentId = desktop.Id.ToSafeInt32(0), OrderNum = (menuIndex++), });
                    helper.Insert(new DbSysMemberMenu() { MenuName = "生成日志", MenuCode = "FlowProjectFile", Url = "/FlowProject/Projects/Files/Index", Buttons = "navbar|show|edit", ParentId = desktop.Id.ToSafeInt32(0), OrderNum = (menuIndex++), });
                }
                desktop = new DbSysMemberMenu() { MenuName = "子账号管理", MenuCode = "SubAccountDesktop", Buttons = "navbar|show", ParentId = topdesktop.Id, OrderNum = (menuIndex++), };
                helper.Insert(desktop);
                {
                    helper.Insert(new DbSysMemberMenu() { MenuName = "子账号列表", MenuCode = "SubAccount", Url = "/Members/SubAccounts/Index", Buttons = "navbar|show|edit|delete", ParentId = desktop.Id.ToSafeInt32(0), OrderNum = (menuIndex++), });
                    helper.Insert(new DbSysMemberMenu() { MenuName = "登录日志", MenuCode = "SubAccountLoginList", Url = "/Members/SubAccounts/LoginList", Buttons = "navbar|show|edit", ParentId = desktop.Id.ToSafeInt32(0), OrderNum = (menuIndex++), });
                }
                desktop = new DbSysMemberMenu() { MenuName = "安全中心", MenuCode = "SecurityDesktop", Buttons = "navbar|show", ParentId = topdesktop.Id, OrderNum = (menuIndex++), };
                helper.Insert(desktop);
                {
                    helper.Insert(new DbSysMemberMenu() { MenuName = "登录日志", MenuCode = "MemberLoginLog", Url = "/Members/Securitys/LoginList", Buttons = "navbar|show", ParentId = desktop.Id.ToSafeInt32(0), OrderNum = (menuIndex++), });
                    helper.Insert(new DbSysMemberMenu() { MenuName = "操作日志", MenuCode = "MemberOperationLog", Url = "/Members/Securitys/OperationList", Buttons = "navbar|show", ParentId = desktop.Id.ToSafeInt32(0), OrderNum = (menuIndex++), });
                }
            }

            var wordsDesktop = new DbSysMemberMenu() { MenuName = "项目管理", MenuCode = "ProjectTopDesktop", Url = "", Buttons = "navbar|show", ParentId = 0, OrderNum = (menuIndex++), };
            helper.Insert(wordsDesktop);
            {
                var topdesktop = wordsDesktop;
                var desktop = new DbSysMemberMenu() { MenuName = "项目信息", MenuCode = "FlowProjectDesktop", Buttons = "navbar|show", ParentId = topdesktop.Id, OrderNum = (menuIndex++), };
                helper.Insert(desktop);
                {
                    helper.Insert(new DbSysMemberMenu() { MenuName = "项目字典", MenuCode = "FlowProjectDict", Url = "", Buttons = "navbar|show|edit|delete|import", ParentId = desktop.Id, OrderNum = (menuIndex++), });
                    helper.Insert(new DbSysMemberMenu() { MenuName = "项目公式", MenuCode = "FlowProjectFormula", Url = "", Buttons = "navbar|show|edit|delete", ParentId = desktop.Id, OrderNum = (menuIndex++), });
					helper.Insert(new DbSysMemberMenu() { MenuName = "项目数据", MenuCode = "FlowProjectData", Url = "", Buttons = "navbar|show|edit|delete", ParentId = desktop.Id, OrderNum = (menuIndex++), });

				}
				desktop = new DbSysMemberMenu() { MenuName = "厂区管理", MenuCode = "FlowFactorysDesktop", Buttons = "navbar|show", ParentId = topdesktop.Id, OrderNum = (menuIndex++), };
                helper.Insert(desktop);
                {
                    helper.Insert(new DbSysMemberMenu() { MenuName = "厂区信息", MenuCode = "FlowFactorys", Url = "", Buttons = "navbar|show|edit|delete", ParentId = desktop.Id, OrderNum = (menuIndex++), });
                    helper.Insert(new DbSysMemberMenu() { MenuName = "厂区机械", MenuCode = "FlowFactoryMachines", Url = "", Buttons = "navbar|show|edit|delete", ParentId = desktop.Id, OrderNum = (menuIndex++), });
                    helper.Insert(new DbSysMemberMenu() { MenuName = "厂区工艺", MenuCode = "TechnologyProcedures", Url = "", Buttons = "navbar|show|edit|delete", ParentId = desktop.Id, OrderNum = (menuIndex++), });
                }
                desktop = new DbSysMemberMenu() { MenuName = "流程设计", MenuCode = "FlowAppDesktop", Buttons = "navbar|show", ParentId = topdesktop.Id, OrderNum = (menuIndex++), };
                helper.Insert(desktop);
                {
                    helper.Insert(new DbSysMemberMenu() { MenuName = "流程信息", MenuCode = "FlowApp", Url = "", Buttons = "navbar|show|edit|delete|export", ParentId = desktop.Id, OrderNum = (menuIndex++), });
                    helper.Insert(new DbSysMemberMenu() { MenuName = "输入检测", MenuCode = "FlowAppInput", Url = "", Buttons = "navbar|show|edit|delete", ParentId = desktop.Id, OrderNum = (menuIndex++), });
                    helper.Insert(new DbSysMemberMenu() { MenuName = "初始值", MenuCode = "FlowAppInitValue", Url = "", Buttons = "navbar|show|edit|delete", ParentId = desktop.Id, OrderNum = (menuIndex++), });
                    helper.Insert(new DbSysMemberMenu() { MenuName = "开始流程设计", MenuCode = "FlowAppFlow", Url = "", Buttons = "navbar|show|edit|delete", ParentId = desktop.Id, OrderNum = (menuIndex++), });
                }
            }


            #region DbSysMemberMenu.ParentIds
            {
                var menus = helper.Select<DbSysMemberMenu>();
                for (int i = 0; i < menus.Count; i++) {
                    var menu = menus[i];
                    if (menu.ParentId == 0) {
                        menu.ParentIds = "-0-";// + menu.Id + "-";
                    } else {
                        var pmenu = menus.Where(q => q.Id == menu.ParentId).First();
                        menu.ParentIds = pmenu.ParentIds + menu.ParentId + "-";
                    }
                    menu.Buttons = menu.Buttons.ToLower();
                    helper.Save(menu);
                }
            }
            #endregion


            #region DbSysMemberMenuButton
            {
                var index = 1;
                var t = "navbar=导航栏&Show=显示&edit=编辑&delete=删除&export=导出&import=导入&build=生成";


                //"&Start=启动&ReStart=重启&Pause=暂停&Stop=停止&Close=关闭" +
                //"&Audit=审核&Revoke=撤销&Invalid=作废&Lock=锁定&UnLock=解锁&redo=重做&Pass=通过&notPass=不通过" +
                //"&Message=短信&search=搜索" +
                //"&Copy=复制&Replace=替换&Build=生成&Print=打印" +
                //"&Instal=安装&UnLoad=卸载&Backup=备份&Restore=还原&export=导出&Import=导入" +
                //"&Authorize=授权&Online=上线&Offline=下线";
                var ts = t.ToLower().Split('&');
                foreach (var s in ts) {
                    var sp = s.Split('=');
                    helper.Insert(new DbSysMemberMenuButton() {
                        ButtonCode = sp[0].ToLower(),
                        ButtonName = sp[1],
                        OrderNum = (index++),
                    });
                }
            }
            #endregion
        }

    }
}
