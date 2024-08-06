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
            toolStripStatusLabel3 = new ToolStripStatusLabel();
            LEDStatus = new ToolStripStatusLabel();
            toolStripStatusLabel4 = new ToolStripStatusLabel();
            SigWinStatus = new ToolStripStatusLabel();
            toolStripStatusLabel5 = new ToolStripStatusLabel();
            LEDWindowButton = new Button();
            ATSResetButton = new Button();
            SignalWindowButton = new Button();
            Retsuban7 = new Button();
            panel1 = new Panel();
            RetsubanRin = new Button();
            RetsubanKai = new Button();
            RetsubanA = new Button();
            RetsubanBack = new Button();
            RetsubanEnter = new Button();
            RetsubanAllDel = new Button();
            Retsuban0 = new Button();
            RetsubanDel = new Button();
            Retsuban1 = new Button();
            Retsuban3 = new Button();
            Retsuban2 = new Button();
            RetsubanK = new Button();
            RetsubanC = new Button();
            Retsuban6 = new Button();
            Retsuban5 = new Button();
            Retsuban4 = new Button();
            RetsubanB = new Button();
            Retsuban9 = new Button();
            Retsuban8 = new Button();
            panel2 = new Panel();
            label2 = new Label();
            CarText = new Label();
            label6 = new Label();
            RetsubanText = new Label();
            label3 = new Label();
            label5 = new Label();
            statusStrip1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("ＭＳ ゴシック", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, TanudenStatus, toolStripStatusLabel2, SignalStatus, toolStripStatusLabel3, LEDStatus, toolStripStatusLabel4, SigWinStatus, toolStripStatusLabel5 });
            statusStrip1.Location = new Point(0, 283);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(498, 25);
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
            TanudenStatus.Text = "仮実装";
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.BorderSides = ToolStripStatusLabelBorderSides.Left;
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new Size(59, 20);
            toolStripStatusLabel2.Text = "信号鯖";
            // 
            // SignalStatus
            // 
            SignalStatus.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Right;
            SignalStatus.Name = "SignalStatus";
            SignalStatus.Size = new Size(59, 20);
            SignalStatus.Text = "実装済";
            // 
            // toolStripStatusLabel3
            // 
            toolStripStatusLabel3.BorderSides = ToolStripStatusLabelBorderSides.Left;
            toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            toolStripStatusLabel3.Size = new Size(51, 20);
            toolStripStatusLabel3.Text = "LED窓";
            // 
            // LEDStatus
            // 
            LEDStatus.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Right;
            LEDStatus.Name = "LEDStatus";
            LEDStatus.Size = new Size(59, 20);
            LEDStatus.Text = "非表示";
            // 
            // toolStripStatusLabel4
            // 
            toolStripStatusLabel4.BorderSides = ToolStripStatusLabelBorderSides.Left;
            toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            toolStripStatusLabel4.Size = new Size(59, 20);
            toolStripStatusLabel4.Text = "信号窓";
            // 
            // SigWinStatus
            // 
            SigWinStatus.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Right;
            SigWinStatus.Name = "SigWinStatus";
            SigWinStatus.Size = new Size(59, 20);
            SigWinStatus.Text = "非表示";
            // 
            // toolStripStatusLabel5
            // 
            toolStripStatusLabel5.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Right;
            toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            toolStripStatusLabel5.Size = new Size(75, 20);
            toolStripStatusLabel5.Text = "Ver0.3.2";
            // 
            // LEDWindowButton
            // 
            LEDWindowButton.Location = new Point(2, 214);
            LEDWindowButton.Name = "LEDWindowButton";
            LEDWindowButton.Size = new Size(96, 50);
            LEDWindowButton.TabIndex = 1;
            LEDWindowButton.Text = "LED表示器";
            LEDWindowButton.UseVisualStyleBackColor = true;
            LEDWindowButton.Click += LEDWindowButton_Click;
            // 
            // ATSResetButton
            // 
            ATSResetButton.Font = new Font("ＭＳ ゴシック", 24F, FontStyle.Regular, GraphicsUnit.Point, 128);
            ATSResetButton.Location = new Point(4, 6);
            ATSResetButton.Name = "ATSResetButton";
            ATSResetButton.Size = new Size(203, 83);
            ATSResetButton.TabIndex = 4;
            ATSResetButton.Text = "ATS復帰";
            ATSResetButton.UseVisualStyleBackColor = true;
            ATSResetButton.Click += ATSResetButton_Click;
            // 
            // SignalWindowButton
            // 
            SignalWindowButton.Location = new Point(2, 158);
            SignalWindowButton.Name = "SignalWindowButton";
            SignalWindowButton.Size = new Size(96, 50);
            SignalWindowButton.TabIndex = 5;
            SignalWindowButton.Text = "信号表示";
            SignalWindowButton.UseVisualStyleBackColor = true;
            SignalWindowButton.Click += SignalWindowButton_Click;
            // 
            // Retsuban7
            // 
            Retsuban7.Location = new Point(3, 3);
            Retsuban7.Name = "Retsuban7";
            Retsuban7.Size = new Size(40, 40);
            Retsuban7.TabIndex = 6;
            Retsuban7.Text = "７";
            Retsuban7.UseVisualStyleBackColor = true;
            Retsuban7.Click += Retsuban7_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.DimGray;
            panel1.Controls.Add(RetsubanRin);
            panel1.Controls.Add(RetsubanKai);
            panel1.Controls.Add(RetsubanA);
            panel1.Controls.Add(RetsubanBack);
            panel1.Controls.Add(RetsubanEnter);
            panel1.Controls.Add(RetsubanAllDel);
            panel1.Controls.Add(Retsuban0);
            panel1.Controls.Add(RetsubanDel);
            panel1.Controls.Add(Retsuban1);
            panel1.Controls.Add(Retsuban3);
            panel1.Controls.Add(Retsuban2);
            panel1.Controls.Add(RetsubanK);
            panel1.Controls.Add(RetsubanC);
            panel1.Controls.Add(Retsuban6);
            panel1.Controls.Add(Retsuban5);
            panel1.Controls.Add(Retsuban4);
            panel1.Controls.Add(RetsubanB);
            panel1.Controls.Add(Retsuban9);
            panel1.Controls.Add(Retsuban8);
            panel1.Controls.Add(Retsuban7);
            panel1.Location = new Point(22, 78);
            panel1.Name = "panel1";
            panel1.Size = new Size(231, 186);
            panel1.TabIndex = 7;
            // 
            // RetsubanRin
            // 
            RetsubanRin.Location = new Point(95, 141);
            RetsubanRin.Name = "RetsubanRin";
            RetsubanRin.Size = new Size(40, 40);
            RetsubanRin.TabIndex = 24;
            RetsubanRin.Text = "Ｘ";
            RetsubanRin.UseVisualStyleBackColor = true;
            RetsubanRin.Click += RetsubanRin_Click;
            // 
            // RetsubanKai
            // 
            RetsubanKai.Location = new Point(49, 141);
            RetsubanKai.Name = "RetsubanKai";
            RetsubanKai.Size = new Size(40, 40);
            RetsubanKai.TabIndex = 23;
            RetsubanKai.Text = "回";
            RetsubanKai.UseVisualStyleBackColor = true;
            RetsubanKai.Click += RetsubanKai_Click;
            // 
            // RetsubanA
            // 
            RetsubanA.Location = new Point(141, 141);
            RetsubanA.Name = "RetsubanA";
            RetsubanA.Size = new Size(40, 40);
            RetsubanA.TabIndex = 22;
            RetsubanA.Text = "Ａ";
            RetsubanA.UseVisualStyleBackColor = true;
            RetsubanA.Click += RetsubanA_Click;
            // 
            // RetsubanBack
            // 
            RetsubanBack.Location = new Point(187, 3);
            RetsubanBack.Name = "RetsubanBack";
            RetsubanBack.Size = new Size(40, 40);
            RetsubanBack.TabIndex = 21;
            RetsubanBack.Text = "戻る";
            RetsubanBack.UseVisualStyleBackColor = true;
            RetsubanBack.Click += RetsubanBack_Click;
            // 
            // RetsubanEnter
            // 
            RetsubanEnter.Location = new Point(187, 141);
            RetsubanEnter.Name = "RetsubanEnter";
            RetsubanEnter.Size = new Size(40, 40);
            RetsubanEnter.TabIndex = 20;
            RetsubanEnter.Text = "設定";
            RetsubanEnter.UseVisualStyleBackColor = true;
            RetsubanEnter.Click += RetsubanEnter_Click;
            // 
            // RetsubanAllDel
            // 
            RetsubanAllDel.Location = new Point(187, 49);
            RetsubanAllDel.Name = "RetsubanAllDel";
            RetsubanAllDel.Size = new Size(40, 40);
            RetsubanAllDel.TabIndex = 19;
            RetsubanAllDel.Text = "全消";
            RetsubanAllDel.UseVisualStyleBackColor = true;
            RetsubanAllDel.Click += RetsubanAllDel_Click;
            // 
            // Retsuban0
            // 
            Retsuban0.Location = new Point(3, 141);
            Retsuban0.Name = "Retsuban0";
            Retsuban0.Size = new Size(40, 40);
            Retsuban0.TabIndex = 18;
            Retsuban0.Text = "０";
            Retsuban0.UseVisualStyleBackColor = true;
            Retsuban0.Click += Retsuban0_Click;
            // 
            // RetsubanDel
            // 
            RetsubanDel.Location = new Point(187, 95);
            RetsubanDel.Name = "RetsubanDel";
            RetsubanDel.Size = new Size(40, 40);
            RetsubanDel.TabIndex = 18;
            RetsubanDel.Text = "消去";
            RetsubanDel.UseVisualStyleBackColor = true;
            RetsubanDel.Click += RetsubanDel_Click;
            // 
            // Retsuban1
            // 
            Retsuban1.Location = new Point(3, 95);
            Retsuban1.Name = "Retsuban1";
            Retsuban1.Size = new Size(40, 40);
            Retsuban1.TabIndex = 17;
            Retsuban1.Text = "１";
            Retsuban1.UseVisualStyleBackColor = true;
            Retsuban1.Click += Retsuban1_Click;
            // 
            // Retsuban3
            // 
            Retsuban3.Location = new Point(95, 95);
            Retsuban3.Name = "Retsuban3";
            Retsuban3.Size = new Size(40, 40);
            Retsuban3.TabIndex = 16;
            Retsuban3.Text = "３";
            Retsuban3.UseVisualStyleBackColor = true;
            Retsuban3.Click += Retsuban3_Click;
            // 
            // Retsuban2
            // 
            Retsuban2.Location = new Point(49, 95);
            Retsuban2.Name = "Retsuban2";
            Retsuban2.Size = new Size(40, 40);
            Retsuban2.TabIndex = 15;
            Retsuban2.Text = "２";
            Retsuban2.UseVisualStyleBackColor = true;
            Retsuban2.Click += Retsuban2_Click;
            // 
            // RetsubanK
            // 
            RetsubanK.Location = new Point(141, 95);
            RetsubanK.Name = "RetsubanK";
            RetsubanK.Size = new Size(40, 40);
            RetsubanK.TabIndex = 14;
            RetsubanK.Text = "Ｋ";
            RetsubanK.UseVisualStyleBackColor = true;
            RetsubanK.Click += RetsubanK_Click;
            // 
            // RetsubanC
            // 
            RetsubanC.Location = new Point(141, 3);
            RetsubanC.Name = "RetsubanC";
            RetsubanC.Size = new Size(40, 40);
            RetsubanC.TabIndex = 13;
            RetsubanC.Text = "Ｃ";
            RetsubanC.UseVisualStyleBackColor = true;
            RetsubanC.Click += RetsubanC_Click;
            // 
            // Retsuban6
            // 
            Retsuban6.Location = new Point(95, 49);
            Retsuban6.Name = "Retsuban6";
            Retsuban6.Size = new Size(40, 40);
            Retsuban6.TabIndex = 12;
            Retsuban6.Text = "６";
            Retsuban6.UseVisualStyleBackColor = true;
            Retsuban6.Click += Retsuban6_Click;
            // 
            // Retsuban5
            // 
            Retsuban5.Location = new Point(49, 49);
            Retsuban5.Name = "Retsuban5";
            Retsuban5.Size = new Size(40, 40);
            Retsuban5.TabIndex = 11;
            Retsuban5.Text = "５";
            Retsuban5.UseVisualStyleBackColor = true;
            Retsuban5.Click += Retsuban5_Click;
            // 
            // Retsuban4
            // 
            Retsuban4.Location = new Point(3, 49);
            Retsuban4.Name = "Retsuban4";
            Retsuban4.Size = new Size(40, 40);
            Retsuban4.TabIndex = 10;
            Retsuban4.Text = "４";
            Retsuban4.UseVisualStyleBackColor = true;
            Retsuban4.Click += Retsuban4_Click;
            // 
            // RetsubanB
            // 
            RetsubanB.Location = new Point(141, 49);
            RetsubanB.Name = "RetsubanB";
            RetsubanB.Size = new Size(40, 40);
            RetsubanB.TabIndex = 9;
            RetsubanB.Text = "Ｂ";
            RetsubanB.UseVisualStyleBackColor = true;
            RetsubanB.Click += RetsubanB_Click;
            // 
            // Retsuban9
            // 
            Retsuban9.Location = new Point(95, 3);
            Retsuban9.Name = "Retsuban9";
            Retsuban9.Size = new Size(40, 40);
            Retsuban9.TabIndex = 8;
            Retsuban9.Text = "９";
            Retsuban9.UseVisualStyleBackColor = true;
            Retsuban9.Click += Retsuban9_Click;
            // 
            // Retsuban8
            // 
            Retsuban8.Location = new Point(49, 3);
            Retsuban8.Name = "Retsuban8";
            Retsuban8.Size = new Size(40, 40);
            Retsuban8.TabIndex = 7;
            Retsuban8.Text = "８";
            Retsuban8.UseVisualStyleBackColor = true;
            Retsuban8.Click += Retsuban8_Click;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            panel2.BackColor = Color.DarkGray;
            panel2.Controls.Add(label2);
            panel2.Controls.Add(CarText);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(RetsubanText);
            panel2.Controls.Add(panel1);
            panel2.Controls.Add(label3);
            panel2.Location = new Point(217, 8);
            panel2.Name = "panel2";
            panel2.Size = new Size(269, 272);
            panel2.TabIndex = 8;
            // 
            // label2
            // 
            label2.BackColor = Color.DimGray;
            label2.Font = new Font("ＭＳ ゴシック", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            label2.ForeColor = Color.Transparent;
            label2.Location = new Point(209, 42);
            label2.Name = "label2";
            label2.RightToLeft = RightToLeft.No;
            label2.Size = new Size(44, 24);
            label2.TabIndex = 11;
            label2.Text = "両";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CarText
            // 
            CarText.BackColor = Color.DimGray;
            CarText.Font = new Font("ＭＳ ゴシック", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            CarText.ForeColor = Color.Transparent;
            CarText.Location = new Point(131, 42);
            CarText.Name = "CarText";
            CarText.RightToLeft = RightToLeft.No;
            CarText.Size = new Size(81, 24);
            CarText.TabIndex = 10;
            CarText.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(40, 48);
            label6.Name = "label6";
            label6.Size = new Size(71, 16);
            label6.TabIndex = 9;
            label6.Text = "編成両数";
            // 
            // RetsubanText
            // 
            RetsubanText.BackColor = Color.DimGray;
            RetsubanText.Font = new Font("ＭＳ ゴシック", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            RetsubanText.ForeColor = Color.Transparent;
            RetsubanText.Location = new Point(131, 12);
            RetsubanText.Name = "RetsubanText";
            RetsubanText.RightToLeft = RightToLeft.No;
            RetsubanText.Size = new Size(122, 24);
            RetsubanText.TabIndex = 8;
            RetsubanText.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(40, 18);
            label3.Name = "label3";
            label3.Size = new Size(71, 16);
            label3.TabIndex = 3;
            label3.Text = "列　　番";
            // 
            // label5
            // 
            label5.BackColor = Color.Gold;
            label5.Font = new Font("ＭＳ ゴシック", 18F, FontStyle.Regular, GraphicsUnit.Point, 128);
            label5.ForeColor = Color.Black;
            label5.Location = new Point(4, 92);
            label5.Name = "label5";
            label5.RightToLeft = RightToLeft.No;
            label5.Size = new Size(203, 60);
            label5.TabIndex = 11;
            label5.Text = "復帰は指令の\r\n指示を受けてから";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            label5.UseMnemonic = false;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(498, 308);
            Controls.Add(panel2);
            Controls.Add(SignalWindowButton);
            Controls.Add(ATSResetButton);
            Controls.Add(LEDWindowButton);
            Controls.Add(statusStrip1);
            Controls.Add(label5);
            Font = new Font("ＭＳ ゴシック", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Name = "MainWindow";
            Text = "MainWindow";
            Load += MainWindow_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
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
        private ToolStripStatusLabel toolStripStatusLabel3;
        private ToolStripStatusLabel LEDStatus;
        private Button ATSResetButton;
        private Button SignalWindowButton;
        private ToolStripStatusLabel toolStripStatusLabel4;
        private ToolStripStatusLabel SigWinStatus;
        private Button Retsuban7;
        private Panel panel1;
        private Button RetsubanEnter;
        private Button RetsubanAllDel;
        private Button Retsuban0;
        private Button RetsubanDel;
        private Button Retsuban1;
        private Button Retsuban3;
        private Button Retsuban2;
        private Button RetsubanK;
        private Button RetsubanC;
        private Button Retsuban6;
        private Button Retsuban5;
        private Button Retsuban4;
        private Button RetsubanB;
        private Button Retsuban9;
        private Button Retsuban8;
        private Button RetsubanRin;
        private Button RetsubanKai;
        private Button RetsubanA;
        private Button RetsubanBack;
        private Panel panel2;
        private Label RetsubanText;
        private Label CarText;
        private Label label5;
        private Label label2;
        private Label label6;
        private Label label3;
        private ToolStripStatusLabel toolStripStatusLabel5;
    }
}