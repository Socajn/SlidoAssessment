using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Reflection;
using System.Windows.Input;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Win32;
using SlidoCodingAssessment.Base;
using SlidoCodingAssessment.Helpers;

namespace SlidoCodingAssessment.ViewModels;

public class MainViewModel : ViewModelBase
{
    #region Commands

    private ICommand _downloadCommand;

    public ICommand DownloadCommand =>
        _downloadCommand ??= new CommandHandler(DownloadFileAndOpenFile, true);

    private ICommand _saveLogsCommand;

    public ICommand SaveLogsCommand =>
        _saveLogsCommand ??= new CommandHandler(GetLogs, true);

    #endregion

    #region Methods

    private async void DownloadFileAndOpenFile()
    {
        LoggerUtils.Instance.LogInfo(@"Downloading and opening presentation.");

        var fileName = "How to use Slido.pptx";
        if (File.Exists(fileName)) File.Delete(fileName);

        using (var client = new HttpClient())
        {
            try
            {
                var uri = new Uri("https://api.slido.com/global/api/powerpoint-addin/presentation");
                await client.DownloadFileTaskAsync(uri, fileName);
            }
            catch (Exception ex)
            {
                LoggerUtils.Instance.LogError(@"Failed to download the file.", ex.Message);
            }
        }

        LoggerUtils.Instance.LogInfo(@"File downloaded.");
        LoggerUtils.Instance.LogInfo(@"Opening presentation.");

        var strExeFilePath = Assembly.GetExecutingAssembly().Location;
        var strWorkPath = Path.GetDirectoryName(strExeFilePath);
        var fullFilePath = Path.Combine(strWorkPath, fileName);

        try
        {
            var ppApp = new Application();
            ppApp.Visible = MsoTriState.msoTrue;
            var ppPresens = ppApp.Presentations;
            var objPres = ppPresens.Open(fullFilePath, MsoTriState.msoFalse, MsoTriState.msoTrue);
        }
        catch (Exception ex)
        {
            LoggerUtils.Instance.LogError(@"Failed to open presentation.", ex.Message);
        }

        LoggerUtils.Instance.LogInfo(@"Presentation opened.");

        //If I want to run slideShow
        // Slides objSlides = objPres.Slides;
        // Microsoft.Office.Interop.PowerPoint.SlideShowSettings objSSS;
        // objSSS = objPres.SlideShowSettings;
        // objSSS.Run();

        //Other way to open PowerPoint but the exe path is not always in the same place
        // string powerPointPath = @"C:\Program Files\Microsoft Office\root\Office16\POWERPNT.EXE";
        // Process powerPoint = new Process();
        // powerPoint.StartInfo.FileName = powerPointPath;
        // powerPoint.StartInfo.Arguments = fullFilePath;
        // powerPoint.Start();
    }

    private void GetLogs()
    {
        LoggerUtils.Instance.LogInfo(@"Creating logs.");

        var keyPath = @"SOFTWARE\Microsoft\Office\PowerPoint\Addins";
        var regKey = Registry.CurrentUser.OpenSubKey(keyPath);
        var addins = regKey?.GetSubKeyNames();

        var fileName = @"log.txt";
        var strExeFilePath = Assembly.GetExecutingAssembly().Location;
        var strWorkPath = Path.GetDirectoryName(strExeFilePath);
        var fullFilePath = Path.Combine(strWorkPath, fileName);


        var fileContent = LoggerUtils.Instance.GetLogContent().ToList();
        if (addins != null) fileContent.AddRange(addins);
        File.AppendAllLines(fullFilePath, fileContent);

        var zipFullPath = Path.Combine(strWorkPath, "Log.zip");

        if (File.Exists(zipFullPath)) File.Delete(zipFullPath);
        using (var zip = ZipFile.Open(zipFullPath, ZipArchiveMode.Create))
        {
            zip.CreateEntryFromFile(fullFilePath, fileName);
        }

        if (File.Exists(fullFilePath)) File.Delete(fullFilePath);
    }

    #endregion
}