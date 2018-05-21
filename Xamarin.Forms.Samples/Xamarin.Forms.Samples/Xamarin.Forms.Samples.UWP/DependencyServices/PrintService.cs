using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Pdf;
using Windows.Graphics.Printing;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Printing;
using Xamarin.Forms;
using Xamarin.Forms.Samples.DependencyServices;
using Xamarin.Forms.Samples.UWP.DependencyServices;
using UListView = Windows.UI.Xaml.Controls.ListView;
using UImage = Windows.UI.Xaml.Controls.Image;

[assembly: Dependency(typeof(PrintService))]

namespace Xamarin.Forms.Samples.UWP.DependencyServices
{
    public class PrintService : IPrintService
    {
        PrintDocument _printDocument;
        IPrintDocumentSource _printDocumentSource;

        UListView _imageList;

        public async Task PrintAsync(string file)
        {
            #region 印刷の準備

            _printDocument = new PrintDocument();
            _printDocumentSource = _printDocument.DocumentSource;
            _printDocument.Paginate += CreatePrintPreviewPages;
            _printDocument.GetPreviewPage += GetPreviewPage;
            _printDocument.AddPages += AddPages;

            var printManager = PrintManager.GetForCurrentView();
            printManager.PrintTaskRequested += PrintTaskRequested;

            #endregion

            try
            {
                #region PDF情報 取得

                var storage = ApplicationData.Current.LocalFolder;
                var path = await storage.GetFileAsync(file);
                var pdfDocument = await PdfDocument.LoadFromFileAsync(path);

                #endregion

                _imageList = new UListView();

                #region 印刷情報 取得

                try
                {
                    for (int i = 0; i < pdfDocument.PageCount; i++)
                    {
                        using (var page = pdfDocument.GetPage((uint)i))
                        {
                            var stream = new Windows.Storage.Streams.InMemoryRandomAccessStream();
                            await page.RenderToStreamAsync(stream);
                            // BitmapImage 作成
                            var src = new Windows.UI.Xaml.Media.Imaging.BitmapImage();

                            var image = new UImage();
                            image.Source = src;

                            // BitmapImageをsrcにセット
                            await src.SetSourceAsync(stream);

                            _imageList.Items.Add(image);
                        }
                    }
                }
                finally
                {
                    pdfDocument = null;
                }

                #endregion

                if (PrintManager.IsSupported())
                {
                    // 印刷ダイアログ表示
                    await PrintManager.ShowPrintUIAsync();
                }

            }
            finally
            {
                // プリンターとの接続切断
                printManager.PrintTaskRequested -= PrintTaskRequested;
            }
        }

        #region PrintTaskRequested

        /// <summary>
        /// 印刷ダイアログ表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs e)
        {
            var printTask = e.Request.CreatePrintTask("UwpPrintTask", req =>
            {
                req.SetSource(_printDocumentSource);
            });

            // 印刷完了時のイベント
            printTask.Completed += (s, args) =>
            {
                _printDocumentSource = null;
                _printDocument = null;
                _imageList = null;
            };
        }

        #endregion

        #region AddPages

        /// <summary>
        /// 印刷情報の設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPages(object sender, AddPagesEventArgs e)
        {
            foreach (UImage image in _imageList.Items)
            {
                _printDocument.AddPage(image);
            }

            // 印刷開始
            _printDocument.AddPagesComplete();
        }

        #endregion

        #region GetPreviewPage

        /// <summary>
        /// プレビュー作成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            var image = (UImage)_imageList.Items[e.PageNumber - 1];
            _printDocument.SetPreviewPage(e.PageNumber, image);
        }

        #endregion

        #region CreatePrintPreviewPages

        /// <summary>
        /// プレビュー ページ数を設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePrintPreviewPages(object sender, PaginateEventArgs e)
        {
            _printDocument.SetPreviewPageCount(_imageList.Items.Count, PreviewPageCountType.Intermediate);
        }

        #endregion

    }
}
