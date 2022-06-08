using Coursework_server.Data.Models;
using Coursework_server.Data.ViewModels;
using Coursework_server.Extensions;
using Coursework_server.Helpers;
using Coursework_server.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Data.Services
{
    public class ItemService
    {
        private readonly AppDbContext _db;
        private readonly TagService _tagService;
        private readonly UserService _userService;
        private readonly CollectionService _collectionService;

        public ItemService(
            AppDbContext context, 
            TagService tagService, 
            UserService userService, 
            CollectionService collectionService)
        {
            _db = context;
            _tagService = tagService;
            _userService = userService;
            _collectionService = collectionService;
        }

        public void AddItem(ShortItemVm request)
        {
            var item = new Item
            {
                Title = request.Title,
                CoverUrl = request.CoverUrl,
                CollectionId = request.CollectionId,
                CreationDate = DateTime.UtcNow,
            };

            _db.Items.Add(item);

            item.Tags = _tagService.GetTagsOrCreate(request.TagNames);
            item.ItemFields = request.FullFieldVMs.Select(f => new FieldItem
            {
                FieldId = f.Id,
                Value = f.Value
            }).ToList();

            _db.SaveChanges();
        }

        public void UpdateItem(Item item)
        {
            _db.Items.Update(item);

            _db.SaveChanges();
        }

        public void UpdateItem(ShortItemVm itemVm)
        {
            var item = _db.Items
                .Include(i => i.ItemFields)
                .Include(i => i.Tags)
                .FirstOrDefault(c => c.Id == itemVm.Id);

            if (item == null)
            {
                throw new InvalidOperationException();
            }

            itemVm.CopyPropertiesTo(item);

            item.ItemFields.Clear();
            item.ItemFields = itemVm.FullFieldVMs.Select(f => new FieldItem
            {
                FieldId = f.Id,
                Value = f.Value
            }).ToList();

            item.Tags.Clear();
            item.Tags = _tagService.GetTagsOrCreate(itemVm.TagNames);

            _db.SaveChanges();
        }

        public void RemoveItem(Guid id)
        {
            var item = GetItemById(id);

            if (item != null)
            {
                _db.Items.Remove(item);
            }

            _db.SaveChanges();
        }

        public void RemoveItem(Item item)
        {
            _db.Items.Remove(item);

            _db.SaveChanges();
        }

        public List<Item> GetItems(Guid collectionId) => _db.Items
                .Where(i => i.CollectionId == collectionId)
                .Include(i => i.ItemFields)
                .ThenInclude(i => i.Field)
                .ThenInclude(i => i.FieldType)
                .Include(i => i.ItemUsers)
                .Include(i => i.Tags)
                .ToList();

        public List<Item> GetItemsByTag(string tagName) => _db.Tags
                .Where(t => t.Name == tagName)
                .SelectMany(t => t.Items)
                .Include(i => i.ItemFields)
                .ThenInclude(i => i.Field)
                .ThenInclude(i => i.FieldType)
                .Include(i => i.ItemUsers)
                .Include(i => i.Tags)
                .ToList();

        public List<Item> GetLastAddedItems() => _db.Items
                .OrderByDescending(i => i.CreationDate)
                .Include(i => i.ItemFields)
                .ThenInclude(i => i.Field)
                .ThenInclude(i => i.FieldType)
                .Include(i => i.ItemUsers)
                .Include(i => i.Tags)
                .ToList();

        public Item? GetItemById(Guid id) => _db.Items
                .Include(i => i.ItemFields)
                .ThenInclude(i => i.Field)
                .ThenInclude(i => i.FieldType)
                .Include(i => i.ItemUsers)
                .Include(i => i.Tags)
                .Include(i => i.Collection)
                .FirstOrDefault(i => i.Id == id);

        public void CreateItem(ShortItemVm request, string? currentUserId)
        {
            if (currentUserId == null)
            {
                throw new UnauthorizedAccessException();
            }

            var currentUser = _userService.GetUserById(currentUserId);
            var collection = _collectionService.GetCollectionById(request.CollectionId);

            if (collection?.OwnerId != currentUser?.Id || currentUser?.UserRole != UserRole.Admin)
            {
                throw new InvalidOperationException();
            }

            AddItem(request);
        }

        public List<ItemVm> GetItemVMs(Guid collectionId, string? currentUserId)
        {
            var items = GetItems(collectionId);
            var itemVMs = items.Select(i => ConvertHelper.ToItemVm(i, currentUserId)).ToList();

            return itemVMs;
        }

        public List<ShortItemVm> GetShortItemVMs(Guid collectionId)
        {
            var items = GetItems(collectionId);
            var itemVMs = items.Select(ConvertHelper.ToShortItemVm).ToList();

            return itemVMs;
        }

        public List<ShortItemVm> GetLastAddedItemVMs(ItemsPageParams pageParams)
        {
            var items = GetLastAddedItems();
            var itemVMs = items
                .Skip((pageParams.Page - 1) * pageParams.Size)
                .Take(pageParams.Size)
                .Select(ConvertHelper.ToShortItemVm)
                .ToList();

            return itemVMs;
        }

        public ItemVm GetItemVm(Guid id, string? currentUserId)
        {
            var item = GetItemById(id);

            if (item == null)
            {
                throw new InvalidOperationException();
            }

            var itemVm = ConvertHelper.ToItemVm(item, currentUserId);

            return itemVm;
        }

        public void EditItem(ShortItemVm itemVm, string? currentUserId)
        {
            if (currentUserId == null)
            {
                throw new UnauthorizedAccessException();
            }

            var currentUser = _userService.GetUserById(currentUserId);
            var collection = _collectionService.GetCollectionById(itemVm.CollectionId);

            if (collection?.OwnerId != currentUser?.Id || currentUser?.UserRole != UserRole.Admin)
            {
                throw new InvalidOperationException();
            }

            UpdateItem(itemVm);
        }

        public void LikeItem(Guid id, string? currentUserId)
        {
            var item = GetItemById(id);

            if (item == null)
            {
                throw new InvalidOperationException();
            }
            
            var itemUser = item.ItemUsers.FirstOrDefault(i => i.UserId == Guid.Parse(currentUserId));

            if (itemUser != null)
            {
                item.ItemUsers.Remove(itemUser);
                UpdateItem(item);

                return;
            }

            item.ItemUsers.Add(new UserItem
            {
                UserId = Guid.Parse(currentUserId),
                IsLiked = true
            });

            UpdateItem(item);
        }

        public List<TagVm> GetFoundTagVMs(string query)
        {
            var tags = _tagService.SearchTags(query);
            var tagVMs = tags.Select(ConvertHelper.ToTagVm).ToList();

            return tagVMs;
        }

        public List<TagVm> GetTopTagVMs()
        {
            var tags = _tagService.GetTopTags();
            var tagVMs = tags.Select(ConvertHelper.ToTagVm).ToList();

            return tagVMs;
        }
    }
}
