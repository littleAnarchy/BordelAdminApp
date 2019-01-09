using System.Collections.ObjectModel;
using System.Threading;
using AdminApp.Common;
using AdminApp.Controllers;
using AdminApp.Models;
using DbController;

namespace AdminApp.ViewModels
{
    public class MainViewModel : BasicViewModel
    {
        public ObservableCollection<Whore> Whores { get; set; } = new ObservableCollection<Whore>();
        public ObservableCollection<PimpModel> Pimps { get; set; } = new ObservableCollection<PimpModel>();

        private readonly SynchronizationContext _uiContext = SynchronizationContext.Current;
        private readonly MsSqlContext _dbContext = new MsSqlContext();

        #region Commands

        public CommandHandler OnAddWhoreBtnCmd { get; set; }

        #endregion

        public MainViewModel()
        {
            OnAddWhoreBtnCmd = new CommandHandler(OnAddWhoreBtn, true);

            UpdateView();
        }

        public void UpdateView()
        {
            _uiContext.Send(state => {
                Whores.Clear();
                foreach (var whore in _dbContext.GetWhores())
                {
                    Whores.Add(whore);
                }

                foreach (var pimp in _dbContext.GetPimps())
                {
                    Pimps.Add(new PimpModel(pimp));
                }
            }, null);
        }

        public void OnAddWhoreBtn()
        {
            var wnd = WindowsCreator.CreateWhoesAddingWindow();

            if (wnd.ShowDialog() == true)
            {
                _dbContext.AddWhore((Whore)wnd.Entity);
                UpdateView();
            }
        }
    }
}
