using System.ComponentModel;

#if HAVE_DYNAMIC
using System.Dynamic;
using System.Linq.Expressions;
#endif

using System.Globalization;
using System.Diagnostics.CodeAnalysis;
using ToolGood.JsonObject.Settings;
using ToolGood.JsonObject.Handlings;
using ToolGood.JsonObject.Exceptions;
using ToolGood.JsonObject.Utilities;
using ToolGood.JsonObject.Commens;

namespace ToolGood.JsonObject
{
	/// <summary>
	/// Represents a JSON object.
	/// </summary>
	public partial class JObject : JContainer, IDictionary<string, JToken>
	{
		private readonly JPropertyKeyedCollection _properties = new JPropertyKeyedCollection();

		/// <summary>
		/// Gets the container's children tokens.
		/// </summary>
		/// <value>The container's children tokens.</value>
		protected override IList<JToken> ChildrenTokens => _properties;

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Initializes a new instance of the <see cref="JObject"/> class.
		/// </summary>
		public JObject()
		{
		}

		internal JObject(JObject other, JsonCloneSettings settings)
			: base(other, settings)
		{
		}

		internal override bool DeepEquals(JToken node)
		{
			if (!(node is JObject t)) {
				return false;
			}

			return _properties.Compare(t._properties);
		}

		internal override int IndexOfItem(JToken item)
		{
			if (item == null) {
				return -1;
			}

			return _properties.IndexOfReference(item);
		}

		internal override bool InsertItem(int index, JToken item, bool skipParentCheck, bool copyAnnotations)
		{
			// don't add comments to JObject, no name to reference comment by
			if (item != null && item.Type == JTokenType.Comment) {
				return false;
			}

			return base.InsertItem(index, item, skipParentCheck, copyAnnotations);
		}

		internal override void ValidateToken(JToken o, JToken existing)
		{
			ValidationUtils.ArgumentNotNull(o, nameof(o));

			if (o.Type != JTokenType.Property) {
				throw new ArgumentException("Can not add {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, o.GetType(), GetType()));
			}

			JProperty newProperty = (JProperty)o;

			if (existing != null) {
				JProperty existingProperty = (JProperty)existing;

				if (newProperty.Name == existingProperty.Name) {
					return;
				}
			}

			if (_properties.TryGetValue(newProperty.Name, out existing)) {
				throw new ArgumentException("Can not add property {0} to {1}. Property with the same name already exists on object.".FormatWith(CultureInfo.InvariantCulture, newProperty.Name, GetType()));
			}
		}

		internal override void MergeItem(object content, JsonMergeSettings settings)
		{
			if (!(content is JObject o)) {
				return;
			}

			foreach (KeyValuePair<string, JToken> contentItem in o) {
				JProperty existingProperty = Property(contentItem.Key, settings?.PropertyNameComparison ?? StringComparison.Ordinal);

				if (existingProperty == null) {
					Add(contentItem.Key, contentItem.Value);
				} else if (contentItem.Value != null) {
					if (!(existingProperty.Value is JContainer existingContainer) || existingContainer.Type != contentItem.Value.Type) {
						if (!IsNull(contentItem.Value) || settings?.MergeNullValueHandling == MergeNullValueHandling.Merge) {
							existingProperty.Value = contentItem.Value;
						}
					} else {
						existingContainer.Merge(contentItem.Value, settings);
					}
				}
			}
		}

		private static bool IsNull(JToken token)
		{
			if (token.Type == JTokenType.Null) {
				return true;
			}

			if (token is JValue v && v.Value == null) {
				return true;
			}

			return false;
		}

		internal override JToken CloneToken(JsonCloneSettings settings)
		{
			return new JObject(this, settings);
		}

		/// <summary>
		/// Gets the node type for this <see cref="JToken"/>.
		/// </summary>
		/// <value>The type.</value>
		public override JTokenType Type => JTokenType.Object;

		/// <summary>
		/// Gets an <see cref="IEnumerable{T}"/> of <see cref="JProperty"/> of this object's properties.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}"/> of <see cref="JProperty"/> of this object's properties.</returns>
		public IEnumerable<JProperty> Properties()
		{
			return _properties.Cast<JProperty>();
		}

		/// <summary>
		/// Gets a <see cref="JProperty"/> with the specified name.
		/// </summary>
		/// <param name="name">The property name.</param>
		/// <returns>A <see cref="JProperty"/> with the specified name or <c>null</c>.</returns>
		public JProperty Property(string name)
		{
			return Property(name, StringComparison.Ordinal);
		}

		/// <summary>
		/// Gets the <see cref="JProperty"/> with the specified name.
		/// The exact name will be searched for first and if no matching property is found then
		/// the <see cref="StringComparison"/> will be used to match a property.
		/// </summary>
		/// <param name="name">The property name.</param>
		/// <param name="comparison">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns>A <see cref="JProperty"/> matched with the specified name or <c>null</c>.</returns>
		public JProperty Property(string name, StringComparison comparison)
		{
			if (name == null) {
				return null;
			}

			if (_properties.TryGetValue(name, out JToken property)) {
				return (JProperty)property;
			}

			// test above already uses this comparison so no need to repeat
			if (comparison != StringComparison.Ordinal) {
				for (int i = 0; i < _properties.Count; i++) {
					JProperty p = (JProperty)_properties[i];
					if (string.Equals(p.Name, name, comparison)) {
						return p;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Gets a <see cref="JEnumerable{T}"/> of <see cref="JToken"/> of this object's property values.
		/// </summary>
		/// <returns>A <see cref="JEnumerable{T}"/> of <see cref="JToken"/> of this object's property values.</returns>
		public JEnumerable<JToken> PropertyValues()
		{
			return new JEnumerable<JToken>(Properties().Select(p => p.Value));
		}

		/// <summary>
		/// Gets the <see cref="JToken"/> with the specified key.
		/// </summary>
		/// <value>The <see cref="JToken"/> with the specified key.</value>
		public override JToken this[object key] {
			get
			{
				ValidationUtils.ArgumentNotNull(key, nameof(key));

				if (!(key is string propertyName)) {
					throw new ArgumentException("Accessed JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}

				return this[propertyName];
			}
			set
			{
				ValidationUtils.ArgumentNotNull(key, nameof(key));

				if (!(key is string propertyName)) {
					throw new ArgumentException("Set JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}

				this[propertyName] = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="JToken"/> with the specified property name.
		/// </summary>
		/// <value></value>
		public JToken this[string propertyName] {
			get
			{
				ValidationUtils.ArgumentNotNull(propertyName, nameof(propertyName));

				JProperty property = Property(propertyName, StringComparison.Ordinal);

				return property?.Value;
			}
			set
			{
				JProperty property = Property(propertyName, StringComparison.Ordinal);
				if (property != null) {
					property.Value = value!;
				} else {
#if HAVE_INOTIFY_PROPERTY_CHANGING
                    OnPropertyChanging(propertyName);
#endif
					Add(propertyName, value);
					OnPropertyChanged(propertyName);
				}
			}
		}

		/// <summary>
		/// Loads a <see cref="JObject"/> from a <see cref="JsonReader"/>.
		/// </summary>
		/// <param name="reader">A <see cref="JsonReader"/> that will be read for the content of the <see cref="JObject"/>.</param>
		/// <param name="settings">The <see cref="JsonLoadSettings"/> used to load the JSON.
		/// If this is <c>null</c>, default load settings will be used.</param>
		/// <returns>A <see cref="JObject"/> that contains the JSON that was read from the specified <see cref="JsonReader"/>.</returns>
		/// <exception cref="JsonReaderException">
		///     <paramref name="reader"/> is not valid JSON.
		/// </exception>
		public new static JObject Load(JsonReader reader, JsonLoadSettings settings)
		{
			ValidationUtils.ArgumentNotNull(reader, nameof(reader));

			if (reader.TokenType == JsonToken.None) {
				if (!reader.Read()) {
					throw JsonReaderException.Create(reader, "Error reading JObject from JsonReader.");
				}
			}

			reader.MoveToContent();

			if (reader.TokenType != JsonToken.StartObject) {
				throw JsonReaderException.Create(reader, "Error reading JObject from JsonReader. Current JsonReader item is not an object: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}

			JObject o = new JObject();
			o.SetLineInfo(reader as IJsonLineInfo, settings);

			o.ReadTokenFrom(reader, settings);

			return o;
		}

		/// <summary>
		/// Load a <see cref="JObject"/> from a string that contains JSON.
		/// </summary>
		/// <param name="json">A <see cref="string"/> that contains JSON.</param>
		/// <returns>A <see cref="JObject"/> populated from the string that contains JSON.</returns>
		/// <exception cref="JsonReaderException">
		///     <paramref name="json"/> is not valid JSON.
		/// </exception>
		public new static JObject Parse(string json)
		{
			return Parse(json, null);
		}

		/// <summary>
		/// Load a <see cref="JObject"/> from a string that contains JSON.
		/// </summary>
		/// <param name="json">A <see cref="string"/> that contains JSON.</param>
		/// <param name="settings">The <see cref="JsonLoadSettings"/> used to load the JSON.
		/// If this is <c>null</c>, default load settings will be used.</param>
		/// <returns>A <see cref="JObject"/> populated from the string that contains JSON.</returns>
		/// <exception cref="JsonReaderException">
		///     <paramref name="json"/> is not valid JSON.
		/// </exception>
		public new static JObject Parse(string json, JsonLoadSettings settings)
		{
			using (JsonReader reader = new JsonTextReader(new StringReader(json))) {
				JObject o = Load(reader, settings);

				while (reader.Read()) {
					// Any content encountered here other than a comment will throw in the reader.
				}

				return o;
			}
		}

		/// <summary>
		/// Writes this token to a <see cref="JsonWriter"/>.
		/// </summary>
		/// <param name="writer">A <see cref="JsonWriter"/> into which this method will write.</param>
		public override void WriteTo(JsonWriter writer)
		{
			writer.WriteStartObject();

			for (int i = 0; i < _properties.Count; i++) {
				_properties[i].WriteTo(writer);
			}

			writer.WriteEndObject();
		}

		/// <summary>
		/// Gets the <see cref="Newtonsoft.Json.Linq.JToken"/> with the specified property name.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns>The <see cref="Newtonsoft.Json.Linq.JToken"/> with the specified property name.</returns>
		public JToken GetValue(string propertyName)
		{
			return GetValue(propertyName, StringComparison.Ordinal);
		}

		/// <summary>
		/// Gets the <see cref="Newtonsoft.Json.Linq.JToken"/> with the specified property name.
		/// The exact property name will be searched for first and if no matching property is found then
		/// the <see cref="StringComparison"/> will be used to match a property.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="comparison">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns>The <see cref="Newtonsoft.Json.Linq.JToken"/> with the specified property name.</returns>
		public JToken GetValue(string propertyName, StringComparison comparison)
		{
			if (propertyName == null) {
				return null;
			}

			// attempt to get value via dictionary first for performance
			var property = Property(propertyName, comparison);

			return property?.Value;
		}

		/// <summary>
		/// Tries to get the <see cref="Newtonsoft.Json.Linq.JToken"/> with the specified property name.
		/// The exact property name will be searched for first and if no matching property is found then
		/// the <see cref="StringComparison"/> will be used to match a property.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="value">The value.</param>
		/// <param name="comparison">One of the enumeration values that specifies how the strings will be compared.</param>
		/// <returns><c>true</c> if a value was successfully retrieved; otherwise, <c>false</c>.</returns>
		public bool TryGetValue(string propertyName, StringComparison comparison, [NotNullWhen(true)] out JToken value)
		{
			value = GetValue(propertyName, comparison);
			return value != null;
		}

		#region IDictionary<string,JToken> Members

		/// <summary>
		/// Adds the specified property name.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="value">The value.</param>
		public void Add(string propertyName, JToken value)
		{
			Add(new JProperty(propertyName, value));
		}

		/// <summary>
		/// Determines whether the JSON object has the specified property name.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns><c>true</c> if the JSON object has the specified property name; otherwise, <c>false</c>.</returns>
		public bool ContainsKey(string propertyName)
		{
			ValidationUtils.ArgumentNotNull(propertyName, nameof(propertyName));

			return _properties.Contains(propertyName);
		}

		ICollection<string> IDictionary<string, JToken>.Keys => _properties.Keys;

		/// <summary>
		/// Removes the property with the specified name.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns><c>true</c> if item was successfully removed; otherwise, <c>false</c>.</returns>
		public bool Remove(string propertyName)
		{
			JProperty property = Property(propertyName, StringComparison.Ordinal);
			if (property == null) {
				return false;
			}

			property.Remove();
			return true;
		}

		/// <summary>
		/// Tries to get the <see cref="Newtonsoft.Json.Linq.JToken"/> with the specified property name.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if a value was successfully retrieved; otherwise, <c>false</c>.</returns>
		public bool TryGetValue(string propertyName, out JToken value)
		{
			JProperty property = Property(propertyName, StringComparison.Ordinal);
			if (property == null) {
				value = null;
				return false;
			}

			value = property.Value;
			return true;
		}

		ICollection<JToken> IDictionary<string, JToken>.Values => throw new NotImplementedException();

		#endregion IDictionary<string,JToken> Members

		#region ICollection<KeyValuePair<string,JToken>> Members

		void ICollection<KeyValuePair<string, JToken>>.Add(KeyValuePair<string, JToken> item)
		{
			Add(new JProperty(item.Key, item.Value));
		}

		void ICollection<KeyValuePair<string, JToken>>.Clear()
		{
			RemoveAll();
		}

		bool ICollection<KeyValuePair<string, JToken>>.Contains(KeyValuePair<string, JToken> item)
		{
			JProperty property = Property(item.Key, StringComparison.Ordinal);
			if (property == null) {
				return false;
			}

			return property.Value == item.Value;
		}

		void ICollection<KeyValuePair<string, JToken>>.CopyTo(KeyValuePair<string, JToken>[] array, int arrayIndex)
		{
			if (array == null) {
				throw new ArgumentNullException(nameof(array));
			}
			if (arrayIndex < 0) {
				throw new ArgumentOutOfRangeException(nameof(arrayIndex), "arrayIndex is less than 0.");
			}
			if (arrayIndex >= array.Length && arrayIndex != 0) {
				throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
			}
			if (Count > array.Length - arrayIndex) {
				throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
			}

			int index = 0;
			foreach (JProperty property in _properties) {
				array[arrayIndex + index] = new KeyValuePair<string, JToken>(property.Name, property.Value);
				index++;
			}
		}

		bool ICollection<KeyValuePair<string, JToken>>.IsReadOnly => false;

		bool ICollection<KeyValuePair<string, JToken>>.Remove(KeyValuePair<string, JToken> item)
		{
			if (!((ICollection<KeyValuePair<string, JToken>>)this).Contains(item)) {
				return false;
			}

			((IDictionary<string, JToken>)this).Remove(item.Key);
			return true;
		}

		#endregion ICollection<KeyValuePair<string,JToken>> Members

		internal override int GetDeepHashCode()
		{
			return ContentsHashCode();
		}

		/// <summary>
		/// Returns an enumerator that can be used to iterate through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<KeyValuePair<string, JToken>> GetEnumerator()
		{
			foreach (JProperty property in _properties) {
				yield return new KeyValuePair<string, JToken>(property.Name, property.Value);
			}
		}

		/// <summary>
		/// Raises the <see cref="PropertyChanged"/> event with the provided arguments.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}