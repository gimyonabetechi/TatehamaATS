using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TatehamaATS.Exceptions;

namespace TatehamaATS.DisplayLED
{
    public partial class LEDWindow : Form
    {
        private Bitmap sourceImage;

        public LEDWindow()
        {
            InitializeComponent();
            sourceImage = Properties.Resources.ATS_LED2;
        }

        /// <summary>
        /// 切り出された画像を引き伸ばし、指定の表示器に表示する
        /// </summary>
        /// <param name="pictureBoxIndex">表示するPictureBoxの番号（1～3）</param>
        /// <param name="imageNumber">表示する画像の番号</param>
        internal void DisplayImage(int pictureBoxIndex, int imageNumber)
        {
            try
            {
                Bitmap croppedImage = GetImageByNumber(imageNumber);
                PictureBox pictureBox = GetPictureBoxByIndex(pictureBoxIndex);

                Bitmap enlargedImage = EnlargePixelArt(croppedImage);

                // 画像を右に3px、下に3px移動する
                Bitmap shiftedImage = new Bitmap(enlargedImage.Width, enlargedImage.Height);
                using (Graphics g = Graphics.FromImage(shiftedImage))
                {
                    g.Clear(Color.Transparent);
                    g.DrawImage(enlargedImage, new Rectangle(0, 0, enlargedImage.Width, enlargedImage.Height));
                }

                pictureBox.BackgroundImage = shiftedImage;
            }
            catch (Exception ex)
            {
                throw new LEDControlException(3, $"エラーが発生しました: {ex.Message} @DisplayImage", ex);
            }
        }

        /// <summary>
        /// ピクセルアートを6倍に拡大する
        /// </summary>
        /// <param name="original">元の画像</param>
        /// <returns>6倍に拡大された画像</returns>
        private Bitmap EnlargePixelArt(Bitmap original)
        {
            int newWidth = original.Width * 6;
            int newHeight = original.Height * 6;

            Bitmap enlargedImage = new Bitmap(newWidth + 1, newHeight + 1);
            using (Graphics g = Graphics.FromImage(enlargedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                for (int y = 0; y < original.Height; y++)
                {
                    for (int x = 0; x < original.Width; x++)
                    {
                        Color pixelColor = original.GetPixel(x, y);
                        for (int dy = 0; dy < 6; dy++)
                        {
                            for (int dx = 0; dx < 6; dx++)
                            {
                                enlargedImage.SetPixel(x * 6 + dx, y * 6 + dy, pixelColor);
                            }
                        }
                    }
                }
            }

            return enlargedImage;
        }


        /// <summary>
        /// 指定した番号に基づいて画像を切り出す
        /// </summary>
        /// <param name="number">切り出す画像の番号</param>
        /// <returns>切り出された画像</returns>
        private Bitmap GetImageByNumber(int number)
        {
            int columns = 4;
            int rows = 27;
            int width = 32;
            int height = 16;
            int margin = 1;

            int colIndex = number / 100;
            int rowIndex = number % 100;

            if (colIndex >= columns || rowIndex >= rows)
            {
                throw new LEDDisplayNumberAbnormal(3, "指定された番号が無効です", new ArgumentOutOfRangeException());
            }

            int x = margin + colIndex * (width + margin);
            int y = margin + rowIndex * (height + margin);

            Bitmap croppedImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(croppedImage))
            {
                g.DrawImage(sourceImage, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
            }

            return croppedImage;
        }

        /// <summary>
        /// 指定された番号に対応するPictureBoxを取得する
        /// </summary>
        /// <param name="index">PictureBoxの番号（1～3）</param>
        /// <returns>対応するPictureBox</returns>
        private PictureBox GetPictureBoxByIndex(int index)
        {
            switch (index)
            {
                case 1:
                    return L1;
                case 2:
                    return L2;
                case 3:
                    return L3;
                default:
                    throw new LEDControlException(3, $"無効な表示器番号: {index}@GetPictureBoxByIndex", new ArgumentOutOfRangeException());
            }
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
    }
}
