using Microsoft.VisualBasic.ApplicationServices;
using opway.mes.Model.Models;

namespace ZiyangPackagingSoftware
{
    internal static class Program
    {

        private static readonly HttpClient httpClient = new HttpClient();

        public static HttpClient HttpClient => httpClient;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 创建 HttpClient 实例
            HttpClient httpClient = new HttpClient();

            // 创建登录窗口并显示
            LoginForm loginForm = new LoginForm(httpClient);
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                Users loggedInUser = loginForm.LoggedInUser; // 假设 LoginForm 中有获取用户信息的方法

                // 创建主窗口并传入登录用户信息和 HttpClient 实例
                MainForm mainForm = new MainForm(loggedInUser, httpClient);
                Application.Run(mainForm);
            }
            else
            {
                // 用户取消登录或者登录失败，退出应用程序
                Application.Exit();
            }
        }

    }
}