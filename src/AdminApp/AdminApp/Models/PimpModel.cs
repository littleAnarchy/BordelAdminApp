using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DbController;

namespace AdminApp.Models
{
    public class PimpModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Whore> Whores { get; set; }
        public string WhoreList { get; set; }

        public PimpModel(Pimp entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Whores = entity.Whores.ToList();

            foreach (var whore in Whores)
            {
                WhoreList += $"{whore.FirstName} {whore.LastName}\n";
            }
        }
    }
}
