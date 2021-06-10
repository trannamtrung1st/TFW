﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstracts.Data
{
    public interface IDbContext
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}