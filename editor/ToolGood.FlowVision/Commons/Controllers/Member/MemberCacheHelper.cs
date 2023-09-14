using System.Collections.Concurrent;
using ToolGood.AntiDuplication;
using ToolGood.Datas;
using ToolGood.FlowVision.Commons.Utils;

namespace ToolGood.FlowVision.Commons.Controllers
{
	public class MemberCacheHelper
	{
		private static readonly ConcurrentDictionary<int, string> MemberSessionIdCaches = new ConcurrentDictionary<int, string>();
		public static readonly AntiDupCache<string, bool> MemberMenuButtonCache = new AntiDupCache<string, bool>(10000, 5);
		//public static readonly AntiDupCache<string, bool> MenuCheckCache = new AntiDupCache<string, bool>(10000, 5);

		static MemberCacheHelper()
		{
			//IMemberApplication MemberApplication = MyIoc.Create<IMemberApplication>();
			//var task = MemberApplication.GetMemberCookieList().RunSync();
			////MemberApplication.GetMemberSessionId().RunSynchronously()
			//foreach (var item in task) {
			//    MemberSessionIdCaches[item.Id] = item.LastSessionID;
			//}
		}

		public static void SetMemberSessionId(int userId, string sessionId)
		{
			MemberSessionIdCaches[userId] = sessionId;
		}

		public static bool CheckMemberSessionId(int userId, string cookie)
		{
			if (MemberSessionIdCaches.TryGetValue(userId, out string value)) {
				return (value == cookie);
			}
			return false;
		}

		public static MemberCookieDto CreateMemberCookieDto(DbSysMember Member, int mins)
		{
			var dt = DateTime.Now;
			var exp = dt.AddMinutes(mins);
			MemberCookieDto userDto = new MemberCookieDto() {
				CreateTime = dt,
				ExpireTime = exp,
				UserId = Member.Id,
				//UserName = Member.Name,
				PasswordHash = HashUtil.GetMd5String(Member.Password),
			};
			return userDto;
		}
	}
}