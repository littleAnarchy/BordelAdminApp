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

        #region GettingMethods

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

        #endregion

        #region AddingMethods

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

        #endregion

        #region ChangingMethods
        public void ChangeWhore(Whore whore)
        {
            using (var ctxt = GetContext())
            {
                var oldItem = ctxt.Whores.FirstOrDefault(x => x.Id == whore.Id);
                oldItem?.Update(whore);
                ctxt.SaveChanges();
            }
        }

        public void ChangePimp(Pimp pimp)
        {
            using (var ctxt = GetContext())
            {
                var oldItem = ctxt.Pimps.FirstOrDefault(x => x.Id == pimp.Id);
                oldItem?.Update(pimp);
                ctxt.SaveChanges();
            }
        }

        #endregion


        public Action<object> GetSavingMethodByType(Type type)
        {
            if (type == typeof(Whore)) return delegate(object o) { ChangeWhore((Whore) o); };
            if (type == typeof(Pimp)) return delegate (object o) { ChangePimp((Pimp) o); };
            return null;
        }

        //getting table by reflection
        public List<object> GetListValuesByType(Type type)
        {
            using (var ctxt = GetContext())
            {
                return ctxt.Database.SqlQuery(type, "SELECT * FROM " + type.Name + "s").ToListAsync().Result;
            }
        }
    }
}
