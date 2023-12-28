using SlidoCodingAssessment.Base;

namespace SlidoCodingAssessment.ViewModels;

public class MainViewModel : ViewModelBase
{
    private string _firstName;

    public MainViewModel()
    {
        FirstName = @"TestName";
    }

    public string FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value);
    }
}