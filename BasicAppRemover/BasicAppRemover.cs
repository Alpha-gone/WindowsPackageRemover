using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management.Automation;
using System.Collections;
using System.Management.Automation.Runspaces;

namespace BasicAppRemover
{
    
    public partial class BasicAppRemover : Form
    {
        
        

        public BasicAppRemover()
        {
            InitializeComponent();

            initData();
            btnRemove.Click += BtnRemove_Click;
            
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            
            ArrayList arrayList = new ArrayList();

            for(int i = 0; i < checkedListPackage.CheckedItems.Count; i++)   
            {
                arrayList.Add(checkedListPackage.CheckedItems[i].ToString());    
            }
             
            //최종 확인 팝업 띄우기
            checkPackage CheckPop = new checkPackage(arrayList.ToArray());

            //삭제를 누르면 삭제 진행
            if (CheckPop.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("삭제를 진행합니다.");

                Runspace runSpace = RunspaceFactory.CreateRunspace();
                runSpace.Open();
                PowerShell ps = PowerShell.Create();
                ps.Runspace = runSpace;
  
                foreach (object value in arrayList.ToArray()) {
                    ps.AddCommand("Get-AppxPackage");
                    ps.AddArgument("AllUsers");;  
                    ps.AddParameter("name", "*" + value.ToString().Trim() + "*");
                    ps.AddCommand("Remove-AppxPackage");
                    ps.AddStatement();
                    //try
                    //{
                    //    ps.AddCommand("Remove-AppxPackage");
                    //    ps.AddParameter("AllUsers");
                    //    ps.AddParameter("Package", "*" + value.ToString() + "*");
                    //    ps.AddStatement();
                    //}
                    //catch (Exception)
                    //{

                    //    throw;
                    //}
                    
                    
                }

                try
                {
                    ps.Invoke();

                }
                catch (Exception) {
                    throw;
                }

                //ps.Invoke();
                //checkedListPackage.Items.Clear();
                //foreach (PSObject result in ps.Invoke())
                //{
                //    checkedListPackage.Items.Add(result.ToString());
                //}
                ps.Dispose();
                runSpace.Dispose();
                initData();
            }
            else
            {
                MessageBox.Show("취소");
            }
        }

        private void initData()
        {
            Runspace runSpace = RunspaceFactory.CreateRunspace();
            runSpace.Open();
            PowerShell ps = PowerShell.Create();
            ps.Runspace = runSpace;
            
            checkedListPackage.Items.Clear();

            //설치된 패키지들의 이름만 검색
            ps.AddCommand("Get-AppxPackage");           
            ps.AddCommand("Select-Object");
            ps.AddArgument("Name");

            foreach (PSObject result in ps.Invoke())
            {
                checkedListPackage.Items.Add(result.ToString().Replace("@{Name=","").Replace('}',' '));  
                //checkedListPackage.Items.Add(result.ToString().Replace("@{PackageFullName=", "").Replace('}', ' '));
            }
            ps.Dispose();
            runSpace.Dispose();
        }
    }
}