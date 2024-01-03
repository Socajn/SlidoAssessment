using System.IO;
using System.Net.Http;
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
        var fileName = "How to use Slido.pptx";

        if (File.Exists(fileName)) File.Delete(fileName);

        using (var client = new HttpClient())
        {
            var uri = new Uri("https://api.slido.com/global/api/powerpoint-addin/presentation");

            await client.DownloadFileTaskAsync(uri, fileName);
        }

        string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        string? strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
        string fullFilePath = System.IO.Path.Combine(strWorkPath, fileName);
        
        var ppApp = new Application();
        ppApp.Visible = MsoTriState.msoTrue;
        Presentations ppPresens = ppApp.Presentations;
        Presentation objPres = ppPresens.Open(fullFilePath, MsoTriState.msoFalse, MsoTriState.msoTrue, MsoTriState.msoTrue);

        //If I want to run slideShow
        // Slides objSlides = objPres.Slides;
        // Microsoft.Office.Interop.PowerPoint.SlideShowSettings objSSS;
        // objSSS = objPres.SlideShowSettings;
        // objSSS.Run();
    }

    private void GetLogs()
    {
        //I don't think that HKEY_LOCAL_USER exists
        //string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Office\PowerPoint";
        var keyPath = @"SOFTWARE\Microsoft\Office\PowerPoint\Addins";
        var regKey = Registry.LocalMachine.OpenSubKey(keyPath);
        var addins = regKey.GetSubKeyNames();
    }
    
    #endregion
    
    
}