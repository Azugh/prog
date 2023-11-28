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


namespace WindowsFormsApplication1
{
    [Serializable()]
    public abstract class AbstractFigure 
    {

        public virtual Point firstPoint
        {
            set { p1 = value; } 
            get { return p1; } 
        }
        public virtual Point secondPoint
        {
            set { p2 = value; }
            get { return p2; }
        }
       
        public int lineWidth
        {
            set
            {
                if (value <= 0)
                    lWidth = 1;
                else
                    lWidth = value;
            }
            get { return lWidth; }
        }
      
        protected Point p1, p2; 
        protected int lWidth; 
        protected Color primaryColor, secondaryColor, frameColor;
        public bool fill;

        public void loadColors(Color pc, Color sc, Color fc)
        {
            primaryColor = pc;
            secondaryColor = sc;
            frameColor = fc;
        }
        public void drawSelection(ref Graphics g)//**НОВОЕ
        {
            Pen p = new Pen(frameColor);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            g.DrawRectangle(p, getRectangle());
            p.Dispose();
        }
        public void drawDragged(ref Graphics g, Point from, Point to)//**НОВОЕ
        {
            move(from, to);
            drawFrame(ref g);
            move(to, from);
        }

        public virtual void move(Point from, Point to)//**НОВОЕ
        {
            int dx = to.X - from.X;
            int dy = to.Y - from.Y;
            p1.X += dx;
            p1.Y += dy;
            p2.X += dx;
            p2.Y += dy;
        }

        public abstract void draw(ref Graphics g);
        public abstract void drawFrame(ref Graphics g);
        public abstract Rectangle getRectangle();
    }
}
