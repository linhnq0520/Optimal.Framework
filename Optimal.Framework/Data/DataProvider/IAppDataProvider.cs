﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimal.Framework.Data.DataProvider
{
    public interface IAppDataProvider
    {
        IQueryable<TEntity> GetTable<TEntity>() where TEntity : BaseEntity;
    }
}