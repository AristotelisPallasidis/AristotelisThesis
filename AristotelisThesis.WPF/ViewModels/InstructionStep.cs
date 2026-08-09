namespace AristotelisThesis.WPF.ViewModels
{
    /// <summary>
    /// One numbered instruction on the registration guidance pages. Declared in the views'
    /// resources so the wording stays in the XAML next to the rest of the page text.
    /// </summary>
    public class InstructionStep
    {
        public int Number { get; set; }

        public string Text { get; set; } = string.Empty;
    }
}
