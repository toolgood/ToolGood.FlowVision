using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text;

//using Color = IronSoftware.Drawing.Color;

namespace ToolGood.FlowVision.Commons.Utils
{
	public sealed class VerifyCodeUtil
	{
		/// <summary>
		/// 生成随机码
		/// </summary>
		/// <param name="CodeLength"></param>
		/// <param name="Letters"></param>
		/// <returns></returns>
		public string GenerateCaptchaCode(int CodeLength = 4, string Letters = "2346789ABCDEFGHJKLMNPRTUVWXYZ")
		{
			var rand = new Random();
			var maxRand = Letters.Length - 1;

			var sb = new StringBuilder();
			for (var i = 0; i < CodeLength; i++) {
				var index = rand.Next(maxRand);
				sb.Append(Letters[index]);
			}

			return sb.ToString();
		}

		/// <summary>
		/// 噪点颜色
		/// </summary>
		private List<Color> NoiseColors = new List<Color>(){
			Color.LightBlue,Color.LightCoral,Color.LightCyan,Color.LightGoldenrodYellow,
			Color.LightGray,Color.LightGreen,Color.LightPink,Color.LightSalmon,
			Color.LightSkyBlue,Color.LightSteelBlue,Color.LightYellow
		};

		/// <summary>
		/// 显示字体
		/// </summary>
		private List<string> ShowFonts = new List<string>(){
			"Arial", "Georgia", "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "宋体", "楷体", "黑体"
		};

		/// <summary>
		/// 显示颜色
		/// </summary>
		private List<Color> ShowColors = new List<Color>(){
			Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple
		};

		/// <summary>
		/// 字体大小
		/// </summary>
		private int FontSize = 33;

		/// <summary>
		/// 边框
		/// </summary>
		private int Padding = 2;

		public byte[] CreateImageBytes(string code)
		{
			int fSize = 28;
			int fWidth = fSize + Padding;
			int Width = (int)(code.Length * fWidth) + 4 + Padding * 2;
			int Height = (int)(fSize * 1.5 + Padding);

			var r = new Random();
			using var image = new Image<Rgba32>(Width, Height);
			// 字体
			List<Font> fonts = new List<Font>();
			fonts.Add(SystemFonts.CreateFont(SystemFonts.Families.First().Name, FontSize, FontStyle.Bold));
			foreach (var fontFamily in SystemFonts.Families) {
				if (ShowFonts.Contains(fontFamily.Name)) {
					fonts.Add(SystemFonts.CreateFont(fontFamily.Name, FontSize, FontStyle.Bold));
				}
			}

			image.Mutate(ctx => {
				// 白底背景
				ctx.Fill(Color.White);

				// 画验证码
				for (int i = 0; i < code.Length; i++) {
					ctx.DrawText(code[i].ToString()
						, fonts[r.Next(fonts.Count)]
						, ShowColors[r.Next(ShowColors.Count)]
						, new PointF(30 * i + 12, r.Next(2, 8)));
				}

				// 画干扰线
				for (int i = 0; i < 6; i++) {
					var pen = new Pen(NoiseColors[r.Next(NoiseColors.Count)], 2);
					var p1 = new PointF(r.Next(Width), r.Next(Height));
					var p2 = new PointF(r.Next(Width), r.Next(Height));
					ctx.DrawLines(pen, p1, p2);
				}

				// 画噪点
				for (int i = 0; i < 150; i++) {
					var pen = new Pen(ShowColors[r.Next(ShowColors.Count)], 1);
					var p1 = new PointF(r.Next(Width), r.Next(Height));
					var p2 = new PointF(p1.X + 1f, p1.Y + 1f);

					ctx.DrawLines(pen, p1, p2);
				}
				var pen2 = new Pen(Color.Gainsboro, 1);
				ctx.DrawPolygon(pen2, new PointF(0, 0), new PointF(Width - 1, 0), new PointF(Width - 1, Height - 1), new PointF(0, Height - 1));
			});
			using var ms = new System.IO.MemoryStream();

			//  格式 自定义
			image.SaveAsPng(ms);
			return ms.ToArray();
		}
	}
}