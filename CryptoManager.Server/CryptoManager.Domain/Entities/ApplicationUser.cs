using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CryptoManager.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? FacebookId { get; set; }
        public string GoogleId { get; set; }
        public string Gender { get; set; }
        public string Locale { get; set; }
        public string PictureUrl { get; set; }
        public bool HasReceivedAirdrop { get; set; }
        public string SolanaWalletAddress { get; set; }
        public Guid? ReferredById { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
