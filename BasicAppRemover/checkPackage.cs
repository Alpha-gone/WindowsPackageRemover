using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicAppRemover
{
    public partial class checkPackage : Form
    {
        public checkPackage()
        {
            InitializeComponent();
        }

        public checkPackage(object[] obj) : this()
        {
            listDeletePackage.Items.Clear();    

            for(int i = 0; i < obj.Length; i++)
            {
                listDeletePackage.Items.Add(obj[i]);
            }
        }
    }
}
