using System;
using System.Windows.Forms;

namespace SecDrvMagicPig
{
    class Program {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            new SecDrvMagicPigForm();
       }
    }
}
