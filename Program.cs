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

            // ���� HttpClient ʵ��
            HttpClient httpClient = new HttpClient();

            // ������¼���ڲ���ʾ
            LoginForm loginForm = new LoginForm(httpClient);
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                Users loggedInUser = loginForm.LoggedInUser; // ���� LoginForm ���л�ȡ�û���Ϣ�ķ���

                // ���������ڲ������¼�û���Ϣ�� HttpClient ʵ��
                MainForm mainForm = new MainForm(loggedInUser, httpClient);
                Application.Run(mainForm);
            }
            else
            {
                // �û�ȡ����¼���ߵ�¼ʧ�ܣ��˳�Ӧ�ó���
                Application.Exit();
            }
        }

    }
}