using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaApplication1.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";

    [ObservableProperty]
    private string textInput = string.Empty;

    [ObservableProperty]
    private string message = "Ready...";

    [RelayCommand]
    private void Press()
    {
        // Use TextInput directly in any algorithm or method
        var result = ProcessText(TextInput);
        Message = result;
    }

    // Example: Your algorithm/method that uses the text input
    private string ProcessText(string input)
    {
        // Example 1: Use it as a string parameter
        if (string.IsNullOrWhiteSpace(input))
        {
            return "Please enter some text!";
        }

        // Example 2: Convert to number if needed
        if (int.TryParse(input, out int number))
        {
            int doubled = number * 2;
            return $"Number {number} doubled is {doubled}";
        }

        // Example 3: String manipulation
        return $"You entered: {input.ToUpper()} (length: {input.Length})";
    }
}
