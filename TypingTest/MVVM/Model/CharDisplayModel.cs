using System.ComponentModel;

namespace TypingTest.MVVM.Model;

public class CharDisplayModel:INotifyPropertyChanged
{
    public char CharValue { get; set; }
    private string _underlineColor = "#444444"; // Темно-серый по умолчанию
    public string UnderlineColor 
    { 
        get => _underlineColor; 
        set { _underlineColor = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnderlineColor))); } 
    }
    public event PropertyChangedEventHandler PropertyChanged;
}