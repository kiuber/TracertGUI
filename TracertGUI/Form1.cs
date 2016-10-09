using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TracertGUI
{
    public partial class Form1 : Form
    {
        Thread th1;
        DataTable dt;
        List<List<string>> allList = new List<List<string>>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cmd = "tracert " + textBox1.Text.ToString();
            th1 = new Thread(new ParameterizedThreadStart(ExeTh));
            th1.Start(cmd);
        }

        private void ExeTh(object obj)
        {
            ExeCmdOrder(obj.ToString());
        }

        private void ExeCmdOrder(string cmd)
        {
            try
            {
                Process pro = new Process();

                ProcessStartInfo psi = pro.StartInfo;
                psi.FileName = "cmd";
                psi.UseShellExecute = false;
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                psi.CreateNoWindow = true;

                pro.OutputDataReceived += Pro_OutputDataReceived;
                pro.Start();
                StreamWriter sw = pro.StandardInput;
                pro.BeginOutputReadLine();
                if (!String.IsNullOrEmpty(cmd))
                {
                    sw.WriteLine(cmd);
                }
                sw.Close();
                pro.WaitForExit();
                pro.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("执行命令失败，请检查输入的命令是否正确！");
            }
        }

        private void Pro_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                this.listBox1.Invoke(new UpdateListBoxItemDelegate(UpdateListBoxItem), new object[] { e.Data });
            }
        }

        private delegate void UpdateListBoxItemDelegate(string str);

        private void UpdateListBoxItem(string str)
        {
            listBox1.Items.Add(str);
            listBox1.TopIndex = listBox1.Items.Count - 1;
            string[] res = str.Split(' ');
            List<string> r = new List<string>();
            for (int i = 0; i < res.Length; i++)
            {
                string tmp = res[i];
                if (!tmp.Equals("") && !tmp.Equals("ms"))
                {
                    r.Add(res[i]);
                }
            }
            allList.Add(r);
            if (allList.Count > 7 && r.Count >= 5)
            {
                DataRow dr = dt.NewRow();
                dr["#"] = r[0];
                dr["IP"] = r[4];
                dr["时间"] = r[1] + "/" + r[2] + "/" + r[3];
                dt.Rows.Add(dr);
                dataGridView1.DataSource = dt;

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            th1.Abort();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dt = new DataTable();
            dt.Columns.Add(new DataColumn("#"));
            dt.Columns.Add(new DataColumn("IP"));
            dt.Columns.Add(new DataColumn("时间"));
        }
    }
}
