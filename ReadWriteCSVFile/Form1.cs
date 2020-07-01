using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadWriteCSVFile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream mystr = null;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((mystr = openFileDialog1.OpenFile()) != null)
                {
                    StreamReader sr = new StreamReader(mystr, Encoding.Default);

                    try
                    {
                        DataTable dt = new DataTable();
                        string[] headers = sr.ReadLine().Split(';');
                        foreach (string header in headers)
                        {
                            dt.Columns.Add(header);
                        }
                        while (!sr.EndOfStream)
                        {
                            string[] rows = sr.ReadLine().Split(';');
                            DataRow dr = dt.NewRow();
                            for (int i = 0; i < headers.Length; i++)
                            {
                                dr[i] = rows[i];
                            }
                            dt.Rows.Add(dr);
                        }

                        for (int i = 0; i < headers.Length; i++)
                        {
                            if (headers[i] == "Value")
                            {
                                dt.DefaultView.Sort = "Value desc";
                            }
                        }

                        dataGridView1.DataSource = dt;
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                    finally { sr.Close(); }
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "CSV|*.csv";

            Stream myStream;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    StreamWriter sw = new StreamWriter(myStream);
                    try
                    {
                        for (int i = 0; i < dataGridView1.ColumnCount; i++)
                        {
                            DataGridViewColumn column = dataGridView1.Columns[i];
                            sw.Write(column.HeaderText);
                            if (i < dataGridView1.ColumnCount - 1)
                            {
                                sw.Write(";");
                            }
                        }
                        sw.WriteLine();

                        for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                        {
                            for (int j = 0; j < dataGridView1.ColumnCount; j++)
                            {
                                sw.Write(dataGridView1.Rows[i].Cells[j].Value.ToString());
                                if (j < dataGridView1.ColumnCount - 1)
                                {
                                    sw.Write(";");
                                }
                            }
                            sw.WriteLine();
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                    finally { sw.Close(); }
                    myStream.Close();
                }
            }
        }
    }
}
