using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace SlidoCodingAssessment;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetForegroundWindow(IntPtr hWnd);
    
    private static Mutex _mutex = null;

    protected override void OnStartup(StartupEventArgs e)
    {
        const string appName = "AcmeApp";
        bool createdNew;

        _mutex = new Mutex(true, appName, out createdNew);

        if (!createdNew)
        {
            //app is already running! Exiting the application
            Process current = Process.GetCurrentProcess();
            foreach (Process process in Process.GetProcessesByName(current.ProcessName))
            {
                if (process.Id != current.Id)
                {
                    SetForegroundWindow(process.MainWindowHandle);
                    Application.Current.Shutdown();
                    break;
                }
            }
            
        }
        
        
        
        base.OnStartup(e);
        

    }
}

