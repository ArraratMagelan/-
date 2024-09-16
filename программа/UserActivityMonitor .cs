using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace программа
{
    internal class UserActivityMonitor: IMessageFilter
    {
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_MOUSEMOVE = 0x0200;

        private DateTime _lastActivityTime;

        public UserActivityMonitor()
        {
            Application.AddMessageFilter(this);
        }

        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_KEYDOWN:
                case WM_LBUTTONDOWN:
                case WM_MOUSEMOVE:
                    _lastActivityTime = DateTime.Now;
                    break;
            }
            return false;
        }

        public DateTime LastActivityTime => _lastActivityTime;

        public void Dispose()
        {
            Application.RemoveMessageFilter(this);
        }
    }
}
