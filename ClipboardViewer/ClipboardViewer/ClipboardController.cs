using System;
using System.Windows;
using System.Windows.Interop;
using Windows.Win32;
using Windows.Win32.Foundation;

namespace ClipboardViewer
{
    /// <summary>
    /// ClipBoardController
    /// </summary>
    internal class ClipboardController : IDisposable
    {
        private HWND hWnd;
        /// <summary>
        /// 更新時に呼ばれるイベントハンドラ
        /// </summary>
        internal event EventHandler ClipboardUpdated;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="window">対象ウィンドウ</param>
        internal unsafe ClipboardController(Window window)
        {
            // WPFでウィンドウプロシージャをフックする場合、
            // 対象WindowからHwndSourceを取得し、それに対してフックを追加する
            HwndSource source = (HwndSource)PresentationSource.FromVisual(window);
            source.AddHook(WndProc);
            // ハンドルを取得しリスナを追加
            hWnd = new(source.Handle);
            _ = PInvoke.AddClipboardFormatListener(hWnd);
        }
        /// <summary>
        /// ウィンドウプロシージャ
        /// </summary>
        /// <param name="hwnd">ウィンドウハンドル</param>
        /// <param name="message">メッセージコード</param>
        /// <param name="wParam">w-パラメータ</param>
        /// <param name="lParam">l-パラメータ</param>
        /// <param name="handled"></param>
        /// <seealso cref="https://docs.microsoft.com/ja-jp/dotnet/desktop/wpf/advanced/walkthrough-hosting-a-win32-control-in-wpf?view=netframeworkdesktop-4.8"/>
        /// <returns></returns>
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            /// <summary>
            /// ClipBpardの内容が変更された時
            /// https://docs.microsoft.com/en-us/windows/win32/dataxchg/wm-clipboardupdate
            if (msg == Constants.WM_CLIPBOARDUPDATE)
            {
                ClipboardUpdated?.Invoke(this, EventArgs.Empty);
                handled = true;
            }
            return IntPtr.Zero;
        }
        /// <summary>
        /// 破棄
        /// </summary>
        void IDisposable.Dispose() => _ = PInvoke.RemoveClipboardFormatListener(hWnd);
    }
}