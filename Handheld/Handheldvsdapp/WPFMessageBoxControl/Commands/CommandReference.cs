using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPFMessageBoxControl.Commands
{
    public class CommandReference : Freezable, ICommand
    {
        public event EventHandler CanExecuteChanged;

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register
        (
            "Command",
            typeof(ICommand),
            typeof(CommandReference),
            new PropertyMetadata(new PropertyChangedCallback((x, y) =>
            {
                var commandReference = x as CommandReference;
                var oldCommand = y.OldValue as ICommand;
                var newCommand = y.NewValue as ICommand;

                if (oldCommand != null) oldCommand.CanExecuteChanged -= commandReference.CanExecuteChanged;
                if (newCommand != null) newCommand.CanExecuteChanged += commandReference.CanExecuteChanged;
            }))
         );

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public bool CanExecute(object parameter)
        {
            if (Command != null) return Command.CanExecute(parameter);
            return false;
        }

        public void Execute(object parameter)
        {
            Command.Execute(parameter);
        }

        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        }
    }
}
