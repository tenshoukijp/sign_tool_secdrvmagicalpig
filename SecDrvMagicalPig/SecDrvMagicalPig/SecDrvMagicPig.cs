using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SecDrvMagicPig
{
    class SecDrvMagicPigForm : Form {
        byte[] datSecDrv;

        public SecDrvMagicPigForm()
        {
            // このプロジェクトのアセンブリのタイプを取得。
            System.Reflection.Assembly prj_assebmly = GetType().Assembly;
            System.Resources.ResourceManager r = new System.Resources.ResourceManager(String.Format("{0}.SecDrvMagicPigRes", prj_assebmly.GetName().Name), prj_assebmly);

            System.Drawing.Icon iconform = (System.Drawing.Icon)(r.GetObject("icon"));
            this.Icon = iconform;

            datSecDrv = (byte[])r.GetObject("dat");

            datSecDrv = UnCompressBytes(datSecDrv);

            WriteSecDrv();
        }

        private void WriteSecDrv() {
            try
            {
                // 
                String fullPathName = "";
                String[] dropfiles = System.Environment.GetCommandLineArgs();

                if (System.IO.File.Exists(".\\secdrv.sys")) {
                    fullPathName = ".\\secdrv.sys";
                } else if  ( dropfiles.Length > 1 && dropfiles[1].ToString().ToLower().EndsWith("secdrv.sys") ) {
                    fullPathName = dropfiles[1];
                }

                // 
                if ( fullPathName != "" ) {

                    // ファイルを作成して書き込む。ファイルが存在しているときは、上書きする
                    System.IO.FileStream fs = new System.IO.FileStream(
                        fullPathName,
                        System.IO.FileMode.Create,
                        System.IO.FileAccess.Write);

                    fs.Write(datSecDrv, 0, datSecDrv.Length);

                    fs.Close();

                    DateTime dt = new DateTime(2009, 9, 13, 5, 37, 0); // タイムスタンプ

                    //作成日時の設定
                    System.IO.File.SetCreationTime(fullPathName, dt);
                    //更新日時の設定
                    System.IO.File.SetLastWriteTime(fullPathName, dt);

                    System.Windows.Forms.MessageBox.Show("(>(●●)<)ノ♪ ", "成功", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("(T(●●)T)/~~~", "失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("(T(●●)T)/~~~", "失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //解凍を行う
        private byte[] UnCompressBytes(byte[] byteComp)
        {
            
            int num;
            byte[] buf = new byte[1024]; // 1Kbytesずつ処理する
            // 入力ストリーム
            System.IO.MemoryStream inStream = new System.IO.MemoryStream(byteComp);
            // 解凍ストリーム
            System.IO.Compression.GZipStream decompStream = new System.IO.Compression.GZipStream(inStream, System.IO.Compression.CompressionMode.Decompress);
            // 出力ストリーム
            System.IO.MemoryStream outStream = new System.IO.MemoryStream();

            while ((num = decompStream.Read(buf, 0, buf.Length)) > 0)
            {
                outStream.Write(buf, 0, num);
            }
            byte[] crypted_data = outStream.ToArray();

            Array.Reverse(crypted_data);

            return crypted_data;
        }
    }
}
