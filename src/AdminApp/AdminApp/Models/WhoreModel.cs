namespace AdminApp.Models
{
    public class WhoreModel : Whore, IEntityKeepable, IIdentable
    {
        public object Entity { get; set; }

        public WhoreModel(Whore entity)
        {
            Entity = entity;

            Id = entity.Id;
            FirstName = entity.FirstName;
            LastName = entity.LastName;
            Age = entity.Age;
            Pimp = entity.Pimp;
        }

        public WhoreModel()
        {
            Entity = this;
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
