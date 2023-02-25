namespace paint
{
    partial class AppPaint
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
            this.PnlControlApp = new System.Windows.Forms.Panel();
            this.PnlControlDrawing = new System.Windows.Forms.Panel();
            this.PnlTitle = new System.Windows.Forms.Panel();
            this.BtnExit = new System.Windows.Forms.Button();
            this.LbAppTitle = new System.Windows.Forms.Label();
            this.PcbAppIcon = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.PnlControlApp.SuspendLayout();
            this.PnlControlDrawing.SuspendLayout();
            this.PnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PcbAppIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // PnlControlApp
            // 
            this.PnlControlApp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(249)))));
            this.PnlControlApp.Controls.Add(this.PnlControlDrawing);
            this.PnlControlApp.Controls.Add(this.PnlTitle);
            this.PnlControlApp.Dock = System.Windows.Forms.DockStyle.Top;
            this.PnlControlApp.Location = new System.Drawing.Point(0, 0);
            this.PnlControlApp.Margin = new System.Windows.Forms.Padding(0);
            this.PnlControlApp.Name = "PnlControlApp";
            this.PnlControlApp.Size = new System.Drawing.Size(1200, 90);
            this.PnlControlApp.TabIndex = 0;
            // 
            // PnlControlDrawing
            // 
            this.PnlControlDrawing.Controls.Add(this.button3);
            this.PnlControlDrawing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlControlDrawing.Location = new System.Drawing.Point(0, 45);
            this.PnlControlDrawing.Name = "PnlControlDrawing";
            this.PnlControlDrawing.Size = new System.Drawing.Size(1200, 45);
            this.PnlControlDrawing.TabIndex = 1;
            // 
            // PnlTitle
            // 
            this.PnlTitle.Controls.Add(this.button2);
            this.PnlTitle.Controls.Add(this.button1);
            this.PnlTitle.Controls.Add(this.BtnExit);
            this.PnlTitle.Controls.Add(this.LbAppTitle);
            this.PnlTitle.Controls.Add(this.PcbAppIcon);
            this.PnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.PnlTitle.Location = new System.Drawing.Point(0, 0);
            this.PnlTitle.Name = "PnlTitle";
            this.PnlTitle.Size = new System.Drawing.Size(1200, 45);
            this.PnlTitle.TabIndex = 0;
            // 
            // BtnExit
            // 
            this.BtnExit.BackColor = System.Drawing.Color.Transparent;
            this.BtnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnExit.FlatAppearance.BorderSize = 0;
            this.BtnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(43)))), ((int)(((byte)(28)))));
            this.BtnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnExit.Location = new System.Drawing.Point(1142, 0);
            this.BtnExit.Margin = new System.Windows.Forms.Padding(0);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(58, 45);
            this.BtnExit.TabIndex = 2;
            this.BtnExit.Text = "X";
            this.BtnExit.UseVisualStyleBackColor = false;
            this.BtnExit.MouseHover += new System.EventHandler(this.button1_MouseHover);
            // 
            // LbAppTitle
            // 
            this.LbAppTitle.AutoSize = true;
            this.LbAppTitle.Font = new System.Drawing.Font("Gadugi", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbAppTitle.Location = new System.Drawing.Point(47, 13);
            this.LbAppTitle.Margin = new System.Windows.Forms.Padding(0);
            this.LbAppTitle.Name = "LbAppTitle";
            this.LbAppTitle.Size = new System.Drawing.Size(109, 19);
            this.LbAppTitle.TabIndex = 1;
            this.LbAppTitle.Text = "Untitled - Paint";
            // 
            // PcbAppIcon
            // 
            this.PcbAppIcon.Dock = System.Windows.Forms.DockStyle.Left;
            this.PcbAppIcon.Location = new System.Drawing.Point(0, 0);
            this.PcbAppIcon.Name = "PcbAppIcon";
            this.PcbAppIcon.Size = new System.Drawing.Size(44, 45);
            this.PcbAppIcon.TabIndex = 0;
            this.PcbAppIcon.TabStop = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(1084, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(58, 45);
            this.button1.TabIndex = 3;
            this.button1.Text = "X";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.Dock = System.Windows.Forms.DockStyle.Right;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(1026, 0);
            this.button2.Margin = new System.Windows.Forms.Padding(0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(58, 45);
            this.button2.TabIndex = 4;
            this.button2.Text = "X";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.Dock = System.Windows.Forms.DockStyle.Left;
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(234)))), ((int)(((byte)(240)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(0, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 45);
            this.button3.TabIndex = 0;
            this.button3.Text = "File";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // AppPaint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(1200, 680);
            this.Controls.Add(this.PnlControlApp);
            this.Font = new System.Drawing.Font("Gadugi", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.DimGray;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AppPaint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Paint";
            this.PnlControlApp.ResumeLayout(false);
            this.PnlControlDrawing.ResumeLayout(false);
            this.PnlTitle.ResumeLayout(false);
            this.PnlTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PcbAppIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PnlControlApp;
        private System.Windows.Forms.Panel PnlTitle;
        private System.Windows.Forms.Panel PnlControlDrawing;
        private System.Windows.Forms.PictureBox PcbAppIcon;
        private System.Windows.Forms.Label LbAppTitle;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}

