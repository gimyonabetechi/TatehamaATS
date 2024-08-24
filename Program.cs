namespace TatehamaATS
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            // アプリケーション終了時に実行するイベントハンドラーを登録
            Application.ApplicationExit += new EventHandler(OnApplicationExit);

            Application.Run(new MainWindow());
        }
        /// <summary>
        /// アプリケーション終了時に実行されるメソッド。
        /// </summary>
        static void OnApplicationExit(object sender, EventArgs e)
        {
            MainWindow.transfer.SendDeleteAsync();
        }
    }
}