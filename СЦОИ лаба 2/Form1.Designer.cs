namespace СЦОИ_лаба_2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBoxImage = new PictureBox();
            histogramBox = new PictureBox();
            curveBox = new PictureBox();
            btnLoad = new Button();
            btnReset = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)histogramBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)curveBox).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxImage
            // 
            pictureBoxImage.Location = new Point(37, 29);
            pictureBoxImage.Name = "pictureBoxImage";
            pictureBoxImage.Size = new Size(267, 160);
            pictureBoxImage.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxImage.TabIndex = 0;
            pictureBoxImage.TabStop = false;
            pictureBoxImage.Click += pictureBox1_Click;
            // 
            // histogramBox
            // 
            histogramBox.Location = new Point(37, 287);
            histogramBox.Name = "histogramBox";
            histogramBox.Size = new Size(450, 190);
            histogramBox.SizeMode = PictureBoxSizeMode.Zoom;
            histogramBox.TabIndex = 1;
            histogramBox.TabStop = false;
            histogramBox.Click += histogramBox_Click;
            // 
            // curveBox
            // 
            curveBox.Location = new Point(528, 124);
            curveBox.Name = "curveBox";
            curveBox.Size = new Size(450, 300);
            curveBox.SizeMode = PictureBoxSizeMode.Zoom;
            curveBox.TabIndex = 2;
            curveBox.TabStop = false;
            curveBox.Click += curveBox_Click;
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(377, 59);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(127, 23);
            btnLoad.TabIndex = 3;
            btnLoad.Text = "Открыть картинку";
            btnLoad.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(377, 106);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(84, 23);
            btnReset.TabIndex = 4;
            btnReset.Text = "Сброс";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1116, 606);
            Controls.Add(btnReset);
            Controls.Add(btnLoad);
            Controls.Add(curveBox);
            Controls.Add(histogramBox);
            Controls.Add(pictureBoxImage);
            DoubleBuffered = true;
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBoxImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)histogramBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)curveBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBoxImage;
        private PictureBox histogramBox;
        private PictureBox curveBox;
        private Button btnLoad;
        private Button btnReset;
    }
}
