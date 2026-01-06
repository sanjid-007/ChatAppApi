using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Entities
{
    public class User
    {
        public string? Id { get; private set; }

        public string? UserName { get; private set; }

        public string? Email { get; private set; }

        public string? DisplayName { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime LastSeenAt { get; private set; }

        public bool IsOnline { get; private set; }
        
        private User() { }

        public static User Create(string userName, string email, string displayName)
        {
            return new User
            {
                UserName = userName,
                Email = email,
                DisplayName = displayName,
                CreatedAt = DateTime.UtcNow,
                IsOnline = false
            };
        }
        public void SetId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Id = id;
            }
        }
        public void UpdateDisplayName(string displayName)
        {
            DisplayName = displayName;
        }

        public void UpdateLastSeen()
        {
            LastSeenAt = DateTime.UtcNow;
        }

        public void SetOnlineStatus(bool isOnline)
        {
            IsOnline = isOnline;
            if(isOnline)
            {
                LastSeenAt = DateTime.UtcNow;
            }
        }

        public void UpdateEmail(string email)
        {
            Email = email;
        }
    }
}
