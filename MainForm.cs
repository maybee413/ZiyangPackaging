using Microsoft.VisualBasic.ApplicationServices;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using opway.mes.Model;
using opway.mes.Model.Enums;
using opway.mes.Model.Models;
using SqlSugar;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ZiyangPackagingSoftware
{
    public partial class MainForm : Form
    {
        //private const string BaseUrl = "http://192.168.51.56:2345/api"; // �滻Ϊ���API����URL
        private const string BaseUrl = "http://localhost:9291/api";
        private readonly HttpClient _httpClient;

        private Users _user;
        public MainForm(Users user, HttpClient httpClient)
        {
            InitializeComponent();
            _user = user;
            _httpClient = httpClient;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // �ڴ��ڼ���ʱ��ʾ�û���Ϣ
            toolStripStatusLabel.Text = $"�û���: {_user.full_name}, ����: {_user.department.department_name}";

        }

        private async void snInput_KeyPressAsync(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar != Keys.Enter)
            {
                return;
            }


            if (snList.Items.Contains(snInput.Text.Trim()))
            {

                MessageBox.Show("�����к��Ѵ��ڣ�");
                return;
            }

            //�ж��Ƿ����������װ��������
            if (snList.Items.Count == int.Parse(ConfigurationManager.AppSettings["MaxPrimaryPackageCount"]))
            {
                MessageBox.Show("��װ������������,������������");
                return;
            }

            //�ж�SN���Ƿ���Ա���װ

            // ����POST����Web API���е�¼��֤
            HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}/ZiYangPackaging/CheckSN?serialNumber={snInput.Text.Trim()}");

            // �����Ӧ�Ƿ�ɹ�
            if (response.IsSuccessStatusCode)
            {
                // ��ȡ��Ӧ����
                var responseContent = await response.Content.ReadAsStringAsync();

                // ������Ӧ����
                var result = JsonConvert.DeserializeObject<MessageModel<bool>>(responseContent);

                if (result != null)
                {

                    if (result.response)
                    {
                        snList.Items.Add(snInput.Text.Trim());
                        this.snInput.Text = "";
                        if (snList.Items.Count == int.Parse(ConfigurationManager.AppSettings["MaxPrimaryPackageCount"]))
                        {
                            DialogResult dialogResult = MessageBox.Show("��װ�����������ޣ��Ƿ������װ��", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.Yes)
                            {
                                // �û������ȷ����ť����������
                                // ���������Ӽ����������߼�
                                executePrimaryPackaging_ClickAsync(null, null);
                            }
                            else
                            {
                                // �û������ȡ����ť��ȡ������
                                // ����������ȡ���������߼�
                                return;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(result.msg);
                        return;
                    }
                }

            }
            else
            {
                // ���������Ӧ
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        private void snInput_TextChanged(object sender, EventArgs e)
        {
            //this.snList.Items.Clear();
        }

        private async void executePrimaryPackaging_ClickAsync(object sender, EventArgs e)
        {
            // ���ð�ť
            executePackaging.Enabled = false;

            // ��������Ӽ����������߼�
            // ���ð�װ����
            bool success = await ExecutePrimaryPackaging();

            if (success)
            {
                snList.Items.Clear();
            }
            // ��װ����ִ����ɺ����ð�ť
            executePackaging.Enabled = true;
        }

        private async Task<bool> ExecutePrimaryPackaging()
        {
            // ִ�а�װ�������߼�
            // ��װ��ɺ󷵻�true��false��ʾ�ɹ���ʧ��
            // ������Ե�����İ�װ������ʾ��Ϊ�첽����

            // ���������л�Ϊ JSON �ַ���
            // ����������
            List<string> _snList = snList.Items.Cast<string>().ToList();

            var serialNumberList = JsonConvert.SerializeObject(_snList);

            // �� JSON �ַ���ת��Ϊ StringContent
            var content = new StringContent(serialNumberList, Encoding.UTF8, "application/json");

            // ����һ�� multipart/form-data ����
            MultipartFormDataContent formData = new MultipartFormDataContent();
            foreach (var item in _snList)
            {
                formData.Add(new StringContent(item), "serialNumberList");
            }
            formData.Add(new StringContent(_user.id.ToString()), "userId");

            // ���� POST ����
            var response = await _httpClient.PostAsync($"{BaseUrl}/ZiYangPackaging/ExecutePrimaryPackaging", formData);

            // �����Ӧ�Ƿ�ɹ�
            if (response.IsSuccessStatusCode)
            {
                // ��ȡ��Ӧ����
                var responseContent = await response.Content.ReadAsStringAsync();

                // ������Ӧ����
                var result = JsonConvert.DeserializeObject<MessageModel<string>>(responseContent);

                if (result != null)
                {

                    if (result.success)
                    {
                        //ִ�а�װ��ӡ
                        List<Dictionary<string, string>> ValuePairs = new List<Dictionary<string, string>>();

                        //��ǩ��ӡ
                        Dictionary<string, string> Values = new Dictionary<string, string>();

                        Values.Add("ProductName", ConfigurationManager.AppSettings["ProductName"]?.ToString());
                        Values.Add("PartNumber", ConfigurationManager.AppSettings["Part"]?.ToString());
                        Values.Add("ItemNumber", ConfigurationManager.AppSettings["Item"]?.ToString());
                        Values.Add("CustomerP/N", ConfigurationManager.AppSettings["CustomerP/N"]?.ToString());
                        Values.Add("Quantity", $"Quantity: {snList.Items.Count} PCS");
                        Values.Add("LotNumber", ConfigurationManager.AppSettings["LotNumber"]?.ToString());
                        Values.Add("PkgNumber", $"Pkg Number: {result.response}");

                        ValuePairs.Add(Values);

                        if (!PrintHelper.PrintHelper.BarTenderPrintInfo(ConfigurationManager.AppSettings["PrinterName"]?.ToString(), Application.StartupPath + $"\\PrinterModule\\{ConfigurationManager.AppSettings["PrimaryModuleName"]?.ToString()}", ValuePairs, "Print"))
                        {
                            MessageBox.Show("��ǩ��ӡʧ��");
                            return false;
                        }
                        else
                        {
                            MessageBox.Show("ִ�а�װ���");
                            return true;
                        }
                    }
                    else
                    {
                        MessageBox.Show(result.msg);
                        return false;
                    }
                }
            }
            return true;
        }

        private void snList_DoubleClick(object sender, EventArgs e)
        {
            // ȷ��������һ���ѡ��
            if (snList.SelectedItem != null)
            {
                // �Ƴ���˫������
                snList.Items.Remove(snList.SelectedItem);
            }
        }

        private async void queryBySN_Click(object sender, EventArgs e)
        {
            // ����POST����Web API���е�¼��֤
            HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}/ZiYangPackaging/FetchData?searchType={ZiYangPackagingSearchTypeEnum.BingSN.ToString()}&query={snSearchInput.Text.Trim()}");
            // �����Ӧ�Ƿ�ɹ�
            if (response.IsSuccessStatusCode)
            {
                // ��ȡ��Ӧ����
                var responseContent = await response.Content.ReadAsStringAsync();

                // ������Ӧ����
                var result = JsonConvert.DeserializeObject<MessageModel<List<ZiYangPackaging>>>(responseContent);

                if (result != null)
                {

                    if (result.success)
                    {
                        dataGridView1.DataSource = result.response;
                    }
                    else
                    {
                        MessageBox.Show(result.msg);
                        return;
                    }
                }

            }
            else
            {
                // ���������Ӧ
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        private async void queryByPackagingNumber_Click(object sender, EventArgs e)
        {
            // ����POST����Web API���е�¼��֤
            HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}/ZiYangPackaging/FetchData?searchType={ZiYangPackagingSearchTypeEnum.PackagingCode.ToString()}&query={packagingNumberSearchInput.Text.Trim()}");
            // �����Ӧ�Ƿ�ɹ�
            if (response.IsSuccessStatusCode)
            {
                // ��ȡ��Ӧ����
                var responseContent = await response.Content.ReadAsStringAsync();

                // ������Ӧ����
                var result = JsonConvert.DeserializeObject<MessageModel<List<ZiYangPackaging>>>(responseContent);

                if (result != null)
                {

                    if (result.success)
                    {
                        dataGridView1.DataSource = result.response;
                    }
                    else
                    {
                        MessageBox.Show(result.msg);
                        return;
                    }
                }

            }
            else
            {
                // ���������Ӧ
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        private async void export_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                Title = "Save Excel File",
                FileName = $"{packagingNumberExportInput.Text.Trim()}.xlsx"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var filePath = saveFileDialog.FileName;
                // ���� API ��ַΪ /api/export/excel
                var response = await _httpClient.GetAsync($"{BaseUrl}/BOSAautotestData/ExportToExcel?packagingCode={packagingNumberExportInput.Text.Trim()}");
                response.EnsureSuccessStatusCode();

                // ���ļ������浽����
                await using var fileStream = await response.Content.ReadAsStreamAsync();
                await using var fileStreamOutput = File.Create(filePath);
                await fileStream.CopyToAsync(fileStreamOutput);

                // �ڱ����ļ���ʹ��Ĭ�Ϲ����ĳ�����ļ�
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
        }

        private async void primaryCodeInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar != Keys.Enter)
            {
                return;
            }


            if (primaryPackagingCodeList.Items.Contains(snInput.Text.Trim()))
            {

                MessageBox.Show("�����к��Ѵ��ڣ�");
                return;
            }

            //�ж��Ƿ����������װ��������
            if (primaryPackagingCodeList.Items.Count == int.Parse(ConfigurationManager.AppSettings["MaxSecondaryPackageCount"]))
            {
                MessageBox.Show("��װ������������,������������");
                return;
            }


            //�ж�SN���Ƿ���Ա���װ

            // ����POST����Web API���е�¼��֤
            HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}/ZiYangPackaging/CheckPrimaryPackagingCode?serialNumber={snInput.Text.Trim()}");

            // �����Ӧ�Ƿ�ɹ�
            if (response.IsSuccessStatusCode)
            {
                // ��ȡ��Ӧ����
                var responseContent = await response.Content.ReadAsStringAsync();

                // ������Ӧ����
                var result = JsonConvert.DeserializeObject<MessageModel<bool>>(responseContent);

                if (result != null)
                {

                    if (result.response)
                    {
                        snList.Items.Add(snInput.Text.Trim());

                        if (snList.Items.Count == int.Parse(ConfigurationManager.AppSettings["MaxSecondaryPackageCount"]))
                        {
                            DialogResult dialogResult = MessageBox.Show("��װ�����������ޣ��Ƿ������װ��", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.Yes)
                            {
                                // �û������ȷ����ť����������
                                // ���������Ӽ����������߼�
                                executePrimaryPackaging_ClickAsync(null, null);
                            }
                            else
                            {
                                // �û������ȡ����ť��ȡ������
                                // ����������ȡ���������߼�
                                return;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(result.msg);
                        return;
                    }
                }

            }
            else
            {
                // ���������Ӧ
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        private async void executeSecondaryPackaging_Click(object sender, EventArgs e)
        {
            // ���ð�ť
            executeSecondaryPackaging.Enabled = false;

            // ��������Ӽ����������߼�
            // ���ð�װ����
            bool success = await ExecuteSecondaryPackaging();

            // ��װ����ִ����ɺ����ð�ť
            executeSecondaryPackaging.Enabled = true;
        }

        private async Task<bool> ExecuteSecondaryPackaging()
        {
            // ִ�а�װ�������߼�
            // ��װ��ɺ󷵻�true��false��ʾ�ɹ���ʧ��
            // ������Ե�����İ�װ������ʾ��Ϊ�첽����

            // ���������л�Ϊ JSON �ַ���
            // ����������
            List<string> _snList = primaryPackagingCodeList.Items.Cast<string>().ToList();

            var serialNumberList = JsonConvert.SerializeObject(_snList);

            // �� JSON �ַ���ת��Ϊ StringContent
            var content = new StringContent(serialNumberList, Encoding.UTF8, "application/json");

            // ����һ�� multipart/form-data ����
            MultipartFormDataContent formData = new MultipartFormDataContent();
            foreach (var item in _snList)
            {
                formData.Add(new StringContent(item), "primaryPackagingCodeList");
            }
            formData.Add(new StringContent(_user.id.ToString()), "userId");

            // ���� POST ����
            var response = await _httpClient.PostAsync($"{BaseUrl}/ZiYangPackaging/ExecuteSecondaryPackaging", formData);

            // �����Ӧ�Ƿ�ɹ�
            if (response.IsSuccessStatusCode)
            {
                // ��ȡ��Ӧ����
                var responseContent = await response.Content.ReadAsStringAsync();

                // ������Ӧ����
                var result = JsonConvert.DeserializeObject<MessageModel<(string, int)>>(responseContent);

                if (result != null)
                {

                    if (result.success)
                    {
                        //ִ�а�װ��ӡ
                        List<Dictionary<string, string>> ValuePairs = new List<Dictionary<string, string>>();

                        //��ǩ��ӡ
                        Dictionary<string, string> Values = new Dictionary<string, string>();

                        Values.Add("ProductName", ConfigurationManager.AppSettings["ProductName"]?.ToString());
                        Values.Add("PartNumber", ConfigurationManager.AppSettings["Part"]?.ToString());
                        Values.Add("ItemNumber", ConfigurationManager.AppSettings["Item"]?.ToString());
                        Values.Add("CustomerP/N", ConfigurationManager.AppSettings["CustomerP/N"]?.ToString());
                        Values.Add("Quantity", $"Quantity: {result.response.Item2} PCS");
                        Values.Add("LotNumber", ConfigurationManager.AppSettings["LotNumber"]?.ToString());
                        Values.Add("PkgNumber", $"Pkg Number: {result.response.Item1}");

                        ValuePairs.Add(Values);

                        if (!PrintHelper.PrintHelper.BarTenderPrintInfo(ConfigurationManager.AppSettings["PrinterName"]?.ToString(), Application.StartupPath + $"\\PrinterModule\\{ConfigurationManager.AppSettings["PrimaryModuleName"]?.ToString()}", ValuePairs, "Print"))
                        {
                            MessageBox.Show("��ǩ��ӡʧ��");
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show(result.msg);
                        return false;
                    }
                }
            }
            return true;
        }

        private void printLabelTest_Click(object sender, EventArgs e)
        {
            //ִ�а�װ��ӡ
            List<Dictionary<string, string>> ValuePairs = new List<Dictionary<string, string>>();

            //��ǩ��ӡ
            Dictionary<string, string> Values = new Dictionary<string, string>();

            Values.Add("ProductName", ConfigurationManager.AppSettings["ProductName"]?.ToString());
            Values.Add("PartNumber", ConfigurationManager.AppSettings["Part"]?.ToString());
            Values.Add("ItemNumber", ConfigurationManager.AppSettings["Item"]?.ToString());
            Values.Add("CustomerP/N", ConfigurationManager.AppSettings["CustomerP/N"]?.ToString());
            Values.Add("Quantity", $"Quantity: 0 PCS");
            Values.Add("LotNumber", ConfigurationManager.AppSettings["LotNumber"]?.ToString());
            Values.Add("PkgNumber", $"Pkg Number: PN19700101-P001");

            ValuePairs.Add(Values);

            if (!PrintHelper.PrintHelper.BarTenderPrintInfo(ConfigurationManager.AppSettings["PrinterName"]?.ToString(), Application.StartupPath + $"\\PrinterModule\\{ConfigurationManager.AppSettings["PrimaryModuleName"]?.ToString()}", ValuePairs, "Print"))
            {
                MessageBox.Show("��ǩ�Դ�ӡʧ��");
            }
            else
            {
                MessageBox.Show("��ǩ�Դ�ӡ���");
            }
        }

        private async void rePrintLabel_Click(object sender, EventArgs e)
        {
            //���������װ�Ż�ȡ��װ����
            HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}/ZiYangPackaging/GetBindingSNCountByPackagingCode?packagingCode={rePrimaryPrintInput.Text.Trim()}");

            // �����Ӧ�Ƿ�ɹ�
            if (response.IsSuccessStatusCode)
            {
                // ��ȡ��Ӧ����
                var responseContent = await response.Content.ReadAsStringAsync();

                // ������Ӧ����
                var result = JsonConvert.DeserializeObject<MessageModel<int>>(responseContent);

                if (result != null)
                {

                    if (result.success)
                    {
                        //ִ�а�װ��ӡ
                        List<Dictionary<string, string>> ValuePairs = new List<Dictionary<string, string>>();

                        //��ǩ��ӡ
                        Dictionary<string, string> Values = new Dictionary<string, string>();

                        Values.Add("ProductName", ConfigurationManager.AppSettings["ProductName"]?.ToString());
                        Values.Add("PartNumber", ConfigurationManager.AppSettings["Part"]?.ToString());
                        Values.Add("ItemNumber", ConfigurationManager.AppSettings["Item"]?.ToString());
                        Values.Add("CustomerP/N", ConfigurationManager.AppSettings["CustomerP/N"]?.ToString());
                        Values.Add("Quantity", $"Quantity: {result.response} PCS");
                        Values.Add("LotNumber", ConfigurationManager.AppSettings["LotNumber"]?.ToString());
                        Values.Add("PkgNumber", $"Pkg Number: {rePrimaryPrintInput.Text.Trim()}");

                        ValuePairs.Add(Values);

                        if (!PrintHelper.PrintHelper.BarTenderPrintInfo(ConfigurationManager.AppSettings["PrinterName"]?.ToString(), Application.StartupPath + $"\\PrinterModule\\{ConfigurationManager.AppSettings["PrimaryModuleName"]?.ToString()}", ValuePairs, "Print"))
                        {
                            MessageBox.Show("��ǩ�Դ�ӡʧ��");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("��ǩ�Դ�ӡ���");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show(result.msg);
                        return;
                    }
                }

            }
            else
            {
                // ���������Ӧ
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        private void printSecondaryLabelTest_Click(object sender, EventArgs e)
        {
            //ִ�а�װ��ӡ
            List<Dictionary<string, string>> ValuePairs = new List<Dictionary<string, string>>();

            //��ǩ��ӡ
            Dictionary<string, string> Values = new Dictionary<string, string>();

            Values.Add("ProductName", ConfigurationManager.AppSettings["ProductName"]?.ToString());
            Values.Add("PartNumber", ConfigurationManager.AppSettings["Part"]?.ToString());
            Values.Add("ItemNumber", ConfigurationManager.AppSettings["Item"]?.ToString());
            Values.Add("CustomerP/N", ConfigurationManager.AppSettings["CustomerP/N"]?.ToString());
            Values.Add("Quantity", $"Quantity: 0 PCS");
            Values.Add("LotNumber", ConfigurationManager.AppSettings["LotNumber"]?.ToString());
            Values.Add("PkgNumber", $"Pkg Number: PN19700101-S001");

            ValuePairs.Add(Values);

            if (!PrintHelper.PrintHelper.BarTenderPrintInfo(ConfigurationManager.AppSettings["PrinterName"]?.ToString(), Application.StartupPath + $"\\PrinterModule\\{ConfigurationManager.AppSettings["SecondaryModuleName"]?.ToString()}", ValuePairs, "Print"))
            {
                MessageBox.Show("��ǩ�Դ�ӡʧ��");
            }
            else
            {
                MessageBox.Show("��ǩ�Դ�ӡ���");
            }
        }

        private async void reSecondaryPrintLabel_Click(object sender, EventArgs e)
        {
            //���������װ�Ż�ȡ��װ����
            HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}/ZiYangPackaging/GetBindingSNCountByPackagingCode?packagingCode={reSecondaryPrintInput.Text.Trim()}");

            // �����Ӧ�Ƿ�ɹ�
            if (response.IsSuccessStatusCode)
            {
                // ��ȡ��Ӧ����
                var responseContent = await response.Content.ReadAsStringAsync();

                // ������Ӧ����
                var result = JsonConvert.DeserializeObject<MessageModel<int>>(responseContent);

                if (result != null)
                {

                    if (result.success)
                    {
                        //ִ�а�װ��ӡ
                        List<Dictionary<string, string>> ValuePairs = new List<Dictionary<string, string>>();

                        //��ǩ��ӡ
                        Dictionary<string, string> Values = new Dictionary<string, string>();

                        Values.Add("ProductName", ConfigurationManager.AppSettings["ProductName"]?.ToString());
                        Values.Add("PartNumber", ConfigurationManager.AppSettings["Part"]?.ToString());
                        Values.Add("ItemNumber", ConfigurationManager.AppSettings["Item"]?.ToString());
                        Values.Add("CustomerP/N", ConfigurationManager.AppSettings["CustomerP/N"]?.ToString());
                        Values.Add("Quantity", $"Quantity: {result.response} PCS");
                        Values.Add("LotNumber", ConfigurationManager.AppSettings["LotNumber"]?.ToString());
                        Values.Add("PkgNumber", $"Pkg Number: {reSecondaryPrintInput.Text.Trim()}");

                        ValuePairs.Add(Values);

                        if (!PrintHelper.PrintHelper.BarTenderPrintInfo(ConfigurationManager.AppSettings["PrinterName"]?.ToString(), Application.StartupPath + $"\\PrinterModule\\{ConfigurationManager.AppSettings["SecondaryModuleName"]?.ToString()}", ValuePairs, "Print"))
                        {
                            MessageBox.Show("��ǩ�Դ�ӡʧ��");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("��ǩ�Դ�ӡ���");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show(result.msg);
                        return;
                    }
                }

            }
            else
            {
                // ���������Ӧ
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        private async void secondaryExport_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                Title = "Save Excel File",
                FileName = $"{secondaryPackagingNumberExportInput.Text.Trim()}.xlsx"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var filePath = saveFileDialog.FileName;
                // ���� API ��ַΪ /api/export/excel
                var response = await _httpClient.GetAsync($"{BaseUrl}/BOSAautotestData/ExportToExcel?packagingCode={secondaryPackagingNumberExportInput.Text.Trim()}");
                response.EnsureSuccessStatusCode();

                // ���ļ������浽����
                await using var fileStream = await response.Content.ReadAsStreamAsync();
                await using var fileStreamOutput = File.Create(filePath);
                await fileStream.CopyToAsync(fileStreamOutput);

                // �ڱ����ļ���ʹ��Ĭ�Ϲ����ĳ�����ļ�
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
        }

        private async void querySecondaryByPackagingNumber_Click(object sender, EventArgs e)
        {
            // ����POST����Web API���е�¼��֤
            HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}/ZiYangPackaging/FetchData?searchType={ZiYangPackagingSearchTypeEnum.PackagingCode.ToString()}&query={secondaryPackagingCodeinput.Text.Trim()}");
            // �����Ӧ�Ƿ�ɹ�
            if (response.IsSuccessStatusCode)
            {
                // ��ȡ��Ӧ����
                var responseContent = await response.Content.ReadAsStringAsync();

                // ������Ӧ����
                var result = JsonConvert.DeserializeObject<MessageModel<List<ZiYangPackaging>>>(responseContent);

                if (result != null)
                {

                    if (result.success)
                    {
                        dataGridView2.DataSource = result.response;
                    }
                    else
                    {
                        MessageBox.Show(result.msg);
                        return;
                    }
                }

            }
            else
            {
                // ���������Ӧ
                throw new Exception($"Error: {response.StatusCode}");
            }
        }
    }
}
