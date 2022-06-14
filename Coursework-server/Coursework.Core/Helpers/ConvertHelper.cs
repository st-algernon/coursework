using Coursework.Core.Data.Models;
using Coursework.Core.Data.ViewModels;
using Coursework.Core.Extensions;

namespace Coursework.Core.Helpers
{
    public static class ConvertHelper
    {
        public static UserVm ToUserVm(User user)
        {
            var userVm = new UserVm
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                UserRole = user.UserRole.ToString(),
                UserState = user.UserState.ToString()
            };

            return userVm;
        }

        public static User ToUser(UserVm userVm)
        {
            var user = new User();

            userVm.CopyPropertiesTo(user);

            return user;
        }

        public static FieldVm ToFieldVm(Field field)
        {
            var fieldVm = new FieldVm();

            field.CopyPropertiesTo(fieldVm);

            return fieldVm;
        }

        public static Field ToField(FieldVm fieldVm)
        {
            var field = new Field();

            fieldVm.CopyPropertiesTo(field);

            return field;
        }

        public static FieldTypeVm ToFieldTypeVm(FieldType fieldType)
        {
            var fieldTypeVm = new FieldTypeVm();

            fieldType.CopyPropertiesTo(fieldTypeVm);

            return fieldTypeVm;
        }

        public static ShortCollectionVm ToShortCollectionVm(Collection collection)
        {
            var collectionVm = new ShortCollectionVm();

            collection.CopyPropertiesTo(collectionVm);

            if (collection.Topic != null)
            {
                collectionVm.TopicName = collection.Topic.Name;
            }

            return collectionVm;
        }

        public static CollectionVm ToCollectionVm(Collection collection)
        {
            var collectionVm = new CollectionVm();

            collection.CopyPropertiesTo(collectionVm);

            collectionVm.FieldVMs = collection.Fields.Select(f => ToFieldVm(f)).ToList();

            return collectionVm;
        }

        public static ShortItemVm ToShortItemVm(Item item)
        {
            var shortItemVm = new ShortItemVm();

            item.CopyPropertiesTo(shortItemVm);

            shortItemVm.FullFieldVMs = item.ItemFields.Select(i => ToFullFieldVm(i)).ToList();
            shortItemVm.TagNames = item.Tags.Select(t => t.Name).ToList();

            return shortItemVm;
        }

        public static ItemVm ToItemVm(Item item, string? currentUserId)
        {
            var itemVm = new ItemVm();

            item.CopyPropertiesTo(itemVm);

            itemVm.TagVMs = item.Tags.Select(f => ToTagVm(f)).ToList();
            itemVm.FullFieldVMs = item.ItemFields.Select(i => ToFullFieldVm(i)).ToList();
            itemVm.UsersItemVm = ToUsersItemVm(item.ItemUsers, currentUserId);
            itemVm.OwnerId = item.Collection.OwnerId;

            return itemVm;
        }

        public static FieldWithTypeNameVm ToFieldWithTypeNameVm(Field field)
        {
            var fieldWithTypeNameVm = new FieldWithTypeNameVm();

            field.CopyPropertiesTo(fieldWithTypeNameVm);

            if (field.FieldType != null)
            {
                fieldWithTypeNameVm.TypeName = field.FieldType.Name;
            }

            return fieldWithTypeNameVm;
        }

        public static FullFieldVm ToFullFieldVm(FieldItem fieldItem)
        {
            var fullFieldVm = new FullFieldVm();

            fieldItem.CopyPropertiesTo(fullFieldVm);
            fullFieldVm.Id = fieldItem.Field.Id;
            fullFieldVm.Name = fieldItem.Field.Name;
            fullFieldVm.TypeName = fieldItem.Field.FieldType.Name;

            return fullFieldVm;
        }

        public static TagVm ToTagVm(Tag tag)
        {
            var tagVm = new TagVm();

            tag.CopyPropertiesTo(tagVm);

            return tagVm;
        }

        public static UsersItemVm ToUsersItemVm(List<UserItem> userItems, string? currentUserId)
        {
            var usersItemVm = new UsersItemVm();

            usersItemVm.CountOfLikes = userItems.Count(u => u.IsLiked);

            if (currentUserId != null)
            {
                usersItemVm.IsLiked = userItems.Any(u => u.UserId == Guid.Parse(currentUserId) && u.IsLiked);

                return usersItemVm;
            }
            
            usersItemVm.IsLiked = false;

            return usersItemVm;
        }

        public static CommentVm ToCommentVm(Comment comment)
        {
            var commentVm = new CommentVm();

            comment.CopyPropertiesTo(commentVm);

            commentVm.AuthorVm = ToUserVm(comment.Author);

            return commentVm;
        }
    }
}
