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
        //private const string BaseUrl = "http://192.168.51.56:2345/api"; // 替换为你的API基础URL
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
            // 在窗口加载时显示用户信息
            toolStripStatusLabel.Text = $"用户名: {_user.full_name}, 部门: {_user.department.department_name}";

        }

        private async void snInput_KeyPressAsync(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar != Keys.Enter)
            {
                return;
            }


            if (snList.Items.Contains(snInput.Text.Trim()))
            {

                MessageBox.Show("该序列号已存在！");
                return;
            }

            //判断是否已添加至包装数量上限
            if (snList.Items.Count == int.Parse(ConfigurationManager.AppSettings["MaxPrimaryPackageCount"]))
            {
                MessageBox.Show("包装数量以至上限,不允许继续添加");
                return;
            }

            //判断SN号是否可以被包装

            // 发送POST请求到Web API进行登录验证
            HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}/ZiYangPackaging/CheckSN?serialNumber={snInput.Text.Trim()}");

            // 检查响应是否成功
            if (response.IsSuccessStatusCode)
            {
                // 读取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();

                // 解析响应内容
                var result = JsonConvert.DeserializeObject<MessageModel<bool>>(responseContent);

                if (result != null)
                {

                    if (result.response)
                    {
                        snList.Items.Add(snInput.Text.Trim());
                        this.snInput.Text = "";
                        if (snList.Items.Count == int.Parse(ConfigurationManager.AppSettings["MaxPrimaryPackageCount"]))
                        {
                            DialogResult dialogResult = MessageBox.Show("包装数量已至上限，是否继续包装？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.Yes)
                            {
                                // 用户点击了确定按钮，继续操作
                                // 这里可以添加继续操作的逻辑
                                executePrimaryPackaging_ClickAsync(null, null);
                            }
                            else
                            {
                                // 用户点击了取消按钮，取消操作
                                // 这里可以添加取消操作的逻辑
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
                // 处理错误响应
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        private void snInput_TextChanged(object sender, EventArgs e)
        {
            //this.snList.Items.Clear();
        }

        private async void executePrimaryPackaging_ClickAsync(object sender, EventArgs e)
        {
            // 禁用按钮
            executePackaging.Enabled = false;

            // 在这里添加继续操作的逻辑
            // 调用包装方法
            bool success = await ExecutePrimaryPackaging();

            if (success)
            {
                snList.Items.Clear();
            }
            // 包装方法执行完成后启用按钮
            executePackaging.Enabled = true;
        }

        private async Task<bool> ExecutePrimaryPackaging()
        {
            // 执行包装方法的逻辑
            // 包装完成后返回true或false表示成功或失败
            // 这里可以调用你的包装方法，示例为异步方法

            // 将对象序列化为 JSON 字符串
            // 构建请求体
            List<string> _snList = snList.Items.Cast<string>().ToList();

            var serialNumberList = JsonConvert.SerializeObject(_snList);

            // 将 JSON 字符串转换为 StringContent
            var content = new StringContent(serialNumberList, Encoding.UTF8, "application/json");

            // 创建一个 multipart/form-data 请求
            MultipartFormDataContent formData = new MultipartFormDataContent();
            foreach (var item in _snList)
            {
                formData.Add(new StringContent(item), "serialNumberList");
            }
            formData.Add(new StringContent(_user.id.ToString()), "userId");

            // 发送 POST 请求
            var response = await _httpClient.PostAsync($"{BaseUrl}/ZiYangPackaging/ExecutePrimaryPackaging", formData);

            // 检查响应是否成功
            if (response.IsSuccessStatusCode)
            {
                // 读取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();

                // 解析响应内容
                var result = JsonConvert.DeserializeObject<MessageModel<string>>(responseContent);

                if (result != null)
                {

                    if (result.success)
                    {
                        //执行包装打印
                        List<Dictionary<string, string>> ValuePairs = new List<Dictionary<string, string>>();

                        //标签打印
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
                            MessageBox.Show("标签打印失败");
                            return false;
                        }
                        else
                        {
                            MessageBox.Show("执行包装完成");
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
            // 确保至少有一个项被选中
            if (snList.SelectedItem != null)
            {
                // 移除被双击的项
                snList.Items.Remove(snList.SelectedItem);
            }
        }

        private async void queryBySN_Click(object sender, EventArgs e)
        {
            // 发送POST请求到Web API进行登录验证
            HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}/ZiYangPackaging/FetchData?searchType={ZiYangPackagingSearchTypeEnum.BingSN.ToString()}&query={snSearchInput.Text.Trim()}");
            // 检查响应是否成功
            if (response.IsSuccessStatusCode)
            {
                // 读取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();

                // 解析响应内容
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
                // 处理错误响应
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        private async void queryByPackagingNumber_Click(object sender, EventArgs e)
        {
            // 发送POST请求到Web API进行登录验证
            HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}/ZiYangPackaging/FetchData?searchType={ZiYangPackagingSearchTypeEnum.PackagingCode.ToString()}&query={packagingNumberSearchInput.Text.Trim()}");
            // 检查响应是否成功
            if (response.IsSuccessStatusCode)
            {
                // 读取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();

                // 解析响应内容
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
                // 处理错误响应
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
                // 假设 API 地址为 /api/export/excel
                var response = await _httpClient.GetAsync($"{BaseUrl}/BOSAautotestData/ExportToExcel?packagingCode={packagingNumberExportInput.Text.Trim()}");
                response.EnsureSuccessStatusCode();

                // 将文件流保存到本地
                await using var fileStream = await response.Content.ReadAsStreamAsync();
                await using var fileStreamOutput = File.Create(filePath);
                await fileStream.CopyToAsync(fileStreamOutput);

                // 在保存文件后，使用默认关联的程序打开文件
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

                MessageBox.Show("该序列号已存在！");
                return;
            }

            //判断是否已添加至包装数量上限
            if (primaryPackagingCodeList.Items.Count == int.Parse(ConfigurationManager.AppSettings["MaxSecondaryPackageCount"]))
            {
                MessageBox.Show("包装数量以至上限,不允许继续添加");
                return;
            }


            //判断SN号是否可以被包装

            // 发送POST请求到Web API进行登录验证
            HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}/ZiYangPackaging/CheckPrimaryPackagingCode?serialNumber={snInput.Text.Trim()}");

            // 检查响应是否成功
            if (response.IsSuccessStatusCode)
            {
                // 读取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();

                // 解析响应内容
                var result = JsonConvert.DeserializeObject<MessageModel<bool>>(responseContent);

                if (result != null)
                {

                    if (result.response)
                    {
                        snList.Items.Add(snInput.Text.Trim());

                        if (snList.Items.Count == int.Parse(ConfigurationManager.AppSettings["MaxSecondaryPackageCount"]))
                        {
                            DialogResult dialogResult = MessageBox.Show("包装数量已至上限，是否继续包装？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.Yes)
                            {
                                // 用户点击了确定按钮，继续操作
                                // 这里可以添加继续操作的逻辑
                                executePrimaryPackaging_ClickAsync(null, null);
                            }
                            else
                            {
                                // 用户点击了取消按钮，取消操作
                                // 这里可以添加取消操作的逻辑
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
                // 处理错误响应
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        private async void executeSecondaryPackaging_Click(object sender, EventArgs e)
        {
            // 禁用按钮
            executeSecondaryPackaging.Enabled = false;

            // 在这里添加继续操作的逻辑
            // 调用包装方法
            bool success = await ExecuteSecondaryPackaging();

            // 包装方法执行完成后启用按钮
            executeSecondaryPackaging.Enabled = true;
        }

        private async Task<bool> ExecuteSecondaryPackaging()
        {
            // 执行包装方法的逻辑
            // 包装完成后返回true或false表示成功或失败
            // 这里可以调用你的包装方法，示例为异步方法

            // 将对象序列化为 JSON 字符串
            // 构建请求体
            List<string> _snList = primaryPackagingCodeList.Items.Cast<string>().ToList();

            var serialNumberList = JsonConvert.SerializeObject(_snList);

            // 将 JSON 字符串转换为 StringContent
            var content = new StringContent(serialNumberList, Encoding.UTF8, "application/json");

            // 创建一个 multipart/form-data 请求
            MultipartFormDataContent formData = new MultipartFormDataContent();
            foreach (var item in _snList)
            {
                formData.Add(new StringContent(item), "primaryPackagingCodeList");
            }
            formData.Add(new StringContent(_user.id.ToString()), "userId");

            // 发送 POST 请求
            var response = await _httpClient.PostAsync($"{BaseUrl}/ZiYangPackaging/ExecuteSecondaryPackaging", formData);

            // 检查响应是否成功
            if (response.IsSuccessStatusCode)
            {
                // 读取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();

                // 解析响应内容
                var result = JsonConvert.DeserializeObject<MessageModel<(string, int)>>(responseContent);

                if (result != null)
                {

                    if (result.success)
                    {
                        //执行包装打印
                        List<Dictionary<string, string>> ValuePairs = new List<Dictionary<string, string>>();

                        //标签打印
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
                            MessageBox.Show("标签打印失败");
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
            //执行包装打印
            List<Dictionary<string, string>> ValuePairs = new List<Dictionary<string, string>>();

            //标签打印
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
                MessageBox.Show("标签试打印失败");
            }
            else
            {
                MessageBox.Show("标签试打印完成");
            }
        }

        private async void rePrintLabel_Click(object sender, EventArgs e)
        {
            //根据输入包装号获取包装数量
            HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}/ZiYangPackaging/GetBindingSNCountByPackagingCode?packagingCode={rePrimaryPrintInput.Text.Trim()}");

            // 检查响应是否成功
            if (response.IsSuccessStatusCode)
            {
                // 读取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();

                // 解析响应内容
                var result = JsonConvert.DeserializeObject<MessageModel<int>>(responseContent);

                if (result != null)
                {

                    if (result.success)
                    {
                        //执行包装打印
                        List<Dictionary<string, string>> ValuePairs = new List<Dictionary<string, string>>();

                        //标签打印
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
                            MessageBox.Show("标签试打印失败");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("标签试打印完成");
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
                // 处理错误响应
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        private void printSecondaryLabelTest_Click(object sender, EventArgs e)
        {
            //执行包装打印
            List<Dictionary<string, string>> ValuePairs = new List<Dictionary<string, string>>();

            //标签打印
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
                MessageBox.Show("标签试打印失败");
            }
            else
            {
                MessageBox.Show("标签试打印完成");
            }
        }

        private async void reSecondaryPrintLabel_Click(object sender, EventArgs e)
        {
            //根据输入包装号获取包装数量
            HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}/ZiYangPackaging/GetBindingSNCountByPackagingCode?packagingCode={reSecondaryPrintInput.Text.Trim()}");

            // 检查响应是否成功
            if (response.IsSuccessStatusCode)
            {
                // 读取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();

                // 解析响应内容
                var result = JsonConvert.DeserializeObject<MessageModel<int>>(responseContent);

                if (result != null)
                {

                    if (result.success)
                    {
                        //执行包装打印
                        List<Dictionary<string, string>> ValuePairs = new List<Dictionary<string, string>>();

                        //标签打印
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
                            MessageBox.Show("标签试打印失败");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("标签试打印完成");
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
                // 处理错误响应
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
                // 假设 API 地址为 /api/export/excel
                var response = await _httpClient.GetAsync($"{BaseUrl}/BOSAautotestData/ExportToExcel?packagingCode={secondaryPackagingNumberExportInput.Text.Trim()}");
                response.EnsureSuccessStatusCode();

                // 将文件流保存到本地
                await using var fileStream = await response.Content.ReadAsStreamAsync();
                await using var fileStreamOutput = File.Create(filePath);
                await fileStream.CopyToAsync(fileStreamOutput);

                // 在保存文件后，使用默认关联的程序打开文件
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
        }

        private async void querySecondaryByPackagingNumber_Click(object sender, EventArgs e)
        {
            // 发送POST请求到Web API进行登录验证
            HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}/ZiYangPackaging/FetchData?searchType={ZiYangPackagingSearchTypeEnum.PackagingCode.ToString()}&query={secondaryPackagingCodeinput.Text.Trim()}");
            // 检查响应是否成功
            if (response.IsSuccessStatusCode)
            {
                // 读取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();

                // 解析响应内容
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
                // 处理错误响应
                throw new Exception($"Error: {response.StatusCode}");
            }
        }
    }
}
