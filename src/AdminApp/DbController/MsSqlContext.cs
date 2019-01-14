using System;
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
                if (whore.Pimp != null) whore.PimpId = whore.Pimp.Id;
                whore.Pimp = null; //TODO: create best resolve of this problem
                ctxt.Whores.Add(whore);
                ctxt.SaveChanges();
            }
        }

        //getting table by reflection
        public List<object> GetListValuesByType(Type type)
        {
            
            using (var ctxt = GetContext())
            {
                return ctxt.Database.SqlQuery(type, "SELECT * FROM " + type.BaseType?.Name + "s").ToListAsync().Result;
            }
        }
    }
}
