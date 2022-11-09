using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Week6.Abstarctions;
using Week6.Entities;

namespace Week6
{
    public partial class Form1 : Form
    {
        List<Toy> _toys = new List<Toy>();
        private IToyFactory _factory;

        public IToyFactory Factory
        {
            get { return _factory; }
            set { _factory = value; }
        }

        public Form1()
        {
            InitializeComponent();
            Factory = new CarFactory();
        }

        private void createTimer_Tick(object sender, EventArgs e)
        {
            var toy = Factory.CreateNew();
            _toys.Add(toy);
            toy.Left = -toy.Width;
            mainPanel.Controls.Add(toy);
        }

        private void conveyorTimer_Tick(object sender, EventArgs e)
        {
            
            int maxPosition = 0;
            foreach (var item in _toys)
            {
                item.Move_Toy();
                if (item.Left >maxPosition)
                {
                    maxPosition = item.Left;
                }
            }
            if (maxPosition>mainPanel.Width)
            {
                Toy Torlendo = _toys[0];
                _toys.Remove(Torlendo);
                mainPanel.Controls.Remove(Torlendo);
            }
            
        }
    }
}
