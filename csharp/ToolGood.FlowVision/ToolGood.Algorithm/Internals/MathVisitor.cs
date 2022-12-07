using Antlr4.Runtime.Tree;
using Esprima.Ast;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.LitJson;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals
{
	sealed class MathVisitor : AbstractParseTreeVisitor<Operand>, ImathVisitor<Operand>
	{
		private static readonly CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-US");
		public event Func<string, Operand> GetParameter;
		public int excelIndex;
		public DistanceUnitType DistanceUnit = DistanceUnitType.M;
		public AreaUnitType AreaUnit = AreaUnitType.M2;
		public VolumeUnitType VolumeUnit = VolumeUnitType.M3;
		public MassUnitType MassUnit = MassUnitType.KG;


		#region base

		public Operand VisitProg(mathParser.ProgContext context)
		{
			return Visit(context.expr());
		}

		public Operand VisitMulDiv_fun(mathParser.MulDiv_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.IsError) { return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.IsError) { return args2; }

			if (args1.Type == OperandType.TEXT) {
				if (double.TryParse(args1.TextValue, NumberStyles.Any, cultureInfo, out double d)) {
					args1 = Operand.Create(d);
				} else if (bool.TryParse(args1.TextValue, out bool b)) {
					args1 = b ? Operand.One : Operand.Zero;
				} else if (TimeSpan.TryParse(args1.TextValue, cultureInfo, out TimeSpan ts)) {
					args1 = Operand.Create(ts);
				} else if (DateTime.TryParse(args1.TextValue, cultureInfo, DateTimeStyles.None, out DateTime dt)) {
					args1 = Operand.Create(new MyDate(dt));
				} else {
					return Operand.Error("两个类型无法乘除");
				}
			}
			if (args2.Type == OperandType.TEXT) {
				if (double.TryParse(args2.TextValue, NumberStyles.Any, cultureInfo, out double d)) {
					args2 = Operand.Create(d);
				} else if (bool.TryParse(args2.TextValue, out bool b)) {
					args2 = b ? Operand.One : Operand.Zero;
				} else if (TimeSpan.TryParse(args2.TextValue, cultureInfo, out TimeSpan ts)) {
					args2 = Operand.Create(ts);
				} else if (DateTime.TryParse(args2.TextValue, cultureInfo, DateTimeStyles.None, out DateTime dt)) {
					args2 = Operand.Create(new MyDate(dt));
				} else {
					return Operand.Error("两个类型无法乘除");
				}
			}
			var t = context.op.Text;
			if (CharUtil.Equals(t, '*')) {
				if (args1.Type == OperandType.DATE) {
					if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '*' parameter 2 is error!"); if (args2.IsError) { return args2; } }
					if (args2.NumberValue == 1) { return args1; }
					return Operand.Create((MyDate)(args1.DateValue * args2.NumberValue));
				} else if (args2.Type == OperandType.DATE) {
					if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '*' parameter 1 is error!"); if (args1.IsError) { return args1; } }
					if (args1.NumberValue == 1) { return args2; }
					return Operand.Create((MyDate)(args2.DateValue * args1.NumberValue));
				}

				if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '*' parameter 1 is error!"); if (args1.IsError) { return args1; } }
				if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '*' parameter 2 is error!"); if (args2.IsError) { return args2; } }
				if (args1.NumberValue == 1) { return args2; }
				if (args2.NumberValue == 1) { return args1; }

				return Operand.Create(args1.NumberValue * args2.NumberValue);
			} else if (CharUtil.Equals(t, '/')) {
				if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '/' parameter 2 is error!"); if (args2.IsError) { return args2; } }
				if (args2.NumberValue == 0) { return Operand.Error("Div 0 is error!"); }
				if (args2.NumberValue == 1) { return args1; }

				if (args1.Type == OperandType.DATE) { return Operand.Create(args1.DateValue / args2.NumberValue); }
				if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '/' parameter 1 is error!"); if (args1.IsError) { return args1; } }

				return Operand.Create(args1.NumberValue / args2.NumberValue);
				//} else if (CharUtil.Equals(t, "%")) {
			} else {
				if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("% fun right value"); if (args1.IsError) { return args1; } }
				if (args1.Type != OperandType.NUMBER) { args2 = args2.ToNumber("% fun right value"); if (args2.IsError) { return args2; } }
				if (args2.NumberValue == 0) { return Operand.Error("Div 0 is error!"); }

				return Operand.Create(args1.NumberValue % args2.NumberValue);
			}
		}

		public Operand VisitAddSub_fun(mathParser.AddSub_funContext context)
		{
			var exprs = context.expr();

			var args1 = this.Visit(exprs[0]); if (args1.IsError) { return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.IsError) { return args2; }
			var t = context.op.Text;
			if (CharUtil.Equals(t, '&')) {
				if (args1.IsNull) {
					if (args2.IsNull) return args1;
					return args2.ToText("Function '&' parameter 2 is error!");
				} else if (args2.IsNull) {
					return args1.ToText("Function '&' parameter 1 is error!");
				}
				if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function '&' parameter 1 is error!"); if (args1.IsError) { return args1; } }
				if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function '&' parameter 2 is error!"); if (args2.IsError) { return args2; } }
				return Operand.Create(args1.TextValue + args2.TextValue);
			}
			if (args1.Type == OperandType.TEXT) {
				if (double.TryParse(args1.TextValue, NumberStyles.Any, cultureInfo, out double d)) {
					args1 = Operand.Create(d);
				} else if (bool.TryParse(args1.TextValue, out bool b)) {
					args1 = b ? Operand.One : Operand.Zero;
				} else if (TimeSpan.TryParse(args1.TextValue, cultureInfo, out TimeSpan ts)) {
					args1 = Operand.Create(ts);
				} else if (DateTime.TryParse(args1.TextValue, cultureInfo, DateTimeStyles.None, out DateTime dt)) {
					args1 = Operand.Create(new MyDate(dt));
				} else {
					return Operand.Error("两个类型无法加减");
				}
			}
			if (args2.Type == OperandType.TEXT) {
				if (double.TryParse(args2.TextValue, NumberStyles.Any, cultureInfo, out double d)) {
					args2 = Operand.Create(d);
				} else if (bool.TryParse(args2.TextValue, out bool b)) {
					args2 = b ? Operand.One : Operand.Zero;
				} else if (TimeSpan.TryParse(args2.TextValue, cultureInfo, out TimeSpan ts)) {
					args2 = Operand.Create(ts);
				} else if (DateTime.TryParse(args2.TextValue, cultureInfo, DateTimeStyles.None, out DateTime dt)) {
					args2 = Operand.Create(new MyDate(dt));
				} else {
					return Operand.Error("两个类型无法加减");
				}
			}
			if (CharUtil.Equals(t, '+')) {
				if (args1.Type == OperandType.DATE) {
					if (args2.Type == OperandType.DATE) return Operand.Create(args1.DateValue + args2.DateValue);
					if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '+' parameter 2 is error!"); if (args2.IsError) { return args2; } }
					if (args2.NumberValue == 0) { return args1; }
					return Operand.Create(args1.DateValue + args2.NumberValue);
				} else if (args2.Type == OperandType.DATE) {
					if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '+' parameter 1 is error!"); if (args1.IsError) { return args1; } }
					if (args1.NumberValue == 0) { return args2; }
					return Operand.Create(args2.DateValue + args1.NumberValue);
				}
				if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '+' parameter 1 is error!"); if (args1.IsError) { return args1; } }
				if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '+' parameter 2 is error!"); if (args2.IsError) { return args2; } }
				if (args1.NumberValue == 0) { return args2; }
				if (args2.NumberValue == 0) { return args1; }

				return Operand.Create(args1.NumberValue + args2.NumberValue);
				//} else if (CharUtil.Equals(t, "-")) {
			} else {
				if (args1.Type == OperandType.DATE) {
					if (args2.Type == OperandType.DATE) return Operand.Create(args1.DateValue - args2.DateValue);
					if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '-' parameter 2 is error!"); if (args2.IsError) { return args2; } }
					if (args2.NumberValue == 0) { return args1; }
					return Operand.Create(args1.DateValue - args2.NumberValue);
				} else if (args2.Type == OperandType.DATE) {
					if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '-' parameter 1 is error!"); if (args1.IsError) { return args1; } }
					return Operand.Create(args1.NumberValue - args2.DateValue);
				}
				if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '-' parameter 1 is error!"); if (args1.IsError) { return args1; } }
				if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '-' parameter 2 is error!"); if (args2.IsError) { return args2; } }
				if (args2.NumberValue == 0) { return args1; }

				return Operand.Create(args1.NumberValue - args2.NumberValue);
			}
		}

		public Operand VisitJudge_fun(mathParser.Judge_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.IsError) { return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.IsError) { return args2; }

			string type = context.op.Text;
			int r;
			if (args1.Type == args2.Type) {
				if (args1.Type == OperandType.NUMBER) {
					return Compare(args1, args2, type);
				} else if (args1.Type == OperandType.TEXT) {
					r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
				} else if (args1.Type == OperandType.DATE || args1.Type == OperandType.BOOLEAN) {
					args1 = args1.ToNumber();
					args2 = args2.ToNumber();
					return Compare(args1, args2, type);
				} else if (args1.Type == OperandType.JSON) {
					args1 = args1.ToText();
					args2 = args2.ToText();
					r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
				} else if (args1.Type == OperandType.NULL) {
					return CharUtil.Equals(type, "=", "==", "===") ? Operand.True : Operand.False;
				} else {
					return Operand.Error("两个类型无法比较");
				}
			} else if (args1.Type == OperandType.NULL || args2.Type == OperandType.NULL) {
				return CharUtil.Equals(type, "<>", "!=", "!==") ? Operand.True : Operand.False;
			} else if (args1.Type == OperandType.TEXT) {
				if (args2.Type == OperandType.BOOLEAN) {
					var a = args1.ToBoolean();
					if (a.IsError == false) {
						if (CharUtil.Equals(type, "=", "==", "===")) {
							return a.BooleanValue == args2.BooleanValue ? Operand.True : Operand.False;
						} else if (CharUtil.Equals(type, "<>", "!=", "!===")) {
							return a.BooleanValue != args2.BooleanValue ? Operand.True : Operand.False;
						}
					}
					args2 = args2.ToText();
					r = string.Compare(args1.TextValue, args2.TextValue, true);
				} else if (args2.Type == OperandType.DATE || args2.Type == OperandType.NUMBER || args2.Type == OperandType.JSON) {
					args2 = args2.ToText();
					r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
				} else {
					return Operand.Error("两个类型无法比较");
				}
			} else if (args2.Type == OperandType.TEXT) {
				if (args1.Type == OperandType.BOOLEAN) {
					var a = args2.ToBoolean();
					if (a.IsError == false) {
						if (CharUtil.Equals(type, "=", "==", "===")) {
							return a.BooleanValue == args1.BooleanValue ? Operand.True : Operand.False;
						} else if (CharUtil.Equals(type, "<>", "!=", "!===")) {
							return a.BooleanValue != args1.BooleanValue ? Operand.True : Operand.False;
						}
					}
					args1 = args1.ToText();
					r = string.Compare(args1.TextValue, args2.TextValue, true);
				} else if (args1.Type == OperandType.DATE || args1.Type == OperandType.NUMBER || args1.Type == OperandType.JSON) {
					args1 = args1.ToText();
					r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
				} else {
					return Operand.Error("两个类型无法比较");
				}
			} else if (args1.Type == OperandType.JSON || args2.Type == OperandType.JSON
				  || args1.Type == OperandType.ARRARY || args2.Type == OperandType.ARRARY
				  || args1.Type == OperandType.ARRARYJSON || args2.Type == OperandType.ARRARYJSON
				  ) {
				return Operand.Error("两个类型无法比较");
			} else {
				if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber($"Function '{type}' parameter 1 is error!"); if (args1.IsError) { return args1; } }
				if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber($"Function '{type}' parameter 2 is error!"); if (args2.IsError) { return args2; } }

				return Compare(args1, args2, type);
			}
			if (type.Length == 1) {
				if (CharUtil.Equals(type, '<')) {
					return Operand.Create(r < 0);
				} else if (CharUtil.Equals(type, '>')) {
					return Operand.Create(r > 0);
				} else /*if (CharUtil.Equals(type, '=')*/{
					return Operand.Create(r == 0);
				}
			} else if (CharUtil.Equals(type, "<=")) {
				return Operand.Create(r <= 0);
			} else if (CharUtil.Equals(type, ">=")) {
				return Operand.Create(r >= 0);
			} else if (CharUtil.Equals(type, "==", "===")) {
				return Operand.Create(r == 0);
			}
			return Operand.Create(r != 0);
		}
		private Operand Compare(Operand op1, Operand op2, string type)
		{
			double t1 = op1.NumberValue;
			double t2 = op2.NumberValue;

			var r = Math.Round(t1 - t2, 10, MidpointRounding.AwayFromZero);
			if (type.Length == 1) {
				if (CharUtil.Equals(type, '<')) {
					return Operand.Create(r < 0);
				} else if (CharUtil.Equals(type, '>')) {
					return Operand.Create(r > 0);
				} else /*if (CharUtil.Equals(type, '=')*/{
					return Operand.Create(r == 0);
				}
			} else if (CharUtil.Equals(type, "<=")) {
				return Operand.Create(r <= 0);
			} else if (CharUtil.Equals(type, ">=")) {
				return Operand.Create(r >= 0);
			} else if (CharUtil.Equals(type, "==", "===")) {
				return Operand.Create(r == 0);
			}
			return Operand.Create(r != 0);
		}

		public Operand VisitAndOr_fun(mathParser.AndOr_funContext context)
		{
			// 程序 && and || or 与 excel的  AND(x,y) OR(x,y) 有区别
			// 在excel内 AND(x,y) OR(x,y) 先报错，
			// 在程序中，&& and  有true 直接返回true 就不会检测下一个会不会报错
			// 在程序中，|| or  有false 直接返回false 就不会检测下一个会不会报错
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.BOOLEAN) { args1 = args1.ToBoolean(); if (args1.IsError) { return args1; } }
			var t = context.op.Text;
			if (CharUtil.Equals(t, "&&", "AND")) {
				if (args1.BooleanValue == false) return Operand.False;
			} else {
				if (args1.BooleanValue) return Operand.True;
			}
			return this.Visit(exprs[1]).ToBoolean();
		}

		#endregion

		#region flow
		public Operand VisitIF_fun(mathParser.IF_funContext context)
		{
			var exprs = context.expr();
			var args1 = Visit(exprs[0]); if (args1.Type != OperandType.BOOLEAN) { args1 = args1.ToBoolean("Function IF first parameter is error!"); if (args1.IsError) { return args1; } }

			if (args1.BooleanValue) {
				if (exprs.Length > 1) { return Visit(exprs[1]); }
				return Operand.True;
			}
			if (exprs.Length == 3) { return Visit(exprs[2]); }
			return Operand.False;
		}

		public Operand VisitIFERROR_fun(mathParser.IFERROR_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]);

			if (args1.IsError) { return this.Visit(exprs[1]); }
			if (exprs.Length == 3) { return this.Visit(exprs[2]); }
			return Operand.False;
		}
		public Operand VisitISNUMBER_fun(mathParser.ISNUMBER_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.IsError) { return args1; }

			if (args1.Type == OperandType.NUMBER) { return Operand.True; }
			return Operand.False;
		}
		public Operand VisitISTEXT_fun(mathParser.ISTEXT_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.IsError) { return args1; }

			if (args1.Type == OperandType.TEXT) { return Operand.True; }
			return Operand.False;
		}
		public Operand VisitISERROR_fun(mathParser.ISERROR_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]);

			if (exprs.Length == 2) {
				if (args1.IsError) { return this.Visit(exprs[1]); }
				return args1;
			}
			if (args1.IsError) { return Operand.True; }
			return Operand.False;
		}

		public Operand VisitISNULL_fun(mathParser.ISNULL_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]);

			if (exprs.Length == 2) {
				if (args1.IsNull) { return this.Visit(exprs[1]); }
				return args1;
			}
			if (args1.IsNull) { return Operand.True; }
			return Operand.False;
		}

		public Operand VisitISNULLORERROR_fun(mathParser.ISNULLORERROR_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]);

			if (exprs.Length == 2) {
				if (args1.IsNull || args1.IsError) { return this.Visit(exprs[1]); }
				return args1;
			}
			if (args1.IsNull || args1.IsError) { return Operand.True; }
			return Operand.False;
		}

		public Operand VisitISEVEN_fun(mathParser.ISEVEN_funContext context)
		{
			var args1 = this.Visit(context.expr());
			if (args1.Type == OperandType.NUMBER) {
				if (args1.IntValue % 2 == 0) { return Operand.True; }
			}
			return Operand.False;
		}

		public Operand VisitISLOGICAL_fun(mathParser.ISLOGICAL_funContext context)
		{
			var args1 = this.Visit(context.expr());
			if (args1.Type == OperandType.BOOLEAN) { return Operand.True; }
			return Operand.False;
		}

		public Operand VisitISODD_fun(mathParser.ISODD_funContext context)
		{
			var args1 = this.Visit(context.expr());
			if (args1.Type == OperandType.NUMBER) {
				if (args1.IntValue % 2 == 1) { return Operand.True; }
			}
			return Operand.False;
		}

		public Operand VisitISNONTEXT_fun(mathParser.ISNONTEXT_funContext context)
		{
			var args1 = this.Visit(context.expr());
			if (args1.Type != OperandType.TEXT) { return Operand.True; }
			return Operand.False;
		}

		public Operand VisitAND_fun(mathParser.AND_funContext context)
		{
			var index = 1;
			bool b = true;
			var exprs = context.expr();
			for (int i = 0; i < exprs.Length; i++) {
				var a = this.Visit(exprs[i]); if (a.Type != OperandType.BOOLEAN) { a = a.ToBoolean($"Function AND parameter {index++} is error!"); if (a.IsError) { return a; } }
				if (a.BooleanValue == false) b = false;
			}
			return b ? Operand.True : Operand.False;
		}
		public Operand VisitOR_fun(mathParser.OR_funContext context)
		{
			var index = 1;
			bool b = false;
			var exprs = context.expr();
			for (int i = 0; i < exprs.Length; i++) {
				var a = this.Visit(exprs[i]); if (a.Type != OperandType.BOOLEAN) { a = a.ToBoolean($"Function OR parameter {index++} is error!"); if (a.IsError) { return a; } }
				if (a.BooleanValue) b = true;
			}
			return b ? Operand.True : Operand.False;
		}
		public Operand VisitNOT_fun(mathParser.NOT_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.BOOLEAN) { args1 = args1.ToBoolean("Function NOT parameter is error!"); if (args1.IsError) { return args1; } }
			return Operand.Create(!args1.BooleanValue);
		}
		public Operand VisitTRUE_fun(mathParser.TRUE_funContext context)
		{
			return Operand.True;
		}
		public Operand VisitFALSE_fun(mathParser.FALSE_funContext context)
		{
			return Operand.False;
		}
		#endregion

		#region math

		#region base

		public Operand VisitE_fun(mathParser.E_funContext context)
		{
			return Operand.Create(Math.E);
		}
		public Operand VisitPI_fun(mathParser.PI_funContext context)
		{
			return Operand.Create(Math.PI);
		}

		public Operand VisitABS_fun(mathParser.ABS_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ABS parameter is error!"); if (args1.IsError) { return args1; } }

			return Operand.Create(Math.Abs(args1.NumberValue));
		}
		public Operand VisitQUOTIENT_fun(mathParser.QUOTIENT_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function QUOTIENT parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function QUOTIENT parameter 2 is error!"); if (args2.IsError) { return args2; } }

			if (args2.NumberValue == 0) {
				return Operand.Error("Function QUOTIENT div 0 error!");
			}
			return Operand.Create((double)(int)(args1.NumberValue / args2.NumberValue));
		}
		public Operand VisitMOD_fun(mathParser.MOD_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function MOD parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function MOD parameter 2 is error!"); if (args2.IsError) { return args2; } }

			if (args2.NumberValue == 0) {
				return Operand.Error("Function MOD div 0 error!");
			}
			return Operand.Create((int)(args1.NumberValue % args2.NumberValue));

		}
		public Operand VisitSIGN_fun(mathParser.SIGN_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function SIGN parameter is error!"); if (args1.IsError) { return args1; } }

			return Operand.Create(Math.Sign(args1.NumberValue));
		}
		public Operand VisitSQRT_fun(mathParser.SQRT_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function SQRT parameter is error!"); if (args1.IsError) { return args1; } }

			return Operand.Create(Math.Sqrt(args1.NumberValue));
		}
		public Operand VisitTRUNC_fun(mathParser.TRUNC_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function TRUNC parameter is error!"); if (args1.IsError) { return args1; } }
			return Operand.Create((int)(args1.NumberValue));
		}
		public Operand VisitINT_fun(mathParser.INT_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function INT parameter is error!"); if (args1.IsError) { return args1; } }
			return Operand.Create((int)(args1.NumberValue));
		}
		public Operand VisitGCD_fun(mathParser.GCD_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function GCD parameter is error!"); }

			return Operand.Create(F_base_gcd(list));
		}
		public Operand VisitLCM_fun(mathParser.LCM_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function GCD parameter is error!"); }

			return Operand.Create(F_base_lgm(list));
		}
		public Operand VisitCOMBIN_fun(mathParser.COMBIN_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function COMBIN parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function COMBIN parameter 2 is error!"); if (args2.IsError) { return args2; } }

			var total = args1.IntValue;
			var count = args2.IntValue;
			double sum = 1;
			double sum2 = 1;
			for (int i = 0; i < count; i++) {
				sum *= (total - i);
				sum2 *= (i + 1);
			}
			return Operand.Create(sum / sum2);
		}
		public Operand VisitPERMUT_fun(mathParser.PERMUT_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function PERMUT parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function PERMUT parameter 2 is error!"); if (args2.IsError) { return args2; } }

			var total = args1.IntValue;
			var count = args2.IntValue;

			double sum = 1;
			for (int i = 0; i < count; i++) {
				sum *= (total - i);
			}
			return Operand.Create(sum);
		}

		public Operand VisitPercentage_fun(mathParser.Percentage_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function Percentage parameter is error!"); if (args1.IsError) { return args1; } }

			return Operand.Create(args1.NumberValue / 100.0);
		}

		private int F_base_gcd(List<double> list)
		{
			list = list.OrderBy(q => q).ToList();
			var g = F_base_gcd((int)list[1], (int)list[0]);
			for (int i = 2; i < list.Count; i++) {
				g = F_base_gcd((int)list[i], g);
			}
			return g;
		}
		private int F_base_gcd(int a, int b)
		{
			if (b == 1) { return 1; }
			if (b == 0) { return a; }
			return F_base_gcd(b, a % b);
		}
		private int F_base_lgm(List<double> list)
		{
			list = list.OrderBy(q => q).ToList();
			list.RemoveAll(q => q <= 1);

			int a = (int)list[0];
			for (int i = 1; i < list.Count; i++) {
				int b = (int)list[i];
				int g = b > a ? F_base_gcd(b, a) : F_base_gcd(a, b);
				a = a / g * b;
			}
			return a;
		}
		#endregion

		#region trigonometric functions
		public Operand VisitDEGREES_fun(mathParser.DEGREES_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function DEGREES parameter is error!"); if (args1.IsError) { return args1; } }

			var z = args1.NumberValue;
			var r = (z / Math.PI * 180);
			return Operand.Create(r);
		}
		public Operand VisitRADIANS_fun(mathParser.RADIANS_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function RADIANS parameter is error!"); if (args1.IsError) { return args1; } }

			var r = args1.NumberValue / 180 * Math.PI;
			return Operand.Create(r);
		}
		public Operand VisitCOS_fun(mathParser.COS_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function COS parameter is error!"); if (args1.IsError) { return args1; } }
			return Operand.Create(Math.Cos(args1.NumberValue));
		}
		public Operand VisitCOSH_fun(mathParser.COSH_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function COSH parameter is error!"); if (args1.IsError) { return args1; } }

			return Operand.Create(Math.Cosh(args1.NumberValue));
		}
		public Operand VisitSIN_fun(mathParser.SIN_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function SIN parameter is error!"); if (args1.IsError) { return args1; } }
			return Operand.Create(Math.Sin(args1.NumberValue));
		}
		public Operand VisitSINH_fun(mathParser.SINH_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function SINH parameter is error!"); if (args1.IsError) { return args1; } }
			return Operand.Create(Math.Sinh(args1.NumberValue));
		}
		public Operand VisitTAN_fun(mathParser.TAN_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function TAN parameter is error!"); if (args1.IsError) { return args1; } }
			return Operand.Create(Math.Tan(args1.NumberValue));
		}
		public Operand VisitTANH_fun(mathParser.TANH_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function TANH parameter is error!"); if (args1.IsError) { return args1; } }
			return Operand.Create(Math.Tanh(args1.NumberValue));
		}
		public Operand VisitACOS_fun(mathParser.ACOS_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ACOS parameter is error!"); if (args1.IsError) { return args1; } }
			var x = args1.NumberValue;
			if (x < -1 && x > 1) {
				return Operand.Error("Function ACOS parameter is error!");
			}
			return Operand.Create(Math.Acos(x));
		}
		public Operand VisitACOSH_fun(mathParser.ACOSH_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ACOSH parameter is error!"); if (args1.IsError) { return args1; } }

			var z = args1.NumberValue;
			if (z < 1) {
				return Operand.Error("Function ACOSH parameter is error!");
			}
			return Operand.Create(Math.Acosh(z));
		}
		public Operand VisitASIN_fun(mathParser.ASIN_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ASIN parameter is error!"); if (args1.IsError) { return args1; } }
			var x = args1.NumberValue;
			if (x < -1 && x > 1) {
				return Operand.Error("Function ASIN parameter is error!");
			}
			return Operand.Create(Math.Asin(x));
		}
		public Operand VisitASINH_fun(mathParser.ASINH_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ASINH parameter is error!"); if (args1.IsError) { return args1; } }
			return Operand.Create(Math.Asinh(args1.NumberValue));
		}
		public Operand VisitATAN_fun(mathParser.ATAN_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ATAN parameter is error!"); if (args1.IsError) { return args1; } }

			return Operand.Create(Math.Atan(args1.NumberValue));
		}
		public Operand VisitATANH_fun(mathParser.ATANH_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ATANH parameter is error!"); if (args1.IsError) { return args1; } }
			var x = args1.NumberValue;
			if (x >= 1 || x <= -1) {
				return Operand.Error("Function ATANH parameter is error!");
			}
			return Operand.Create(Math.Atanh(x));
		}
		public Operand VisitATAN2_fun(mathParser.ATAN2_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ATAN2 parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function ATAN2 parameter 2 is error!"); if (args2.IsError) { return args2; } }

			return Operand.Create(Math.Atan2(args2.NumberValue, args1.NumberValue));
		}
		public Operand VisitFIXED_fun(mathParser.FIXED_funContext context)
		{
			var exprs = context.expr();
			var num = 2;
			if (exprs.Length > 1) {
				var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function FIXED parameter 2 is error!"); if (args2.IsError) { return args2; } }
				num = args2.IntValue;
			}
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FIXED parameter 1 is error!"); if (args1.IsError) { return args1; } }

			var s = Math.Round(args1.NumberValue, num, MidpointRounding.AwayFromZero);
			var no = false;
			if (exprs.Length == 3) {
				var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function FIXED parameter 3 is error!"); if (args3.IsError) { return args3; } }
				no = args3.BooleanValue;
			}
			if (no == false) {
				return Operand.Create(s.ToString('N' + num.ToString(), cultureInfo));
			}
			return Operand.Create(s.ToString(cultureInfo));
		}

		#endregion


		#region rounding

		public Operand VisitROUND_fun(mathParser.ROUND_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ROUND parameter 1 is error!"); if (args1.IsError) { return args1; } }
			if (exprs.Length == 1) {
				return Operand.Create((double)Math.Round((decimal)args1.NumberValue, 0, MidpointRounding.AwayFromZero));
			}
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function ROUND parameter 2 is error!"); if (args2.IsError) { return args2; } }
			return Operand.Create((double)Math.Round((decimal)args1.NumberValue, args2.IntValue, MidpointRounding.AwayFromZero));
		}
		public Operand VisitROUNDDOWN_fun(mathParser.ROUNDDOWN_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ROUNDDOWN parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function ROUNDDOWN parameter 2 is error!"); if (args2.IsError) { return args2; } }

			if (args1.NumberValue == 0.0) {
				return args1;
			}
			var a = Math.Pow(10, args2.IntValue);
			var b = args1.NumberValue;

			b = ((double)(int)(b * a)) / a;
			return Operand.Create(b);
		}
		public Operand VisitROUNDUP_fun(mathParser.ROUNDUP_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ROUNDUP parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function ROUNDUP parameter 2 is error!"); if (args2.IsError) { return args2; } }

			if (args1.NumberValue == 0.0) { return args1; }
			var a = Math.Pow(10, args2.IntValue);
			var b = args1.NumberValue;

			var t = (Math.Ceiling(Math.Abs(b) * a)) / a;
			if (b > 0) return Operand.Create(t);
			return Operand.Create(-t);
		}
		public Operand VisitCEILING_fun(mathParser.CEILING_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function CEILING parameter 1 is error!"); if (args1.IsError) { return args1; } }

			if (exprs.Length == 1)
				return Operand.Create(Math.Ceiling(args1.NumberValue));

			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function CEILING parameter 2 is error!"); if (args2.IsError) { return args2; } }
			var b = args2.NumberValue;
			if (b == 0) { return Operand.Create(0); }
			if (b < 0) { return Operand.Error("Function CEILING parameter 2 is error!"); }

			var a = args1.NumberValue;
			var d = Math.Ceiling(a / b) * b;
			return Operand.Create(d);
		}
		public Operand VisitFLOOR_fun(mathParser.FLOOR_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FLOOR parameter 1 is error!"); if (args1.IsError) { return args1; } }

			if (exprs.Length == 1) return Operand.Create(Math.Floor(args1.NumberValue));

			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function FLOOR parameter 2 is error!"); if (args2.IsError) { return args2; } }
			var b = args2.NumberValue;
			if (b >= 1) { return Operand.Create(args1.IntValue); }
			if (b <= 0) { return Operand.Error("Function FLOOR parameter 2 is error!"); }

			var a = args1.NumberValue;
			var d = Math.Floor(a / b) * b;
			return Operand.Create(d);
		}
		public Operand VisitEVEN_fun(mathParser.EVEN_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function EVEN parameter is error!"); if (args1.IsError) { return args1; } }

			var z = args1.NumberValue;
			if (z % 2 == 0) { return args1; }
			z = Math.Ceiling(z);
			if (z % 2 == 0) { return Operand.Create(z); }
			z++;
			return Operand.Create(z);
		}
		public Operand VisitODD_fun(mathParser.ODD_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ODD parameter is error!"); if (args1.IsError) { return args1; } }
			var z = args1.NumberValue;
			if (z % 2 == 1) { return args1; }
			z = Math.Ceiling(z);
			if (z % 2 == 1) { return Operand.Create(z); }
			z++;
			return Operand.Create(z);
		}
		public Operand VisitMROUND_fun(mathParser.MROUND_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function MROUND parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function MROUND parameter 2 is error!"); if (args2.IsError) { return args2; } }

			var a = args2.NumberValue;
			if (a <= 0) { return Operand.Error("Function MROUND parameter 2 is error!"); }

			var b = args1.NumberValue;
			var r = Math.Round(b / a, 0, MidpointRounding.AwayFromZero) * a;
			return Operand.Create(r);
		}
		#endregion

		#region RAND
		public Operand VisitRAND_fun(mathParser.RAND_funContext context)
		{
			var tick = DateTime.Now.Ticks;
			Random rand = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
			return Operand.Create(rand.NextDouble());
		}
		public Operand VisitRANDBETWEEN_fun(mathParser.RANDBETWEEN_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function RANDBETWEEN parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function RANDBETWEEN parameter 2 is error!"); if (args2.IsError) { return args2; } }

			var tick = DateTime.Now.Ticks;
			Random rand = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
			return Operand.Create(rand.NextDouble() * (args2.NumberValue - args1.NumberValue) + args1.NumberValue);
		}
		#endregion

		#region  power logarithm factorial
		public Operand VisitFACT_fun(mathParser.FACT_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FACT parameter is error!"); if (args1.IsError) { return args1; } }

			var z = args1.IntValue;
			if (z < 0) { return Operand.Error("Function FACT parameter is error!"); }

			double d = 1;
			for (int i = 1; i <= z; i++) {
				d *= i;
			}
			return Operand.Create(d);
		}
		public Operand VisitFACTDOUBLE_fun(mathParser.FACTDOUBLE_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FACTDOUBLE parameter is error!"); if (args1.IsError) { return args1; } }

			var z = args1.IntValue;
			if (z < 0) { return Operand.Error("Function FACTDOUBLE parameter is error!"); }

			double d = 1;
			for (int i = z; i > 0; i -= 2) {
				d *= i;
			}
			return Operand.Create(d);
		}
		public Operand VisitPOWER_fun(mathParser.POWER_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function POWER parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function POWER parameter 2 is error!"); if (args2.IsError) { return args2; } }

			return Operand.Create(Math.Pow(args1.NumberValue, args2.NumberValue));
		}
		public Operand VisitEXP_fun(mathParser.EXP_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function EXP parameter is error!"); if (args1.IsError) { return args1; } }

			return Operand.Create(Math.Exp(args1.NumberValue));
		}
		public Operand VisitLN_fun(mathParser.LN_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function LN parameter is error!"); if (args1.IsError) { return args1; } }
			return Operand.Create(Math.Log(args1.NumberValue));
		}
		public Operand VisitLOG_fun(mathParser.LOG_funContext context)
		{
			var exprs = context.expr();

			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function LOG parameter 1 is error!"); if (args1.IsError) { return args1; } }
			if (exprs.Length > 1) {
				var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function POWER parameter 2 is error!"); if (args2.IsError) { return args2; } }
				return Operand.Create(Math.Log(args1.NumberValue, args2.NumberValue));
			}
			return Operand.Create(Math.Log(args1.NumberValue, 10));
		}
		public Operand VisitLOG10_fun(mathParser.LOG10_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function LOG10 parameter is error!"); if (args1.IsError) { return args1; } }
			return Operand.Create(Math.Log(args1.NumberValue, 10));
		}
		public Operand VisitMULTINOMIAL_fun(mathParser.MULTINOMIAL_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function MULTINOMIAL parameter is error!"); }

			int sum = 0;
			int n = 1;
			for (int i = 0; i < list.Count; i++) {
				var a = (int)list[i];
				n *= F_base_Factorial(a);
				sum += a;
			}

			var r = F_base_Factorial(sum) / n;
			return Operand.Create(r);
		}
		public Operand VisitPRODUCT_fun(mathParser.PRODUCT_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function PRODUCT parameter is error!"); }

			double d = 1;
			for (int i = 0; i < list.Count; i++) {
				var a = list[i];
				d *= a;
			}

			return Operand.Create(d);
		}
		public Operand VisitSQRTPI_fun(mathParser.SQRTPI_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function SQRTPI parameter is error!"); if (args1.IsError) { return args1; } }
			return Operand.Create(Math.Sqrt(args1.NumberValue * Math.PI));
		}
		public Operand VisitSUMSQ_fun(mathParser.SUMSQ_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function SUMSQ parameter is error!"); }

			double d = 0;
			for (int i = 0; i < list.Count; i++) {
				var a = list[i];
				d += a * a;
			}

			return Operand.Create(d);
		}
		private int F_base_Factorial(int a)
		{
			if (a == 0) { return 1; }

			int r = 1;
			for (int i = a; i > 0; i--) {
				r *= i;
			}
			return r;
		}
		#endregion

		#endregion

		#region string

		public Operand VisitASC_fun(mathParser.ASC_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function ASC parameter is error!"); if (args1.IsError) { return args1; } }

			return Operand.Create(F_base_ToDBC(args1.TextValue));
		}
		public Operand VisitJIS_fun(mathParser.JIS_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function JIS parameter is error!"); if (args1.IsError) { return args1; } }

			return Operand.Create(F_base_ToSBC(args1.TextValue));
		}
		public Operand VisitCHAR_fun(mathParser.CHAR_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function CHAR parameter is error!"); if (args1.IsError) { return args1; } }

			char c = (char)(int)args1.NumberValue;
			return Operand.Create(c.ToString());
		}
		public Operand VisitCLEAN_fun(mathParser.CLEAN_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function CLEAN parameter is error!"); if (args1.IsError) { return args1; } }

			var t = args1.TextValue;
			ValueStringBuilder sb = new ValueStringBuilder(t.Length);
			for (int i = 0; i < t.Length; i++) {
				var c = t[i];
				if (c != '\f' && c != '\n' && c != '\r' && c != '\t' && c != '\v') {
					sb.Append(c);
				}
			}
			return Operand.Create(sb.ToString());
		}
		public Operand VisitCODE_fun(mathParser.CODE_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function CODE parameter is error!"); if (args1.IsError) { return args1; } }

			var t = args1.TextValue;
			if (t.Length == 0) { return Operand.Error("Function CODE parameter is error!"); }

			return Operand.Create((double)(int)(char)t[0]);
		}
		public Operand VisitCONCATENATE_fun(mathParser.CONCATENATE_funContext context)
		{
			var exprs = context.expr();
			ValueStringBuilder sb = new ValueStringBuilder();
			for (int i = 0; i < exprs.Length; i++) {
				var a = this.Visit(exprs[i]); if (a.Type != OperandType.TEXT) { a = a.ToText($"Function CONCATENATE parameter {i + 1} is error!"); if (a.IsError) { return a; } }
				sb.Append(a.TextValue);
			}
			return Operand.Create(sb.ToString());
		}
		public Operand VisitEXACT_fun(mathParser.EXACT_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function EXACT parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function EXACT parameter 2 is error!"); if (args2.IsError) { return args2; } }

			return Operand.Create(args1.TextValue == args2.TextValue);
		}
		public Operand VisitFIND_fun(mathParser.FIND_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function FIND parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function FIND parameter 2 is error!"); if (args2.IsError) { return args2; } }

			if (exprs.Length == 2) {
				var p = args2.TextValue.AsSpan().IndexOf(args1.TextValue) + excelIndex;
				return Operand.Create(p);
			}
			var count = this.Visit(exprs[2]).ToNumber("Function FIND parameter 3 is error!"); if (count.IsError) { return count; }
			var p2 = args2.TextValue.AsSpan(count.IntValue).IndexOf(args1.TextValue) + count.IntValue + excelIndex;
			return Operand.Create(p2);
		}
		public Operand VisitLEFT_fun(mathParser.LEFT_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function LEFT parameter 1 is error!"); if (args1.IsError) { return args1; } }

			if (exprs.Length == 1) {
				return Operand.Create(args1.TextValue[0].ToString());
			}
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function LEFT parameter 2 is error!"); if (args2.IsError) { return args2; } }
			return Operand.Create(args1.TextValue.AsSpan(0, args2.IntValue).ToString());
		}
		public Operand VisitLEN_fun(mathParser.LEN_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function LEN parameter is error!"); if (args1.IsError) { return args1; } }

			return Operand.Create(args1.TextValue.Length);
		}
		public Operand VisitLOWER_fun(mathParser.LOWER_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function LOWER/TOLOWER parameter is error!"); if (args1.IsError) { return args1; } }

			return Operand.Create(args1.TextValue.ToLower());
		}
		public Operand VisitMID_fun(mathParser.MID_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function MID parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function MID parameter 2 is error!"); if (args2.IsError) { return args2; } }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function MID parameter 3 is error!"); if (args3.IsError) { return args3; } }

			return Operand.Create(args1.TextValue.AsSpan(args2.IntValue - excelIndex, args3.IntValue).ToString());
		}
		public Operand VisitPROPER_fun(mathParser.PROPER_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function PROPER parameter is error!"); if (args1.IsError) { return args1; } }

			var text = args1.TextValue;
			ValueStringBuilder sb = new ValueStringBuilder(text.Length);
			bool isFirst = true;
			for (int i = 0; i < text.Length; i++) {
				var t = text[i];
				if (t == ' ' || t == '\r' || t == '\n' || t == '\t' || t == '.') {
					isFirst = true;
					sb.Append(t);
				} else if (isFirst) {
					sb.Append(char.ToUpper(t));
					isFirst = false;
				} else {
					sb.Append(t);
				}
			}
			return Operand.Create(sb.ToString());
		}
		public Operand VisitREPLACE_fun(mathParser.REPLACE_funContext context)
		{
			var exprs = context.expr();

			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function REPLACE parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var oldtext = args1.TextValue;
			if (exprs.Length == 3) {
				var args22 = this.Visit(exprs[1]); if (args22.Type != OperandType.TEXT) { args22 = args22.ToText("Function REPLACE parameter 2 is error!"); if (args22.IsError) { return args22; } }
				var args32 = this.Visit(exprs[2]); if (args32.Type != OperandType.TEXT) { args32 = args32.ToText("Function REPLACE parameter 3 is error!"); if (args32.IsError) { return args32; } }

				var old = args22.TextValue;
				var newstr = args32.TextValue;
				return Operand.Create(oldtext.Replace(old, newstr));
			}


			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function REPLACE parameter 2 is error!"); if (args2.IsError) { return args2; } }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function REPLACE parameter 3 is error!"); if (args3.IsError) { return args3; } }
			var args4 = this.Visit(exprs[3]); if (args4.Type != OperandType.TEXT) { args4 = args4.ToText("Function REPLACE parameter 4 is error!"); if (args4.IsError) { return args4; } }

			var start = args2.IntValue - excelIndex;
			var length = args3.IntValue;
			var newtext = args4.TextValue;

			ValueStringBuilder sb = new ValueStringBuilder(oldtext.Length + newtext.Length);
			for (int i = 0; i < oldtext.Length; i++) {
				if (i < start) {
					sb.Append(oldtext[i]);
				} else if (i == start) {
					sb.Append(newtext);
				} else if (i >= start + length) {
					sb.Append(oldtext[i]);
				}
			}
			return Operand.Create(sb.ToString());
		}
		public Operand VisitREPT_fun(mathParser.REPT_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function REPT parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function REPT parameter 2 is error!"); if (args2.IsError) { return args2; } }

			var newtext = args1.TextValue;
			var length = args2.IntValue;
			ValueStringBuilder sb = new ValueStringBuilder(newtext.Length * length);
			for (int i = 0; i < length; i++) {
				sb.Append(newtext);
			}
			return Operand.Create(sb.ToString());
		}
		public Operand VisitRIGHT_fun(mathParser.RIGHT_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function RIGHT parameter 1 is error!"); if (args1.IsError) { return args1; } }

			if (exprs.Length == 1) {
				return Operand.Create(args1.TextValue[args1.TextValue.Length - 1].ToString());
			}
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function RIGHT parameter 2 is error!"); if (args2.IsError) { return args2; } }
			return Operand.Create(args1.TextValue.AsSpan(args1.TextValue.Length - args2.IntValue, args2.IntValue).ToString());
		}
		public Operand VisitRMB_fun(mathParser.RMB_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function RMB parameter is error!"); if (args1.IsError) { return args1; } }

			return Operand.Create(F_base_ToChineseRMB(args1.NumberValue));
		}
		public Operand VisitSEARCH_fun(mathParser.SEARCH_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function SEARCH parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function SEARCH parameter 2 is error!"); if (args2.IsError) { return args2; } }

			if (exprs.Length == 2) {
				var p = args2.TextValue.AsSpan().IndexOf(args1.TextValue, StringComparison.OrdinalIgnoreCase) + excelIndex;
				return Operand.Create(p);
			}
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function SEARCH parameter 3 is error!"); if (args3.IsError) { return args3; } }
			var p2 = args2.TextValue.AsSpan(args3.IntValue).IndexOf(args1.TextValue, StringComparison.OrdinalIgnoreCase) + args3.IntValue + excelIndex;
			return Operand.Create(p2);
		}
		public Operand VisitSUBSTITUTE_fun(mathParser.SUBSTITUTE_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function SUBSTITUTE parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function SUBSTITUTE parameter 2 is error!"); if (args2.IsError) { return args2; } }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.TEXT) { args3 = args3.ToText("Function SUBSTITUTE parameter 3 is error!"); if (args3.IsError) { return args3; } }

			if (exprs.Length == 3) {
				return Operand.Create(args1.TextValue.Replace(args2.TextValue, args3.TextValue));
			}
			var args4 = this.Visit(exprs[3]); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function SUBSTITUTE parameter 4 is error!"); if (args4.IsError) { return args4; } }

			string text = args1.TextValue;
			string oldtext = args2.TextValue;
			string newtext = args3.TextValue;
			int index = args4.IntValue;

			int index2 = 0;
			ValueStringBuilder sb = new ValueStringBuilder(text.Length + newtext.Length);
			for (int i = 0; i < text.Length; i++) {
				bool b = true;
				for (int j = 0; j < oldtext.Length; j++) {
					var t = text[i + j];
					var t2 = oldtext[j];
					if (t != t2) {
						b = false;
						break;
					}
				}
				if (b) {
					index2++;
				}
				if (b && index2 == index) {
					sb.Append(newtext);
					i += oldtext.Length - 1;
				} else {
					sb.Append(text[i]);
				}
			}
			return Operand.Create(sb.ToString());
		}
		public Operand VisitT_fun(mathParser.T_funContext context)
		{
			var args1 = this.Visit(context.expr());
			if (args1.Type == OperandType.TEXT) {
				return args1;
			}
			return Operand.Create("");
		}
		public Operand VisitTEXT_fun(mathParser.TEXT_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.IsError) { return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function TEXT parameter 2 is error!"); if (args2.IsError) { return args2; } }

			if (args1.Type == OperandType.TEXT) {
				return args1;
			} else if (args1.Type == OperandType.BOOLEAN) {
				return Operand.Create(args1.BooleanValue ? "TRUE" : "FALSE");
			} else if (args1.Type == OperandType.NUMBER) {
				return Operand.Create(args1.NumberValue.ToString(args2.TextValue, cultureInfo));
			} else if (args1.Type == OperandType.DATE) {
				return Operand.Create(args1.DateValue.ToString(args2.TextValue));
			}
			args1 = args1.ToText("Function TEXT parameter 1 is error!"); if (args1.IsError) { return args1; }
			return Operand.Create(args1.TextValue.ToString());
		}
		public Operand VisitTRIM_fun(mathParser.TRIM_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function TRIM parameter is error!"); if (args1.IsError) { return args1; } }
			return Operand.Create(args1.TextValue.AsSpan().Trim().ToString());
		}
		public Operand VisitUPPER_fun(mathParser.UPPER_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function UPPER/TOUPPER parameter is error!"); if (args1.IsError) { return args1; } }
			return Operand.Create(args1.TextValue.ToUpper());
		}
		public Operand VisitVALUE_fun(mathParser.VALUE_funContext context)
		{
			var args1 = this.Visit(context.expr());
			if (args1.Type == OperandType.NUMBER) { return args1; }
			if (args1.Type == OperandType.BOOLEAN) { return args1.BooleanValue ? Operand.One : Operand.Zero; }
			if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function VALUE parameter is error!"); if (args1.IsError) { return args1; } }

			if (double.TryParse(args1.TextValue, NumberStyles.Any, cultureInfo, out double d)) {
				return Operand.Create(d);
			}
			return Operand.Error("Function VALUE parameter is error!");
		}

		private String F_base_ToSBC(String input)
		{
			ValueStringBuilder sb = new ValueStringBuilder(input.Length);
			for (int i = 0; i < input.Length; i++) {
				var c = input[i];
				if (c == ' ') {
					sb.Append((char)12288);
				} else if (c < 127) {
					sb.Append((char)(c + 65248));
				} else {
					sb.Append(c);
				}
			}
			return sb.ToString();
		}
		private String F_base_ToDBC(String input)
		{
			ValueStringBuilder sb = new ValueStringBuilder(input.Length);
			for (int i = 0; i < input.Length; i++) {
				var c = input[i];
				if (c == 12288) {
					sb.Append((char)32);
				} else if (c > 65280 && c < 65375) {
					sb.Append((char)(c - 65248));
				} else {
					sb.Append(c);
				}
			}
			return sb.ToString();
		}
		private string F_base_ToChineseRMB(double x)
		{
			string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A", cultureInfo);
			string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}", RegexOptions.Compiled);
			return Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString(), RegexOptions.Compiled);
		}
		#endregion

		#region MyDate time

		public Operand VisitDATEVALUE_fun(mathParser.DATEVALUE_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function DATEVALUE parameter is error!"); if (args1.IsError) { return args1; } }

			if (DateTime.TryParse(args1.TextValue, cultureInfo, DateTimeStyles.None, out DateTime dt)) {
				return Operand.Create(dt);
			}
			return Operand.Error("Function DATEVALUE parameter is error!");
		}
		public Operand VisitTIMEVALUE_fun(mathParser.TIMEVALUE_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function TIMEVALUE parameter is error!"); if (args1.IsError) { return args1; } }

			if (TimeSpan.TryParse(args1.TextValue, cultureInfo, out TimeSpan dt)) {
				return Operand.Create(dt);
			}
			return Operand.Error("Function TIMEVALUE parameter is error!");
		}
		public Operand VisitDATE_fun(mathParser.DATE_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function DATE parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function DATE parameter 2 is error!"); if (args2.IsError) { return args2; } }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function DATE parameter 3 is error!"); if (args3.IsError) { return args3; } }

			MyDate d;
			if (exprs.Length == 3) {
				d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, 0, 0, 0);
			} else if (exprs.Length == 4) {
				var args4 = this.Visit(exprs[3]); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function DATE parameter 4 is error!"); if (args4.IsError) { return args4; } }

				d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, 0, 0);
			} else if (exprs.Length == 5) {
				var args4 = this.Visit(exprs[3]); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function DATE parameter 4 is error!"); if (args4.IsError) { return args4; } }
				var args5 = this.Visit(exprs[4]); if (args5.Type != OperandType.NUMBER) { args5 = args5.ToNumber("Function DATE parameter 5 is error!"); if (args5.IsError) { return args5; } }
				d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, args5.IntValue, 0);
			} else {
				var args4 = this.Visit(exprs[3]); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function DATE parameter 4 is error!"); if (args4.IsError) { return args4; } }
				var args5 = this.Visit(exprs[4]); if (args5.Type != OperandType.NUMBER) { args5 = args5.ToNumber("Function DATE parameter 5 is error!"); if (args5.IsError) { return args5; } }
				var args6 = this.Visit(exprs[5]); if (args6.Type != OperandType.NUMBER) { args6 = args6.ToNumber("Function DATE parameter 6 is error!"); if (args6.IsError) { return args6; } }
				d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, args5.IntValue, args6.IntValue);
			}
			return Operand.Create(d);
		}
		public Operand VisitTIME_fun(mathParser.TIME_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function TIME parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function TIME parameter 2 is error!"); if (args2.IsError) { return args2; } }

			MyDate d;
			if (exprs.Length == 3) {
				var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function TIME parameter 3 is error!"); if (args3.IsError) { return args3; } }
				d = new MyDate(0, 0, 0, args1.IntValue, args2.IntValue, args3.IntValue);
			} else {
				d = new MyDate(0, 0, 0, args1.IntValue, args2.IntValue, 0);
			}
			return Operand.Create(d);
		}
		public Operand VisitNOW_fun(mathParser.NOW_funContext context)
		{
			return Operand.Create(new MyDate(DateTime.Now));
		}
		public Operand VisitTODAY_fun(mathParser.TODAY_funContext context)
		{
			return Operand.Create(new MyDate(DateTime.Today));
		}
		public Operand VisitYEAR_fun(mathParser.YEAR_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function YEAR parameter is error!"); if (args1.IsError) { return args1; } }
			if (args1.DateValue.Year == null) {
				return Operand.Error("Function YEAR is error!");
			}
			return Operand.Create(args1.DateValue.Year.Value);
		}
		public Operand VisitMONTH_fun(mathParser.MONTH_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function MONTH parameter is error!"); if (args1.IsError) { return args1; } }
			if (args1.DateValue.Month == null) {
				return Operand.Error("Function MONTH is error!");
			}
			return Operand.Create((int)args1.DateValue.Month.Value);
		}
		public Operand VisitDAY_fun(mathParser.DAY_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function DAY parameter is error!"); if (args1.IsError) { return args1; } }
			if (args1.DateValue.Day == null) {
				return Operand.Error("Function DAY is error!");
			}
			return Operand.Create(args1.DateValue.Day.Value);
		}
		public Operand VisitHOUR_fun(mathParser.HOUR_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function HOUR parameter is error!"); if (args1.IsError) { return args1; } }

			return Operand.Create(args1.DateValue.Hour);
		}
		public Operand VisitMINUTE_fun(mathParser.MINUTE_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function MINUTE parameter is error!"); if (args1.IsError) { return args1; } }

			return Operand.Create(args1.DateValue.Minute);
		}
		public Operand VisitSECOND_fun(mathParser.SECOND_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function SECOND parameter is error!"); if (args1.IsError) { return args1; } }

			return Operand.Create(args1.DateValue.Second);
		}
		public Operand VisitWEEKDAY_fun(mathParser.WEEKDAY_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function WEEKDAY parameter 1 is error!"); if (args1.IsError) { return args1; } }

			var type = 1;
			if (exprs.Length == 2) {
				var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function WEEKDAY parameter 2 is error!"); if (args2.IsError) { return args2; } }
				type = args2.IntValue;
			}

			var t = ((DateTime)args1.DateValue).DayOfWeek;
			if (type == 1) {
				return Operand.Create((double)(t + 1));
			} else if (type == 2) {
				if (t == 0) return Operand.Create(7d);
				return Operand.Create((double)t);
			}
			if (t == 0) {
				return Operand.Create(6d);
			}
			return Operand.Create((double)(t - 1));
		}
		public Operand VisitDATEDIF_fun(mathParser.DATEDIF_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function DATEDIF parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.DATE) { args2 = args2.ToMyDate("Function DATEDIF parameter 2 is error!"); if (args2.IsError) { return args2; } }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.TEXT) { args3 = args3.ToText("Function DATEDIF parameter 3 is error!"); if (args3.IsError) { return args3; } }

			var startMyDate = (DateTime)args1.DateValue;
			var endMyDate = (DateTime)args2.DateValue;
			var t = args3.TextValue.ToLower();

			if (CharUtil.Equals(t, 'Y')) {
				#region y
				bool b = false;
				if (startMyDate.Month < endMyDate.Month) {
					b = true;
				} else if (startMyDate.Month == endMyDate.Month) {
					if (startMyDate.Day <= endMyDate.Day) b = true;
				}
				if (b) {
					return Operand.Create((endMyDate.Year - startMyDate.Year));
				} else {
					return Operand.Create((endMyDate.Year - startMyDate.Year - 1));
				}
				#endregion
			} else if (CharUtil.Equals(t, 'M')) {
				#region m
				bool b = false;
				if (startMyDate.Day <= endMyDate.Day) b = true;
				if (b) {
					return Operand.Create((endMyDate.Year * 12 + endMyDate.Month - startMyDate.Year * 12 - startMyDate.Month));
				} else {
					return Operand.Create((endMyDate.Year * 12 + endMyDate.Month - startMyDate.Year * 12 - startMyDate.Month - 1));
				}
				#endregion
			} else if (CharUtil.Equals(t, 'D')) {
				return Operand.Create((endMyDate - startMyDate).Days);
			} else if (CharUtil.Equals(t, "YD")) {
				#region yd
				var day = endMyDate.DayOfYear - startMyDate.DayOfYear;
				if (endMyDate.Year > startMyDate.Year && day < 0) {
					var days = new DateTime(startMyDate.Year, 12, 31).DayOfYear;
					day = days + day;
				}
				return Operand.Create((day));
				#endregion
			} else if (CharUtil.Equals(t, "MD")) {
				#region md
				var mo = endMyDate.Day - startMyDate.Day;
				if (mo < 0) {
					int days;
					if (startMyDate.Month == 12) {
						days = new DateTime(startMyDate.Year + 1, 1, 1).AddDays(-1).Day;
					} else {
						days = new DateTime(startMyDate.Year, startMyDate.Month + 1, 1).AddDays(-1).Day;

					}
					mo += days;
				}
				return Operand.Create((mo));
				#endregion
			} else if (CharUtil.Equals(t, "YM")) {
				#region ym
				var mo = endMyDate.Month - startMyDate.Month;
				if (endMyDate.Day < startMyDate.Day) mo--;
				if (mo < 0) mo += 12;
				return Operand.Create((mo));
				#endregion
			}
			return Operand.Error("Function DATEDIF parameter 3 is error!");
		}
		public Operand VisitDAYS360_fun(mathParser.DAYS360_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function DAYS360 parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.DATE) { args2 = args2.ToMyDate("Function DAYS360 parameter 2 is error!"); if (args2.IsError) { return args2; } }

			var startMyDate = (DateTime)args1.DateValue;
			var endMyDate = (DateTime)args2.DateValue;

			var method = false;
			if (exprs.Length == 3) {
				var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.DATE) { args3 = args3.ToMyDate("Function DAYS360 parameter 3 is error!"); if (args3.IsError) { return args3; } }
				method = args3.BooleanValue;
			}
			var days = endMyDate.Year * 360 + (endMyDate.Month - 1) * 30
						- startMyDate.Year * 360 - (startMyDate.Month - 1) * 30;
			if (method) {
				if (endMyDate.Day == 31) days += 30;
				if (startMyDate.Day == 31) days -= 30;
			} else {
				if (startMyDate.Month == 12) {
					if (startMyDate.Day == new DateTime(startMyDate.Year + 1, 1, 1).AddDays(-1).Day) {
						days -= 30;
					} else {
						days -= startMyDate.Day;
					}
				} else {
					if (startMyDate.Day == new DateTime(startMyDate.Year, startMyDate.Month + 1, 1).AddDays(-1).Day) {
						days -= 30;
					} else {
						days -= startMyDate.Day;
					}
				}
				if (endMyDate.Month == 12) {
					if (endMyDate.Day == new DateTime(endMyDate.Year + 1, 1, 1).AddDays(-1).Day) {
						if (startMyDate.Day < 30) {
							days += 31;
						} else {
							days += 30;
						}
					} else {
						days += endMyDate.Day;
					}
				} else {
					if (endMyDate.Day == new DateTime(endMyDate.Year, endMyDate.Month + 1, 1).AddDays(-1).Day) {
						if (startMyDate.Day < 30) {
							days += 31;
						} else {
							days += 30;
						}
					} else {
						days += endMyDate.Day;
					}
				}
			}
			return Operand.Create(days);
		}
		public Operand VisitEDATE_fun(mathParser.EDATE_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function EDATE parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function EDATE parameter 2 is error!"); if (args2.IsError) { return args2; } }

			return Operand.Create((MyDate)(((DateTime)args1.DateValue).AddMonths(args2.IntValue)));
		}
		public Operand VisitEOMONTH_fun(mathParser.EOMONTH_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function EOMONTH parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function EOMONTH parameter 2 is error!"); if (args2.IsError) { return args2; } }

			var dt = ((DateTime)args1.DateValue).AddMonths(args2.IntValue + 1);
			dt = new DateTime(dt.Year, dt.Month, 1).AddDays(-1);
			return Operand.Create(dt);
		}
		public Operand VisitNETWORKDAYS_fun(mathParser.NETWORKDAYS_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var a = this.Visit(exprs[i]); if (a.Type != OperandType.DATE) { a = a.ToMyDate($"Function NETWORKDAYS parameter {i + 1} is error!"); if (a.IsError) { return a; } } args.Add(a); }

			var startMyDate = (DateTime)args[0].DateValue;
			var endMyDate = (DateTime)args[1].DateValue;

			List<DateTime> list = new List<DateTime>();
			for (int i = 2; i < args.Count; i++) {
				list.Add(args[i].DateValue);
			}

			var days = 0;
			while (startMyDate <= endMyDate) {
				if (startMyDate.DayOfWeek != DayOfWeek.Sunday && startMyDate.DayOfWeek != DayOfWeek.Saturday) {
					if (list.Contains(startMyDate) == false) {
						days++;
					}
				}
				startMyDate = startMyDate.AddDays(1);
			}
			return Operand.Create(days);
		}
		public Operand VisitWORKDAY_fun(mathParser.WORKDAY_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

			var args1 = args[0]; if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function WORKDAY parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = args[1]; if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function WORKDAY parameter 2 is error!"); if (args2.IsError) { return args2; } }


			var startMyDate = (DateTime)args1.DateValue;
			var days = args2.IntValue;
			List<DateTime> list = new List<DateTime>();
			for (int i = 2; i < args.Count; i++) {
				if (args[i].Type != OperandType.DATE) { args[i] = args[i].ToMyDate($"Function WORKDAY parameter {i + 1} is error!"); if (args[i].IsError) { return args[i]; } }
				list.Add(args[i].DateValue);
			}
			while (days > 0) {
				startMyDate = startMyDate.AddDays(1);
				if (startMyDate.DayOfWeek == DayOfWeek.Saturday) continue;
				if (startMyDate.DayOfWeek == DayOfWeek.Sunday) continue;
				if (list.Contains(startMyDate)) continue;
				days--;
			}
			return Operand.Create(startMyDate);
		}
		public Operand VisitWEEKNUM_fun(mathParser.WEEKNUM_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function WEEKNUM parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var startMyDate = (DateTime)args1.DateValue;

			var days = startMyDate.DayOfYear + (int)(new DateTime(startMyDate.Year, 1, 1).DayOfWeek);
			if (exprs.Length == 2) {
				var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function WEEKNUM parameter 2 is error!"); if (args2.IsError) { return args2; } }
				if (args2.IntValue == 2) {
					days--;
				}
			}

			var week = Math.Ceiling(days / 7.0);
			return Operand.Create(week);
		}

		#endregion

		#region sum

		public Operand VisitMAX_fun(mathParser.MAX_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.Type != OperandType.NUMBER) { aa = aa.ToNumber($"Function MAX parameter {i + 1} is error!"); if (aa.IsError) { return aa; } } args.Add(aa); }

			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function MAX parameter error!"); }

			return Operand.Create(list.Max());
		}
		public Operand VisitMEDIAN_fun(mathParser.MEDIAN_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.Type != OperandType.NUMBER) { aa = aa.ToNumber($"Function MEDIAN parameter {i + 1} is error!"); if (aa.IsError) { return aa; } } args.Add(aa); }

			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function MEDIAN parameter error!"); }

			list = list.OrderBy(q => q).ToList();
			return Operand.Create(list[list.Count / 2]);
		}
		public Operand VisitMIN_fun(mathParser.MIN_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.Type != OperandType.NUMBER) { aa = aa.ToNumber($"Function MIN parameter {i + 1} is error!"); if (aa.IsError) { return aa; } } args.Add(aa); }

			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function MIN parameter error!"); }

			return Operand.Create(list.Min());
		}
		public Operand VisitQUARTILE_fun(mathParser.QUARTILE_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.ARRARY) { args1 = args1.ToArray("Function QUARTILE parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function QUARTILE parameter 2 is error!"); if (args2.IsError) { return args2; } }

			List<double> list = new List<double>();
			var o = F_base_GetList(args1, list);
			if (o == false) { return Operand.Error("Function QUARTILE parameter 1 error!"); }

			var quant = args2.IntValue;
			if (quant < 0 || quant > 4) {
				return Operand.Error("Function QUARTILE parameter 2 is error!");
			}
			return Operand.Create(ExcelFunctions.Quartile(list.ToArray(), quant));
		}
		public Operand VisitMODE_fun(mathParser.MODE_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.Type != OperandType.NUMBER) { aa = aa.ToNumber($"Function MODE parameter {i + 1} is error!"); if (aa.IsError) { return aa; } } args.Add(aa); }

			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function MODE parameter error!"); }


			Dictionary<double, int> dict = new Dictionary<double, int>();
			for (int i = 0; i < list.Count; i++) {
				var item = list[i];
				if (dict.ContainsKey(item)) {
					dict[item] += 1;
				} else {
					dict[item] = 1;
				}
			}
			return Operand.Create(dict.OrderByDescending(q => q.Value).First().Key);
		}
		public Operand VisitLARGE_fun(mathParser.LARGE_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

			var args1 = args[0]; if (args1.Type != OperandType.ARRARY) { args1 = args1.ToArray("Function LARGE parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = args[1]; if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function LARGE parameter 2 is error!"); if (args2.IsError) { return args2; } }


			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function LARGE parameter error!"); }


			list = list.OrderByDescending(q => q).ToList();
			var quant = args2.IntValue;
			return Operand.Create(list[args2.IntValue - excelIndex]);
		}
		public Operand VisitSMALL_fun(mathParser.SMALL_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

			var args1 = args[0]; if (args1.Type != OperandType.ARRARY) { args1 = args1.ToArray("Function SMALL parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = args[1]; if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function SMALL parameter 2 is error!"); if (args2.IsError) { return args2; } }

			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function SMALL parameter error!"); }

			list = list.OrderBy(q => q).ToList();
			var quant = args2.IntValue;
			return Operand.Create(list[args2.IntValue - excelIndex]);
		}
		public Operand VisitPERCENTILE_fun(mathParser.PERCENTILE_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

			var args1 = args[0]; if (args1.Type != OperandType.ARRARY) { args1 = args1.ToArray("Function PERCENTILE parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = args[1]; if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function PERCENTILE parameter 2 is error!"); if (args2.IsError) { return args2; } }

			List<double> list = new List<double>();
			var o = F_base_GetList(args1, list);
			if (o == false) { return Operand.Error("Function PERCENTILE parameter error!"); }

			var k = args2.NumberValue;
			return Operand.Create(ExcelFunctions.Percentile(list.ToArray(), k));
		}
		public Operand VisitPERCENTRANK_fun(mathParser.PERCENTRANK_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

			var args1 = args[0]; if (args1.Type != OperandType.ARRARY) { args1 = args1.ToArray("Function PERCENTRANK parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = args[1]; if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function PERCENTRANK parameter 2 is error!"); if (args2.IsError) { return args2; } }

			List<double> list = new List<double>();
			var o = F_base_GetList(args1, list);
			if (o == false) { return Operand.Error("Function PERCENTRANK parameter error!"); }

			var k = args2.NumberValue;
			var v = ExcelFunctions.PercentRank(list.ToArray(), k);
			var d = 3;
			if (args.Count == 3) {
				var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function PERCENTRANK parameter 3 is error!"); if (args3.IsError) { return args3; } }
				d = args3.IntValue;
			}
			return Operand.Create(Math.Round(v, d, MidpointRounding.AwayFromZero));
		}
		public Operand VisitAVERAGE_fun(mathParser.AVERAGE_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function AVERAGE parameter error!"); }

			return Operand.Create(list.Average());
		}


		public Operand VisitAVERAGEIF_fun(mathParser.AVERAGEIF_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

			List<double> list = new List<double>();
			var o = F_base_GetList(args[0], list);
			if (o == false) { return Operand.Error("Function AVERAGE parameter 1 error!"); }

			List<double> sumdbs;
			if (args.Count == 3) {
				sumdbs = new List<double>();
				var o2 = F_base_GetList(args[2], sumdbs);
				if (o2 == false) { return Operand.Error("Function AVERAGE parameter 3 error!"); }
			} else {
				sumdbs = list;
			}

			double sum;
			int count;
			if (args[1].Type == OperandType.NUMBER) {
				count = F_base_countif(list, args[1].NumberValue);
				sum = count * args[1].NumberValue;
			} else {
				if (double.TryParse(args[1].TextValue.Trim(), NumberStyles.Any, cultureInfo, out double d)) {
					count = F_base_countif(list, d);
					sum = F_base_sumif(list, d, sumdbs);
				} else {
					var sunif = args[1].TextValue.Trim();
					var m2 = sumifMatch(sunif);
					if (m2 != null) {
						count = F_base_countif(list, m2.Item1, m2.Item2);
						sum = F_base_sumif(list, m2.Item1, m2.Item2, sumdbs);
					} else {
						return Operand.Error("Function AVERAGE parameter 2 error!");
					}
				}
			}
			return Operand.Create(sum / count);
		}
		public Operand VisitGEOMEAN_fun(mathParser.GEOMEAN_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

			if (args.Count == 1) return args[0];

			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function GEOMEAN parameter error!"); }

			double sum = 1;
			for (int i = 0; i < list.Count; i++) {
				var db = list[i];
				sum *= db;
			}
			return Operand.Create(Math.Pow(sum, 1.0 / list.Count));
		}
		public Operand VisitHARMEAN_fun(mathParser.HARMEAN_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

			if (args.Count == 1) return args[0];

			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function HARMEAN parameter error!"); }

			double sum = 0;
			for (int i = 0; i < list.Count; i++) {
				var db = list[i];
				if (db == 0) {
					return Operand.Error("Function HARMEAN parameter error!");
				}
				sum += 1 / db;
			}
			return Operand.Create(list.Count / sum);
		}
		public Operand VisitCOUNT_fun(mathParser.COUNT_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function COUNT parameter error!"); }

			return Operand.Create(list.Count);
		}
		public Operand VisitCOUNTIF_fun(mathParser.COUNTIF_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

			List<double> list = new List<double>();
			var o = F_base_GetList(args[0], list);
			if (o == false) { return Operand.Error("Function COUNTIF parameter error!"); }

			int count;
			if (args[1].Type == OperandType.NUMBER) {
				count = F_base_countif(list, args[1].NumberValue);
			} else {
				if (double.TryParse(args[1].TextValue.Trim(), NumberStyles.Any, cultureInfo, out double d)) {
					count = F_base_countif(list, d);
				} else {
					var sunif = args[1].TextValue.Trim();
					var m2 = sumifMatch(sunif);
					if (m2 != null) {
						count = F_base_countif(list, m2.Item1, m2.Item2);
					} else {
						return Operand.Error("Function COUNTIF parameter 2 error!");
					}
				}
			}
			return Operand.Create(count);
		}
		private Tuple<string, double> sumifMatch(string s)
		{
			var c = s[0];
			if (c == '>') {
				if (s.Length > 1 && s[1] == '=') {
					if (double.TryParse(s.AsSpan(2).Trim(), out double d)) {
						return Tuple.Create(">=", d);
					}
				} else if (double.TryParse(s.AsSpan(1).Trim(), out double d)) {
					return Tuple.Create(">", d);
				}
			} else if (c == '<') {
				if (s.Length > 1 && s[1] == '=') {
					if (double.TryParse(s.AsSpan(2).Trim(), out double d)) {
						return Tuple.Create("<=", d);
					}
				} else if (s.Length > 1 && s[1] == '>') {
					if (double.TryParse(s.AsSpan(2).Trim(), out double d)) {
						return Tuple.Create("!=", d);
					}
				} else if (double.TryParse(s.AsSpan(1).Trim(), out double d)) {
					return Tuple.Create("<", d);
				}
			} else if (c == '=') {
				var index = 1;
				if (s.Length > 1 && s[1] == '=') {
					index = 2;
					if (s.Length > 2 && s[2] == '=') {
						index = 3;
					}
				}
				if (double.TryParse(s.AsSpan(index).Trim(), out double d)) {
					return Tuple.Create("=", d);
				}
			} else if (c == '!') {
				var index = 1;
				if (s.Length > 1 && s[1] == '=') {
					index = 2;
					if (s.Length > 2 && s[2] == '=') {
						index = 3;
					}
				}
				if (double.TryParse(s.AsSpan(index).Trim(), out double d)) {
					return Tuple.Create("!=", d);
				}
			}
			return null;
		}

		public Operand VisitSUM_fun(mathParser.SUM_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) {
				var aa = this.Visit(exprs[i]); if (aa.Type != OperandType.NUMBER) { aa = aa.ToNumber($"Function SUM parameter {i + 1} error!"); if (aa.IsError) { return aa; } }
				args.Add(aa);
			}

			if (args.Count == 1) return args[0];

			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function SUM parameter error!"); }

			double sum = list.Sum();
			return Operand.Create(sum);
		}
		public Operand VisitSUMIF_fun(mathParser.SUMIF_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

			List<double> list = new List<double>();
			var o = F_base_GetList(args[0], list);
			if (o == false) { return Operand.Error("Function SUMIF parameter 1 error!"); }

			List<double> sumdbs;
			if (args.Count == 3) {
				sumdbs = new List<double>();
				var o2 = F_base_GetList(args[2], sumdbs);
				if (o2 == false) { return Operand.Error("Function SUMIF parameter 3 error!"); }
			} else {
				sumdbs = list;
			}

			double sum;
			if (args[1].Type == OperandType.NUMBER) {
				sum = F_base_countif(list, args[1].NumberValue) * args[1].NumberValue;
			} else {
				if (double.TryParse(args[1].TextValue.Trim(), NumberStyles.Any, cultureInfo, out double d)) {
					sum = F_base_sumif(list, d, sumdbs);
				} else {
					var sunif = args[1].TextValue.Trim();
					var m2 = sumifMatch(sunif);
					if (m2 != null) {
						sum = F_base_sumif(list, m2.Item1, m2.Item2, sumdbs);
					} else {
						return Operand.Error("Function SUMIF parameter 2 error!");
					}
				}
			}
			return Operand.Create(sum);
		}
		public Operand VisitAVEDEV_fun(mathParser.AVEDEV_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) {
				var aa = this.Visit(exprs[i]); if (aa.Type != OperandType.NUMBER) { aa = aa.ToNumber($"Function AVEDEV parameter {i + 1} error!"); if (aa.IsError) { return aa; } }
				args.Add(aa);
			}

			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function AVEDEV parameter error!"); }

			double avg = list.Average();
			double sum = 0;
			for (int i = 0; i < list.Count; i++) {
				sum += Math.Abs(list[i] - avg);
			}
			return Operand.Create(sum / list.Count);
		}

		public Operand VisitSTDEV_fun(mathParser.STDEV_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) {
				var aa = this.Visit(exprs[i]); if (aa.Type != OperandType.NUMBER) { aa = aa.ToNumber($"Function STDEV parameter {i + 1} error!"); if (aa.IsError) { return aa; } }
				args.Add(aa);
			}

			if (args.Count == 1) {
				return Operand.Error("Function STDEV parameter only one error!");
			}
			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function STDEV parameter error!"); }

			double avg = list.Average();
			double sum = 0;
			for (int i = 0; i < list.Count; i++) {
				sum += (list[i] - avg) * (list[i] - avg);
			}
			return Operand.Create(Math.Sqrt(sum / (list.Count - 1)));
		}
		public Operand VisitSTDEVP_fun(mathParser.STDEVP_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) {
				var aa = this.Visit(exprs[i]); if (aa.Type != OperandType.NUMBER) { aa = aa.ToNumber($"Function STDEVP parameter {i + 1} error!"); if (aa.IsError) { return aa; } }
				args.Add(aa);
			}

			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function STDEVP parameter error!"); }

			double sum = 0;
			double avg = list.Average();

			for (int i = 0; i < list.Count; i++) {
				sum += (list[i] - avg) * (list[i] - avg);
			}
			return Operand.Create(Math.Sqrt(sum / (list.Count)));
		}
		public Operand VisitDEVSQ_fun(mathParser.DEVSQ_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) {
				var aa = this.Visit(exprs[i]); if (aa.Type != OperandType.NUMBER) { aa = aa.ToNumber($"Function DEVSQ parameter {i + 1} error!"); if (aa.IsError) { return aa; } }
				args.Add(aa);
			}

			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function DEVSQ parameter error!"); }

			double avg = list.Average();
			double sum = 0;
			for (int i = 0; i < list.Count; i++) {
				sum += (list[i] - avg) * (list[i] - avg);
			}
			return Operand.Create(sum);
		}
		public Operand VisitVAR_fun(mathParser.VAR_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) {
				var aa = this.Visit(exprs[i]); if (aa.Type != OperandType.NUMBER) { aa = aa.ToNumber($"Function VAR parameter {i + 1} error!"); if (aa.IsError) { return aa; } }
				args.Add(aa);
			}

			if (args.Count == 1) {
				return Operand.Error("Function VAR parameter only one error!");
			}
			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function VAR parameter error!"); }

			double sum = 0;
			double sum2 = 0;
			for (int i = 0; i < list.Count; i++) {
				sum += list[i] * list[i];
				sum2 += list[i];
			}
			return Operand.Create((list.Count * sum - sum2 * sum2) / list.Count / (list.Count - 1));
		}
		public Operand VisitVARP_fun(mathParser.VARP_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) {
				var aa = this.Visit(exprs[i]); if (aa.Type != OperandType.NUMBER) { aa = aa.ToNumber($"Function VARP parameter {i + 1} error!"); if (aa.IsError) { return aa; } }
				args.Add(aa);
			}

			List<double> list = new List<double>();
			var o = F_base_GetList(args, list);
			if (o == false) { return Operand.Error("Function VARP parameter error!"); }

			double sum = 0;
			double avg = list.Average();
			for (int i = 0; i < list.Count; i++) {
				sum += (avg - list[i]) * (avg - list[i]);
			}
			return Operand.Create(sum / list.Count);
		}
		public Operand VisitNORMDIST_fun(mathParser.NORMDIST_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function NORMDIST parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function NORMDIST parameter 2 error!"); if (args2.IsError) return args2; }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function NORMDIST parameter 3 error!"); if (args3.IsError) return args3; }
			var args4 = this.Visit(exprs[3]); if (args4.Type != OperandType.BOOLEAN) { args4 = args4.ToBoolean("Function NORMDIST parameter 4 error!"); if (args4.IsError) return args4; }

			var num = args1.NumberValue;
			var avg = args2.NumberValue;
			var STDEV = args3.NumberValue;
			var b = args4.BooleanValue;
			return Operand.Create(ExcelFunctions.NormDist(num, avg, STDEV, b));
		}
		public Operand VisitNORMINV_fun(mathParser.NORMINV_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function NORMINV parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function NORMINV parameter 2 error!"); if (args2.IsError) return args2; }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function NORMINV parameter 3 error!"); if (args3.IsError) return args3; }

			var num = args1.NumberValue;
			var avg = args2.NumberValue;
			var STDEV = args3.NumberValue;

			return Operand.Create(ExcelFunctions.NormInv(num, avg, STDEV));
		}
		public Operand VisitNORMSDIST_fun(mathParser.NORMSDIST_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function NORMSDIST parameter 1 error!"); if (args1.IsError) { return args1; } }

			var k = args1.NumberValue;
			return Operand.Create(ExcelFunctions.NormSDist(k));
		}
		public Operand VisitNORMSINV_fun(mathParser.NORMSINV_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function NORMSINV parameter 1 error!"); if (args1.IsError) { return args1; } }

			var k = args1.NumberValue;
			return Operand.Create(ExcelFunctions.NormSInv(k));
		}
		public Operand VisitBETADIST_fun(mathParser.BETADIST_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function BETADIST parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function BETADIST parameter 2 error!"); if (args2.IsError) return args2; }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function BETADIST parameter 3 error!"); if (args3.IsError) return args3; }

			var x = args1.NumberValue;
			var alpha = args2.NumberValue;
			var beta = args3.NumberValue;

			if (alpha < 0.0 || beta < 0.0) {
				return Operand.Error("Function BETADIST parameter error!");
			}

			return Operand.Create(ExcelFunctions.BetaDist(x, alpha, beta));
		}
		public Operand VisitBETAINV_fun(mathParser.BETAINV_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function BETADIST parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function BETADIST parameter 2 error!"); if (args2.IsError) return args2; }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function BETADIST parameter 3 error!"); if (args3.IsError) return args3; }

			var probability = args1.NumberValue;
			var alpha = args2.NumberValue;
			var beta = args3.NumberValue;
			if (alpha < 0.0 || beta < 0.0 || probability < 0.0 || probability > 1.0) {
				return Operand.Error("Function BETAINV parameter error!");
			}
			return Operand.Create(ExcelFunctions.BetaInv(probability, alpha, beta));
		}
		public Operand VisitBINOMDIST_fun(mathParser.BINOMDIST_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function BINOMDIST parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function BINOMDIST parameter 2 error!"); if (args2.IsError) return args2; }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function BINOMDIST parameter 3 error!"); if (args3.IsError) return args3; }
			var args4 = this.Visit(exprs[3]); if (args4.Type != OperandType.BOOLEAN) { args4 = args4.ToBoolean("Function BINOMDIST parameter 4 error!"); if (args4.IsError) return args4; }

			if (!(args3.NumberValue >= 0.0 && args3.NumberValue <= 1.0 && args2.NumberValue >= 0)) {
				return Operand.Error("Function BINOMDIST parameter error!");
			}
			return Operand.Create(ExcelFunctions.BinomDist(args1.IntValue, args2.IntValue, args3.NumberValue, args4.BooleanValue));
		}
		public Operand VisitEXPONDIST_fun(mathParser.EXPONDIST_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function EXPONDIST parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function EXPONDIST parameter 2 error!"); if (args2.IsError) return args2; }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function EXPONDIST parameter 3 error!"); if (args3.IsError) return args3; }

			if (args1.NumberValue < 0.0) {
				return Operand.Error("Function EXPONDIST parameter error!");
			}

			return Operand.Create(ExcelFunctions.ExponDist(args1.NumberValue, args2.NumberValue, args3.BooleanValue));
		}
		public Operand VisitFDIST_fun(mathParser.FDIST_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FDIST parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function FDIST parameter 2 error!"); if (args2.IsError) return args2; }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function FDIST parameter 3 error!"); if (args3.IsError) return args3; }

			var x = args1.NumberValue;
			var degreesFreedom = args2.IntValue;
			var degreesFreedom2 = args3.IntValue;
			if (degreesFreedom <= 0.0 || degreesFreedom2 <= 0.0) {
				return Operand.Error("Function FDIST parameter error!");
			}
			return Operand.Create(ExcelFunctions.FDist(x, degreesFreedom, degreesFreedom2));
		}
		public Operand VisitFINV_fun(mathParser.FINV_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FINV parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function FINV parameter 2 error!"); if (args2.IsError) return args2; }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function FINV parameter 3 error!"); if (args3.IsError) return args3; }

			var probability = args1.NumberValue;
			var degreesFreedom = args2.IntValue;
			var degreesFreedom2 = args3.IntValue;
			if (degreesFreedom <= 0.0 || degreesFreedom2 <= 0.0) {
				return Operand.Error("Function FINV parameter error!");
			}
			return Operand.Create(ExcelFunctions.FInv(probability, degreesFreedom, degreesFreedom2));
		}
		public Operand VisitFISHER_fun(mathParser.FISHER_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FISHER parameter error!"); if (args1.IsError) { return args1; } }

			var x = args1.NumberValue;
			if (x >= 1 || x <= -1) {
				return Operand.Error("Function FISHER parameter error!");
			}
			var n = 0.5 * Math.Log((1 + x) / (1 - x));
			return Operand.Create(n);
		}
		public Operand VisitFISHERINV_fun(mathParser.FISHERINV_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FISHERINV parameter error!"); if (args1.IsError) { return args1; } }

			var x = args1.NumberValue;
			var n = (Math.Exp(2 * x) - 1) / (Math.Exp(2 * x) + 1);
			return Operand.Create(n);
		}
		public Operand VisitGAMMADIST_fun(mathParser.GAMMADIST_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function GAMMADIST parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function GAMMADIST parameter 2 error!"); if (args2.IsError) return args2; }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function GAMMADIST parameter 3 error!"); if (args3.IsError) return args3; }
			var args4 = this.Visit(exprs[3]); if (args4.Type != OperandType.BOOLEAN) { args4 = args4.ToBoolean("Function GAMMADIST parameter 4 error!"); if (args4.IsError) return args4; }


			var x = args1.NumberValue;
			var alpha = args2.NumberValue;
			var beta = args3.NumberValue;
			var cumulative = args4.BooleanValue;
			if (alpha < 0.0 || beta < 0.0) {
				return Operand.Error("Function GAMMADIST parameter error!");
			}
			return Operand.Create(ExcelFunctions.GammaDist(x, alpha, beta, cumulative));
		}
		public Operand VisitGAMMAINV_fun(mathParser.GAMMAINV_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function GAMMAINV parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function GAMMAINV parameter 2 error!"); if (args2.IsError) return args2; }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function GAMMAINV parameter 3 error!"); if (args3.IsError) return args3; }

			var probability = args1.NumberValue;
			var alpha = args2.NumberValue;
			var beta = args3.NumberValue;
			if (alpha < 0.0 || beta < 0.0 || probability < 0 || probability > 1.0) {
				return Operand.Error("Function GAMMAINV parameter error!");
			}
			return Operand.Create(ExcelFunctions.GammaInv(probability, alpha, beta));
		}
		public Operand VisitGAMMALN_fun(mathParser.GAMMALN_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function GAMMALN parameter error!"); if (args1.IsError) { return args1; } }

			return Operand.Create(ExcelFunctions.GAMMALN(args1.NumberValue));
		}
		public Operand VisitHYPGEOMDIST_fun(mathParser.HYPGEOMDIST_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function HYPGEOMDIST parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function HYPGEOMDIST parameter 2 error!"); if (args2.IsError) return args2; }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function HYPGEOMDIST parameter 3 error!"); if (args3.IsError) return args3; }
			var args4 = this.Visit(exprs[3]); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function HYPGEOMDIST parameter 4 error!"); if (args4.IsError) return args4; }

			int k = args1.IntValue;
			int draws = args2.IntValue;
			int success = args3.IntValue;
			int population = args4.IntValue;
			if (!(population >= 0 && success >= 0 && draws >= 0 && success <= population && draws <= population)) {
				return Operand.Error("Function HYPGEOMDIST parameter error!");
			}
			return Operand.Create(ExcelFunctions.HypgeomDist(k, draws, success, population));
		}
		public Operand VisitLOGINV_fun(mathParser.LOGINV_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function LOGINV parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function LOGINV parameter 2 error!"); if (args2.IsError) return args2; }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function LOGINV parameter 3 error!"); if (args3.IsError) return args3; }

			if (args3.NumberValue < 0.0) {
				return Operand.Error("Function LOGINV parameter error!");
			}
			return Operand.Create(ExcelFunctions.LogInv(args1.NumberValue, args2.NumberValue, args3.NumberValue));
		}
		public Operand VisitLOGNORMDIST_fun(mathParser.LOGNORMDIST_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function LOGNORMDIST parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function LOGNORMDIST parameter 2 error!"); if (args2.IsError) return args2; }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function LOGNORMDIST parameter 3 error!"); if (args3.IsError) return args3; }

			if (args3.NumberValue < 0.0) {
				return Operand.Error("Function LOGNORMDIST parameter error!");
			}
			return Operand.Create(ExcelFunctions.LognormDist(args1.NumberValue, args2.NumberValue, args3.NumberValue));
		}
		public Operand VisitNEGBINOMDIST_fun(mathParser.NEGBINOMDIST_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function NEGBINOMDIST parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function NEGBINOMDIST parameter 2 error!"); if (args2.IsError) return args2; }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function NEGBINOMDIST parameter 3 error!"); if (args3.IsError) return args3; }

			int k = args1.IntValue;
			double r = args2.NumberValue;
			double p = args3.NumberValue;

			if (!(r >= 0.0 && p >= 0.0 && p <= 1.0)) {
				return Operand.Error("Function NEGBINOMDIST parameter error!");
			}
			return Operand.Create(ExcelFunctions.NegbinomDist(k, r, p));
		}
		public Operand VisitPOISSON_fun(mathParser.POISSON_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function POISSON parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function POISSON parameter 2 error!"); if (args2.IsError) return args2; }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function POISSON parameter 3 error!"); if (args3.IsError) return args3; }

			int k = args1.IntValue;
			double lambda = args2.NumberValue;
			bool state = args3.BooleanValue;
			if (!(lambda > 0.0)) {
				return Operand.Error("Function POISSON parameter error!");
			}
			return Operand.Create(ExcelFunctions.POISSON(k, lambda, state));
		}
		public Operand VisitTDIST_fun(mathParser.TDIST_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function TDIST parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function TDIST parameter 2 error!"); if (args2.IsError) return args2; }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function TDIST parameter 3 error!"); if (args3.IsError) return args3; }

			var x = args1.NumberValue;
			var degreesFreedom = args2.IntValue;
			var tails = args3.IntValue;
			if (degreesFreedom <= 0.0 || tails < 1 || tails > 2) {
				return Operand.Error("Function TDIST parameter error!");
			}
			return Operand.Create(ExcelFunctions.TDist(x, degreesFreedom, tails));
		}
		public Operand VisitTINV_fun(mathParser.TINV_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function TDIST parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function TDIST parameter 2 error!"); if (args2.IsError) return args2; }

			var probability = args1.NumberValue;
			var degreesFreedom = args2.IntValue;
			if (degreesFreedom <= 0.0) {
				return Operand.Error("Function TINV parameter error!");
			}
			return Operand.Create(ExcelFunctions.TInv(probability, degreesFreedom));
		}
		public Operand VisitWEIBULL_fun(mathParser.WEIBULL_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function WEIBULL parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function WEIBULL parameter 2 error!"); if (args2.IsError) return args2; }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function WEIBULL parameter 3 error!"); if (args3.IsError) return args3; }
			var args4 = this.Visit(exprs[3]); if (args4.Type != OperandType.BOOLEAN) { args4 = args4.ToBoolean("Function WEIBULL parameter 4 error!"); if (args4.IsError) return args4; }

			var x = args1.NumberValue;
			var shape = args2.NumberValue;
			var scale = args3.NumberValue;
			var state = args4.BooleanValue;
			if (shape <= 0.0 || scale <= 0.0) {
				return Operand.Error("Function WEIBULL parameter error!");
			}
			return Operand.Create(ExcelFunctions.WEIBULL(x, shape, scale, state));
		}

		private int F_base_countif(List<double> dbs, double d)
		{
			int count = 0;
			d = Math.Round(d, 10, MidpointRounding.AwayFromZero);
			for (int i = 0; i < dbs.Count; i++) {
				var item = dbs[i];
				if (Math.Round(item, 10, MidpointRounding.AwayFromZero) == d) {
					count++;
				}
			}
			return count;
		}

		private int F_base_countif(List<double> dbs, string s, double d)
		{
			int count = 0;
			for (int i = 0; i < dbs.Count; i++) {
				var item = dbs[i];
				if (F_base_compare(item, d, s)) {
					count++;
				}
			}
			return count;
		}
		private double F_base_sumif(List<double> dbs, double d, List<double> sumdbs)
		{
			double sum = 0;
			for (int i = 0; i < dbs.Count; i++) {
				var item = dbs[i];
				if (Math.Round(item, 10, MidpointRounding.AwayFromZero) == d) {
					sum += item;
				}
			}
			return sum;
		}
		private double F_base_sumif(List<double> dbs, string s, double d, List<double> sumdbs)
		{
			double sum = 0;
			for (int i = 0; i < dbs.Count; i++) {
				if (F_base_compare(dbs[i], d, s)) {
					sum += sumdbs[i];
				}
			}
			return sum;
		}
		private bool F_base_compare(double a, double b, string ss)
		{
			if (ss.Length == 1) {
				if (CharUtil.Equals(ss, '<')) {
					return Math.Round(a - b, 10, MidpointRounding.AwayFromZero) < 0;
				} else if (CharUtil.Equals(ss, '>')) {
					return Math.Round(a - b, 10, MidpointRounding.AwayFromZero) > 0;
				} else /*if (CharUtil.Equals(ss, '='))*/ {
					return Math.Round(a - b, 10, MidpointRounding.AwayFromZero) == 0;
				}
			} else if (CharUtil.Equals(ss, "<=")) {
				return Math.Round(a - b, 10, MidpointRounding.AwayFromZero) <= 0;
			} else if (CharUtil.Equals(ss, ">=")) {
				return Math.Round(a - b, 10, MidpointRounding.AwayFromZero) >= 0;
			} else if (CharUtil.Equals(ss, "!=")) {
				return Math.Round(a - b, 10, MidpointRounding.AwayFromZero) != 0;
			} else if (CharUtil.Equals(ss, "==")) {
				return Math.Round(a - b, 10, MidpointRounding.AwayFromZero) == 0;
			} else if (CharUtil.Equals(ss, "===")) {
				return Math.Round(a - b, 10, MidpointRounding.AwayFromZero) == 0;
			}
			return Math.Round(a - b, 10, MidpointRounding.AwayFromZero) != 0;
		}

		private bool F_base_GetList(List<Operand> args, List<double> list)
		{
			for (int j = 0; j < args.Count; j++) {
				var item = args[j];
				if (item.Type == OperandType.NUMBER) {
					list.Add(item.NumberValue);
				} else if (item.Type == OperandType.ARRARY || item.Type == OperandType.ARRARYJSON) {
					var o = F_base_GetList(item.ArrayValue, list);
					if (o == false) { return false; }
				} else if (item.Type == OperandType.JSON) {
					var i = item.ToArray(null); if (i.IsError) { return false; }
					var o = F_base_GetList(i.ArrayValue, list);
					if (o == false) { return false; }
				} else {
					var o = item.ToNumber(null); if (o.IsError) { return false; }
					list.Add(o.NumberValue);
				}
			}
			return true;
		}
		private bool F_base_GetList(Operand args, List<double> list)
		{
			if (args.IsError) { return false; }
			if (args.Type == OperandType.NUMBER) {
				list.Add(args.NumberValue);
			} else if (args.Type == OperandType.ARRARY || args.Type == OperandType.ARRARYJSON) {
				var o = F_base_GetList(args.ArrayValue, list);
				if (o == false) { return false; }
			} else if (args.Type == OperandType.JSON) {
				var i = args.ToArray(null); if (i.IsError) { return false; }
				var o = F_base_GetList(i.ArrayValue, list);
				if (o == false) { return false; }
			} else {
				var o = args.ToNumber(null); if (o.IsError) { return false; }
				list.Add(o.NumberValue);
			}
			return true;
		}

		private bool F_base_GetList(Operand args, List<string> list)
		{
			if (args.IsError) { return false; }
			if (args.Type == OperandType.ARRARY || args.Type == OperandType.ARRARYJSON) {
				var o = F_base_GetList(args.ArrayValue, list);
				if (o == false) { return false; }
			} else if (args.Type == OperandType.JSON) {
				var i = args.ToArray(null); if (i.IsError) { return false; }
				var o = F_base_GetList(i.ArrayValue, list);
				if (o == false) { return false; }
			} else {
				var o = args.ToText(null); if (o.IsError) { return false; }
				list.Add(o.TextValue);
			}
			return true;
		}


		private bool F_base_GetList(List<Operand> args, List<string> list)
		{
			for (int j = 0; j < args.Count; j++) {
				var item = args[j];
				if (item.Type == OperandType.ARRARY || item.Type == OperandType.ARRARYJSON) {
					var o = F_base_GetList(item.ArrayValue, list);
					if (o == false) { return false; }
				} else if (item.Type == OperandType.JSON) {
					var i = item.ToArray(null); if (i.IsError) { return false; }
					var o = F_base_GetList(i.ArrayValue, list);
					if (o == false) { return false; }
				} else {
					var o = item.ToText(null); if (o.IsError) { return false; }
					list.Add(o.TextValue);
				}
			}
			return true;
		}

		#endregion

		#region csharp

		public Operand VisitREGEXREPALCE_fun(mathParser.REGEXREPALCE_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function REGEXREPALCE parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function REGEXREPALCE parameter 2 error!"); if (args2.IsError) return args2; }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.TEXT) { args3 = args3.ToText("Function REGEXREPALCE parameter 3 error!"); if (args3.IsError) return args3; }

			var b = Regex.Replace(args1.TextValue, args2.TextValue, args3.TextValue);
			return Operand.Create(b);
		}
		public Operand VisitISREGEX_fun(mathParser.ISREGEX_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function REGEXREPALCE parameter 1 error!"); if (args1.IsError) return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function REGEXREPALCE parameter 2 error!"); if (args2.IsError) return args2; }

			var b = Regex.IsMatch(args1.TextValue, args2.TextValue);
			return Operand.Create(b);
		}

		public Operand VisitTRIMSTART_fun(mathParser.TRIMSTART_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function TRIMSTART parameter 1 error!"); if (args1.IsError) return args1; }

			var text = args1.TextValue;
			if (exprs.Length == 2) {
				var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function TRIMSTART parameter 2 error!"); if (args2.IsError) return args2; }
				return Operand.Create(text.AsSpan().TrimStart(args2.TextValue.AsSpan()).ToString());
			}
			return Operand.Create(text.AsSpan().TrimStart().ToString());
		}

		public Operand VisitTRIMEND_fun(mathParser.TRIMEND_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function TRIMEND parameter 1 error!"); if (args1.IsError) return args1; }

			var text = args1.TextValue;
			if (exprs.Length == 2) {
				var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function TRIMEND parameter 2 error!"); if (args2.IsError) return args2; }
				return Operand.Create(text.AsSpan().TrimEnd(args2.TextValue.AsSpan()).ToString());
			}
			return Operand.Create(text.AsSpan().TrimEnd().ToString());
		}

		public Operand VisitINDEXOF_fun(mathParser.INDEXOF_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function INDEXOF parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function INDEXOF parameter 2 is error!"); if (args2.IsError) { return args2; } }

			var text = args1.TextValue;
			if (exprs.Length == 2) {
				return Operand.Create(text.AsSpan().IndexOf(args2.TextValue) + excelIndex);
			}
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.TEXT) { args3 = args3.ToText("Function INDEXOF parameter 3 is error!"); if (args3.IsError) { return args3; } }
			if (exprs.Length == 3) {
				return Operand.Create(text.AsSpan(args3.IntValue).IndexOf(args2.TextValue) + args3.IntValue + excelIndex);
			}
			var args4 = this.Visit(exprs[3]); if (args4.Type != OperandType.TEXT) { args4 = args4.ToText("Function INDEXOF parameter 4 is error!"); if (args4.IsError) { return args4; } }
			return Operand.Create(text.IndexOf(args2.TextValue, args3.IntValue, args4.IntValue) + excelIndex);
		}
		public Operand VisitLASTINDEXOF_fun(mathParser.LASTINDEXOF_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function LASTINDEXOF parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function LASTINDEXOF parameter 2 is error!"); if (args2.IsError) { return args2; } }

			var text = args1.TextValue;
			if (exprs.Length == 2) {
				return Operand.Create(text.AsSpan().LastIndexOf(args2.TextValue) + excelIndex);
			}
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.TEXT) { args3 = args3.ToText("Function LASTINDEXOF parameter 3 is error!"); if (args3.IsError) { return args3; } }
			if (exprs.Length == 3) {
				return Operand.Create(text.AsSpan(args3.IntValue).LastIndexOf(args2.TextValue) + args3.IntValue + excelIndex);
			}
			var args4 = this.Visit(exprs[3]); if (args4.Type != OperandType.TEXT) { args4 = args4.ToText("Function LASTINDEXOF parameter 4 is error!"); if (args4.IsError) { return args4; } }
			return Operand.Create(text.LastIndexOf(args2.TextValue, args3.IntValue, args4.IntValue) + excelIndex);
		}
		public Operand VisitSPLIT_fun(mathParser.SPLIT_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var a = this.Visit(exprs[i]); if (a.Type != OperandType.TEXT) { a = a.ToText($"Function SPLIT parameter {i + 1} is error!"); if (a.IsError) { return a; } } args.Add(a); }
			return Operand.Create(args[0].TextValue.Split(args[1].TextValue.ToArray()));
		}
		public Operand VisitJOIN_fun(mathParser.JOIN_funContext context)
		{
			var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
			for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

			var args1 = args[0]; if (args1.IsError) { return args1; }
			if (args1.Type == OperandType.JSON) {
				var o = args1.ToArray(null);
				if (o.IsError == false) {
					args1 = o;
				}
			}
			if (args1.Type == OperandType.ARRARY || args1.Type == OperandType.ARRARYJSON) {
				List<string> list = new List<string>();
				var o = F_base_GetList(args1, list);
				if (o == false) return Operand.Error("Function JOIN parameter 1 is error!");

				var args2 = args[1]; if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function JOIN parameter 2 is error!"); if (args2.IsError) { return args2; } }

				return Operand.Create(string.Join(args2.TextValue, list));
			} else {
				if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function JOIN parameter 1 is error!"); if (args1.IsError) { return args1; } }

				List<string> list = new List<string>();
				for (int i = 1; i < args.Count; i++) {
					var o = F_base_GetList(args[i], list);
					if (o == false) return Operand.Error($"Function JOIN parameter {i + 1} is error!");
				}

				return Operand.Create(string.Join(args1.TextValue, list));
			}
		}
		public Operand VisitSUBSTRING_fun(mathParser.SUBSTRING_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function SUBSTRING parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function SUBSTRING parameter 2 is error!"); if (args2.IsError) { return args2; } }

			var text = args1.TextValue;
			if (exprs.Length == 2) {
				return Operand.Create(text.AsSpan(args2.IntValue - excelIndex).ToString());
			}
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function SUBSTRING parameter 3 is error!"); if (args3.IsError) { return args3; } }
			return Operand.Create(text.AsSpan(args2.IntValue - excelIndex, args3.IntValue).ToString());
		}
		public Operand VisitSTARTSWITH_fun(mathParser.STARTSWITH_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function STARTSWITH parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function STARTSWITH parameter 2 is error!"); if (args2.IsError) { return args2; } }

			var text = args1.TextValue;
			if (exprs.Length == 2) {
				return Operand.Create(text.AsSpan().StartsWith(args2.TextValue.AsSpan()));
			}
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function STARTSWITH parameter 3 is error!"); if (args3.IsError) { return args3; } }
			return Operand.Create(text.AsSpan().StartsWith(args2.TextValue.AsSpan(), args3.BooleanValue ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
		}
		public Operand VisitENDSWITH_fun(mathParser.ENDSWITH_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function ENDSWITH parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function ENDSWITH parameter 2 is error!"); if (args2.IsError) { return args2; } }

			var text = args1.TextValue;
			if (exprs.Length == 2) {
				return Operand.Create(text.AsSpan().EndsWith(args2.TextValue.AsSpan()));
			}
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function ENDSWITH parameter 3 is error!"); if (args3.IsError) { return args3; } }
			return Operand.Create(text.AsSpan().EndsWith(args2.TextValue.AsSpan(), args3.BooleanValue ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
		}
		public Operand VisitISNULLOREMPTY_fun(mathParser.ISNULLOREMPTY_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function ISNULLOREMPTY parameter 1 is error!"); if (args1.IsError) { return args1; } }

			return Operand.Create(string.IsNullOrEmpty(args1.TextValue));
		}
		public Operand VisitISNULLORWHITESPACE_fun(mathParser.ISNULLORWHITESPACE_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function ISNULLORWHITESPACE parameter 1 is error!"); if (args1.IsError) { return args1; } }

			return Operand.Create(string.IsNullOrWhiteSpace(args1.TextValue));
		}
		public Operand VisitREMOVESTART_fun(mathParser.REMOVESTART_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function REMOVESTART parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function REMOVESTART parameter 2 is error!"); if (args2.IsError) { return args2; } }

			StringComparison comparison = StringComparison.Ordinal;
			if (exprs.Length == 3) {
				var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function REMOVESTART parameter 3 is error!"); if (args3.IsError) { return args3; } }
				if (args3.BooleanValue) {
					comparison = StringComparison.OrdinalIgnoreCase;
				}
			}
			var text = args1.TextValue;
			if (text.StartsWith(args2.TextValue, comparison)) {
				return Operand.Create(text.AsSpan(args2.TextValue.Length).ToString());
			}
			return args1;
		}
		public Operand VisitREMOVEEND_fun(mathParser.REMOVEEND_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function REMOVEEND parameter 1 is error!"); if (args1.IsError) { return args1; } }
			var args2 = this.Visit(exprs[1]); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function REMOVEEND parameter 2 is error!"); if (args2.IsError) { return args2; } }

			StringComparison comparison = StringComparison.Ordinal;
			if (exprs.Length == 3) {
				var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function REMOVESTART parameter 3 is error!"); if (args3.IsError) { return args3; } }
				if (args3.BooleanValue) {
					comparison = StringComparison.OrdinalIgnoreCase;
				}
			}
			var text = args1.TextValue;
			if (text.EndsWith(args2.TextValue, comparison)) {
				return Operand.Create(text.AsSpan(0, text.Length - args2.TextValue.Length).ToString());
			}
			return args1;
		}
		#endregion

		#region Lookup

		public Operand VisitLOOKUP_fun(mathParser.LOOKUP_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.IsError) { return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.IsError) { return args2; }
			if (args1.Type == OperandType.ARRARYJSON) {
				args2 = args2.ToNumber(); if (args2.IsError) { return args2; }
				var range = false;
				if (exprs.Length == 3) {
					var t = this.Visit(exprs[2]).ToBoolean("Function LOOKUP parameter 3 is error!"); if (t.IsError) { return t; }
					range = t.BooleanValue;
				}
				if (((OperandKeyValueList)args1).TryGetValueFloor(args2.NumberValue, range, out Operand operand)) {
					return operand;
				}
				return Operand.Error("Function LOOKUP not find !");
			}
			if (args1.Type != OperandType.ARRARY) { args1 = args1.ToArray("Function LOOKUP parameter 1 is error!"); if (args1.IsError) { return args1; } }
			if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function LOOKUP parameter 2 is error!"); if (args2.IsError) { return args2; } }
			var args3 = this.Visit(exprs[2]); if (args3.Type != OperandType.TEXT) { args3 = args3.ToText("Function LOOKUP parameter 3 is error!"); if (args3.IsError) { return args3; } }

			if (string.IsNullOrWhiteSpace(args2.TextValue)) { return Operand.Error("Function LOOKUP parameter 2 is null!"); }

			var engine = new AntlrLookupEngine();
			if (engine.Parse(args2.TextValue) == false) { return Operand.Error("Function LOOKUP parameter 2 Parse is error!"); }

			var arr = args1.ArrayValue;
			for (int i = 0; i < arr.Count; i++) {
				var item = arr[i];
				var json = item.ToJson(null);
				if (json.IsError == false) {
					engine.Json = json;
					try {
						var o = engine.Evaluate().ToBoolean(null);
						if (o.IsError == false) {
							if (o.BooleanValue) {
								var v = json.JsonValue[args3.TextValue];
								if (v != null) {
									if (v.IsString) return Operand.Create(v.StringValue);
									if (v.IsBoolean) return Operand.Create(v.BooleanValue);
									if (v.IsDouble) return Operand.Create(v.NumberValue);
									if (v.IsObject) return Operand.Create(v);
									if (v.IsArray) return Operand.Create(v);
									if (v.IsNull) return Operand.CreateNull();
									return Operand.Create(v);
								}
							}
						}
					} catch (Exception) { }
				}
			}
			return Operand.Error("Function LOOKUP not find!");
		}

		#endregion

		#region getValue
		public Operand VisitBracket_fun(mathParser.Bracket_funContext context)
		{
			return this.Visit(context.expr());
		}
		public Operand VisitNUM_fun(mathParser.NUM_funContext context)
		{
			var d = double.Parse(context.num().GetText(), NumberStyles.Any, cultureInfo);
			if (context.unit() == null) { return Operand.Create(d); }

			var unit = context.unit().GetText();
			var dict = NumberUnitTypeHelper.GetUnitTypedict();
			d = NumberUnitTypeHelper.TransformationUnit(d, dict[unit], DistanceUnit, AreaUnit, VolumeUnit, MassUnit);
			return Operand.Create(d);
		}
		public Operand VisitNum(mathParser.NumContext context)
		{
			var d = double.Parse(context.GetText(), NumberStyles.Any, cultureInfo);
			return Operand.Create(d);
		}
		public Operand VisitUnit(mathParser.UnitContext context)
		{
			string text = context.GetText();
			return Operand.Create(text);
		}

		public Operand VisitSTRING_fun(mathParser.STRING_funContext context)
		{
			var opd = context.GetText();
			ValueStringBuilder sb = new ValueStringBuilder(opd.Length - 2);
			int index = 1;
			while (index < opd.Length - 1) {
				var c = opd[index++];
				if (c == '\\') {
					var c2 = opd[index++];
					if (c2 == 'n') sb.Append('\n');
					else if (c2 == 'r') sb.Append('\r');
					else if (c2 == 't') sb.Append('\t');
					else if (c2 == '0') sb.Append('\0');
					else if (c2 == 'v') sb.Append('\v');
					else if (c2 == 'a') sb.Append('\a');
					else if (c2 == 'b') sb.Append('\b');
					else if (c2 == 'f') sb.Append('\f');
					else sb.Append(opd[index++]);
				} else {
					sb.Append(c);
				}
			}
			return Operand.Create(sb.ToString());
		}
		public Operand VisitNULL_fun(mathParser.NULL_funContext context)
		{
			return Operand.CreateNull();
		}
		public Operand VisitPARAMETER_fun(mathParser.PARAMETER_funContext context)
		{
			ITerminalNode node = context.PARAMETER();
			if (node != null) {
				return GetParameter(node.GetText());
			}
			node = context.PARAMETER2();
			if (node != null) {
				string str = node.GetText();
				if (str.StartsWith('@')) {
					return GetParameter(str.AsSpan(1).ToString());
				}
				return GetParameter(str.AsSpan(1, str.Length - 2).ToString());
			}
			//防止 多重引用 
			if (context.expr() != null) {
				var p = this.Visit(context.expr()); if (p.Type != OperandType.TEXT) { p = p.ToText("Function PARAMETER first parameter is error!"); if (p.IsError) return p; }

				if (GetParameter != null) {
					return GetParameter(p.TextValue);
				}
			}
			return Operand.Error("Function PARAMETER first parameter is error!");
		}


		public Operand VisitParameter2(mathParser.Parameter2Context context)
		{
			return Operand.Create(context.children[0].GetText());
		}

		#endregion

		public Operand VisitJSON_fun(mathParser.JSON_funContext context)
		{
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function JSON parameter is error!"); if (args1.IsError) { return args1; } }
			var txt = args1.TextValue.Trim();
			if ((txt.StartsWith('{') && txt.EndsWith('}')) || (txt.StartsWith('[') && txt.EndsWith(']'))) {
				try {
					var json = JsonMapper.ToObject(txt);
					return Operand.Create(json);
				} catch (Exception) { }
			}
			return Operand.Error("Function JSON parameter is error!");
		}
		public Operand VisitArrayJson_fun(mathParser.ArrayJson_funContext context)
		{
			OperandKeyValueList result = new OperandKeyValueList(null, excelIndex);
			var js = context.arrayJson();
			for (int i = 0; i < js.Length; i++) {
				var item = js[i];
				var aa = this.Visit(item); if (aa.IsError) { return aa; }
				result.AddValue(((OperandKeyValue)aa).Value);
			}
			return result;
		}

		public Operand VisitArrayJson(mathParser.ArrayJsonContext context)
		{
			KeyValue keyValue = new KeyValue();
			if (context.NUM() != null) {
				keyValue.Index = int.Parse(context.NUM().GetText());
			}
			if (context.STRING() != null) {
				var opd = context.STRING().GetText();
				ValueStringBuilder sb = new ValueStringBuilder(opd.Length - 2);
				int index = 1;
				while (index < opd.Length - 1) {
					var c = opd[index++];
					if (c == '\\') {
						var c2 = opd[index++];
						if (c2 == 'n') sb.Append('\n');
						else if (c2 == 'r') sb.Append('\r');
						else if (c2 == 't') sb.Append('\t');
						else if (c2 == '0') sb.Append('\0');
						else if (c2 == 'v') sb.Append('\v');
						else if (c2 == 'a') sb.Append('\a');
						else if (c2 == 'b') sb.Append('\b');
						else if (c2 == 'f') sb.Append('\f');
						else sb.Append(opd[index++]);
					} else {
						sb.Append(c);
					}
				}
				keyValue.Key = sb.ToString();
			}
			if (context.parameter2() != null) {
				keyValue.Key = context.parameter2().GetText();
			}
			keyValue.Value = Visit(context.expr());
			return new OperandKeyValue(keyValue);
		}
		public Operand VisitGetJsonValue_fun(mathParser.GetJsonValue_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.IsError) { return args1; }

			var obj = args1;
			Operand op;
			if (context.parameter2() != null) {
				op = this.Visit(context.parameter2());
			} else {
				op = this.Visit(exprs[1]);
				if (op.IsError) { op = Operand.Create(exprs[1].GetText()); }
			}

			if (obj.Type == OperandType.ARRARY) {
				if (op.Type != OperandType.NUMBER) { op = op.ToNumber("ARRARY index is error!"); if (op.IsError) { return op; } }

				var index = op.IntValue - excelIndex;
				if (index < obj.ArrayValue.Count)
					return obj.ArrayValue[index];
				return Operand.Error($"ARRARY index {index} greater than maximum length!");
			}
			if (obj.Type == OperandType.ARRARYJSON) {
				if (op.Type == OperandType.NUMBER) {
					if (((OperandKeyValueList)obj).TryGetValue(op.IntValue, out Operand operand)) {
						return operand;
					}
				} else if (op.Type == OperandType.TEXT) {
					if (((OperandKeyValueList)obj).TryGetValue(op.TextValue, out Operand operand)) {
						return operand;
					}
					return Operand.Error($"Parameter name `{op.TextValue}` is missing!");
				}
				return Operand.Error("Parameter name is missing!");
			}
			if (obj.Type == OperandType.JSON) {
				var json = obj.JsonValue;
				if (json.IsArray) {
					if (op.Type != OperandType.NUMBER) { op = op.ToNumber("JSON parameter index is error!"); if (op.IsError) { return op; } }
					var index = op.IntValue - excelIndex;
					if (index < json.Count) {
						var v = json[index];
						if (v.IsString) return Operand.Create(v.StringValue);
						if (v.IsBoolean) return Operand.Create(v.BooleanValue);
						if (v.IsDouble) return Operand.Create(v.NumberValue);
						if (v.IsObject) return Operand.Create(v);
						if (v.IsArray) return Operand.Create(v);
						if (v.IsNull) return Operand.CreateNull();
						return Operand.Create(v);
					}
					return Operand.Error($"JSON index {index} greater than maximum length!");
				} else {
					if (op.Type != OperandType.TEXT) { op = op.ToText("JSON parameter name is error!"); if (op.IsError) { return op; } }

					var v = json[op.TextValue];
					if (v != null) {
						if (v.IsString) return Operand.Create(v.StringValue);
						if (v.IsBoolean) return Operand.Create(v.BooleanValue);
						if (v.IsDouble) return Operand.Create(v.NumberValue);
						if (v.IsObject) return Operand.Create(v);
						if (v.IsArray) return Operand.Create(v);
						if (v.IsNull) return Operand.CreateNull();
						return Operand.Create(v);
					}
				}
			}
			return Operand.Error(" Operator is error!");
		}

		public Operand VisitHAS_fun(mathParser.HAS_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.IsError) { return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.IsError) { return args2; }

			if (((OperandKeyValueList)args1).ContainsValue(args2)) { return Operand.True; }
			return Operand.False;
		}

		public Operand VisitIN_fun(mathParser.IN_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.IsError) { return args1; }
			var args2 = this.Visit(exprs[1]); if (args2.IsError) { return args2; }

			if (args1.Type == OperandType.NUMBER) {
				if (((OperandKeyValueList)args2).ContainsValue(args1.IntValue)) {
					return Operand.True;
				}
			} else if (args1.Type == OperandType.TEXT) {
				if (((OperandKeyValueList)args2).ContainsValue(args1.TextValue)) {
					return Operand.True;
				}
			}
			return Operand.False;
		}
		public Operand VisitERROR_fun(mathParser.ERROR_funContext context)
		{
			if (context.expr() == null) { return Operand.Error(""); }
			var args1 = this.Visit(context.expr()); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText(); if (args1.IsError) return args1; }
			return Operand.Error(args1.TextValue);
		}

		public Operand VisitPARAM_fun(mathParser.PARAM_funContext context)
		{
			var exprs = context.expr();
			var args1 = this.Visit(exprs[0]); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText(); if (args1.IsError) return args1; }
			var result = GetParameter(args1.TextValue);
			if (result.IsError) {
				if (exprs.Length == 2) {
					return this.Visit(exprs[1]);
				}
			}
			return result;
		}
	}
}
