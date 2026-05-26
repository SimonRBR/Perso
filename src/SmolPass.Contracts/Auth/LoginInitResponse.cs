using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmolPass.Contracts.Auth
{
    public record LoginInitResponse
    (
        byte[] AuthSalt,
        byte[] EncryptionSalt,
        int KdfIterations
    );
}
