#region License

// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

#endregion

#if HAVE_BIG_INTEGER
using System.Numerics;
#endif
#if !HAVE_GUID_TRY_PARSE

using System.Text.RegularExpressions;

#endif

namespace ToolGood.JsonObject.Utilities
{
	internal enum PrimitiveTypeCode
	{
		Empty = 0,
		Object = 1,
		Char = 2,
		CharNullable = 3,
		Boolean = 4,
		BooleanNullable = 5,
		SByte = 6,
		SByteNullable = 7,
		Int16 = 8,
		Int16Nullable = 9,
		UInt16 = 10,
		UInt16Nullable = 11,
		Int32 = 12,
		Int32Nullable = 13,
		Byte = 14,
		ByteNullable = 15,
		UInt32 = 16,
		UInt32Nullable = 17,
		Int64 = 18,
		Int64Nullable = 19,
		UInt64 = 20,
		UInt64Nullable = 21,
		Single = 22,
		SingleNullable = 23,
		Double = 24,
		DoubleNullable = 25,
		DateTime = 26,
		DateTimeNullable = 27,
		DateTimeOffset = 28,
		DateTimeOffsetNullable = 29,
		Decimal = 30,
		DecimalNullable = 31,
		Guid = 32,
		GuidNullable = 33,
		TimeSpan = 34,
		TimeSpanNullable = 35,
		BigInteger = 36,
		BigIntegerNullable = 37,
		Uri = 38,
		String = 39,
		Bytes = 40,
		DBNull = 41
	}

	internal enum ParseResult
	{
		None = 0,
		Success = 1,
		Overflow = 2,
		Invalid = 3
	}

	internal static class ConvertUtils
	{
		private static readonly Dictionary<Type, PrimitiveTypeCode> TypeCodeMap =
			new Dictionary<Type, PrimitiveTypeCode>
			{
				{ typeof(char), PrimitiveTypeCode.Char },
				{ typeof(char?), PrimitiveTypeCode.CharNullable },
				{ typeof(bool), PrimitiveTypeCode.Boolean },
				{ typeof(bool?), PrimitiveTypeCode.BooleanNullable },
				{ typeof(sbyte), PrimitiveTypeCode.SByte },
				{ typeof(sbyte?), PrimitiveTypeCode.SByteNullable },
				{ typeof(short), PrimitiveTypeCode.Int16 },
				{ typeof(short?), PrimitiveTypeCode.Int16Nullable },
				{ typeof(ushort), PrimitiveTypeCode.UInt16 },
				{ typeof(ushort?), PrimitiveTypeCode.UInt16Nullable },
				{ typeof(int), PrimitiveTypeCode.Int32 },
				{ typeof(int?), PrimitiveTypeCode.Int32Nullable },
				{ typeof(byte), PrimitiveTypeCode.Byte },
				{ typeof(byte?), PrimitiveTypeCode.ByteNullable },
				{ typeof(uint), PrimitiveTypeCode.UInt32 },
				{ typeof(uint?), PrimitiveTypeCode.UInt32Nullable },
				{ typeof(long), PrimitiveTypeCode.Int64 },
				{ typeof(long?), PrimitiveTypeCode.Int64Nullable },
				{ typeof(ulong), PrimitiveTypeCode.UInt64 },
				{ typeof(ulong?), PrimitiveTypeCode.UInt64Nullable },
				{ typeof(float), PrimitiveTypeCode.Single },
				{ typeof(float?), PrimitiveTypeCode.SingleNullable },
				{ typeof(double), PrimitiveTypeCode.Double },
				{ typeof(double?), PrimitiveTypeCode.DoubleNullable },
				{ typeof(DateTime), PrimitiveTypeCode.DateTime },
				{ typeof(DateTime?), PrimitiveTypeCode.DateTimeNullable },

				{ typeof(decimal), PrimitiveTypeCode.Decimal },
				{ typeof(decimal?), PrimitiveTypeCode.DecimalNullable },
				{ typeof(Guid), PrimitiveTypeCode.Guid },
				{ typeof(Guid?), PrimitiveTypeCode.GuidNullable },
				{ typeof(TimeSpan), PrimitiveTypeCode.TimeSpan },
				{ typeof(TimeSpan?), PrimitiveTypeCode.TimeSpanNullable },

				{ typeof(Uri), PrimitiveTypeCode.Uri },
				{ typeof(string), PrimitiveTypeCode.String },
				{ typeof(byte[]), PrimitiveTypeCode.Bytes },
			};

		public static PrimitiveTypeCode GetTypeCode(Type t)
		{
			return GetTypeCode(t, out _);
		}

		public static PrimitiveTypeCode GetTypeCode(Type t, out bool isEnum)
		{
			if (TypeCodeMap.TryGetValue(t, out PrimitiveTypeCode typeCode)) {
				isEnum = false;
				return typeCode;
			}

			if (t.IsEnum) {
				isEnum = true;
				return GetTypeCode(Enum.GetUnderlyingType(t));
			}

			isEnum = false;
			return PrimitiveTypeCode.Object;
		}

		public static TimeSpan ParseTimeSpan(string input)
		{
			return TimeSpan.Parse(input);
		}

		public static ParseResult Int32TryParse(char[] chars, int start, int length, out int value)
		{
			value = 0;

			if (length == 0) {
				return ParseResult.Invalid;
			}

			bool isNegative = chars[start] == '-';

			if (isNegative) {
				// text just a negative sign
				if (length == 1) {
					return ParseResult.Invalid;
				}

				start++;
				length--;
			}

			int end = start + length;

			// Int32.MaxValue and MinValue are 10 chars
			// Or is 10 chars and start is greater than two
			// Need to improve this!
			if (length > 10 || length == 10 && chars[start] - '0' > 2) {
				// invalid result takes precedence over overflow
				for (int i = start; i < end; i++) {
					int c = chars[i] - '0';

					if (c < 0 || c > 9) {
						return ParseResult.Invalid;
					}
				}

				return ParseResult.Overflow;
			}

			for (int i = start; i < end; i++) {
				int c = chars[i] - '0';

				if (c < 0 || c > 9) {
					return ParseResult.Invalid;
				}

				int newValue = 10 * value - c;

				// overflow has caused the number to loop around
				if (newValue > value) {
					i++;

					// double check the rest of the string that there wasn't anything invalid
					// invalid result takes precedence over overflow result
					for (; i < end; i++) {
						c = chars[i] - '0';

						if (c < 0 || c > 9) {
							return ParseResult.Invalid;
						}
					}

					return ParseResult.Overflow;
				}

				value = newValue;
			}

			// go from negative to positive to avoids overflow
			// negative can be slightly bigger than positive
			if (!isNegative) {
				// negative integer can be one bigger than positive
				if (value == int.MinValue) {
					return ParseResult.Overflow;
				}

				value = -value;
			}

			return ParseResult.Success;
		}

		public static ParseResult Int64TryParse(char[] chars, int start, int length, out long value)
		{
			value = 0;

			if (length == 0) {
				return ParseResult.Invalid;
			}

			bool isNegative = chars[start] == '-';

			if (isNegative) {
				// text just a negative sign
				if (length == 1) {
					return ParseResult.Invalid;
				}

				start++;
				length--;
			}

			int end = start + length;

			// Int64.MaxValue and MinValue are 19 chars
			if (length > 19) {
				// invalid result takes precedence over overflow
				for (int i = start; i < end; i++) {
					int c = chars[i] - '0';

					if (c < 0 || c > 9) {
						return ParseResult.Invalid;
					}
				}

				return ParseResult.Overflow;
			}

			for (int i = start; i < end; i++) {
				int c = chars[i] - '0';

				if (c < 0 || c > 9) {
					return ParseResult.Invalid;
				}

				long newValue = 10 * value - c;

				// overflow has caused the number to loop around
				if (newValue > value) {
					i++;

					// double check the rest of the string that there wasn't anything invalid
					// invalid result takes precedence over overflow result
					for (; i < end; i++) {
						c = chars[i] - '0';

						if (c < 0 || c > 9) {
							return ParseResult.Invalid;
						}
					}

					return ParseResult.Overflow;
				}

				value = newValue;
			}

			// go from negative to positive to avoids overflow
			// negative can be slightly bigger than positive
			if (!isNegative) {
				// negative integer can be one bigger than positive
				if (value == long.MinValue) {
					return ParseResult.Overflow;
				}

				value = -value;
			}

			return ParseResult.Success;
		}

		public static ParseResult DecimalTryParse(char[] chars, int start, int length, out decimal value)
		{
			value = 0M;
			const decimal decimalMaxValueHi28 = 7922816251426433759354395033M;
			const ulong decimalMaxValueHi19 = 7922816251426433759UL;
			const ulong decimalMaxValueLo9 = 354395033UL;
			const char decimalMaxValueLo1 = '5';

			if (length == 0) {
				return ParseResult.Invalid;
			}

			bool isNegative = chars[start] == '-';
			if (isNegative) {
				// text just a negative sign
				if (length == 1) {
					return ParseResult.Invalid;
				}

				start++;
				length--;
			}

			int i = start;
			int end = start + length;
			int numDecimalStart = end;
			int numDecimalEnd = end;
			int exponent = 0;
			ulong hi19 = 0UL;
			ulong lo10 = 0UL;
			int mantissaDigits = 0;
			int exponentFromMantissa = 0;
			char? digit29 = null;
			bool? storeOnly28Digits = null;
			for (; i < end; i++) {
				char c = chars[i];
				switch (c) {
					case '.':
						if (i == start) {
							return ParseResult.Invalid;
						}
						if (i + 1 == end) {
							return ParseResult.Invalid;
						}

						if (numDecimalStart != end) {
							// multiple decimal points
							return ParseResult.Invalid;
						}

						numDecimalStart = i + 1;
						break;

					case 'e':
					case 'E':
						if (i == start) {
							return ParseResult.Invalid;
						}
						if (i == numDecimalStart) {
							// E follows decimal point
							return ParseResult.Invalid;
						}
						i++;
						if (i == end) {
							return ParseResult.Invalid;
						}

						if (numDecimalStart < end) {
							numDecimalEnd = i - 1;
						}

						c = chars[i];
						bool exponentNegative = false;
						switch (c) {
							case '-':
								exponentNegative = true;
								i++;
								break;

							case '+':
								i++;
								break;
						}

						// parse 3 digit
						for (; i < end; i++) {
							c = chars[i];
							if (c < '0' || c > '9') {
								return ParseResult.Invalid;
							}

							int newExponent = 10 * exponent + (c - '0');
							// stops updating exponent when overflowing
							if (exponent < newExponent) {
								exponent = newExponent;
							}
						}

						if (exponentNegative) {
							exponent = -exponent;
						}
						break;

					default:
						if (c < '0' || c > '9') {
							return ParseResult.Invalid;
						}

						if (i == start && c == '0') {
							i++;
							if (i != end) {
								c = chars[i];
								if (c == '.') {
									goto case '.';
								}
								if (c == 'e' || c == 'E') {
									goto case 'E';
								}

								return ParseResult.Invalid;
							}
						}

						if (mantissaDigits < 29 && (mantissaDigits != 28 || !(storeOnly28Digits ?? (storeOnly28Digits = hi19 > decimalMaxValueHi19 || hi19 == decimalMaxValueHi19 && (lo10 > decimalMaxValueLo9 || lo10 == decimalMaxValueLo9 && c > decimalMaxValueLo1)).GetValueOrDefault()))) {
							if (mantissaDigits < 19) {
								hi19 = hi19 * 10UL + (ulong)(c - '0');
							} else {
								lo10 = lo10 * 10UL + (ulong)(c - '0');
							}
							++mantissaDigits;
						} else {
							if (!digit29.HasValue) {
								digit29 = c;
							}
							++exponentFromMantissa;
						}
						break;
				}
			}

			exponent += exponentFromMantissa;

			// correct the decimal point
			exponent -= numDecimalEnd - numDecimalStart;

			if (mantissaDigits <= 19) {
				value = hi19;
			} else {
				value = hi19 / new decimal(1, 0, 0, false, (byte)(mantissaDigits - 19)) + lo10;
			}

			if (exponent > 0) {
				mantissaDigits += exponent;
				if (mantissaDigits > 29) {
					return ParseResult.Overflow;
				}
				if (mantissaDigits == 29) {
					if (exponent > 1) {
						value /= new decimal(1, 0, 0, false, (byte)(exponent - 1));
						if (value > decimalMaxValueHi28) {
							return ParseResult.Overflow;
						}
					} else if (value == decimalMaxValueHi28 && digit29 > decimalMaxValueLo1) {
						return ParseResult.Overflow;
					}
					value *= 10M;
				} else {
					value /= new decimal(1, 0, 0, false, (byte)exponent);
				}
			} else {
				if (digit29 >= '5' && exponent >= -28) {
					++value;
				}
				if (exponent < 0) {
					if (mantissaDigits + exponent + 28 <= 0) {
						value = isNegative ? -0M : 0M;
						return ParseResult.Success;
					}
					if (exponent >= -28) {
						value *= new decimal(1, 0, 0, false, (byte)-exponent);
					} else {
						value /= 1e28M;
						value *= new decimal(1, 0, 0, false, (byte)(-exponent - 28));
					}
				}
			}

			if (isNegative) {
				value = -value;
			}

			return ParseResult.Success;
		}

		public static bool TryConvertGuid(string s, out Guid g)
		{
			if (s == null) {
				throw new ArgumentNullException("s");
			}

			Regex format = new Regex("^[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}$");
			Match match = format.Match(s);
			if (match.Success) {
				g = new Guid(s);
				return true;
			}

			g = Guid.Empty;
			return false;
		}

		public static bool TryHexTextToInt(char[] text, int start, int end, out int value)
		{
			value = 0;

			for (int i = start; i < end; i++) {
				char ch = text[i];
				int chValue;

				if (ch <= 57 && ch >= 48) {
					chValue = ch - 48;
				} else if (ch <= 70 && ch >= 65) {
					chValue = ch - 55;
				} else if (ch <= 102 && ch >= 97) {
					chValue = ch - 87;
				} else {
					value = 0;
					return false;
				}

				value += chValue << (end - 1 - i) * 4;
			}

			return true;
		}
	}
}