using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace itp380
{
    public partial class NewMapDialog : Form
    {
        LevelEditor parent;
        public NewMapDialog(LevelEditor parentEditor)
        {
            parent = parentEditor;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parent.createNewMapOfSize(Convert.ToInt32(this.RowInput.Text), Convert.ToInt32(this.ColumnInput.Text));
            this.Close();
        }

        private void ColumnInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void RowInput_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
