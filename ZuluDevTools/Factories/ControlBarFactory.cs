using System;
using System.Windows;
using System.Windows.Interop;
using Zulu;

namespace ZuluDevTools.Factories
{
    public delegate IControlBar ControlBarHandler(string caption, int style);

    public class ControlBarFactory
    {
        private const UInt32 DLGC_WANTARROWS = 0x0001;
        private const UInt32 DLGC_WANTTAB = 0x0002;
        private const UInt32 DLGC_WANTALLKEYS = 0x0004;
        private const UInt32 DLGC_HASSETSEL = 0x0008;
        private const UInt32 DLGC_WANTCHARS = 0x0080;
        private const UInt32 WM_GETDLGCODE = 0x0087;

        private readonly ControlBarHandler _controlBarHandler;
        private const int WS_CHILD = 0x40000000,
                          WS_VISIBLE = 0x10000000;
        public ControlBarFactory(ControlBarHandler controlBarHandler)
        {
            _controlBarHandler = controlBarHandler;
        }
        public IControlBar Create<TView, TViewModel>(string caption, int styles, params object[] viewModelArgs)
            where TView : FrameworkElement
            where TViewModel : class
        {
            TView view = Activator.CreateInstance<TView>();
            view.DataContext = Activator.CreateInstance(typeof(TViewModel), viewModelArgs);

            return Create(view, caption, styles);
        }
        public IControlBar Create(FrameworkElement view, string caption, int style)
        {
            try
            {
                IControlBar controlBar = _controlBarHandler.Invoke(caption, style);

                HwndSourceParameters pars = new HwndSourceParameters
                {
                    WindowStyle = WS_CHILD | WS_VISIBLE,
                    PositionX = 0,
                    PositionY = 0,
                    ParentWindow = (IntPtr)controlBar.hWnd
                };

                HwndSource source = new HwndSource(pars)
                {
                    RootVisual = view,
                    SizeToContent = SizeToContent.WidthAndHeight
                };

                source.AddHook(ChildHwndSourceHook);

                controlBar.EmbedWindow(source.Handle.ToInt32());

                return controlBar;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static IntPtr ChildHwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg != WM_GETDLGCODE) return IntPtr.Zero;

            handled = true;

            return new IntPtr(DLGC_WANTCHARS | DLGC_WANTARROWS | DLGC_HASSETSEL);
        }
    }
}
