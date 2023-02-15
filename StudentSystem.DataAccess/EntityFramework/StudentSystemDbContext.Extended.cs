using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SystemTech.Core.Entities;
using SystemTech.Core.Extensions;

namespace StudentSystem.DataAccess.EntityFramework
{
    public partial class StudentSystemDbContext
    {
        private const string PrimaryKeyName = "Id";
        private const string CreatedDateName = "CreatedDate";
        private const string UpdatedDateName = "UpdatedDate";
        public override int SaveChanges()
        {
            OnBeforeSaveChanges();
            return base.SaveChanges();
        }
        
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            OnBeforeSaveChanges();
            if (cancellationToken.IsCancellationRequested)
                return null;
            return base.SaveChangesAsync(cancellationToken);
        }
        
        #region Insert
        
        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            PopulateGuidField(entity);
            
            return base.Add(entity);
        }
        
        public override EntityEntry Add(object entity)
        {
            PopulateGuidField(entity);
            return base.Add(entity);
        }
        
        public override ValueTask<EntityEntry> AddAsync(object entity,
            CancellationToken cancellationToken = new CancellationToken())
        {
            PopulateGuidField(entity);
            return base.AddAsync(entity, cancellationToken);
        }
        
        public override ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity,
            CancellationToken cancellationToken = new CancellationToken())
        {
            PopulateGuidField(entity);
            return base.AddAsync(entity, cancellationToken);
        }
        
        public override void AddRange(IEnumerable<object> entities)
        {
            var enumerable = entities.ToList();
            foreach (object entity in enumerable)
            {
                PopulateGuidField(entity);
            }
        
            base.AddRange(enumerable);
        }
        
        public override void AddRange(params object[] entities)
        {
            foreach (object entity in entities)
            {
                PopulateGuidField(entity);
            }
            base.AddRange(entities);
        }
        
        public override Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = new CancellationToken())
        {
            var enumerable = entities.ToList();
            foreach (object entity in enumerable)
            {
                PopulateGuidField(entity);
            }
            return base.AddRangeAsync(enumerable, cancellationToken);
        }
        
        public override Task AddRangeAsync(params object[] entities)
        {
            foreach (object entity in entities)
            {
                PopulateGuidField(entity);
            }
            return base.AddRangeAsync(entities);
        }
        
        #endregion
        
        #region Modify
        
        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            return base.Update(entity);
        }
        
        public override EntityEntry Update(object entity)
        {
            return base.Update(entity);
        }
        
        public List<T> UpdateRange<T>(List<T> entities) where T : class
        {
            if (entities.Count > 0)
                base.UpdateRange(entities);

            return entities;
        }
        
        public T[] UpdateRange<T>(params T[] entities) where T : class
        {
            var result = entities.ToList();
            if (result.Count > 0)
            {
                base.UpdateRange(result);
            }

            return result.ToArray();
        }
        
        #endregion
        
        #region Remove
        
        public override EntityEntry<TEntity> Remove<TEntity>(TEntity entityDb)
        {
            EntityEntry<TEntity> result;

            if (entityDb is ISoftDeletedFields softDeletedEntity)
            {
                softDeletedEntity.IsDeleted = true;
                result = base.Update(entityDb);
            }
            else
            {
                result = base.Remove(entityDb);
            }
            
            return result;
        }
        
        public override EntityEntry Remove(object entityDb)
        {
            EntityEntry result;

            if (entityDb is ISoftDeletedFields softDeletedEntity)
            {
                softDeletedEntity.IsDeleted = true;
                result = base.Update(entityDb);
            }
            else
            {
                result = base.Remove(entityDb);
            }
            
            return result;
        }
        
        public override void RemoveRange(params object[] entities)
        {
            var enumerable = entities.ToList();
            bool isSoftDeletedEntity = enumerable.First() is ISoftDeletedFields;
            foreach (object entity in enumerable)
            {
                if (entity is ISoftDeletedFields softDeletedEntity)
                {
                    softDeletedEntity.IsDeleted = true;
                }
            }
        
            if (isSoftDeletedEntity)
            {
                base.UpdateRange(entities);
            }
            else
            {
                base.RemoveRange(entities);
            }
        }
        
        public override void RemoveRange(IEnumerable<object> entities)
        {
            var objects = entities.ToList();
            bool isSoftDeletedEntity = objects.First() is ISoftDeletedFields;
            foreach (object entity in objects)
            {
                if (entity is ISoftDeletedFields softDeletedEntity)
                {
                    softDeletedEntity.IsDeleted = true;
                }
            }
        
            if (isSoftDeletedEntity)
            {
                base.UpdateRange(objects);
            }
            else
            {
                base.RemoveRange(objects);
            }
        }

        #endregion

        #region Delete
        public void DeleteRange(IEnumerable<object> entities)
        {
            var objects = entities.ToList();
            base.RemoveRange(objects);
        }

        public void Delete(object entity)
        {
            base.Remove(entity);
        }

        #endregion

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.SetQueryFilterOnAllEntities<ISoftDeletedFields>(p => !p.IsDeleted);
        }
        
        private void OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            
            var modifiedEntries = ChangeTracker.Entries();
            
            foreach (var entry in modifiedEntries)
            {
                PopulateNoneAuditFields(entry);
            }
        }
        
        private void PopulateGuidField(object entry)
        {
            PropertyInfo primaryKey = entry.GetType().GetProperties().FirstOrDefault(_ => _.Name == PrimaryKeyName);
            if (primaryKey == null)
                return;
            if (primaryKey.PropertyType == typeof(int))
                return;
            if (primaryKey.PropertyType == typeof(long))
                return;
            
            string guid = Guid.NewGuid().ToString();
            primaryKey.SetValue(entry, guid, null);
        }
        
        private void PopulateAuditFields(EntityEntry entry)
        {
            var now = DateTime.Now;
            if (entry.Entity is IAuditFields entityEntry)
            {
                if (entry.State == EntityState.Added)
                {
                    entityEntry.CreatedDate = now;
                    entityEntry.UpdatedDate = now;
                    entityEntry.CreatedBy = entityEntry.UpdatedBy;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entityEntry.UpdatedDate = now;
                }
            }
        }

        private void PopulateNoneAuditFields(EntityEntry entry)
        {
            var entity = entry.Entity;

            if (entry.Entity is IAuditFields)
            {
                PopulateAuditFields(entry);
                return;
            }
            
            var now = DateTime.Now;
            PropertyInfo createdDateProperty = entity.GetType().GetProperties().FirstOrDefault(_ => _.Name == CreatedDateName);
            PropertyInfo updatedDateProperty = entity.GetType().GetProperties().FirstOrDefault(_ => _.Name == UpdatedDateName);
                
            if (entry.State == EntityState.Added)
            {
                if (createdDateProperty != null)
                    createdDateProperty.SetValue(entity, now, null);
                if (updatedDateProperty != null)
                    updatedDateProperty.SetValue(entity, now, null);
            }
            else if (entry.State == EntityState.Modified)
            {
                if (createdDateProperty != null)
                    createdDateProperty.SetValue(entity, now, null);
            }
        }
    }
    
    
    public enum DeleteTypeEnum
    {
        False = 0,
        True = 1
    }
}