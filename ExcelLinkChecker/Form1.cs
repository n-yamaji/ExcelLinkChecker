using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelLinkChecker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.InitializeHandler();
        }

        private void InitializeHandler()
        {
            this.txtFile.DragEnter += (sender, e) =>
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.None;
            };

            this.txtFile.DragDrop += (sender, e) =>
            {
                string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                this.txtFile.Text = fileNames.First();
            };

            this.btnLoad.Click += (sender, e) =>
            {
                this.MainLoad();
            };
        }

        private void MainLoad()
        {
            var map = this.LoadExcel(this.txtFile.Text);
            this.dataGridView1.DataSource = map;
        }

        private Dictionary<string, XLHyperlink> LoadExcel(string file)
        {
            var map = new Dictionary<string, XLHyperlink>();

            using (var workbook = new ClosedXML.Excel.XLWorkbook(file))
            {
                var sheet = workbook.Worksheet(this.txtSheet.Text);
                foreach (var cell in sheet.CellsUsed())
                {
                    if (!cell.HasHyperlink) { continue; }

                    map.Add(cell.Address.ToString(), cell.GetHyperlink());
                }
            }

            return map;
        }
    }
}
