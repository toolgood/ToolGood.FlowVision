using System.Globalization;
using ToolGood.JsonObject.Commens;
using ToolGood.JsonObject.Settings;
using ToolGood.JsonObject.Utilities;

namespace ToolGood.JsonObject
{
	/// <summary>
	/// Represents a value in JSON (string, integer, date, etc).
	/// </summary>
	public partial class JValue : JToken, IEquatable<JValue>, IComparable, IComparable<JValue>
	{
		private JTokenType _valueType;
		private object _value;

		internal JValue(object value, JTokenType type)
		{
			_value = value;
			_valueType = type;
		}

		internal JValue(JValue other, JsonCloneSettings settings)
			: this(other.Value, other.Type)
		{
			if (settings?.CopyAnnotations ?? true) {
				CopyAnnotations(this, other);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JValue"/> class from another <see cref="JValue"/> object.
		/// </summary>
		/// <param name="other">A <see cref="JValue"/> object to copy from.</param>
		public JValue(JValue other)
			: this(other.Value, other.Type)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JValue"/> class with the given value.
		/// </summary>
		/// <param name="value">The value.</param>
		public JValue(long value)
			: this(BoxedPrimitives.Get(value), JTokenType.Integer)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JValue"/> class with the given value.
		/// </summary>
		/// <param name="value">The value.</param>
		public JValue(decimal value)
			: this(BoxedPrimitives.Get(value), JTokenType.Float)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JValue"/> class with the given value.
		/// </summary>
		/// <param name="value">The value.</param>
		public JValue(char value)
			: this(value, JTokenType.String)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JValue"/> class with the given value.
		/// </summary>
		/// <param name="value">The value.</param>
		//[CLSCompliant(false)]
		public JValue(ulong value)
			: this(value, JTokenType.Integer)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JValue"/> class with the given value.
		/// </summary>
		/// <param name="value">The value.</param>
		public JValue(double value)
			: this(BoxedPrimitives.Get(value), JTokenType.Float)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JValue"/> class with the given value.
		/// </summary>
		/// <param name="value">The value.</param>
		public JValue(float value)
			: this(value, JTokenType.Float)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JValue"/> class with the given value.
		/// </summary>
		/// <param name="value">The value.</param>
		public JValue(DateTime value)
			: this(value, JTokenType.Date)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JValue"/> class with the given value.
		/// </summary>
		/// <param name="value">The value.</param>
		public JValue(bool value)
			: this(BoxedPrimitives.Get(value), JTokenType.Boolean)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JValue"/> class with the given value.
		/// </summary>
		/// <param name="value">The value.</param>
		public JValue(string value)
			: this(value, JTokenType.String)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JValue"/> class with the given value.
		/// </summary>
		/// <param name="value">The value.</param>
		public JValue(Guid value)
			: this(value, JTokenType.Guid)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JValue"/> class with the given value.
		/// </summary>
		/// <param name="value">The value.</param>
		public JValue(Uri value)
			: this(value, value != null ? JTokenType.Uri : JTokenType.Null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JValue"/> class with the given value.
		/// </summary>
		/// <param name="value">The value.</param>
		public JValue(TimeSpan value)
			: this(value, JTokenType.TimeSpan)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JValue"/> class with the given value.
		/// </summary>
		/// <param name="value">The value.</param>
		public JValue(object value)
			: this(value, GetValueType(null, value))
		{
		}

		internal override bool DeepEquals(JToken node)
		{
			if (!(node is JValue other)) {
				return false;
			}
			if (other == this) {
				return true;
			}

			return ValuesEquals(this, other);
		}

		/// <summary>
		/// Gets a value indicating whether this token has child tokens.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this token has child values; otherwise, <c>false</c>.
		/// </value>
		public override bool HasValues => false;

		internal static int Compare(JTokenType valueType, object objA, object objB)
		{
			if (objA == objB) {
				return 0;
			}
			if (objB == null) {
				return 1;
			}
			if (objA == null) {
				return -1;
			}

			switch (valueType) {
				case JTokenType.Integer: {
						if (objA is ulong || objB is ulong || objA is decimal || objB is decimal) {
							return Convert.ToDecimal(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToDecimal(objB, CultureInfo.InvariantCulture));
						} else if (objA is float || objB is float || objA is double || objB is double) {
							return CompareFloat(objA, objB);
						} else {
							return Convert.ToInt64(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToInt64(objB, CultureInfo.InvariantCulture));
						}
					}
				case JTokenType.Float: {
						if (objA is ulong || objB is ulong || objA is decimal || objB is decimal) {
							return Convert.ToDecimal(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToDecimal(objB, CultureInfo.InvariantCulture));
						}
						return CompareFloat(objA, objB);
					}
				case JTokenType.Comment:
				case JTokenType.String:
				case JTokenType.Raw:
					string s1 = Convert.ToString(objA, CultureInfo.InvariantCulture);
					string s2 = Convert.ToString(objB, CultureInfo.InvariantCulture);

					return string.CompareOrdinal(s1, s2);

				case JTokenType.Boolean:
					bool b1 = Convert.ToBoolean(objA, CultureInfo.InvariantCulture);
					bool b2 = Convert.ToBoolean(objB, CultureInfo.InvariantCulture);

					return b1.CompareTo(b2);

				case JTokenType.Date:

					DateTime dateA = (DateTime)objA;
					DateTime dateB; {
						dateB = Convert.ToDateTime(objB, CultureInfo.InvariantCulture);
					}
					return dateA.CompareTo(dateB);

				case JTokenType.Bytes:
					if (!(objB is byte[] bytesB)) {
						throw new ArgumentException("Object must be of type byte[].");
					}

					byte[] bytesA = objA as byte[];
					MiscellaneousUtils.Assert(bytesA != null);

					return MiscellaneousUtils.ByteArrayCompare(bytesA!, bytesB);

				case JTokenType.Guid:
					if (!(objB is Guid)) {
						throw new ArgumentException("Object must be of type Guid.");
					}

					Guid guid1 = (Guid)objA;
					Guid guid2 = (Guid)objB;

					return guid1.CompareTo(guid2);

				case JTokenType.Uri:
					Uri uri2 = objB as Uri;
					if (uri2 == null) {
						throw new ArgumentException("Object must be of type Uri.");
					}

					Uri uri1 = (Uri)objA;

					return Comparer<string>.Default.Compare(uri1.ToString(), uri2.ToString());

				case JTokenType.TimeSpan:
					if (!(objB is TimeSpan)) {
						throw new ArgumentException("Object must be of type TimeSpan.");
					}

					TimeSpan ts1 = (TimeSpan)objA;
					TimeSpan ts2 = (TimeSpan)objB;

					return ts1.CompareTo(ts2);

				default:
					throw MiscellaneousUtils.CreateArgumentOutOfRangeException(nameof(valueType), valueType, "Unexpected value type: {0}".FormatWith(CultureInfo.InvariantCulture, valueType));
			}
		}

		private static int CompareFloat(object objA, object objB)
		{
			double d1 = Convert.ToDouble(objA, CultureInfo.InvariantCulture);
			double d2 = Convert.ToDouble(objB, CultureInfo.InvariantCulture);

			// take into account possible floating point errors
			if (MathUtils.ApproxEquals(d1, d2)) {
				return 0;
			}

			return d1.CompareTo(d2);
		}

		internal override JToken CloneToken(JsonCloneSettings settings)
		{
			return new JValue(this, settings);
		}

		/// <summary>
		/// Creates a <see cref="JValue"/> comment with the given value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>A <see cref="JValue"/> comment with the given value.</returns>
		public static JValue CreateComment(string value)
		{
			return new JValue(value, JTokenType.Comment);
		}

		/// <summary>
		/// Creates a <see cref="JValue"/> string with the given value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>A <see cref="JValue"/> string with the given value.</returns>
		public static JValue CreateString(string value)
		{
			return new JValue(value, JTokenType.String);
		}

		/// <summary>
		/// Creates a <see cref="JValue"/> null value.
		/// </summary>
		/// <returns>A <see cref="JValue"/> null value.</returns>
		public static JValue CreateNull()
		{
			return new JValue(null, JTokenType.Null);
		}

		/// <summary>
		/// Creates a <see cref="JValue"/> undefined value.
		/// </summary>
		/// <returns>A <see cref="JValue"/> undefined value.</returns>
		public static JValue CreateUndefined()
		{
			return new JValue(null, JTokenType.Undefined);
		}

		private static JTokenType GetValueType(JTokenType? current, object value)
		{
			if (value == null) {
				return JTokenType.Null;
			} else if (value is string) {
				return GetStringValueType(current);
			} else if (value is long || value is int || value is short || value is sbyte
					   || value is ulong || value is uint || value is ushort || value is byte) {
				return JTokenType.Integer;
			} else if (value is Enum) {
				return JTokenType.Integer;
			} else if (value is double || value is float || value is decimal) {
				return JTokenType.Float;
			} else if (value is DateTime) {
				return JTokenType.Date;
			} else if (value is byte[]) {
				return JTokenType.Bytes;
			} else if (value is bool) {
				return JTokenType.Boolean;
			} else if (value is Guid) {
				return JTokenType.Guid;
			} else if (value is Uri) {
				return JTokenType.Uri;
			} else if (value is TimeSpan) {
				return JTokenType.TimeSpan;
			}

			throw new ArgumentException("Could not determine JSON object type for type {0}.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
		}

		private static JTokenType GetStringValueType(JTokenType? current)
		{
			if (current == null) {
				return JTokenType.String;
			}

			switch (current.GetValueOrDefault()) {
				case JTokenType.Comment:
				case JTokenType.String:
				case JTokenType.Raw:
					return current.GetValueOrDefault();

				default:
					return JTokenType.String;
			}
		}

		/// <summary>
		/// Gets the node type for this <see cref="JToken"/>.
		/// </summary>
		/// <value>The type.</value>
		public override JTokenType Type => _valueType;

		/// <summary>
		/// Gets or sets the underlying token value.
		/// </summary>
		/// <value>The underlying token value.</value>
		public object Value {
			get => _value;
			set
			{
				Type currentType = _value?.GetType();
				Type newType = value?.GetType();

				if (currentType != newType) {
					_valueType = GetValueType(_valueType, value);
				}

				_value = value;
			}
		}

		/// <summary>
		/// Writes this token to a <see cref="JsonWriter"/>.
		/// </summary>
		/// <param name="writer">A <see cref="JsonWriter"/> into which this method will write.</param>
		/// <param name="converters">A collection of <see cref="JsonConverter"/>s which will be used when writing the token.</param>
		public override void WriteTo(JsonWriter writer)
		{
			switch (_valueType) {
				case JTokenType.Comment:
					writer.WriteComment(_value?.ToString());
					return;

				case JTokenType.Raw:
					writer.WriteRawValue(_value?.ToString());
					return;

				case JTokenType.Null:
					writer.WriteNull();
					return;

				case JTokenType.Undefined:
					writer.WriteUndefined();
					return;

				case JTokenType.Integer:
					if (_value is int i) {
						writer.WriteValue(i);
					} else if (_value is long l) {
						writer.WriteValue(l);
					} else if (_value is ulong ul) {
						writer.WriteValue(ul);
					} else {
						writer.WriteValue(Convert.ToInt64(_value, CultureInfo.InvariantCulture));
					}
					return;

				case JTokenType.Float:
					if (_value is decimal dec) {
						writer.WriteValue(dec);
					} else if (_value is double d) {
						writer.WriteValue(d);
					} else if (_value is float f) {
						writer.WriteValue(f);
					} else {
						writer.WriteValue(Convert.ToDouble(_value, CultureInfo.InvariantCulture));
					}
					return;

				case JTokenType.String:
					writer.WriteValue(_value?.ToString());
					return;

				case JTokenType.Boolean:
					writer.WriteValue(Convert.ToBoolean(_value, CultureInfo.InvariantCulture));
					return;

				case JTokenType.Date: {
						writer.WriteValue(Convert.ToDateTime(_value, CultureInfo.InvariantCulture));
					}
					return;

				case JTokenType.Bytes:
					writer.WriteValue((byte[])_value);
					return;

				case JTokenType.Guid:
					writer.WriteValue(_value != null ? (Guid?)_value : null);
					return;

				case JTokenType.TimeSpan:
					writer.WriteValue(_value != null ? (TimeSpan?)_value : null);
					return;

				case JTokenType.Uri:
					writer.WriteValue((Uri)_value);
					return;
			}

			throw MiscellaneousUtils.CreateArgumentOutOfRangeException(nameof(Type), _valueType, "Unexpected token type.");
		}

		internal override int GetDeepHashCode()
		{
			int valueHashCode = _value != null ? _value.GetHashCode() : 0;

			// GetHashCode on an enum boxes so cast to int
			return ((int)_valueType).GetHashCode() ^ valueHashCode;
		}

		private static bool ValuesEquals(JValue v1, JValue v2)
		{
			return v1 == v2 || v1._valueType == v2._valueType && Compare(v1._valueType, v1._value, v2._value) == 0;
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(JValue other)
		{
			if (other == null) {
				return false;
			}

			return ValuesEquals(this, other);
		}

		/// <summary>
		/// Determines whether the specified <see cref="object"/> is equal to the current <see cref="object"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to compare with the current <see cref="object"/>.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="object"/> is equal to the current <see cref="object"/>; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj is JValue v) {
				return Equals(v);
			}

			return false;
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="object"/>.
		/// </returns>
		public override int GetHashCode()
		{
			if (_value == null) {
				return 0;
			}

			return _value.GetHashCode();
		}

		/// <summary>
		/// Returns a <see cref="string"/> that represents this instance.
		/// </summary>
		/// <remarks>
		/// <c>ToString()</c> returns a non-JSON string value for tokens with a type of <see cref="JTokenType.String"/>.
		/// If you want the JSON for all token types then you should use <see cref="WriteTo(JsonWriter, JsonConverter[])"/>.
		/// </remarks>
		/// <returns>
		/// A <see cref="string"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if (_value == null) {
				return string.Empty;
			}

			return _value.ToString()!;
		}

		int IComparable.CompareTo(object obj)
		{
			if (obj == null) {
				return 1;
			}

			JTokenType comparisonType;
			object otherValue;
			if (obj is JValue value) {
				otherValue = value.Value;
				comparisonType = _valueType == JTokenType.String && _valueType != value._valueType
					? value._valueType
					: _valueType;
			} else {
				otherValue = obj;
				comparisonType = _valueType;
			}

			return Compare(comparisonType, _value, otherValue);
		}

		/// <summary>
		/// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings:
		/// Value
		/// Meaning
		/// Less than zero
		/// This instance is less than <paramref name="obj"/>.
		/// Zero
		/// This instance is equal to <paramref name="obj"/>.
		/// Greater than zero
		/// This instance is greater than <paramref name="obj"/>.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// 	<paramref name="obj"/> is not of the same type as this instance.
		/// </exception>
		public int CompareTo(JValue obj)
		{
			if (obj == null) {
				return 1;
			}

			JTokenType comparisonType = _valueType == JTokenType.String && _valueType != obj._valueType
				? obj._valueType
				: _valueType;

			return Compare(comparisonType, _value, obj._value);
		}
	}
}