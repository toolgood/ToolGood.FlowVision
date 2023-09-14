namespace ToolGood.Algorithm2
{
	/// <summary>
	/// 自定义信息
	/// </summary>
	public sealed class DiyNameInfo
	{
		/// <summary>
		/// 参数名
		/// </summary>
		//public List<String> Parameters = new List<String>();

		public List<ParameterInfo> Parameters = new List<ParameterInfo>();
		/// <summary>
		///
		/// </summary>
	}

	public sealed class ParameterInfo
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="name"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		public ParameterInfo(string name, int start, int end)
		{
			Name = name;
			Start = start;
			End = end;
		}

		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 开始位置
		/// </summary>
		public int Start { get; set; }

		/// <summary>
		/// 结束位置
		/// </summary>
		public int End { get; set; }
	}
}