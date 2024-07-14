using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TatehamaATS.DisplayLED
{
    public partial class LEDWindow : Form
    {
        private int[,] ledArray;
        private const int ledRows = 16;
        private const int ledCols = 32;
        private const int ledPanels = 3;
        private const int ledMargin = 10; // 枠の幅

        public LEDWindow()
        {
            InitializeComponent();
            InitializeLEDDisplay();
            this.Resize += new EventHandler(this.LEDWindow_Resize);
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(16, 23, 26);
            this.TransparencyKey = Color.Lime;

            // 枠の内側を灰色に設定
            pictureBox1.BackColor = Color.FromArgb(16, 23, 26);

            // ウィンドウのサイズ変更を可能にする
            this.MouseDown += new MouseEventHandler(this.LEDWindow_MouseDown);
        }

        /// <summary>
        /// LED表示器の初期設定
        /// </summary>
        private void InitializeLEDDisplay()
        {
            ledArray = new int[ledRows, ledCols];
            // 仮のデータを設定
            for (int i = 0; i < ledRows; i++)
            {
                for (int j = 0; j < ledCols; j++)
                {
                    ledArray[i, j] = (i + j) % 2;
                }
            }
        }

        /// <summary>
        /// LED表示器の描画処理
        /// </summary>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // 描画領域のサイズを取得
            int panelWidth = pictureBox1.ClientSize.Width;
            int panelHeight = pictureBox1.ClientSize.Height / ledPanels;

            // 各LEDのサイズを計算
            int ledSize = Math.Min(
                (panelWidth - 2 * ledMargin) / ledCols,
                (panelHeight - 2 * ledMargin) / ledRows
            );

            // LEDの中央に配置するためのオフセットを計算
            int offsetX = (panelWidth - (ledCols * ledSize)) / 2;
            int offsetY;

            for (int p = 0; p < ledPanels; p++)
            {
                offsetY = (panelHeight * p) + ((panelHeight - (ledRows * ledSize)) / 2);

                for (int i = 0; i < ledRows; i++)
                {
                    for (int j = 0; j < ledCols; j++)
                    {
                        if (ledArray[i, j] == 1)
                        {
                            g.FillEllipse(Brushes.Red, offsetX + j * ledSize, offsetY + i * ledSize, ledSize, ledSize);
                        }
                        else
                        {
                            g.FillEllipse(Brushes.Black, offsetX + j * ledSize, offsetY + i * ledSize, ledSize, ledSize);
                        }
                    }
                }

                // 枠を描く
                g.DrawRectangle(Pens.Gray, offsetX - ledMargin, offsetY - ledMargin, ledCols * ledSize + 2 * ledMargin, ledRows * ledSize + 2 * ledMargin);
            }
        }

        /// <summary>
        /// ウィンドウサイズ変更時の処理
        /// </summary>
        private void LEDWindow_Resize(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        /// <summary>
        /// ウィンドウのドラッグ開始位置
        /// </summary>
        private Point dragStartPoint;

        /// <summary>
        /// マウスダウンイベントの処理
        /// </summary>
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragStartPoint = e.Location;
            }
        }

        /// <summary>
        /// マウスムーブイベントの処理
        /// </summary>
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Left + e.X - dragStartPoint.X, this.Top + e.Y - dragStartPoint.Y);
            }
        }

        // Windows APIの宣言
        private const int HT_CAPTION = 0x2;
        private const int HT_LEFT = 0xA;
        private const int HT_RIGHT = 0xB;
        private const int HT_TOP = 0xC;
        private const int HT_TOPLEFT = 0xD;
        private const int HT_TOPRIGHT = 0xE;
        private const int HT_BOTTOM = 0xF;
        private const int HT_BOTTOMLEFT = 0x10;
        private const int HT_BOTTOMRIGHT = 0x11;
        private const int WM_NCHITTEST = 0x84;
        private const int WM_SETCURSOR = 0x20;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        /// <summary>
        /// マウスダウンイベントの処理
        /// </summary>
        private void LEDWindow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCHITTEST, 0, HT_CAPTION);
            }
        }

        /// <summary>
        /// サイズ変更処理のためのウィンドウメッセージのオーバーライド
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            const int resizeAreaSize = 20; // サイズ変更エリアの広さを10に設定

            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    var cursor = this.PointToClient(Cursor.Position);

                    // 左端
                    if (cursor.X <= resizeAreaSize)
                    {
                        if (cursor.Y <= resizeAreaSize) m.Result = (IntPtr)HT_TOPLEFT;
                        else if (cursor.Y >= this.ClientSize.Height - resizeAreaSize) m.Result = (IntPtr)HT_BOTTOMLEFT;
                        else m.Result = (IntPtr)HT_LEFT;
                    }
                    // 右端
                    else if (cursor.X >= this.ClientSize.Width - resizeAreaSize)
                    {
                        if (cursor.Y <= resizeAreaSize) m.Result = (IntPtr)HT_TOPRIGHT;
                        else if (cursor.Y >= this.ClientSize.Height - resizeAreaSize) m.Result = (IntPtr)HT_BOTTOMRIGHT;
                        else m.Result = (IntPtr)HT_RIGHT;
                    }
                    // 上端
                    else if (cursor.Y <= resizeAreaSize)
                    {
                        m.Result = (IntPtr)HT_TOP;
                    }
                    // 下端
                    else if (cursor.Y >= this.ClientSize.Height - resizeAreaSize)
                    {
                        m.Result = (IntPtr)HT_BOTTOM;
                    }
                    break;

                case WM_SETCURSOR:
                    base.WndProc(ref m);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }
    }
}
