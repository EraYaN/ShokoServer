﻿using System.Collections.Generic;
using JMMServer.Entities;
using NHibernate.Criterion;

namespace JMMServer.Repositories
{
    public class RenameScriptRepository : BaseDirectRepository<RenameScript,int>
    {

        public RenameScript GetDefaultScript()
        {
            using (var session = JMMService.SessionFactory.OpenSession())
            {
                RenameScript cr = session
                    .CreateCriteria(typeof(RenameScript))
                    .Add(Restrictions.Eq("IsEnabledOnImport", 1))
                    .UniqueResult<RenameScript>();
                return cr;
            }
        }

    }
}