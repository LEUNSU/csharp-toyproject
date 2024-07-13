using System.Windows.Forms;
using MetroFramework.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Scheduler
{
    public partial class FrmScheduler : MetroForm
    {
        private string? date; // ��¥ ���� ����
        private string? path; // ���� ��� ���� ����

        private void FrmScheduler_Load(object sender, EventArgs e)
        {
            FrmLogin frm = new FrmLogin();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.TopMost = true;
            frm.ShowDialog();

            //CreateDirectory();
            TodayDate();  // �� �ε�� TodayDate �޼��带 ȣ���ؼ� ������ ���� ǥ��
        }
        public FrmScheduler()
        {
            InitializeComponent();
        }

        /*private void CreateDirectory() // !!!!!!!!�� �ż� c:\ ���͸��� �̸� 'MyDiary' ���� ���� �� ����!!!!!!!!!
        {
            DirectoryInfo dir = new DirectoryInfo(@"c:\");
            dir.CreateSubdirectory("MyDiary");
        }
        */



        private void TodayDate() // ���� ��¥�� ������ ������ �ҷ�����
        {
            date = DateTime.Now.ToString("yyyy-MM-dd");
            path = string.Format(@"c:\MyDiary\{0}.txt", date);

            try
            {
                if (File.Exists(path))
                {
                    string text = File.ReadAllText(path);
                    AddItemsToListBox(text);
                }
                else
                {
                    // ������ ���� �� �ʱ�ȭ
                    ScheduleList.Rows.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("����" + ex.Message);
            }
        }

        private void AddItemsToListBox(string text)
        {
            // �ؽ�Ʈ�� �� ������ �����Ͽ� ListBox�� ������ �߰�
            ScheduleList.Rows.Clear();
            string[] items = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in items)
            {
                ScheduleList.Rows.Add(item);
            }
        }

        private void MonthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            DateTimePicker.Value = MonthCalendar.SelectionStart; // MonthCalender�� ���õ� ��¥�� DateTimePicker �� ������Ʈ

            // ������ ��¥ TxtDate �ؽ�Ʈ�ڽ��� ǥ��
            if (MonthCalendar.SelectionRange.Start == MonthCalendar.SelectionRange.End)
            {
                TxtDate.Text = MonthCalendar.SelectionRange.Start.ToString("yyyy-MM-dd");
            }
            else
            {
                TxtDate.Text = MonthCalendar.SelectionRange.Start.ToString("yyyy-MM-dd") + "~" + MonthCalendar.SelectionRange.End.ToString("yyyy-MM-dd");
            }
        }

        private void MonthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            date = e.Start.ToShortDateString(); // ������ ��¥�� ������ date�� ����
            path = Path.Combine(@"c:\MyDiary", $"{date}.txt"); // ������ ��¥�� �ش��ϴ� ���� ��� ����

            try
            {
                if (File.Exists(path))
                {
                    string text = File.ReadAllText(path);
                    AddItemsToListBox(text);

                }
                else
                {
                    ScheduleList.Rows.Clear();
                    MessageBox.Show("�ش� ��¥�� ������ �����ϴ�.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("������ �о���� ���� ������ �߻��߽��ϴ�: " + ex.Message);
            }
        }
        private void DateTimePicker_ValueChanged(object sender, EventArgs e) // ��¥ ���ñ⿡�� ��¥�� ����Ǿ��� ��
        {
            MonthCalendar.SetDate(DateTimePicker.Value); // DateTimePicker�� ������ MonthCalendar ���� ��¥ ������Ʈ

            TxtDate.Text = DateTimePicker.Value.ToString("yyyy-MM-dd");
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtSchedule.Text))
            {
                MessageBox.Show("������ �Է��ϼ���.");
                return;
            }

            ScheduleList.Rows.Add(TxtSchedule.Text, false);
            TxtSchedule.Text = "";

            try
            {
                var tempDate = TxtDate.Text;
                if (tempDate.IndexOf("~") > -1) // ��¥�� ������ ���
                {
                    var dates = tempDate.Split('~');
                    var startDate = DateTime.Parse(dates[0]);
                    var endDate = DateTime.Parse(dates[1]);

                    for (DateTime curDate = startDate; curDate <= endDate; curDate = curDate.AddDays(1))
                    {
                        var path = Path.Combine(@"c:\MyDiary", $"{curDate.ToString("yyyy-MM-dd")}.txt");

                        // DataGridView�� ��� ���� �ؽ�Ʈ�� ��ȯ�Ͽ� ����
                        string newText = "";
                        foreach (DataGridViewRow row in ScheduleList.Rows)
                        {
                            newText += row.Cells[0].Value?.ToString() + Environment.NewLine;
                        }
                        File.WriteAllText(path, newText);
                    }

                    MessageBox.Show("������ ���������� ����Ǿ����ϴ�.");
                }
                else // ���� ��¥�� ���
                {
                    var path = Path.Combine(@"c:\MyDiary", $"{date}.txt");

                    // DataGridView�� ��� ���� �ؽ�Ʈ�� ��ȯ�Ͽ� ����
                    string newText = "";
                    foreach (DataGridViewRow row in ScheduleList.Rows)
                    {
                        newText += row.Cells[0].Value?.ToString() + Environment.NewLine;
                    }
                    File.WriteAllText(path, newText);

                    MessageBox.Show("������ ���������� ����Ǿ����ϴ�.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������ �߻��߽��ϴ�: {ex.Message}");
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (ScheduleList.SelectedRows.Count == 0)
            {
                MessageBox.Show("������ ������ �����ϼ���.");
                return;
            }

            foreach (DataGridViewRow row in ScheduleList.SelectedRows)
            {
                ScheduleList.Rows.Remove(row);
            }

            try
            {
                var path = Path.Combine(@"c:\MyDiary", $"{date}.txt");
                if (File.Exists(path))
                {
                    // DataGridView�� ��� ���� �ؽ�Ʈ�� ��ȯ�Ͽ� ����
                    string newText = "";
                    foreach (DataGridViewRow row in ScheduleList.Rows)
                    {
                        newText += row.Cells[0].Value?.ToString() + Environment.NewLine;
                    }
                    File.WriteAllText(path, newText);

                    MessageBox.Show("������ ���������� �����Ǿ����ϴ�.");
                }
                else
                {
                    MessageBox.Show("������ ������ �������� �ʽ��ϴ�.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("������ �����ϴ� ���� ������ �߻��߽��ϴ�: " + ex.Message);
            }
        }
        private void ScheduleList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ScheduleList.CurrentCell != null)
            {
                if ((bool)ScheduleList.SelectedRows[0].Cells[1].Value == true)
                    ScheduleList.SelectedRows[0].Cells[1].Value = false;
                else
                    ScheduleList.SelectedRows[0].Cells[1].Value = true;
            }
        }

        private void Schedule_Enter(object sender, EventArgs e)
        {

        }

        /* ����
private void ScheduleList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
{
if (ScheduleList.IsCurrentCellDirty)
{
ScheduleList.CommitEdit(DataGridViewDataErrorContexts.Commit);
}
}

private void ScheduleList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
{
if (ScheduleList.Columns[e.ColumnIndex].Name == "CheckBoxes")
{
DataGridViewDisableButtonCell buttonCell =
 (DataGridViewDisableButtonCell)ScheduleList.
 Rows[e.RowIndex].Cells["Buttons"];

DataGridViewCheckBoxCell checkCell =
 (DataGridViewCheckBoxCell)ScheduleList.
 Rows[e.RowIndex].Cells["CheckBoxes"];
buttonCell.Enabled = !(bool)checkCell.Value;

ScheduleList.Invalidate();
}
}*/
    }
}

/*TODO
 * üũ����Ʈ ����ó��
 * ���ٶ˹汸������ ��ġ��
*/