namespace TatehamaATS.DisplayLED
{
    partial class LEDWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            L1 = new PictureBox();
            panel1 = new Panel();
            L3 = new PictureBox();
            L2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)L1).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)L3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)L2).BeginInit();
            SuspendLayout();
            // 
            // L1
            // 
            L1.BackColor = Color.Transparent;
            L1.BackgroundImage = Properties.Resources.Null;
            L1.BackgroundImageLayout = ImageLayout.Stretch;
            L1.Image = Properties.Resources.LED_Waku;
            L1.ImageLocation = "";
            L1.Location = new Point(6, 6);
            L1.Margin = new Padding(0);
            L1.Name = "L1";
            L1.Size = new Size(193, 97);
            L1.SizeMode = PictureBoxSizeMode.StretchImage;
            L1.TabIndex = 0;
            L1.TabStop = false;
            L1.MouseDown += pictureBox1_MouseDown;
            L1.MouseMove += pictureBox1_MouseMove;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(33, 33, 41);
            panel1.Controls.Add(L3);
            panel1.Controls.Add(L1);
            panel1.Controls.Add(L2);
            panel1.Location = new Point(16, 6);
            panel1.Name = "panel1";
            panel1.Size = new Size(205, 319);
            panel1.TabIndex = 3;
            panel1.MouseDown += pictureBox1_MouseDown;
            panel1.MouseMove += pictureBox1_MouseMove;
            // 
            // L3
            // 
            L3.BackColor = Color.Transparent;
            L3.BackgroundImage = Properties.Resources._3B1;
            L3.BackgroundImageLayout = ImageLayout.Stretch;
            L3.Image = Properties.Resources.LED_Waku;
            L3.ImageLocation = "";
            L3.Location = new Point(6, 216);
            L3.Margin = new Padding(0);
            L3.Name = "L3";
            L3.Size = new Size(193, 97);
            L3.SizeMode = PictureBoxSizeMode.StretchImage;
            L3.TabIndex = 6;
            L3.TabStop = false;
            L3.MouseDown += pictureBox1_MouseDown;
            L3.MouseMove += pictureBox1_MouseMove;
            // 
            // L2
            // 
            L2.BackgroundImage = Properties.Resources.Null;
            L2.BackgroundImageLayout = ImageLayout.Stretch;
            L2.Image = Properties.Resources.LED_Waku;
            L2.ImageLocation = "";
            L2.Location = new Point(6, 111);
            L2.Margin = new Padding(0);
            L2.Name = "L2";
            L2.Size = new Size(193, 97);
            L2.SizeMode = PictureBoxSizeMode.StretchImage;
            L2.TabIndex = 5;
            L2.TabStop = false;
            L2.MouseDown += pictureBox1_MouseDown;
            L2.MouseMove += pictureBox1_MouseMove;
            // 
            // LEDWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(237, 331);
            ControlBox = false;
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LEDWindow";
            Text = "LEDWIndow";
            TopMost = true;
            MouseDown += pictureBox1_MouseDown;
            MouseMove += pictureBox1_MouseMove;
            ((System.ComponentModel.ISupportInitialize)L1).EndInit();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)L3).EndInit();
            ((System.ComponentModel.ISupportInitialize)L2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private PictureBox L1;
        private PictureBox L2;
        private PictureBox L3;
    }
}