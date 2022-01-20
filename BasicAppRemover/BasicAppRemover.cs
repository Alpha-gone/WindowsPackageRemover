using System;
using System.Collections;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Windows.Forms;

namespace BasicAppRemover
{

    public partial class BasicAppRemover : Form
    {
        public BasicAppRemover()
        {
            InitializeComponent();

            initData();
            btnRemove.Click += BtnRemove_Click;
            runSpace = RunspaceFactory.CreateRunspace();
            runSpace.Open();
        }
        Runspace runSpace;

        private void BtnRemove_Click(object sender, EventArgs e)
        {

            ArrayList arrayList = new ArrayList();

            for (int i = 0; i < checkedListPackage.CheckedItems.Count; i++)
            {
                arrayList.Add(checkedListPackage.CheckedItems[i].ToString());
            }

            //최종 확인 팝업 띄우기
            checkPackage CheckPop = new checkPackage(arrayList.ToArray());

            //삭제를 누르면 삭제 진행
            if (CheckPop.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("삭제를 진행합니다.");


                using (PowerShell ps = PowerShell.Create())
                {
                    ps.Runspace = runSpace;

                    foreach (object value in arrayList.ToArray())
                    {
                        ps.AddCommand("Get-AppxPackage");
                        ps.AddParameter("AllUsers");
                        ps.AddParameter("name", "*" + value.ToString().Trim() + "*");
                        ps.AddCommand("Remove-AppxPackage");
                        Console.WriteLine(ps.Commands.Commands[0].ToString() + " " +
                                            ps.Commands.Commands[0].Parameters[0].Name.ToString() +" " +
                                            ps.Commands.Commands[0].Parameters[0].Value.ToString() + " " +
                                            ps.Commands.Commands[0].Parameters[1].Name.ToString() + " " +
                                            ps.Commands.Commands[0].Parameters[1].Value.ToString() + "|" + 
                                            ps.Commands.Commands[1].ToString());
                        ps.Invoke();
                    }

                    Console.WriteLine("명령어 실행 완료");
                    initData();
                }
            }
            else
            {
                MessageBox.Show("취소");
            }
        }

        private void initData()     
        {
            checkedListPackage.Items.Clear();

            using (PowerShell ps = PowerShell.Create())
            {
                ps.Runspace = runSpace;

                //설치된 패키지들의 이름만 검색
                ps.AddCommand("Get-AppxPackage");
                ps.AddCommand("Select-Object");
                ps.AddArgument("Name");

                foreach (PSObject result in ps.Invoke())
                {
                    checkedListPackage.Items.Add(result.ToString().Replace("@{Name=", "").Replace('}', ' '));
                    //checkedListPackage.Items.Add(result.ToString().Replace("@{PackageFullName=", "").Replace('}', ' '));
                }
            }
        }
    }
}