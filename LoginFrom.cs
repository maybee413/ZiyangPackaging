using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using opway.mes.Model;
using opway.mes.Model.Models;
using opway.mes.Model.ViewModels;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZiyangPackagingSoftware
{
    public partial class LoginForm : Form
    {
        private readonly HttpClient _httpClient;
        private Users _user = new Users();
        // 设置一个公共属性来允许外部访问用户信息
        public Users LoggedInUser
        {
            get { return _user; }
        }
        //private const string BaseUrl = "http://192.168.51.56:2345/api"; // 替换为你的API基础URL
        private const string BaseUrl = "http://localhost:9291/api"; // 替换为你的API基础URL
        public LoginForm(HttpClient httpClient)
        {
            InitializeComponent();
            this._httpClient = httpClient;
        }

        public Users GetUser() => _user; // 返回保存的用户信息

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            await Login();
        }

        private async void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                await Login();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private async Task Login()
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // 检查用户名和密码是否为空
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("用户名和密码不能为空！");
                return;
            }

            // 创建登录请求的数据
            var loginData = new
            {
                Username = username,
                Password = password
            };

            // 将登录数据转换为JSON字符串
            string jsonLoginData = Newtonsoft.Json.JsonConvert.SerializeObject(loginData);

            // 发送POST请求到Web API进行登录验证
            HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}/Login/JWTToken3.0?name={username}&pass={password}");

            if (response.IsSuccessStatusCode)
            {
                // 登录成功
                string responseData = await response.Content.ReadAsStringAsync();

                JsonSerializerSettings setting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                var model = JsonConvert.DeserializeObject<MessageModel<TokenInfoViewModel>>(responseData, setting);


                if (model != null && model.success)
                {

                    // 根据用户名获取用户信息
                    HttpResponseMessage userResponse = await _httpClient.GetAsync($"{BaseUrl}/Users/GetUserByUserName?userName={username}");
                    if (userResponse.IsSuccessStatusCode)
                    {
                        // 获取用户信息成功
                        string userJson = await userResponse.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<MessageModel<Users>>(userJson, setting);
                        if (data != null && data.success)
                        {
                            _user = data.response;
                            this.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            MessageBox.Show("获取用户信息失败！");
                        }
                    }
                    else
                    {
                        // 获取用户信息失败
                        MessageBox.Show("获取用户信息失败！");
                    }
                }
                else if (model == null)
                {
                    MessageBox.Show("登陆失败！");
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.msg))
                    {
                        MessageBox.Show(model.msg);
                    }
                    else
                    {
                        MessageBox.Show("登陆失败！");
                    }
                }
            }
            else
            {
                // 登录失败
                MessageBox.Show("用户名或密码错误，请重试！");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 取消按钮的点击事件，关闭登录窗口
            this.Close();
        }
    }
}
