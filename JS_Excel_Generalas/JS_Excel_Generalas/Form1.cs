using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace JS_Excel_Generalas
{
    public partial class Form1 : Form
    {

        List<Flat> FlatLista;
        RealEstateEntities context = new RealEstateEntities();
        Excel.Application xlApp; // A Microsoft Excel alkalmazás
        Excel.Workbook xlWB; // A létrehozott munkafüzet
        Excel.Worksheet xlSheet; // Munkalap a munkafüzeten belül
        public Form1()
        {
            InitializeComponent();
            LoadData();
            CreateExcel();

        }

        void LoadData()
        {
            FlatLista = context.Flats.ToList();
        }

        void CreateExcel()
        {
            try
            {
                // Excel elindítása és az applikáció objektum betöltése
                xlApp = new Excel.Application();

                // Új munkafüzet
                xlWB = xlApp.Workbooks.Add(Missing.Value);

                // Új munkalap
                xlSheet = xlWB.ActiveSheet;

                // Tábla létrehozása
                CreateTable(); // Ennek megírása a következő feladatrészben következik

                // Control átadása a felhasználónak
                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex) // Hibakezelés a beépített hibaüzenettel
            {
                string errMsg = string.Format("Error: {0}\nLine: {1}", ex.Message, ex.Source);
                MessageBox.Show(errMsg, "Error");

                // Hiba esetén az Excel applikáció bezárása automatikusan
                xlWB.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlWB = null;
                xlApp = null;
            }
        }

        void CreateTable()
        {
            string[] Fejlecek = new string[]
            {
                     "Kód",
                     "Eladó",
                     "Oldal",
                     "Kerület",
                     "Lift",
                     "Szobák száma",
                     "Alapterület (m2)",
                     "Ár (mFt)",
                     "Négyzetméter ár (Ft/m2)"
            };

            for (int i = 0; i < Fejlecek.Length; i++)
            {
                xlSheet.Cells[1, i + 1] = Fejlecek[i];

            }

            object[,] values = new object[FlatLista.Count, Fejlecek.Length];

            int counter = 0;
            foreach (Flat item in FlatLista)
            {
                values[counter, 0] = item.Code;
                values[counter, 1] = item.Vendor;
                values[counter, 2] = item.Side;
                values[counter, 3] = item.District;
                values[counter, 4] = item.Elevator;
                values[counter, 5] = item.NumberOfRooms;
                values[counter, 6] = item.FloorArea;
                values[counter, 7] = item.Price;
                values[counter, 8] = "=H" + (counter+2).ToString() + "/G" + (counter + 2).ToString() + "*1000000";
                counter++;
            }
            xlSheet.get_Range(
            GetCell(2, 1),
            GetCell(1 + FlatLista.Count, Fejlecek.Length)).Value2 = values;

            Excel.Range headerRange = xlSheet.get_Range(GetCell(1, 1), GetCell(1, Fejlecek.Length));
            headerRange.Font.Bold = true;
            headerRange.EntireColumn.AutoFit();
            headerRange.RowHeight = 40;
            headerRange.Interior.Color = Color.LightBlue;
            headerRange.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);
        }
        private string GetCell(int x, int y)
        {
            string ExcelCoordinate = "";
            int dividend = y;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                ExcelCoordinate = Convert.ToChar(65 + modulo).ToString() + ExcelCoordinate;
                dividend = (int)((dividend - modulo) / 26);
            }
            ExcelCoordinate += x.ToString();

            return ExcelCoordinate;
        }
    }
}
