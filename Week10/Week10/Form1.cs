using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorldsHardestGame;

namespace Week10
{
    public partial class Form1 : Form
    {
        GameController gc = new GameController();
        GameArea ga;
        int populationSize = 1000;
        int nbrOfSteps = 10;
        int nbrOfStepsIncrement = 10;
        int generation = 1;
        Brain gyozoagy = null;

        public Form1()
        {
            InitializeComponent();

            ga = gc.ActivateDisplay();
            Controls.Add(ga);
            gc.GameOver += PopulacioFrissit;

            for (int i = 0; i < populationSize; i++)
            {
                gc.AddPlayer(nbrOfSteps);
            }
            gc.Start(true);             
        }

        private void PopulacioFrissit(object sender)
        {
            generation++;

            // generáció kiírás
            label1.BringToFront();
            label1.Text = string.Format(
                "{0}.generáció", 
                generation);



            // top játékosok gyűjtése
            var playerList = from p in gc.GetCurrentPlayers()
                             orderby p.GetFitness() descending
                             select p;
            var topPerformers = playerList.Take(populationSize / 2).ToList();


            // van nyertes? ha van befejezi
            var winners = from p in topPerformers
                          where p.IsWinner
                          select p;

            if (winners.Count() > 0)
            {
                gyozoagy = winners.FirstOrDefault().Brain.Clone();
                gc.GameOver -= PopulacioFrissit;
                return;
            }



            gc.ResetCurrentLevel();


            //top performers mutálása
            foreach (var p in topPerformers)
            {
                var b = p.Brain.Clone();
                if (generation % 3 == 0)
                    gc.AddPlayer(b.ExpandBrain(nbrOfStepsIncrement));
                else
                    gc.AddPlayer(b);

                if (generation % 3 == 0)
                    gc.AddPlayer(b.Mutate().ExpandBrain(nbrOfStepsIncrement));
                else
                    gc.AddPlayer(b.Mutate());
            }


            gc.Start();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            gc.ResetCurrentLevel();
            gc.AddPlayer(winnerBrain.Clone());
            gc.AddPlayer();
            ga.Focus();
            gc.Start(true);
        }
    }
}
