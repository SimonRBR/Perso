using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmolPass.Domain.Entities
{
    public class VaultItem
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public byte[] EncryptedBlob { get; init; } = Array.Empty<byte>();
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
    }
}
