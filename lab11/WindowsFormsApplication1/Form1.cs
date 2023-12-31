﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public int wCount = 0; 
        public int lineWidth = 1;  
        public int pictHeight = 600, pictWidth = 800; 
        public bool solidFill = false;
        public int figureID = 0;
        public bool textMode = false;
        public Font textFont = new Font("Times New Roman", 12);
        public bool selection = false; 

        public void newWindow()
        {
            Form2 f2 = new Form2(); 
            f2.MdiParent = this;
            f2.lineWidth = lineWidth;
            f2.primaryColor = primColorDialog.Color;
            f2.secondaryColor = secondColorDialog.Color;
            f2.backColor = backColorDialog.Color;
            f2.solidFill = solidFill;
            f2.figureID = figureID;
            f2.textFont = textFont;
   
            f2.pictHeight = pictHeight;
            f2.pictWidth = pictWidth;
            f2.AutoScrollMinSize = new Size(pictWidth, pictHeight);
            f2.AutoScroll = true;
            f2.Text = "Изображение № " + (++wCount);
            f2.Show();
        }

      

        public void fileOperationsMenu(bool act)
        { 
            saveToolStripMenuItem.Enabled = act;
            saveAsToolStripMenuItem.Enabled = act;
        }

        public void setWindowSizeCaption(int w, int h)
        {
            if (w >= 0 && h >= 0)
                statusBarPanel4.Text = "Размер рисунка:(" + w + "," + h + ")";
            else
                statusBarPanel4.Text = "";
        }
     
        public void setMousePositionCaption(int x, int y)
        {
            if (x >= 0 && y >= 0)
                statusBarPanel5.Text = x + "," + y;
            else
                statusBarPanel5.Text = "";
        }

        public void statusMessage() 
        {
            if (textMode)
            {
                statusBar1.Panels[5].Text = textFont.Name + " " + textFont.Size + "pt";
            }
            else
                statusBar1.Panels[5].Text = "";
        }


        public void openFile()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                newWindow(); 
                ((Form2)this.ActiveMdiChild).LoadFile(openFileDialog1.FileName);
                ((Form2)this.ActiveMdiChild).fromFile = true;
                ((Form2)this.ActiveMdiChild).fileName = openFileDialog1.FileName;
                this.ActiveMdiChild.Text = openFileDialog1.FileName; 
            }
            fileOperationsMenu(true);
        }

        public void saveAsFile()
        {
            saveFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ((Form2)this.ActiveMdiChild).SaveFile(saveFileDialog1.FileName);
                ((Form2)this.ActiveMdiChild).fileName = saveFileDialog1.FileName;
                ((Form2)this.ActiveMdiChild).fromFile = true;
                this.ActiveMdiChild.Text = saveFileDialog1.FileName; 
            }
        }

        public void saveFile()
        {
            if (((Form2)this.ActiveMdiChild).fromFile)
                ((Form2)this.ActiveMdiChild).SaveFile(((Form2)this.ActiveMdiChild).fileName); 
            else
                saveAsFile(); 
        }

        public void setPenWidth()
        {
            penWidthDialog pwd = new penWidthDialog();
            pwd.Val = lineWidth; 
            if (pwd.ShowDialog() == DialogResult.OK)
            {
                foreach (Form f2 in this.MdiChildren)
                    ((Form2)f2).lineWidth = pwd.Val; 
                lineWidth = pwd.Val;
                statusBarPanel1.Text = "Линия:" + lineWidth + " размер" ;
            }
        }

        public void setPictureSize()
        {
            pictureSizeDialog psd = new pictureSizeDialog();
            if (psd.ShowDialog() == DialogResult.OK)
            {
                pictHeight = psd.newHeight;
                pictWidth = psd.newWidth;
            }
        }

        public void setPrimaryColor()
        {
            if (primColorDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (Form f2 in this.MdiChildren)
                    ((Form2)f2).primaryColor = primColorDialog.Color;
                statusBar1.Refresh();
            }

        }

        public void setSecondaryColor()
        {
            if (secondColorDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (Form f2 in this.MdiChildren)
                    ((Form2)f2).secondaryColor = secondColorDialog.Color;
                statusBar1.Refresh();
            }
        }

        public void setFont()
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                textFont = fontDialog1.Font;
                statusMessage();
                foreach (Form f2 in this.MdiChildren)
                    ((Form2)f2).textFont = textFont;

            }
        }
     
        public void allDown()
        { 
            lieToolStripMenuItem.Checked = false;
            rectangleToolStripMenuItem.Checked = false;
            ellipseToolStripMenuItem.Checked = false;
            curveToolStripMenuItem.Checked = false;
            textToolStripMenuItem.Checked = false;
            textMode = false;
            statusMessage(); 
        }

        public void setFigureType()
        {
            foreach (Form f2 in this.MdiChildren)
                ((Form2)f2).figureID = figureID;
        }

        public void setFill() 
        {
            if (!fillToolStripMenuItem.Checked)
            {
                solidFill = true;
                fillToolStripMenuItem.Checked = true;
            }
            else
            {
                solidFill = false;
                fillToolStripMenuItem.Checked = false;
            }
            foreach (Form f2 in this.MdiChildren)
                ((Form2)f2).solidFill = solidFill;

        }

        public void setLine()
        {
            allDown();
            lieToolStripMenuItem.Checked = true;
            figureID = 0;
            setFigureType();
        }

        public void setCurve()
        {
            allDown();
            curveToolStripMenuItem.Checked = true;
            figureID = 1;
            setFigureType();
        }

        public void setRectangle()
        {
            allDown();
            rectangleToolStripMenuItem.Checked = true;
            figureID = 2;
            setFigureType();
        }

        public void setEllipse()
        {
            allDown();
            ellipseToolStripMenuItem.Checked = true;
            figureID = 3;
            setFigureType();
        }

        public void setTextLabel()
        {
            allDown();
            textToolStripMenuItem.Checked = true;
            textMode = true;
            statusMessage();
            figureID = 4;
            setFigureType();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            newWindow();
            fileOperationsMenu(true);
        }

        private void oPenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAsFile();
        }

        private void lineWidthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setPenWidth();
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setSecondaryColor();
        }

        private void lineColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setPrimaryColor();
        }

        private void newPicturesSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setPictureSize();
        }

        private void fillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setFill();
        }

        private void lieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setLine();
        }

        private void curveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setCurve();
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setRectangle();
        }

        private void ellipseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setEllipse();
        }

        private void statusBar1_DrawItem(object sender, StatusBarDrawItemEventArgs sbdevent)
        {
            Graphics g = statusBar1.CreateGraphics();
            g.FillRectangle(new SolidBrush(primColorDialog.Color), 100, 2, sbdevent.Panel.Width, statusBar1.Height);
            g.FillRectangle(new SolidBrush(secondColorDialog.Color), 150, 2, sbdevent.Panel.Width, statusBar1.Height);
            g.Dispose();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            newWindow();
            fileOperationsMenu(true);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            setPenWidth();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            setPrimaryColor();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            setSecondaryColor();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            setPictureSize();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            setLine();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            setCurve();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            setRectangle();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            setEllipse();
        }
      
        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            setFill();
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setTextLabel();
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            setTextLabel();
        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            setFont();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setFont();
        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {

        }
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void toolStripSeparator2_Click(object sender, EventArgs e)
        {

        }

        private void правкаToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
