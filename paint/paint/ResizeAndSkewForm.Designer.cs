namespace paint
{
    partial class ResizeAndSkewForm
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
            this.PnlContainer = new System.Windows.Forms.Panel();
            this.PnlControl = new System.Windows.Forms.Panel();
            this.PnlSkew = new System.Windows.Forms.Panel();
            this.TxtSkewVer = new System.Windows.Forms.TextBox();
            this.LbSkewVer = new System.Windows.Forms.Label();
            this.TxtSkewHor = new System.Windows.Forms.TextBox();
            this.LbSkewHor = new System.Windows.Forms.Label();
            this.LbSkew = new System.Windows.Forms.Label();
            this.PnlResize = new System.Windows.Forms.Panel();
            this.TxtVerRatio = new System.Windows.Forms.TextBox();
            this.LbResizeVer = new System.Windows.Forms.Label();
            this.BtnResizeRatioConnected = new System.Windows.Forms.Button();
            this.TxtHorRatio = new System.Windows.Forms.TextBox();
            this.LbResizeHor = new System.Windows.Forms.Label();
            this.RadPixel = new System.Windows.Forms.RadioButton();
            this.RadPercentage = new System.Windows.Forms.RadioButton();
            this.LbResizeSkew = new System.Windows.Forms.Label();
            this.LbResize = new System.Windows.Forms.Label();
            this.PnlChoice = new System.Windows.Forms.Panel();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnOk = new System.Windows.Forms.Button();
            this.PnlTitle = new System.Windows.Forms.Panel();
            this.BtnExit = new System.Windows.Forms.Button();
            this.PnlContainer.SuspendLayout();
            this.PnlControl.SuspendLayout();
            this.PnlSkew.SuspendLayout();
            this.PnlResize.SuspendLayout();
            this.PnlChoice.SuspendLayout();
            this.PnlTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlContainer
            // 
            this.PnlContainer.Controls.Add(this.PnlControl);
            this.PnlContainer.Controls.Add(this.PnlChoice);
            this.PnlContainer.Controls.Add(this.PnlTitle);
            this.PnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlContainer.Location = new System.Drawing.Point(0, 0);
            this.PnlContainer.Margin = new System.Windows.Forms.Padding(0);
            this.PnlContainer.Name = "PnlContainer";
            this.PnlContainer.Size = new System.Drawing.Size(368, 542);
            this.PnlContainer.TabIndex = 1;
            // 
            // PnlControl
            // 
            this.PnlControl.BackColor = System.Drawing.Color.White;
            this.PnlControl.Controls.Add(this.PnlSkew);
            this.PnlControl.Controls.Add(this.PnlResize);
            this.PnlControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlControl.Location = new System.Drawing.Point(0, 37);
            this.PnlControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PnlControl.Name = "PnlControl";
            this.PnlControl.Size = new System.Drawing.Size(368, 405);
            this.PnlControl.TabIndex = 2;
            // 
            // PnlSkew
            // 
            this.PnlSkew.Controls.Add(this.TxtSkewVer);
            this.PnlSkew.Controls.Add(this.LbSkewVer);
            this.PnlSkew.Controls.Add(this.TxtSkewHor);
            this.PnlSkew.Controls.Add(this.LbSkewHor);
            this.PnlSkew.Controls.Add(this.LbSkew);
            this.PnlSkew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlSkew.Location = new System.Drawing.Point(0, 249);
            this.PnlSkew.Margin = new System.Windows.Forms.Padding(1);
            this.PnlSkew.Name = "PnlSkew";
            this.PnlSkew.Padding = new System.Windows.Forms.Padding(30);
            this.PnlSkew.Size = new System.Drawing.Size(368, 156);
            this.PnlSkew.TabIndex = 3;
            // 
            // TxtSkewVer
            // 
            this.TxtSkewVer.Location = new System.Drawing.Point(185, 97);
            this.TxtSkewVer.Name = "TxtSkewVer";
            this.TxtSkewVer.Size = new System.Drawing.Size(136, 30);
            this.TxtSkewVer.TabIndex = 13;
            this.TxtSkewVer.Text = "0°";
            // 
            // LbSkewVer
            // 
            this.LbSkewVer.AutoSize = true;
            this.LbSkewVer.Location = new System.Drawing.Point(185, 62);
            this.LbSkewVer.Margin = new System.Windows.Forms.Padding(0);
            this.LbSkewVer.Name = "LbSkewVer";
            this.LbSkewVer.Size = new System.Drawing.Size(66, 20);
            this.LbSkewVer.TabIndex = 12;
            this.LbSkewVer.Text = "Vertical";
            // 
            // TxtSkewHor
            // 
            this.TxtSkewHor.Location = new System.Drawing.Point(34, 97);
            this.TxtSkewHor.Name = "TxtSkewHor";
            this.TxtSkewHor.Size = new System.Drawing.Size(136, 30);
            this.TxtSkewHor.TabIndex = 10;
            this.TxtSkewHor.Text = "0°";
            // 
            // LbSkewHor
            // 
            this.LbSkewHor.AutoSize = true;
            this.LbSkewHor.Location = new System.Drawing.Point(34, 62);
            this.LbSkewHor.Margin = new System.Windows.Forms.Padding(0);
            this.LbSkewHor.Name = "LbSkewHor";
            this.LbSkewHor.Size = new System.Drawing.Size(88, 20);
            this.LbSkewHor.TabIndex = 9;
            this.LbSkewHor.Text = "Horizontal";
            // 
            // LbSkew
            // 
            this.LbSkew.AutoSize = true;
            this.LbSkew.Font = new System.Drawing.Font("Gadugi", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbSkew.Location = new System.Drawing.Point(30, 16);
            this.LbSkew.Name = "LbSkew";
            this.LbSkew.Size = new System.Drawing.Size(52, 20);
            this.LbSkew.TabIndex = 2;
            this.LbSkew.Text = "Skew";
            // 
            // PnlResize
            // 
            this.PnlResize.Controls.Add(this.TxtVerRatio);
            this.PnlResize.Controls.Add(this.LbResizeVer);
            this.PnlResize.Controls.Add(this.BtnResizeRatioConnected);
            this.PnlResize.Controls.Add(this.TxtHorRatio);
            this.PnlResize.Controls.Add(this.LbResizeHor);
            this.PnlResize.Controls.Add(this.RadPixel);
            this.PnlResize.Controls.Add(this.RadPercentage);
            this.PnlResize.Controls.Add(this.LbResizeSkew);
            this.PnlResize.Controls.Add(this.LbResize);
            this.PnlResize.Dock = System.Windows.Forms.DockStyle.Top;
            this.PnlResize.Location = new System.Drawing.Point(0, 0);
            this.PnlResize.Margin = new System.Windows.Forms.Padding(1);
            this.PnlResize.Name = "PnlResize";
            this.PnlResize.Padding = new System.Windows.Forms.Padding(30);
            this.PnlResize.Size = new System.Drawing.Size(368, 249);
            this.PnlResize.TabIndex = 2;
            // 
            // TxtVerRatio
            // 
            this.TxtVerRatio.Location = new System.Drawing.Point(224, 180);
            this.TxtVerRatio.Name = "TxtVerRatio";
            this.TxtVerRatio.Size = new System.Drawing.Size(100, 30);
            this.TxtVerRatio.TabIndex = 8;
            this.TxtVerRatio.Text = "100";
            // 
            // LbResizeVer
            // 
            this.LbResizeVer.AutoSize = true;
            this.LbResizeVer.Location = new System.Drawing.Point(225, 145);
            this.LbResizeVer.Margin = new System.Windows.Forms.Padding(0);
            this.LbResizeVer.Name = "LbResizeVer";
            this.LbResizeVer.Size = new System.Drawing.Size(66, 20);
            this.LbResizeVer.TabIndex = 7;
            this.LbResizeVer.Text = "Vertical";
            // 
            // BtnResizeRatioConnected
            // 
            this.BtnResizeRatioConnected.Location = new System.Drawing.Point(166, 180);
            this.BtnResizeRatioConnected.Name = "BtnResizeRatioConnected";
            this.BtnResizeRatioConnected.Size = new System.Drawing.Size(30, 30);
            this.BtnResizeRatioConnected.TabIndex = 6;
            this.BtnResizeRatioConnected.Text = "=";
            this.BtnResizeRatioConnected.UseVisualStyleBackColor = true;
            // 
            // TxtHorRatio
            // 
            this.TxtHorRatio.Location = new System.Drawing.Point(34, 180);
            this.TxtHorRatio.Name = "TxtHorRatio";
            this.TxtHorRatio.Size = new System.Drawing.Size(100, 30);
            this.TxtHorRatio.TabIndex = 5;
            this.TxtHorRatio.Text = "100";
            // 
            // LbResizeHor
            // 
            this.LbResizeHor.AutoSize = true;
            this.LbResizeHor.Location = new System.Drawing.Point(34, 145);
            this.LbResizeHor.Margin = new System.Windows.Forms.Padding(0);
            this.LbResizeHor.Name = "LbResizeHor";
            this.LbResizeHor.Size = new System.Drawing.Size(88, 20);
            this.LbResizeHor.TabIndex = 4;
            this.LbResizeHor.Text = "Horizontal";
            // 
            // RadPixel
            // 
            this.RadPixel.AutoSize = true;
            this.RadPixel.Location = new System.Drawing.Point(229, 109);
            this.RadPixel.Name = "RadPixel";
            this.RadPixel.Size = new System.Drawing.Size(72, 24);
            this.RadPixel.TabIndex = 3;
            this.RadPixel.TabStop = true;
            this.RadPixel.Text = "Pixels";
            this.RadPixel.UseVisualStyleBackColor = true;
            // 
            // RadPercentage
            // 
            this.RadPercentage.AutoSize = true;
            this.RadPercentage.Location = new System.Drawing.Point(34, 109);
            this.RadPercentage.Name = "RadPercentage";
            this.RadPercentage.Size = new System.Drawing.Size(116, 24);
            this.RadPercentage.TabIndex = 2;
            this.RadPercentage.TabStop = true;
            this.RadPercentage.Text = "Percentage";
            this.RadPercentage.UseVisualStyleBackColor = true;
            // 
            // LbResizeSkew
            // 
            this.LbResizeSkew.AutoSize = true;
            this.LbResizeSkew.Font = new System.Drawing.Font("Gadugi", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbResizeSkew.Location = new System.Drawing.Point(25, 25);
            this.LbResizeSkew.Margin = new System.Windows.Forms.Padding(0);
            this.LbResizeSkew.Name = "LbResizeSkew";
            this.LbResizeSkew.Size = new System.Drawing.Size(187, 27);
            this.LbResizeSkew.TabIndex = 0;
            this.LbResizeSkew.Text = "Resize and Skew";
            // 
            // LbResize
            // 
            this.LbResize.AutoSize = true;
            this.LbResize.Font = new System.Drawing.Font("Gadugi", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbResize.Location = new System.Drawing.Point(30, 67);
            this.LbResize.Name = "LbResize";
            this.LbResize.Size = new System.Drawing.Size(58, 20);
            this.LbResize.TabIndex = 1;
            this.LbResize.Text = "Resize";
            // 
            // PnlChoice
            // 
            this.PnlChoice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.PnlChoice.Controls.Add(this.BtnCancel);
            this.PnlChoice.Controls.Add(this.BtnOk);
            this.PnlChoice.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PnlChoice.Location = new System.Drawing.Point(0, 442);
            this.PnlChoice.Margin = new System.Windows.Forms.Padding(1);
            this.PnlChoice.Name = "PnlChoice";
            this.PnlChoice.Padding = new System.Windows.Forms.Padding(30);
            this.PnlChoice.Size = new System.Drawing.Size(368, 100);
            this.PnlChoice.TabIndex = 1;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(185, 32);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(150, 40);
            this.BtnCancel.TabIndex = 1;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(32, 32);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(150, 40);
            this.BtnOk.TabIndex = 0;
            this.BtnOk.Text = "OK";
            this.BtnOk.UseVisualStyleBackColor = true;
            // 
            // PnlTitle
            // 
            this.PnlTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(243)))), ((int)(((byte)(249)))));
            this.PnlTitle.Controls.Add(this.BtnExit);
            this.PnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.PnlTitle.Location = new System.Drawing.Point(0, 0);
            this.PnlTitle.Margin = new System.Windows.Forms.Padding(1);
            this.PnlTitle.Name = "PnlTitle";
            this.PnlTitle.Size = new System.Drawing.Size(368, 37);
            this.PnlTitle.TabIndex = 0;
            this.PnlTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PnlTitle_MouseDown);
            this.PnlTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PnlTitle_MouseMove);
            // 
            // BtnExit
            // 
            this.BtnExit.BackColor = System.Drawing.Color.Transparent;
            this.BtnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnExit.FlatAppearance.BorderSize = 0;
            this.BtnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnExit.Font = new System.Drawing.Font("Gadugi", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnExit.Location = new System.Drawing.Point(323, 0);
            this.BtnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(45, 37);
            this.BtnExit.TabIndex = 0;
            this.BtnExit.Text = "X";
            this.BtnExit.UseVisualStyleBackColor = false;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // ResizeAndSkewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 542);
            this.Controls.Add(this.PnlContainer);
            this.Font = new System.Drawing.Font("Gadugi", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ResizeAndSkewForm";
            this.Text = "ResizeAndSkewForm";
            this.PnlContainer.ResumeLayout(false);
            this.PnlControl.ResumeLayout(false);
            this.PnlSkew.ResumeLayout(false);
            this.PnlSkew.PerformLayout();
            this.PnlResize.ResumeLayout(false);
            this.PnlResize.PerformLayout();
            this.PnlChoice.ResumeLayout(false);
            this.PnlTitle.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel PnlContainer;
        private System.Windows.Forms.Panel PnlTitle;
        private System.Windows.Forms.Panel PnlControl;
        private System.Windows.Forms.Panel PnlChoice;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Label LbResizeSkew;
        private System.Windows.Forms.Label LbResize;
        private System.Windows.Forms.Panel PnlResize;
        private System.Windows.Forms.Panel PnlSkew;
        private System.Windows.Forms.RadioButton RadPercentage;
        private System.Windows.Forms.RadioButton RadPixel;
        private System.Windows.Forms.Button BtnResizeRatioConnected;
        private System.Windows.Forms.TextBox TxtHorRatio;
        private System.Windows.Forms.Label LbResizeHor;
        private System.Windows.Forms.TextBox TxtVerRatio;
        private System.Windows.Forms.Label LbResizeVer;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Label LbSkew;
        private System.Windows.Forms.TextBox TxtSkewVer;
        private System.Windows.Forms.Label LbSkewVer;
        private System.Windows.Forms.TextBox TxtSkewHor;
        private System.Windows.Forms.Label LbSkewHor;
    }
}