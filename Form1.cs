using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using System.Diagnostics;

namespace Tubes_Stima_2
{
    public partial class Form1 : Form
    {
        GViewer viewer = new GViewer();
        public Form1()
        {
            InitializeComponent();
        }
        private void Button1_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string dir = fbd.SelectedPath;
                label1.Text = dir;
                MessageBox.Show(dir);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.Items[comboBox2.SelectedIndex].ToString() == "DFS")
            {
                string dir = label1.Text;
                string filename = textBox1.Text;
                DFS test = new DFS();
                test.SearchDFS(dir, filename, true);
                test.createGraphDFS();
                string duration = test.getTimeElapsed();
                label2.Text = duration;
                panel1.SuspendLayout();
                panel1.Controls.Add(viewer);
                panel1.ResumeLayout();
                panel1.Show();
                viewer.Graph = test.getGraphDFS();
                viewer.Dock = DockStyle.Fill;
            }
            else
            {
                string dir = label1.Text;
                string filename = textBox1.Text;
                BFS test = new BFS();
                test.SearchBFS(dir, filename, true);
                test.createGraphBFS();
                string duration = test.getTimeElapsed();
                label2.Text = duration;
                panel1.SuspendLayout();
                panel1.Controls.Add(viewer);
                panel1.ResumeLayout();
                panel1.Show();
                viewer.Graph = test.getGraphBFS();
                viewer.Dock = DockStyle.Fill;
            }
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox2.Items[comboBox2.SelectedIndex].ToString() == "DFS")
            {
                string dir = label1.Text;
                string filename = textBox1.Text;
                DFS test = new DFS();
                test.SearchDFS(dir, filename, true);
                test.createGraphDFS();
                string duration = test.getTimeElapsed();
                label2.Text = duration;
                panel1.SuspendLayout();
                panel1.Controls.Add(viewer);
                panel1.ResumeLayout();
                panel1.Show();
                viewer.Graph = test.getGraphDFS();
                viewer.Dock = DockStyle.Fill;
            }
            else
            {
                string dir = label1.Text;
                string filename = textBox1.Text;
                BFS test = new BFS();
                test.SearchBFS(dir, filename, true);
                test.createGraphBFS();
                string duration = test.getTimeElapsed();
                label2.Text = duration;
                panel1.SuspendLayout();
                panel1.Controls.Add(viewer);
                panel1.ResumeLayout();
                panel1.Show();
                viewer.Graph = test.getGraphBFS();
                viewer.Dock = DockStyle.Fill;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
