using Coursework_server.Data.Models;
using Coursework_server.Data.ViewModels;
using Coursework_server.Extensions;
using Coursework_server.Helpers;
using Coursework_server.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Data.Services
{
    public class CollectionService
    {
        private readonly AppDbContext _db;
        private readonly UserService _userService;
        
        public CollectionService(AppDbContext context, UserService userService)
        {
            _db = context;
            _userService = userService;
        }

        public void AddCollection(CollectionVm request)
        {
            _db.Collections.Add(new Collection
            {
                CoverUrl = request.CoverUrl ?? string.Empty,
                Title = request.Title,
                Description = request.Description,
                TopicId = request.TopicId,
                OwnerId = request.OwnerId,
                Fields = request.FieldVMs
                                .Select(f => new Field
                                {
                                    Name = f.Name,
                                    FieldTypeId = f.FieldTypeId
                                }).ToList()
            });

            _db.SaveChanges();
        }

        public bool CheckCollectionExistence(Guid id) => _db.Collections.Any(c => c.Id == id);

        public void UpdateCollection(Collection collection)
        {
            _db.Collections.Update(collection);

            _db.SaveChanges();
        }

        public void UpdateCollection(CollectionVm collectionVm)
        {
            var collection = _db.Collections.FirstOrDefault(c => c.Id == collectionVm.Id);

            if (collection != null)
            {
                collectionVm.CopyPropertiesTo(collection);

                collection.Fields = GetFieldsOrCreate(collectionVm.FieldVMs);

                _db.SaveChanges();
            }
        }

        public void RemoveCollection(Guid id)
        {
            var collection = GetCollectionById(id);

            if (collection != null)
            {
                _db.Collections.Remove(collection);
            }

            _db.SaveChanges();
        }

        public Collection? GetCollectionById(Guid id) => _db.Collections
            .Include(c => c.Topic)
            .Include(c => c.Fields)
            .FirstOrDefault(c => c.Id == id);

        public List<Field> GetCollectionFields(Guid id) => _db.Fields
            .Where(c => c.CollectionId == id)
            .Include(f => f.FieldType)
            .ToList();

        public List<Tag> GetCollectionTags(Guid collectionId) => _db.Items
            .Where(i => i.CollectionId == collectionId)
            .SelectMany(i => i.Tags)
            .Distinct()
            .ToList();

        public List<Collection> GetUserCollections(Guid userId) => _db.Collections
            .Where(c => c.OwnerId == userId)
            .Include(c => c.Topic)
            .ToList();

        public List<Collection> GetLargestCollections() => _db.Collections
            .OrderByDescending(c => c.Items.Count)
            .Include(c => c.Topic)
            .ToList();

        public List<FieldType> GetFieldTypes() => _db.FieldTypes.ToList();

        public List<Field> GetFieldsOrCreate(List<FieldVm> fieldVMs)
        {
            var fields = new List<Field>();

            foreach (var fieldVm in fieldVMs)
            {
                var field = _db.Fields.FirstOrDefault(t => t.Name == fieldVm.Name) ?? new Field
                {
                    Name = fieldVm.Name,
                    FieldTypeId = fieldVm.FieldTypeId,
                };

                fields.Add(field);
            }

            return fields;
        }

        public void CreateCollection(CollectionVm collectionVm, string? currentUserId)
        {
            if (currentUserId == null)
            {
                throw new UnauthorizedAccessException();
            }
                
            var currentUser = _userService.GetUserById(currentUserId);

            if (collectionVm.OwnerId == currentUser?.Id || currentUser?.UserRole == UserRole.Admin)
            {
                AddCollection(collectionVm);
            }
        }

        public CollectionVm GetCollectionVm(Guid id)
        {
            var collection = GetCollectionById(id);

            if (collection == null)
            {
                throw new InvalidOperationException();
            }

            return ConvertHelper.ToCollectionVm(collection);
        }

        public ShortCollectionVm GetShortCollectionVm(Guid id)
        {
            var collection = GetCollectionById(id);

            if (collection == null)
            {
                throw new InvalidOperationException();
            }

            return ConvertHelper.ToShortCollectionVm(collection);
        }

        public List<ShortCollectionVm> GetUserShortCollectionVMs(Guid userId)
        {
            var user = _userService.GetUserById(userId);

            if (user == null)
            {
                throw new InvalidOperationException();
            }

            var collections = GetUserCollections(userId);
            var shortCollectionVMs = collections.Select(ConvertHelper.ToShortCollectionVm).ToList();

            return shortCollectionVMs;
        }

        public List<ShortCollectionVm> GetLargestShortCollectionVMs()
        {
            var collections = GetLargestCollections();
            var shortCollectionVMs = collections.Select(ConvertHelper.ToShortCollectionVm).ToList();

            return shortCollectionVMs;
        }

        public void EditCollection(CollectionVm collectionVm, string? currentUserId)
        {
            if (currentUserId == null)
            {
                throw new UnauthorizedAccessException();
            }

            var currentUser = _userService.GetUserById(currentUserId);

            if (collectionVm.OwnerId != currentUser?.Id || currentUser?.UserRole != UserRole.Admin)
            {
                throw new InvalidOperationException();
            }

            UpdateCollection(collectionVm);
        }

        public List<FieldWithTypeNameVm> GetCollectionFieldsWithTypeNames(Guid id)
        {
            var result = CheckCollectionExistence(id);

            if (result == false)
            {
                throw new InvalidOperationException();
            }

            var fields = GetCollectionFields(id);
            var fieldWithTypeNameVMs = fields.Select(ConvertHelper.ToFieldWithTypeNameVm).ToList();

            return fieldWithTypeNameVMs;
        }

        public List<TagVm> GetCollectionTagVMs(Guid id)
        {
            var result = CheckCollectionExistence(id);

            if (result == false)
            {
                throw new InvalidOperationException();
            }

            var tags = GetCollectionTags(id);
            var tagVMs = tags.Select(ConvertHelper.ToTagVm).ToList();

            return tagVMs;
        }

        public List<FieldTypeVm> GetFieldTypeVMs()
        {
            var fieldTypes = GetFieldTypes();
            var fieldTypeVMs = fieldTypes.Select(ConvertHelper.ToFieldTypeVm).ToList();

            return fieldTypeVMs;
        }
    }
}
