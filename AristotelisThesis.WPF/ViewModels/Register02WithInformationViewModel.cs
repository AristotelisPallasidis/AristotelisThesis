using AristotelisThesis.WPF.Commands;
using AristotelisThesis.WPF.State;
using AristotelisThesis.WPF.State.Navigators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace AristotelisThesis.WPF.ViewModels
{
    public class Register02WithInformationViewModel : ViewModelBase
    {
        /// <summary>Shared registration data; the form binds its fields to this.</summary>
        public RegistrationStore Registration { get; }

        /// <summary>Selectable entry years for the dropdown: this year back to 16 years ago.</summary>
        public IReadOnlyList<string> Years { get; } =
            Enumerable.Range(0, 17).Select(i => (DateTime.Now.Year - i).ToString()).ToList();

        public ICommand GoToViewRegister01Command { get; } // Back
        public ICommand GoToViewRegister03InstructionsForPalmprintCommand { get; } // Next

        public Register02WithInformationViewModel(RegistrationStore registration, IRenavigator register01Renavigator, IRenavigator register03Renavigator)
        {
            Registration = registration;
            GoToViewRegister01Command = new RenavigateCommand(register01Renavigator);
            // Next is only enabled while every personal-info field is valid; WPF re-checks
            // CanExecute as the user types (RelayCommand hooks CommandManager.RequerySuggested).
            GoToViewRegister03InstructionsForPalmprintCommand = new RelayCommand(
                _ => register03Renavigator.Renavigate(),
                _ => Registration.IsPersonalInfoValid());
        }
    }
}
