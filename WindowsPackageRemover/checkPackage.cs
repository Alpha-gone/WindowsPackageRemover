using System.Windows.Forms;

namespace WindowsPackageRemover
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

            for (int i = 0; i < obj.Length; i++)
            {
                listDeletePackage.Items.Add(obj[i]);
            }
        }
    }
}
