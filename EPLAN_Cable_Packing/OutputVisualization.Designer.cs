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
            this.FileName = new System.Windows.Forms.Label();
            this.FileNameLabel = new System.Windows.Forms.Label();
            this.UiPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Canvas
            // 
            this.Canvas.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Canvas.Location = new System.Drawing.Point(0, 125);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(782, 828);
            this.Canvas.TabIndex = 0;
            this.Canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.Canvas_Paint);
            // 
            // UiPanel
            // 
            this.UiPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.UiPanel.Controls.Add(this.FileName);
            this.UiPanel.Controls.Add(this.FileNameLabel);
            this.UiPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.UiPanel.Location = new System.Drawing.Point(0, 0);
            this.UiPanel.Name = "UiPanel";
            this.UiPanel.Size = new System.Drawing.Size(782, 125);
            this.UiPanel.TabIndex = 1;
            // 
            // FileName
            // 
            this.FileName.AutoSize = true;
            this.FileName.Location = new System.Drawing.Point(95, 13);
            this.FileName.Name = "FileName";
            this.FileName.Size = new System.Drawing.Size(27, 20);
            this.FileName.TabIndex = 1;
            this.FileName.Text = "---";
            // 
            // FileNameLabel
            // 
            this.FileNameLabel.AutoSize = true;
            this.FileNameLabel.Location = new System.Drawing.Point(13, 13);
            this.FileNameLabel.Name = "FileNameLabel";
            this.FileNameLabel.Size = new System.Drawing.Size(76, 20);
            this.FileNameLabel.TabIndex = 0;
            this.FileNameLabel.Text = "File name:";
            // 
            // OutputVisualization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 953);
            this.Controls.Add(this.UiPanel);
            this.Controls.Add(this.Canvas);
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
    }
}

