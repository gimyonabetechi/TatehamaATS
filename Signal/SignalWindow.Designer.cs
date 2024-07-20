namespace TatehamaATS.Signal
{
    partial class SignalWindow
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
            SignaiPic = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)SignaiPic).BeginInit();
            SuspendLayout();
            // 
            // SignaiPic
            // 
            SignaiPic.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            SignaiPic.BackColor = Color.Transparent;
            SignaiPic.BackgroundImage = Properties.Resources._3_g;
            SignaiPic.BackgroundImageLayout = ImageLayout.Center;
            SignaiPic.Location = new Point(0, 0);
            SignaiPic.Name = "SignaiPic";
            SignaiPic.Size = new Size(300, 540);
            SignaiPic.TabIndex = 0;
            SignaiPic.TabStop = false;
            SignaiPic.MouseDown += pictureBox1_MouseDown;
            SignaiPic.MouseMove += pictureBox1_MouseMove;
            // 
            // SignalWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(300, 540);
            ControlBox = false;
            Controls.Add(SignaiPic);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            Name = "SignalWindow";
            ShowIcon = false;
            Text = "SignalWindow";
            TopMost = true;
            ((System.ComponentModel.ISupportInitialize)SignaiPic).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox SignaiPic;
    }
}