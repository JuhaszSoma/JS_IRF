using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week6.Abstarctions;
using System.Drawing;

namespace Week6.Entities
{
    public class Present : Toy
    {
        public SolidBrush szalagColor { get; set; }
        public SolidBrush dobozColor { get; set; }

        protected override void DrawImage(Graphics g)
        {
            g.FillRectangle(dobozColor, 0, 0, Width, Height);
            g.FillRectangle(szalagColor, 0, 10, Width, 10);
            g.FillRectangle(szalagColor, 10, 0, 10, Height);
        }
        public Present(Color szalag, Color doboz)
        {
            szalagColor = new SolidBrush(szalag);
            dobozColor = new SolidBrush(doboz);

        }
    }
}
