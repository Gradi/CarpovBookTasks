using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Milan.Helpers
{
    public class CharReader : IEnumerable<char>, IDisposable
    {
        private readonly TextReader _reader;
        private readonly bool _disposeReader;

        private bool _isDisposed;

        public CharReader(TextReader reader, bool disposeReader = false)
        {
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
            _disposeReader = disposeReader;
            _isDisposed = false;
        }

        public IEnumerator<char> GetEnumerator()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(CharReader));

            int input = -1;
            while ((input = _reader.Read()) !=  -1)
            {
                yield return (char)input;
            }

            if (_disposeReader)
            {
                _reader.Dispose();
            }
            _isDisposed = true;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Dispose()
        {
            if (_isDisposed)
            {
                if (_disposeReader)
                    _reader.Dispose();
                _isDisposed = true;
            }
        }
    }
}
