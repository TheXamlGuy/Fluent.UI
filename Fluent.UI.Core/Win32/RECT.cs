using System.Runtime.InteropServices;

namespace Fluent.UI.Core.Win32
{
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
}