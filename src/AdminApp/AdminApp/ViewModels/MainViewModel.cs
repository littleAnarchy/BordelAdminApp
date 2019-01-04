using System.Collections.ObjectModel;
using System.Threading;
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

        public MainViewModel()
        {
            Update();
        }

        public void Update()
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
    }
}
