using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop
{
    public partial class TreeViewForm : Form
    {
        public TreeViewForm()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public System.Windows.Forms.TreeView GetTreeView()
        {
            return treeView1;
        }
    }
}
