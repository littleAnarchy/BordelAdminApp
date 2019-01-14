using System.Collections.ObjectModel;
using System.Threading;
using AdminApp.Common;
using AdminApp.Controllers;
using AdminApp.Models;
using AdminApp.Windows;
using DbController;

namespace AdminApp.ViewModels
{
    public class MainViewModel : BasicViewModel
    {
        public ObservableCollection<WhoreModel> Whores { get; set; } = new ObservableCollection<WhoreModel>();
        public ObservableCollection<PimpModel> Pimps { get; set; } = new ObservableCollection<PimpModel>();

        public object SelectedObj { get; set; }

        private readonly SynchronizationContext _uiContext = SynchronizationContext.Current;
        private readonly MsSqlContext _dbContext = new MsSqlContext();
        private readonly WindowsCreator _windowsCreator;

        #region Commands

        public CommandHandler OnAddWhoreBtnCmd { get; set; }


        //ContextMenu
        public CommandHandler OnChangeItemCmd { get; set; }
        #endregion

        public MainViewModel()
        {
            _windowsCreator = new WindowsCreator(_dbContext);

            OnAddWhoreBtnCmd = new CommandHandler(OnAddWhoreBtn, true);
            OnChangeItemCmd = new CommandHandler(OnChangeItem, true);

            UpdateView();
        }

        public void UpdateView()
        {
            _uiContext.Send(state => {
                Whores.Clear();
                foreach (var whore in _dbContext.GetWhores())
                {
                    Whores.Add(new WhoreModel(whore));
                }

                foreach (var pimp in _dbContext.GetPimps())
                {
                    Pimps.Add(new PimpModel(pimp));
                }
            }, null);
        }

        public void OnAddWhoreBtn()
        {
            var wnd = _windowsCreator.CreateAddingWindow<Whore>();

            if (wnd.ShowDialog() == true)
            {
                _dbContext.AddWhore((Whore)wnd.Entity);
                UpdateView();
            }
        }

        //Todo: refactor
        public void OnChangeItem()
        {
            BasicWindow wnd;

            switch (SelectedObj)
            {
                case Whore _:
                    wnd = _windowsCreator.CreateChangingWindow<Whore>(SelectedObj);
                    break;
                case Pimp _:
                    wnd = _windowsCreator.CreateChangingWindow<Pimp>(SelectedObj);
                    break;
                default:
                    return;
            }

            if (wnd.ShowDialog() == true)
            {
                _dbContext.AddWhore((Whore)wnd.Entity);
                UpdateView();
            }
        }
    }
}
