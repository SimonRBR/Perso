using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmolPass.Contracts.Vault
{
    //envoi simple du Blob
    public record CreateVaultItemRequest(byte[] EncryptedBlob);
}
