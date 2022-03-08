using Dating_Api.Data.IRepository;
using Dating_Api.Helper;
using Dating_Api.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dating_Api.Data.Repository
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;

        public DatingRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Like> GetLike(int userId, int recipientId)
        {
            return await _context.Likes.FirstOrDefaultAsync(x => x.LikerId == userId && x.LikeeId == recipientId); ;
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.Where(x => x.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<Message> GetMessage(int Id)
        {
            return await _context.Messages.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessagesParam messagesParam)
        {
            var message = _context.Messages.Include(x => x.Sender).ThenInclude(x => x.Photos)
                 .Include(x => x.Recipient).ThenInclude(z => z.Photos).AsQueryable();

            switch (messagesParam.MessageContainer)
            {
                case "Inbox":
                    message = message.Where(u => u.RecipientId == messagesParam.UserId && u.RecipientDeleted == false);
                    break;
                case "Outbox":
                    message = message.Where(u => u.SenderId == messagesParam.UserId && u.SenderDeleted == false);
                    break;
                default:
                    message = message.Where(u => u.RecipientId == messagesParam.UserId && u.RecipientDeleted == false && u.IsRead == false);
                    break;
            }

            message = message.OrderByDescending(d => d.MessageSent);
            return await PagedList<Message>.CreateAsync(message, messagesParam.PageNumber, messagesParam.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
            var messages = await _context.Messages.Include(x => x.Sender).ThenInclude(x => x.Photos)
                .Include(x => x.Recipient).ThenInclude(z => z.Photos)
                .Where(
                   x => x.RecipientId == userId && x.SenderId == recipientId && x.RecipientDeleted == false 
                || x.SenderId == userId && x.RecipientId == recipientId && x.SenderDeleted == false)
                .OrderByDescending(x => x.MessageSent).ToListAsync();

            return messages;
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Id == id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.Include(p => p.Photos).OrderByDescending(u => u.LastActive).AsQueryable();

            users.Where(u => u.Id == userParams.UserId);

            users.Where(u => u.Gender == userParams.Gender);

            if (userParams.Likers)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikers.Contains(u.Id));
            }
            if (userParams.Likees)
            {
                var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikees.Contains(u.Id));
            }

            if (userParams.MinAge != 18 || userParams.MaxAge != 90)
            {
                var MinDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);

                var MaxDob = DateTime.Today.AddYears(-userParams.MinAge);

                users.Where(u => u.DateOfBirth >= MinDob && u.DateOfBirth <= MaxDob);
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.CreationDate);
                        break;

                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        //Helper Method
        private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers)
        {
            var user = await _context.Users
                .Include(x => x.Likers)
                .Include(x => x.Likees)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (likers)
                return user.Likers.Where(u => u.LikeeId == id).Select(i => i.LikerId);
            else
                return user.Likees.Where(u => u.LikerId == id).Select(i => i.LikeeId);

        }
    }
}
