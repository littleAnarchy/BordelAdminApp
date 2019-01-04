using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DbController
{
    public class MsSqlContext
    {
        public BordelEntities GetContext()
        {
            return new BordelEntities();
        }

        public List<Whore> GetWhores()
        {
            using (var ctxt = GetContext())
            {
                return ctxt.Whores.Include(w => w.Pimp).ToList();
            }
        }

        public List<Pimp> GetPimps()
        {
            using (var ctxt = GetContext())
            {
                return ctxt.Pimps.Include(p => p.Whores).ToList();
            }
        }

        public void AddWhore(Whore whore)
        {
            using (var ctxt = GetContext())
            {
                ctxt.Whores.Add(whore);
                ctxt.SaveChanges();
            }
        } 
    }
}
