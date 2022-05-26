using System;

namespace F_Tool
{
    public class WindowListItem
    {
        public IntPtr HWnd;
        public string WindowName = string.Empty;

        public IntPtr GetHwnd() => this.HWnd;

        public override string ToString() => this.WindowName;
    }
}
