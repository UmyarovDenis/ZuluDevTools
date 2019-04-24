using System;
using System.Windows;
using System.Windows.Interop;
using Zulu;

namespace ZuluDevTools.Factories
{
    /// <summary>
    /// Предоставляет методы по созданию объектов типа IControlBar
    /// </summary>
    public class ControlBarFactory
    {
        private const UInt32 DLGC_WANTARROWS = 0x0001;
        private const UInt32 DLGC_WANTTAB = 0x0002;
        private const UInt32 DLGC_WANTALLKEYS = 0x0004;
        private const UInt32 DLGC_HASSETSEL = 0x0008;
        private const UInt32 DLGC_WANTCHARS = 0x0080;
        private const UInt32 WM_GETDLGCODE = 0x0087;

        private readonly Func<string, int, IControlBar> _controlBarHandler;
        private const int WS_CHILD = 0x40000000,
                          WS_VISIBLE = 0x10000000;
        /// <summary>
        /// Предоставляет методы по созданию объектов типа IControlBar
        /// </summary>
        /// <param name="controlBarHandler">Функция по созданию объекта типа IControlBar</param>
        public ControlBarFactory(Func<string, int, IControlBar> controlBarHandler)
        {
            _controlBarHandler = controlBarHandler;
        }
        /// <summary>
        /// Создает представление по шаблону MVVM и внедряет его в докируемую панель
        /// </summary>
        /// <typeparam name="TView">Тип представления</typeparam>
        /// <typeparam name="TViewModel">Тип контекста данных</typeparam>
        /// <param name="caption">Заголовок окна</param>
        /// <param name="styles">Стиль окна</param>
        /// <param name="viewModelArgs">Параметры контекста данных</param>
        /// <returns></returns>
        public IControlBar Create<TView, TViewModel>(string caption, int styles, params object[] viewModelArgs)
            where TView : FrameworkElement
            where TViewModel : class
        {
            TView view = Activator.CreateInstance<TView>();
            view.DataContext = Activator.CreateInstance(typeof(TViewModel), viewModelArgs);

            return Create(view, caption, styles);
        }
        /// <summary>
        /// Внедряет представление в докируемую панель
        /// </summary>
        /// <param name="view">Тип представления</param>
        /// <param name="caption">Заголовок окна</param>
        /// <param name="style">Стиль окна</param>
        /// <returns></returns>
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
