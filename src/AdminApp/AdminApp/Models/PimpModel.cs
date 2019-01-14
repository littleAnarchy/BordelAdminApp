using System.Linq;

namespace AdminApp.Models
{
    public sealed class PimpModel : Pimp, IEntityKeepable, IIdentable
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

        public override string ToString()
        {
            return Name;
        }
    }
}
