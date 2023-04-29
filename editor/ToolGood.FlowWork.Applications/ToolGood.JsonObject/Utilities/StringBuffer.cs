using ToolGood.JsonObject.Commens;

namespace ToolGood.JsonObject.Utilities
{
	/// <summary>
	/// Builds a string. Unlike <see cref="System.Text.StringBuilder"/> this class lets you reuse its internal buffer.
	/// </summary>
	internal struct StringBuffer
	{
		private char[] _buffer;
		private int _position;

		public int Position {
			get => _position;
			set => _position = value;
		}

		public bool IsEmpty => _buffer == null;

		public StringBuffer(IArrayPool<char> bufferPool, int initalSize) : this(BufferUtils.RentBuffer(bufferPool, initalSize))
		{
		}

		private StringBuffer(char[] buffer)
		{
			_buffer = buffer;
			_position = 0;
		}

		public void Append(IArrayPool<char> bufferPool, char value)
		{
			// test if the buffer array is large enough to take the value
			if (_position == _buffer!.Length) {
				EnsureSize(bufferPool, 1);
			}

			// set value and increment poisition
			_buffer![_position++] = value;
		}

		public void Append(IArrayPool<char> bufferPool, char[] buffer, int startIndex, int count)
		{
			if (_position + count >= _buffer!.Length) {
				EnsureSize(bufferPool, count);
			}

			Array.Copy(buffer, startIndex, _buffer, _position, count);

			_position += count;
		}

		public void Clear(IArrayPool<char> bufferPool)
		{
			if (_buffer != null) {
				BufferUtils.ReturnBuffer(bufferPool, _buffer);
				_buffer = null;
			}
			_position = 0;
		}

		private void EnsureSize(IArrayPool<char> bufferPool, int appendLength)
		{
			char[] newBuffer = BufferUtils.RentBuffer(bufferPool, (_position + appendLength) * 2);

			if (_buffer != null) {
				Array.Copy(_buffer, newBuffer, _position);
				BufferUtils.ReturnBuffer(bufferPool, _buffer);
			}

			_buffer = newBuffer;
		}

		public override string ToString()
		{
			return ToString(0, _position);
		}

		public string ToString(int start, int length)
		{
			MiscellaneousUtils.Assert(_buffer != null);
			return new string(_buffer, start, length);
		}

		public char[] InternalBuffer => _buffer;
	}
}