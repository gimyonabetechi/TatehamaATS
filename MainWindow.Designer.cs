namespace TatehamaATS
{
    partial class MainWindow
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
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            TanudenStatus = new ToolStripStatusLabel();
            toolStripStatusLabel2 = new ToolStripStatusLabel();
            SignalStatus = new ToolStripStatusLabel();
            LEDWindowButton = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            ToL1Button = new Button();
            LEDDisplayNum = new NumericUpDown();
            ToL2Button = new Button();
            ToL3Button = new Button();
            toolStripStatusLabel3 = new ToolStripStatusLabel();
            LEDStatus = new ToolStripStatusLabel();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LEDDisplayNum).BeginInit();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("ＭＳ ゴシック", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, TanudenStatus, toolStripStatusLabel2, SignalStatus, toolStripStatusLabel3, LEDStatus });
            statusStrip1.Location = new Point(0, 455);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(914, 25);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.BorderSides = ToolStripStatusLabelBorderSides.Left;
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(59, 20);
            toolStripStatusLabel1.Text = "タヌ電";
            // 
            // TanudenStatus
            // 
            TanudenStatus.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Right;
            TanudenStatus.Name = "TanudenStatus";
            TanudenStatus.Size = new Size(59, 20);
            TanudenStatus.Text = "接続中";
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.BorderSides = ToolStripStatusLabelBorderSides.Left;
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new Size(43, 20);
            toolStripStatusLabel2.Text = "信号";
            // 
            // SignalStatus
            // 
            SignalStatus.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Right;
            SignalStatus.Name = "SignalStatus";
            SignalStatus.Size = new Size(59, 20);
            SignalStatus.Text = "接続中";
            // 
            // LEDWindowButton
            // 
            LEDWindowButton.Location = new Point(12, 12);
            LEDWindowButton.Name = "LEDWindowButton";
            LEDWindowButton.Size = new Size(96, 50);
            LEDWindowButton.TabIndex = 1;
            LEDWindowButton.Text = "LED表示器";
            LEDWindowButton.UseVisualStyleBackColor = true;
            LEDWindowButton.Click += LEDWindowButton_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(89, 80);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 83);
            label1.Name = "label1";
            label1.Size = new Size(71, 16);
            label1.TabIndex = 3;
            label1.Text = "列番設定";
            // 
            // ToL1Button
            // 
            ToL1Button.Location = new Point(680, 12);
            ToL1Button.Name = "ToL1Button";
            ToL1Button.Size = new Size(96, 27);
            ToL1Button.TabIndex = 4;
            ToL1Button.Text = "L1表示";
            ToL1Button.UseVisualStyleBackColor = true;
            ToL1Button.Click += ToL1Button_Click;
            // 
            // LEDDisplayNum
            // 
            LEDDisplayNum.Location = new Point(782, 12);
            LEDDisplayNum.Maximum = new decimal(new int[] { 400, 0, 0, 0 });
            LEDDisplayNum.Name = "LEDDisplayNum";
            LEDDisplayNum.Size = new Size(120, 23);
            LEDDisplayNum.TabIndex = 5;
            // 
            // ToL2Button
            // 
            ToL2Button.Location = new Point(680, 45);
            ToL2Button.Name = "ToL2Button";
            ToL2Button.Size = new Size(96, 27);
            ToL2Button.TabIndex = 6;
            ToL2Button.Text = "L2表示";
            ToL2Button.UseVisualStyleBackColor = true;
            ToL2Button.Click += ToL2Button_Click;
            // 
            // ToL3Button
            // 
            ToL3Button.Location = new Point(680, 78);
            ToL3Button.Name = "ToL3Button";
            ToL3Button.Size = new Size(96, 27);
            ToL3Button.TabIndex = 7;
            ToL3Button.Text = "L2表示";
            ToL3Button.UseVisualStyleBackColor = true;
            ToL3Button.Click += ToL3Button_Click;
            // 
            // toolStripStatusLabel3
            // 
            toolStripStatusLabel3.BorderSides = ToolStripStatusLabelBorderSides.Left;
            toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            toolStripStatusLabel3.Size = new Size(35, 20);
            toolStripStatusLabel3.Text = "LED";
            // 
            // LEDStatus
            // 
            LEDStatus.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Right;
            LEDStatus.Name = "LEDStatus";
            LEDStatus.Size = new Size(59, 20);
            LEDStatus.Text = "非表示";
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 480);
            Controls.Add(ToL3Button);
            Controls.Add(ToL2Button);
            Controls.Add(LEDDisplayNum);
            Controls.Add(ToL1Button);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(LEDWindowButton);
            Controls.Add(statusStrip1);
            Font = new Font("ＭＳ ゴシック", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Name = "MainWindow";
            Text = "MainWindow";
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)LEDDisplayNum).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel TanudenStatus;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripStatusLabel SignalStatus;
        private Button LEDWindowButton;
        private TextBox textBox1;
        private Label label1;
        private Button ToL1Button;
        private NumericUpDown LEDDisplayNum;
        private Button ToL2Button;
        private Button ToL3Button;
        private ToolStripStatusLabel toolStripStatusLabel3;
        private ToolStripStatusLabel LEDStatus;
    }
}