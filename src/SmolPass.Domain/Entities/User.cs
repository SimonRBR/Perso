using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmolPass.Domain.Entities
{
    public class User
    {
        public Guid Id { get; init; }
        public string Email { get; init; }
        public byte[] AuthSalt { get; init; } = Array.Empty<byte>();
        public byte[] AuthHash { get; init; } = Array.Empty<byte>();
        public byte[] EncryptionSalt { get; init; } = Array.Empty<byte>();
        public int KdfIterations { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
