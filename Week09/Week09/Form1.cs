using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Week09.Entities;

namespace Week09
{
    public partial class Form1 : Form
    {

        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();
        Random rng = new Random(1234);
        List<Eredmeny> Eredmenyek = new List<Eredmeny>();

        public Form1()
        {
            InitializeComponent();
            
            // Population = GetPopulation(@"C:\Temp\nép.csv");
            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\halál.csv");
        }

        private void Szimulacio()
        {
            Eredmenyek.Clear();           
            richTextBox1.Text = "";
            int maxYear = (int)numericUpDown1.Value;
            for (int year = 2005; year <= maxYear; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                }
                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();
                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();

                Eredmeny actualEredmeny = new Eredmeny()
                {
                    Year = year,
                    NumFerfi = nbrOfMales,
                    NumNo = nbrOfFemales
                };
                Eredmenyek.Add(actualEredmeny);
            }
        }

        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();            
            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = int.Parse(line[2])
                    });
                }
            }
            return population;
        }

        public List<BirthProbability> GetBirthProbabilities(string csvpath)
        {
            List<BirthProbability> BirthProbabilities = new List<BirthProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    BirthProbabilities.Add(new BirthProbability()
                    {
                        Age = int.Parse(line[0]),
                        NbrOfChildren = int.Parse(line[1]),
                        BirthChance = double.Parse(line[2])
                    });
                }
            }

            return BirthProbabilities;
        }

        public List<DeathProbability> GetDeathProbabilities(string csvpath)
        {
            List<DeathProbability> deathProbabilities = new List<DeathProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    deathProbabilities.Add(new DeathProbability()
                    {
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        Age = int.Parse(line[0]),
                        DeathChance = double.Parse(line[1])
                    });
                }
            }

            return deathProbabilities;
        }
        public void SimStep(int year, Person szemely)
        {
            if (!szemely.IsAlive)
            {
                return;
            }
            int age = year - szemely.BirthYear;

            var Halalprob = (from x in DeathProbabilities
                             where x.Age == age
                             select x.DeathChance).FirstOrDefault();

            if (rng.NextDouble() < Halalprob)
            {
                szemely.IsAlive = false;
            }

            if (!szemely.IsAlive || szemely.Gender != Gender.Female)
            {
                return;
            }

            var szulVal = (from x in BirthProbabilities
                           where x.Age == age
                           select x.BirthChance).FirstOrDefault();

            if (rng.NextDouble() < szulVal)
            {
                Person Ujszulott = new Person
                {
                    BirthYear = year,
                    Gender = (Gender)rng.Next(1, 3),
                    NbrOfChildren = 0
                };
                Population.Add(Ujszulott);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Szimulacio();
            DisplayResults();
        }

        private void button2_Click(object sender, EventArgs e)
        {            
            OpenFileDialog op = new OpenFileDialog();
            if (op.ShowDialog() == DialogResult.OK)
            {
                string fajlNev = op.FileName;
                textBox1.Text = fajlNev;
                Population = GetPopulation(fajlNev);
            }           
        }

        public void DisplayResults()
        {
            string szoveg = "";
            foreach (var item in Eredmenyek)
            {
                szoveg = szoveg + 
                    string.Format("Szimulációs év: {0} \n \t Fiúk:{1} \n \t Lányok:{2} \n \n", 
                    item.Year, item.NumNo, item.NumFerfi);
            }
            richTextBox1.Text = szoveg;
        }
    }
}
