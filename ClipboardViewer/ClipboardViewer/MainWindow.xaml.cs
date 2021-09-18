using System;
using System.Windows;

namespace ClipboardViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Win32との相互運用用のメソッド
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            // クリップボード操作用クラスを初期化
            ClipboardController controller = new(this);
            // 更新イベントが呼ばれたときにクリップボードのテキストを取得
            controller.ClipboardUpdated += (_, __) =>
            {
                if (Clipboard.ContainsText())
                {
                    this.textBlock.Text = Clipboard.GetText();
                }
            };
        }
    }
}