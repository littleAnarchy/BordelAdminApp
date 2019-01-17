using WPFRelectionControls.Interfaces;

namespace DbController
{
    public partial class Whore : IIdentable
    {

        public override string ToString()
        {
            return FirstName + " " + LastName;
            
        }

        public void Update(Whore entity)
        {
            FirstName = entity.FirstName;
            LastName = entity.LastName;
            Age = entity.Age;
            PimpId = entity.Pimp?.Id ?? entity.PimpId;
            PricePerHour = entity.PricePerHour;
        }
    }

    public partial class Pimp : IIdentable
    {
        public override string ToString()
        {
            return Name;
        }

        public void Update(Pimp entity)
        {
            Name = entity.Name;
        }
    }

}
