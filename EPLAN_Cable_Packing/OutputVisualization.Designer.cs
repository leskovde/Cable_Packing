namespace EPLAN_Cable_Packing
{
    partial class OutputVisualization
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
            this.Canvas = new System.Windows.Forms.Panel();
            this.UiPanel = new System.Windows.Forms.Panel();
            this.SinglePassSwitch = new System.Windows.Forms.RadioButton();
            this.Status = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.LinearSwitch = new System.Windows.Forms.RadioButton();
            this.GreedySwitch = new System.Windows.Forms.RadioButton();
            this.StartButton = new System.Windows.Forms.Button();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.OutputDiameterText = new System.Windows.Forms.Label();
            this.OutputDiameter = new System.Windows.Forms.Label();
            this.FileName = new System.Windows.Forms.Label();
            this.FileNameLabel = new System.Windows.Forms.Label();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.UiPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Canvas
            // 
            this.Canvas.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Canvas.Location = new System.Drawing.Point(0, 131);
            this.Canvas.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(782, 662);
            this.Canvas.TabIndex = 0;
            this.Canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.Canvas_Paint);
            // 
            // UiPanel
            // 
            this.UiPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.UiPanel.Controls.Add(this.SinglePassSwitch);
            this.UiPanel.Controls.Add(this.Status);
            this.UiPanel.Controls.Add(this.StatusLabel);
            this.UiPanel.Controls.Add(this.LinearSwitch);
            this.UiPanel.Controls.Add(this.GreedySwitch);
            this.UiPanel.Controls.Add(this.StartButton);
            this.UiPanel.Controls.Add(this.BrowseButton);
            this.UiPanel.Controls.Add(this.OutputDiameterText);
            this.UiPanel.Controls.Add(this.OutputDiameter);
            this.UiPanel.Controls.Add(this.FileName);
            this.UiPanel.Controls.Add(this.FileNameLabel);
            this.UiPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.UiPanel.Location = new System.Drawing.Point(0, 0);
            this.UiPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.UiPanel.Name = "UiPanel";
            this.UiPanel.Size = new System.Drawing.Size(782, 132);
            this.UiPanel.TabIndex = 1;
            // 
            // SinglePassSwitch
            // 
            this.SinglePassSwitch.AutoSize = true;
            this.SinglePassSwitch.Location = new System.Drawing.Point(423, 62);
            this.SinglePassSwitch.Name = "SinglePassSwitch";
            this.SinglePassSwitch.Size = new System.Drawing.Size(165, 21);
            this.SinglePassSwitch.TabIndex = 10;
            this.SinglePassSwitch.Text = "Single Pass algorithm";
            this.SinglePassSwitch.UseVisualStyleBackColor = true;
            this.SinglePassSwitch.CheckedChanged += new System.EventHandler(this.SinglePassSwitch_CheckedChanged);
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.Location = new System.Drawing.Point(71, 93);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(23, 17);
            this.Status.TabIndex = 9;
            this.Status.Text = "---";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(13, 93);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(52, 17);
            this.StatusLabel.TabIndex = 8;
            this.StatusLabel.Text = "Status:";
            // 
            // LinearSwitch
            // 
            this.LinearSwitch.AutoSize = true;
            this.LinearSwitch.Location = new System.Drawing.Point(423, 89);
            this.LinearSwitch.Name = "LinearSwitch";
            this.LinearSwitch.Size = new System.Drawing.Size(219, 21);
            this.LinearSwitch.TabIndex = 7;
            this.LinearSwitch.Text = "Approximation algorithm (WIP)";
            this.LinearSwitch.UseVisualStyleBackColor = true;
            this.LinearSwitch.CheckedChanged += new System.EventHandler(this.LinearSwitch_CheckedChanged);
            // 
            // GreedySwitch
            // 
            this.GreedySwitch.AutoSize = true;
            this.GreedySwitch.Checked = true;
            this.GreedySwitch.Location = new System.Drawing.Point(423, 33);
            this.GreedySwitch.Name = "GreedySwitch";
            this.GreedySwitch.Size = new System.Drawing.Size(138, 21);
            this.GreedySwitch.TabIndex = 6;
            this.GreedySwitch.TabStop = true;
            this.GreedySwitch.Text = "Greedy algorithm";
            this.GreedySwitch.UseVisualStyleBackColor = true;
            this.GreedySwitch.CheckedChanged += new System.EventHandler(this.GreedySwitch_CheckedChanged);
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(684, 47);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(86, 36);
            this.StartButton.TabIndex = 5;
            this.StartButton.Text = "Run";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(12, 47);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(86, 36);
            this.BrowseButton.TabIndex = 4;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // OutputDiameterText
            // 
            this.OutputDiameterText.AutoSize = true;
            this.OutputDiameterText.Location = new System.Drawing.Point(297, 93);
            this.OutputDiameterText.Name = "OutputDiameterText";
            this.OutputDiameterText.Size = new System.Drawing.Size(23, 17);
            this.OutputDiameterText.TabIndex = 3;
            this.OutputDiameterText.Text = "---";
            // 
            // OutputDiameter
            // 
            this.OutputDiameter.AutoSize = true;
            this.OutputDiameter.Location = new System.Drawing.Point(176, 93);
            this.OutputDiameter.Name = "OutputDiameter";
            this.OutputDiameter.Size = new System.Drawing.Size(115, 17);
            this.OutputDiameter.TabIndex = 2;
            this.OutputDiameter.Text = "Bundle diameter:";
            // 
            // FileName
            // 
            this.FileName.AutoSize = true;
            this.FileName.Location = new System.Drawing.Point(95, 9);
            this.FileName.Name = "FileName";
            this.FileName.Size = new System.Drawing.Size(23, 17);
            this.FileName.TabIndex = 1;
            this.FileName.Text = "---";
            // 
            // FileNameLabel
            // 
            this.FileNameLabel.AutoSize = true;
            this.FileNameLabel.Location = new System.Drawing.Point(13, 9);
            this.FileNameLabel.Name = "FileNameLabel";
            this.FileNameLabel.Size = new System.Drawing.Size(73, 17);
            this.FileNameLabel.TabIndex = 0;
            this.FileNameLabel.Text = "File name:";
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.FileName = "OpenFileDialog";
            // 
            // OutputVisualization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 793);
            this.Controls.Add(this.UiPanel);
            this.Controls.Add(this.Canvas);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "OutputVisualization";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cable Packing Visualization";
            this.UiPanel.ResumeLayout(false);
            this.UiPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Canvas;
        private System.Windows.Forms.Panel UiPanel;
        private System.Windows.Forms.Label FileName;
        private System.Windows.Forms.Label FileNameLabel;
        private System.Windows.Forms.Label OutputDiameterText;
        private System.Windows.Forms.Label OutputDiameter;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.RadioButton LinearSwitch;
        private System.Windows.Forms.RadioButton GreedySwitch;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Label Status;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.RadioButton SinglePassSwitch;
    }
}

