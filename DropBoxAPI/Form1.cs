using Dropbox.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DropBoxAPI
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 訪問所有資料的DropBox Token
        /// </summary>
        private string AccessToken = "Token";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //呼叫DropBox api
            var task = Task.Run((Func<Task>)ListRootFolder);
            task.Wait();
            //呼叫DropBox api 取得檔案下載連結
            var taskLink = Task.Run(() => Download("/image/docker.jpg"));
            taskLink.Wait();
        }

        /// <summary>
        /// 取得DropBox API 資料夾的Method
        /// </summary>
        /// <returns></returns>
        async Task ListRootFolder()
        {
            using (var dbx = new DropboxClient(this.AccessToken))
            {
                #region //位置 => 根目錄
                //根目錄的folder(資料夾)
                var list = await dbx.Files.ListFolderAsync(string.Empty, recursive: false);// Result;
                foreach (var item in list.Entries.Where(i => i.IsFolder))
                {
                    Console.WriteLine("D  {0}/", item.Name);
                }
                //位置 => 根目錄
                //根目錄的所有檔案
                foreach (var item in list.Entries.Where(i => i.IsFile))
                {
                    Console.WriteLine("F{0,8} {1}", item.AsFile.Size, item.Name);
                }
                #endregion

                #region //位置 => 根目錄/image
                // 根目錄/image 的所有檔案
                var list2 = await dbx.Files.ListFolderAsync("/image/");
                foreach (var item in list2.Entries.Where(i => i.IsFile))
                {
                    Console.WriteLine("F{0,8} {1}", item.AsFile.Size, item.Name);
                }
                #endregion



            }
        }

        /// <summary>
        /// 呼叫DorpBox Image資料夾底下的圖片
        /// </summary>
        /// <returns></returns>
        async Task Download(string path)
        {
            using (var dbx = new DropboxClient(this.AccessToken))
            {
                try
                {
                    var result = await dbx.Files.GetTemporaryLinkAsync(path);
                    Console.Write(string.Format("download link : {0}", result.Link));
                }
                catch (Exception ex)
                {

                }

            }
        }
    }
}
