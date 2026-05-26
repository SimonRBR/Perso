using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmolPass.Contracts.Auth
{
    public record RegisterRequest
    (
        string Email,
        byte[] AuthSalt,
        byte[] AuthHash,
        byte[] EncryptionSalt,
        int KdfIterations
    );
}
