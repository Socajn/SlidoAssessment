using System.Windows.Input;

namespace SlidoCodingAssessment.Base;

public class CommandHandler(Action action, bool canExecute) : ICommand
{
    public bool CanExecute(object? parameter)
    {
        return canExecute;
    }

    public void Execute(object? parameter)
    {
        action();
    }

    public event EventHandler? CanExecuteChanged;
}