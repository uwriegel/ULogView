using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace ULogView
{
	static class DropFile
	{
		public static void Initialize(Window mainWindow)
		{
			var windowHandle = new WindowInteropHelper(mainWindow).Handle;
			var webViewWindow = GetWindow(windowHandle, GetWindowType.GW_CHILD);
			var chromeContainer = GetWindow(webViewWindow, GetWindowType.GW_CHILD);
			var chrome = GetWindow(chromeContainer, GetWindowType.GW_CHILD);

			RevokeDragDrop(chrome);
			RegisterDragDrop(chrome, dropTarget);
		}

        static DropTarget dropTarget = new DropTarget();

        class DropTarget : IDropTarget
        {
            public void DragEnter(IDataObject pDataObj, uint grfKeyState, POINTL pt, ref uint pdwEffect) { }

            public void DragOver(uint grfKeyState, POINTL pt, ref uint pdwEffect) { }

            public void DragLeave() { }

            public void Drop(IDataObject pDataObj, uint grfKeyState, POINTL pt, ref uint pdwEffect)
            {
                var ftc = new FORMATETC { cfFormat = 15, lindex = -1, dwAspect = DVASPECT.DVASPECT_CONTENT, ptd = IntPtr.Zero, tymed = TYMED.TYMED_HGLOBAL };
                pDataObj.GetData(ftc, out var med);
                var zahl = DragQueryFile(med.data, -1, null, 0);
                var sb = new StringBuilder(300);
                DragQueryFile(med.data, 0, sb, sb.Capacity);
                var watt = sb.ToString();
            }

            [DllImport("shell32.dll")]
            static extern uint DragQueryFile(IntPtr hDrop, int iFile, [Out] StringBuilder lpszFile, int cch);
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000010E-0000-0000-C000-000000000046")]
        interface IDataObject
        {
            void GetData([In] ref FORMATETC format, out STGMEDIUM medium);
            void GetDataHere([In] ref FORMATETC format, ref STGMEDIUM medium);
            [PreserveSig]
            int QueryGetData([In] ref FORMATETC format);
            [PreserveSig]
            int GetCanonicalFormatEtc([In] ref FORMATETC formatIn, out FORMATETC formatOut);
            void SetData([In] ref FORMATETC formatIn, [In] ref STGMEDIUM medium, [MarshalAs(UnmanagedType.Bool)] bool release);
            void EnumFormatEtc();
            [PreserveSig]
            int DAdvise([In] ref FORMATETC pFormatetc);
            void DUnadvise(int connection);
            [PreserveSig]
            int EnumDAdvise();
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STGMEDIUM
        {
            [MarshalAs(UnmanagedType.U4)]
            public int tymed;
            public IntPtr data;
            [MarshalAs(UnmanagedType.IUnknown)]
            public object pUnkForRelease;
        }

        enum GetWindowType : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        [StructLayout(LayoutKind.Sequential)]
        struct FORMATETC
        {
            public short cfFormat;
            public IntPtr ptd;
            [MarshalAs(UnmanagedType.U4)]
            public DVASPECT dwAspect;
            public int lindex;
            [MarshalAs(UnmanagedType.U4)]
            public TYMED tymed;
        };

        [Flags]
        public enum DVASPECT
        {
            DVASPECT_CONTENT = 1,
            DVASPECT_THUMBNAIL = 2,
            DVASPECT_ICON = 4,
            DVASPECT_DOCPRINT = 8
        }

        [Flags]
        enum TYMED
        {
            TYMED_NULL = 0,
            TYMED_HGLOBAL = 1,
            TYMED_FILE = 2,
            TYMED_ISTREAM = 4,
            TYMED_ISTORAGE = 8,
            TYMED_GDI = 16,
            TYMED_MFPICT = 32,
            TYMED_ENHMF = 64,
        }

        struct POINTL
        {
            [ComAliasName("Microsoft.VisualStudio.OLE.Interop.LONG")]
            public int x;
            [ComAliasName("Microsoft.VisualStudio.OLE.Interop.LONG")]
            public int y;
        }

        [Guid("00000122-0000-0000-C000-000000000046")]
        [InterfaceType(1)]
        interface IDropTarget
        {
            void DragEnter(IDataObject pDataObj, uint grfKeyState, POINTL pt, ref uint pdwEffect);
            void DragOver(uint grfKeyState, POINTL pt, ref uint pdwEffect);
            void DragLeave();
            void Drop(IDataObject pDataObj, uint grfKeyState, POINTL pt, ref uint pdwEffect);
        }

        [DllImport("user32.dll", SetLastError = true)]
		static extern IntPtr GetWindow(IntPtr hWnd, GetWindowType uCmd);
        [DllImport("ole32.dll")]
        static extern int RevokeDragDrop(IntPtr hwnd);
        [DllImport("ole32.dll")]
        static extern int RegisterDragDrop(IntPtr hwnd, IDropTarget pDropTarget);
    }
}
