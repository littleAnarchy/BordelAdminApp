using System.Linq;
using DbController;
using WPFRelectionControls.Interfaces;

namespace AdminApp.Models
{
    public sealed class PimpModel : Pimp, IEntityKeepable
    {
        public object Entity { get; set; }
        public string WhoreList { get; set; }

        public PimpModel(Pimp entity)
        {
            Entity = entity;

            Id = entity.Id;
            Name = entity.Name;
            Whores = entity.Whores.ToList();

            foreach (var whore in Whores)
            {
                WhoreList += $"{whore.FirstName} {whore.LastName}\n";
            }
        }

        public PimpModel()
        {
            Entity = this;
        }
    }
}
