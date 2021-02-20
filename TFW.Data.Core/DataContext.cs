﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Entities;
using TFW.Cross.Models.Common;
using TFW.Data.Core.EntityConfigs;
using TFW.Framework.Cross.Models;
using TFW.Framework.DI;
using TFW.Framework.EFCore.Context;

namespace TFW.Data.Core
{
    [ProviderService(ServiceLifetime.Scoped, ServiceType = typeof(DbContext))]
    [ProviderService(ServiceLifetime.Scoped, ServiceType = typeof(IFullAuditableDbContext))]
    [ProviderService(ServiceLifetime.Scoped, ServiceType = typeof(IBaseDbContext))]
    public partial class DataContext : BaseIdentityDbContext<AppUser, AppRole, string, IdentityUserClaim<string>,
        AppUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public virtual DbSet<Note> Note { get; set; }
        public virtual DbSet<NoteCategory> NoteCategory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable(DataConsts.ConnStrVarName));
                //require Microsoft.EntityFrameworkCore.Proxies
                //.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AppUserEntityConfig());

            modelBuilder.ApplyConfiguration(new AppRoleEntityConfig());

            modelBuilder.ApplyConfiguration(new NoteEntityConfig());

            modelBuilder.ApplyConfiguration(new NoteCategoryEntityConfig());
        }

        public override void PrepareAdd(object entity)
        {
            base.PrepareAdd(entity);

            if (entity is IAppAuditableEntity == false) return;

            var auditableEntity = entity as IAppAuditableEntity;
            auditableEntity.CreatedUserId = PrincipalInfo.Current?.UserId;
        }

        public override void PrepareModify(object entity)
        {
            base.PrepareModify(entity);

            var isSoftDeleted = false;

            if (entity is IAppSoftDeleteEntity)
            {
                var softDeleteEntity = entity as IAppSoftDeleteEntity;

                if (softDeleteEntity.IsDeleted)
                {
                    softDeleteEntity.DeletedUserId = PrincipalInfo.Current?.UserId;
                    isSoftDeleted = true;
                }
            }

            if (isSoftDeleted || entity is IAppAuditableEntity == false) return;

            var auditableEntity = entity as IAppAuditableEntity;
            auditableEntity.LastModifiedUserId = PrincipalInfo.Current?.UserId;
        }
    }

}
