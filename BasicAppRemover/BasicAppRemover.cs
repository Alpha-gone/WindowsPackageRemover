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

        PowerShell ps = PowerShell.Create();


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

                foreach (String name in arrayList.ToArray()) {
                    ps.AddCommand("Get-AppxPackage -AllUsers");
                    ps.AddParameter("name", name);
                    ps.AddStatement().AddCommand("Remove-AppxPackage");
                    ps.Invoke();
                }
                initData();
            }
            else
            {
                MessageBox.Show("Cancel");
            }
        }

        private void initData()
        {
            checkedListPackage.Items.Clear();
            //파워쉘 내에서 관리자 권한 실행 (새로운 파워쉘 실행 사용 x)
            /*ps.AddCommand("start-process");
            ps.AddParameter("FilePath", "powershell");
            ps.AddParameter("verb", "runAs");*/

            //설치된 패키지들의 이름만 검색 (현재 packageFullName이 출력됨 수정 필요)
            ps.AddCommand("Get-AppxPackage");
            ps.AddStatement();
            ps.AddCommand("Select Name");

            foreach(PSObject result in ps.Invoke())
            {
                checkedListPackage.Items.Add(result.ToString());  
            }
            
        }
    }
}
