using System.Windows;
using SlidoCodingAssessment.Helpers;

namespace SlidoCodingAssessment;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        IconHelper.RemoveIcon(this);
        base.OnSourceInitialized(e);
    }
}