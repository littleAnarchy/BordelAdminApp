using System;
using System.Windows;
using System.Windows.Input;
using WPFRelectionControls.Common;

namespace WPFRelectionControls.Reflection
{
    public class BasicReflectionViewModel : BasicViewModel
    {
        protected Window Owner;
        protected readonly Type EntityType;

        public object Entity { get; protected set; }

        public ICommand CancelBtnCmd { get; set; }
        public ICommand Btn2Cmd { get; set; }

        protected BasicReflectionViewModel(Type type, object entity)
        {
            EntityType = type;
            Entity = entity;
            CancelBtnCmd = new CommandHandler(OnCancelClick, true);
        }

        //for xaml identity
        public BasicReflectionViewModel()
        {
            //throw new Exception("This constructor only for XAML identity!");
        }

        /// <summary>
        /// Init of owner window
        /// </summary>
        /// <param name="owner"></param>
        public void Initialize(Window owner)
        {
            Owner = owner;
        }

        private void OnCancelClick()
        {
            Owner.DialogResult = false;
            Owner.Close();
        }
    }
}
