using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Drawing.Imaging;

namespace WindowsFormsApplication1
{
	public partial class Form2 : Form
	{

		public string fileName; 
		public bool fromFile = false; 
		bool paintAction = false; 
		bool changedCanvas = false; 
		Point start,finish; 
		List<AbstractFigure> fstorage = new List<AbstractFigure>(); 
		AbstractFigure toPaint; 
        Bitmap canvas = new Bitmap(10,10);
		public Color backColor = Color.White;	
		public Color frameColor = Color.Black;		
		public Color primaryColor = Color.Black;    
		public Color secondaryColor = Color.Black;    
		public int lineWidth = 1; 
		public bool solidFill = false;
        public int figureID = 0; 
		public int pictWidth=1,pictHeight=1;
        public Font textFont;
        public bool selection = false; 

	
		public void drawCanvas() 
		{
			canvas.Dispose();                         
            canvas = new Bitmap(pictWidth,pictHeight); 
			Graphics g = Graphics.FromImage(canvas);  
            g.Clear(backColor);
            foreach (AbstractFigure go in fstorage) 
            {
                go.draw(ref g);
         
            }
            if (paintAction)
                toPaint.drawFrame(ref g); 
            g.Dispose();
		}

		private void initPainter() 
		{   
			switch(figureID)
			{
				case 0:	toPaint = new GLine(); break;
				case 1: toPaint = new GCurve(); break;
				case 2: toPaint = new GRectangle(); break;
				case 3: toPaint = new GEllipse(); break;
                case 4:
                    toPaint = new GTextLabel();
                    ((GTextLabel)toPaint).tFont = textFont;
                    ((GTextLabel)toPaint).tbParent = this;
                    break;
				default: toPaint = new GLine(); break; 
			}
			
			toPaint.loadColors(primaryColor,secondaryColor,frameColor); 
			toPaint.firstPoint = start; 
            toPaint.secondPoint = start; 
            toPaint.lineWidth = lineWidth; 
			toPaint.fill = solidFill; 
		}
       
        public void redrawAll()
        {
            drawCanvas();
            Refresh();
        }
		

		public void SaveFile(string name)
		{
            
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(name,FileMode.Create,FileAccess.Write,FileShare.None);
			formatter.Serialize(stream,pictWidth); 
            formatter.Serialize(stream,pictHeight);
			formatter.Serialize(stream,backColor); 
			formatter.Serialize(stream,fstorage);  
			stream.Close();
			changedCanvas = false; 
		}

		public void LoadFile(string name)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.Read);

            pictWidth = (int)formatter.Deserialize(stream); 
            pictHeight = (int)formatter.Deserialize(stream);
			backColor = (Color)formatter.Deserialize(stream);
			fstorage = (List<AbstractFigure>)formatter.Deserialize(stream);
			stream.Close();
            drawCanvas(); 
            Refresh(); 
        }

		public Form2()
		{
			InitializeComponent();
		}

		private void Form2_Shown(object sender, EventArgs e)
		{
			canvas = new Bitmap(pictWidth,pictHeight);
			Graphics.FromImage(canvas).Clear(backColor);
        }

		private void Form2_Activated(object sender, EventArgs e)
		{
			((Form1)this.ParentForm).setWindowSizeCaption(pictWidth,pictHeight);
            redrawAll();
        }



        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            int eX = e.X - AutoScrollPosition.X;
            int eY = e.Y - AutoScrollPosition.Y;
            if (eX <= pictWidth && eY <= pictHeight)
            {
                if (e.Button == MouseButtons.Left && !selection)
                {
                
                    start.X = eX;
                    start.Y = eY;
                    finish = start;
                    initPainter();
                    paintAction = true; 
                }
               
            }
        }

        public void pasteData()
        {

            MemoryStream ms = (MemoryStream)Clipboard.GetDataObject().GetData(typeof(MemoryStream));
            BinaryFormatter formatter = new BinaryFormatter();
            List<AbstractFigure> toc = (List<AbstractFigure>)formatter.Deserialize(ms);
            ms.Close();

            int minx = Int32.MaxValue, miny = Int32.MaxValue;
            foreach (AbstractFigure af in toc)
            {
                Rectangle rect = af.getRectangle();
                if (rect.Width > pictWidth || rect.Height > pictHeight)
                {
                    MessageBox.Show("Изображение слишком большое!");
                    return;
                }
                minx = Math.Min(minx, rect.Left);
                miny = Math.Min(miny, rect.Top);
            }

            foreach (AbstractFigure af in toc)
            {
                Rectangle rect = af.getRectangle();
                af.move(new Point(rect.Left, rect.Top), new Point(rect.Left - minx, rect.Top - miny));
                af.selected = true;
                fstorage.Add(af);
            }

            redrawAll();
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e) 
		{ 
			int eX = e.X - AutoScrollPosition.X; 
			int eY = e.Y - AutoScrollPosition.Y;
            if (paintAction) 
			{
				finish.X = eX; 
				finish.Y = eY;
                toPaint.secondPoint = finish; 
                redrawAll();
            }
           
			((Form1)this.ParentForm).setMousePositionCaption(eX,eY); 
        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            int eX = e.X - AutoScrollPosition.X; 
            int eY = e.Y - AutoScrollPosition.Y;
            finish.X = eX;	
            finish.Y = eY;
            if (paintAction) 
            {
                paintAction = false; 
                toPaint.secondPoint = finish; 
                changedCanvas = true;
                if (isInside(toPaint.getRectangle(), new Rectangle(0, 0, pictWidth, pictHeight))) 
                    fstorage.Add(toPaint); 
            }
           
            redrawAll();
        }

        public bool isInside(Rectangle sm, Rectangle lg) // ** НОВОЕ
        {
            return (isInsideOfRectangle(lg, new Point(sm.Left, sm.Top)) && isInsideOfRectangle(lg, new Point(sm.Left + sm.Width, sm.Top + sm.Height)));
        }

        public bool isInsideOfRectangle(Rectangle rect, Point p) // ** НОВОЕ
        {
            return ((p.X >= rect.Left) && (p.X <= rect.Left + rect.Width) && (p.Y >= rect.Top) && (p.Y <= rect.Top + rect.Height));
        }

        private void Form2_MouseLeave(object sender, EventArgs e)
		{
			((Form1)this.ParentForm).setMousePositionCaption(-1,-1);
        }


		private void Form2_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(Color.LightGray); 
            e.Graphics.DrawImage(canvas,AutoScrollPosition.X,AutoScrollPosition.Y); 
        }

		private void Form2_Resize(object sender, EventArgs e)
		{
            redrawAll();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
		{
			if(this.ParentForm.MdiChildren.Length==1)
			{
                ((Form1)this.ParentForm).fileOperationsMenu(false); 
                ((Form1)this.ParentForm).setMousePositionCaption(-1, -1);
                ((Form1)this.ParentForm).setWindowSizeCaption(-1, -1);
            }
		}

		private void Form2_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(changedCanvas) 
				switch(MessageBox.Show("Сохранить изменения в \""+this.Text+"\"?","",MessageBoxButtons.YesNoCancel))
				{
					case DialogResult.Yes:
						((Form1)this.ParentForm).saveFile();
					break;
					case DialogResult.Cancel:
						e.Cancel = true;
					break;
				}
		}
	}

}
