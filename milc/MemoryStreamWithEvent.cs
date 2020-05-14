using System;
using System.IO;

namespace milc
{
    public class MemoryStreamWithEvent : MemoryStream
    {
        public event EventHandler<MemoryStream>? OnClose;

        public override void Close()
        {
            OnClose?.Invoke(this, this);
            base.Close();
        }
    }
}
