using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Week6.Abstarctions;

namespace Week6
{
    public class Ball : Toy
    {
        public SolidBrush BallColor { get; private set; }
        

        public Ball(Color ballColor)
        {
            BallColor = new SolidBrush(ballColor);
        }
        protected override void DrawImage(Graphics g)
        {   
            g.FillEllipse(BallColor, 0, 0, Width, Height);
        }

    }
}
