using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Domain.Entities.Identity;


namespace Market.Domain.Entities.Identity
{
    public class PasswordResetToken
    {
        public int Id { get; set; }

        public string Token { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; }

        // Relacija sa User
        public int UserId { get; set; }
        public MarketUserEntity User { get; set; } = null!;

    }
}

