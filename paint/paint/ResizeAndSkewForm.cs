using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace paint
{
    public partial class ResizeAndSkewForm : Form
    {
        //
        Point mousePoint;
        //
        bool isPercentage = true;
        int horResize = 800;
        int verResize = 300;
        int horResizePercent = 100;
        int verResizePercent = 100;
        //
        int horSkew = 0;
        int vertSkew = 0;
        //
        //
        public int HorResize { get => horResize; set => horResize = value; }
        public int VertResize { get => verResize; set => verResize = value; }
        public int HorSkew { get => horSkew; set => horSkew = value; }
        public int VertSkew { get => vertSkew; set => vertSkew = value; }

        //
        //
        public ResizeAndSkewForm()
        {
            InitializeComponent();
            isPercentage = true;
            horResizePercent = 100;
            verResizePercent = 100;
            RadPercentage.Checked = isPercentage;
            TxtHorResize.Text = horResizePercent.ToString();
            TxtVerResize.Text = verResizePercent.ToString();
            TxtSkewHor.Text = horSkew.ToString();
            TxtSkewVer.Text = vertSkew.ToString();
        }
        public ResizeAndSkewForm(Size pcbSize)
        {
            InitializeComponent();
            isPercentage = true;
            horResizePercent = 100;
            verResizePercent = 100;
            RadPercentage.Checked = isPercentage;
            TxtHorResize.Text = horResizePercent.ToString();
            TxtVerResize.Text = verResizePercent.ToString();
            TxtSkewHor.Text = horSkew.ToString();
            TxtSkewVer.Text = vertSkew.ToString();
            //
            horResize = pcbSize.Width;
            verResize = pcbSize.Height;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void PnlTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mousePoint = new Point(-e.X, -e.Y);
            }
        }

        private void PnlTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                this.WindowState = FormWindowState.Normal;
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mousePoint);
                this.Location = mousePos;
            }
        }

        private void Handler_ResizeRadios_Click(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton.Tag.ToString() == "percentage")
            {
                SetHorizontalResize();
                SetVerticalResize();
                this.isPercentage = true;
                TxtHorResize.Text = horResizePercent.ToString();
                TxtVerResize.Text = verResizePercent.ToString();

            }
            else if( radioButton.Tag.ToString() == "pixel")
            {
                SetHorizontalResize();
                SetVerticalResize();
                this.isPercentage = false;
                TxtHorResize.Text = horResize.ToString();
                TxtVerResize.Text = verResize.ToString();
            }
        }
        private void SetHorizontalResize()
        {
            if (this.isPercentage)
            {
                horResize = (int)(horResize * float.Parse(TxtHorResize.Text) / 100);
            }
            else
            {
                horResize = int.Parse(TxtHorResize.Text);
            }
        }
        private void SetVerticalResize()
        {
            if (this.isPercentage)
            {
                verResize = (int)(verResize * float.Parse(TxtVerResize.Text) / 100);

            }
            else
            {
                verResize = int.Parse(TxtVerResize.Text);
            }
        }
        private void BtnOk_Click(object sender, EventArgs e)
        {
            SetHorizontalResize();
            SetVerticalResize();
            horSkew = int.Parse(TxtSkewHor.Text);
            vertSkew = int.Parse(TxtSkewVer.Text);
            this.Close();
        }

        
    }
}
