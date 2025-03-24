using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ImageHistogramAnalyzer
{
    public partial class HistogramForm : Form
    {
        private Bitmap originalImage;
        private Bitmap currentImage;
        private Point[] curvePoints = new Point[256];
        private bool isDraggingPoint = false;
        private int draggedPointIndex = -1;

        // Элементы управления
        private PictureBox pictureBoxImage;
        private PictureBox histogramBox;
        private PictureBox curveBox;
        private Button btnLoad;
        private Button btnReset;

        public HistogramForm()
        {
            InitializeComponents();
            InitializeCurve();
        }

        private void InitializeComponents()
        {
            // Настройка формы
            this.Size = new Size(1000, 700);
            this.DoubleBuffered = true;

            // PictureBox для изображения
            pictureBoxImage = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Left,
                Width = 500,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(pictureBoxImage);

            // PictureBox для гистограммы
            histogramBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(510, 20),
                Size = new Size(450, 200),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            this.Controls.Add(histogramBox);

            // PictureBox для кривой
            curveBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(510, 240),
                Size = new Size(450, 300),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            curveBox.MouseDown += CurveBox_MouseDown;
            curveBox.MouseMove += CurveBox_MouseMove;
            curveBox.MouseUp += CurveBox_MouseUp;
            curveBox.Paint += CurveBox_Paint;
            this.Controls.Add(curveBox);

            // Кнопка загрузки
            btnLoad = new Button
            {
                Text = "Загрузить изображение",
                Location = new Point(510, 560),
                Size = new Size(150, 40),
                Font = new Font("Microsoft Sans Serif", 9),
                UseVisualStyleBackColor = true
            };
            btnLoad.Click += BtnLoad_Click;
            this.Controls.Add(btnLoad);

            // Кнопка сброса
            btnReset = new Button
            {
                Text = "Сбросить коррекцию",
                Location = new Point(670, 560),
                Size = new Size(150, 40),
                Font = new Font("Microsoft Sans Serif", 9),
                UseVisualStyleBackColor = true
            };
            btnReset.Click += BtnReset_Click;
            this.Controls.Add(btnReset);
        }

        private void InitializeCurve()
        {
            for (int i = 0; i < 256; i++)
            {
                curvePoints[i] = new Point(i, i);
            }
            DrawCurve();
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    originalImage = new Bitmap(openFileDialog.FileName);
                    currentImage = new Bitmap(originalImage);
                    pictureBoxImage.Image = currentImage;
                    DrawHistogram();
                    DrawCurve();
                }
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (originalImage != null)
            {
                currentImage = new Bitmap(originalImage);
                pictureBoxImage.Image = currentImage;
                InitializeCurve();
                DrawHistogram();
            }
        }

        private unsafe void DrawHistogram()
        {
            if (currentImage == null) return;

            int[] histogramR = new int[256];
            int[] histogramG = new int[256];
            int[] histogramB = new int[256];
            int[] histogramL = new int[256];

            BitmapData bmpData = currentImage.LockBits(
                new Rectangle(0, 0, currentImage.Width, currentImage.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            try
            {
                int bytesPerPixel = 3;
                int height = currentImage.Height;
                int width = currentImage.Width;
                byte* ptr = (byte*)bmpData.Scan0;

                for (int y = 0; y < height; y++)
                {
                    byte* row = ptr + (y * bmpData.Stride);
                    for (int x = 0; x < width; x++)
                    {
                        int b = row[x * bytesPerPixel];
                        int g = row[x * bytesPerPixel + 1];
                        int r = row[x * bytesPerPixel + 2];

                        int brightness = (int)(r + g + b)/3;
                        histogramL[brightness]++;
                    }
                }
            }
            finally
            {
                currentImage.UnlockBits(bmpData);
            }

            Bitmap histogramImage = new Bitmap(450, 200);
            using (Graphics g = Graphics.FromImage(histogramImage))
            {
                g.Clear(Color.White);

                int max = 1;
                for (int i = 0; i < 256; i++)
                {
                    if (histogramR[i] > max) max = histogramR[i];
                    if (histogramG[i] > max) max = histogramG[i];
                    if (histogramB[i] > max) max = histogramB[i];
                    if (histogramL[i] > max) max = histogramL[i];
                }

                for (int i = 0; i < 255; i++)
                {
                    int hL = (int)(200 * histogramL[i] / (double)max);
                    using (var pen = new Pen(Color.FromArgb(80, 128, 128, 128), 2))
                        g.DrawLine(pen, i * 450 / 256, 199, i * 450 / 256, 199 - hL);

                    int hR = (int)(200 * histogramR[i] / (double)max);
                    using (var pen = new Pen(Color.FromArgb(150, 255, 0, 0), 1))
                        g.DrawLine(pen, i * 450 / 256, 199, i * 450 / 256, 199 - hR);

                    int hG = (int)(200 * histogramG[i] / (double)max);
                    using (var pen = new Pen(Color.FromArgb(150, 0, 255, 0), 1))
                        g.DrawLine(pen, i * 450 / 256, 199, i * 450 / 256, 199 - hG);

                    int hB = (int)(200 * histogramB[i] / (double)max);
                    using (var pen = new Pen(Color.FromArgb(150, 0, 0, 255), 1))
                        g.DrawLine(pen, i * 450 / 256, 199, i * 450 / 256, 199 - hB);
                }
            }

            histogramBox.Image = histogramImage;
        }

        private void DrawCurve()
        {
            Bitmap curveBmp = new Bitmap(450, 300);
            using (Graphics g = Graphics.FromImage(curveBmp))
            {
                g.Clear(Color.White);

                // Сетка
                for (int i = 0; i < 256; i += 32)
                {
                    g.DrawLine(Pens.LightGray, i * 450 / 256, 0, i * 450 / 256, 299);
                    g.DrawLine(Pens.LightGray, 0, i * 300 / 256, 449, i * 300 / 256);
                }

                // Оси
                g.DrawLine(Pens.Black, 0, 299, 449, 299); // X ось
                g.DrawLine(Pens.Black, 0, 299, 0, 0);     // Y ось

                // Кривая (лево-низ → право-верх)
                Point[] scaledPoints = new Point[256];
                for (int i = 0; i < 256; i++)
                {
                    scaledPoints[i] = new Point(
                        i * 450 / 256,
                        299 - curvePoints[i].Y * 300 / 256);
                }
                g.DrawCurve(Pens.Blue, scaledPoints);

                // Контрольные точки
                for (int i = 0; i < 256; i += 32)
                {
                    g.FillEllipse(Brushes.Red,
                        scaledPoints[i].X - 3,
                        scaledPoints[i].Y - 3,
                        6, 6);
                }
            }

            curveBox.Image = curveBmp;
        }

        private unsafe void ApplyCurve()
        {
            if (originalImage == null) return;

            byte[] lookupTable = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                lookupTable[i] = (byte)(curvePoints[i].Y);
            }

            currentImage = new Bitmap(originalImage.Width, originalImage.Height);

            BitmapData origData = originalImage.LockBits(
                new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            BitmapData currentData = currentImage.LockBits(
                new Rectangle(0, 0, currentImage.Width, currentImage.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            try
            {
                int bytesPerPixel = 3;
                int height = originalImage.Height;
                int width = originalImage.Width;

                byte* origPtr = (byte*)origData.Scan0;
                byte* currentPtr = (byte*)currentData.Scan0;

                for (int y = 0; y < height; y++)
                {
                    byte* origRow = origPtr + (y * origData.Stride);
                    byte* currentRow = currentPtr + (y * currentData.Stride);

                    for (int x = 0; x < width; x++)
                    {
                        currentRow[x * bytesPerPixel] = lookupTable[origRow[x * bytesPerPixel]];       // B
                        currentRow[x * bytesPerPixel + 1] = lookupTable[origRow[x * bytesPerPixel + 1]]; // G
                        currentRow[x * bytesPerPixel + 2] = lookupTable[origRow[x * bytesPerPixel + 2]]; // R
                    }
                }
            }
            finally
            {
                originalImage.UnlockBits(origData);
                currentImage.UnlockBits(currentData);
            }

            pictureBoxImage.Image = currentImage;
            DrawHistogram();
        }

        private void CurveBox_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < 256; i += 32)
            {
                int x = i * 450 / 256;
                int y = 299 - curvePoints[i].Y * 300 / 256;

                if (Math.Abs(e.X - x) < 10 && Math.Abs(e.Y - y) < 10)
                {
                    isDraggingPoint = true;
                    draggedPointIndex = i;
                    break;
                }
            }
        }

        private void CurveBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingPoint && draggedPointIndex >= 0)
            {
                int newY = 299 - e.Y;
                newY = Math.Max(0, Math.Min(255, newY * 256 / 300));
                curvePoints[draggedPointIndex].Y = newY;

                InterpolateCurve(draggedPointIndex);
                DrawCurve();
                ApplyCurve();
            }
        }

        private void InterpolateCurve(int mainIndex)
        {
            int prevIndex = Math.Max(0, mainIndex - 32);
            int nextIndex = Math.Min(255, mainIndex + 32);

            for (int i = prevIndex; i < mainIndex; i++)
            {
                float t = (float)(i - prevIndex) / (mainIndex - prevIndex);
                curvePoints[i].Y = (int)(curvePoints[prevIndex].Y +
                    t * (curvePoints[mainIndex].Y - curvePoints[prevIndex].Y));
            }

            for (int i = mainIndex + 1; i <= nextIndex; i++)
            {
                float t = (float)(i - mainIndex) / (nextIndex - mainIndex);
                curvePoints[i].Y = (int)(curvePoints[mainIndex].Y +
                    t * (curvePoints[nextIndex].Y - curvePoints[mainIndex].Y));
            }
        }

        private void CurveBox_MouseUp(object sender, MouseEventArgs e)
        {
            isDraggingPoint = false;
            draggedPointIndex = -1;
        }

        private void CurveBox_Paint(object sender, PaintEventArgs e)
        {
            DrawCurve();
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HistogramForm());
        }
    }
}