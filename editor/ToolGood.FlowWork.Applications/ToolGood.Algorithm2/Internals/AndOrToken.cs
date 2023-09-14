using Antlr4.Runtime;

namespace ToolGood.Algorithm2.Internals
{
	public class AndOrToken : IToken
	{
		public AndOrToken(string text)
		{ Text = text; }

		public string Text { get; private set; }
		public int Type => 0;
		public int Line => 0;
		public int Column => 0;
		public int Channel => 0;
		public int TokenIndex => 0;
		public int StartIndex => 0;
		public int StopIndex => 0;
		public ITokenSource TokenSource => null;
		public ICharStream InputStream => null;
	}
}