using System;
using System.Collections;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Windows.Forms;

namespace WindowsPackageRemover
{
    public partial class WindowsPackageRemover : Form
    {
        public WindowsPackageRemover()
        {
            InitializeComponent();

            initPacakges();
            btnRemove.Click += BtnRemove_Click;

            //런스페이스 생성 및 오픈
            runSpace = RunspaceFactory.CreateRunspace();
            runSpace.Open();
        }

        Runspace runSpace;

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            //삭제할 패키지를 저장할 목록
            ArrayList deletePacakges = new ArrayList();

            for (int i = 0; i < checkedListPackage.CheckedItems.Count; i++)
            {
                deletePacakges.Add(checkedListPackage.CheckedItems[i].ToString());
            }

            //최종 확인 팝업 띄우기
            checkPackage CheckPop = new checkPackage(deletePacakges.ToArray());

            //삭제를 누르면 삭제 진행
            if (CheckPop.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("삭제를 진행합니다.");

                //PowerShell 객체 생성
                using (PowerShell ps = PowerShell.Create())
                {                    
                    ps.Runspace = runSpace;

                    //삭제할 패키지들로 파워쉘 명령어 추가 
                    foreach (object value in deletePacakges.ToArray())
                    {
                        ////Get-AppxPackage AllUsers -Name *value* | Remove-AppxPackage                        
                        ps.AddCommand("Get-AppxPackage");
                        ps.AddParameter("AllUsers");
                        ps.AddParameter("name", "*" + value.ToString().Trim() + "*");
                        //자동 파이프라인
                        ps.AddCommand("Remove-AppxPackage");

                        //명령어 종결(명령어 여러 줄 사용 가능)
                        ps.AddStatement();
                    }

                    //명령어 실행 
                    ps.Invoke();                  
                }

                initPacakges();
            }
            else
            {
                MessageBox.Show("취소");
            }
        }

        private void initPacakges()     
        {
            checkedListPackage.Items.Clear();
            string packageName;

            using (PowerShell ps = PowerShell.Create())
            {
                ps.Runspace = runSpace;

                //설치된 패키지들의 이름만 검색
                //Get-AppxPackage | Select-Object -Property Name
                ps.AddCommand("Get-AppxPackage");
                ps.AddCommand("Select-Object");
                ps.AddParameter("Property", "Name");

                //패키지들의 이름을 체크드리스트박스에 추가
                foreach (PSObject result in ps.Invoke())
                {
                    packageName = result.ToString().Replace("@{Name=", "").Replace('}', ' ').Trim();

                    //코타나 이름 표시
                    if (packageName.Equals("Microsoft.549981C3F5F10"))
                    {
                        checkedListPackage.Items.Add(packageName.Replace("549981C3F5F10", "Cortana"));
                        continue;
                    }

                    checkedListPackage.Items.Add(packageName);                    
                }
            }
        }
    }
}