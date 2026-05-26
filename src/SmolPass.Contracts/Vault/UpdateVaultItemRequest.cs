using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmolPass.Contracts.Vault
{
    //Modifier un item
    public record UpdateVaultItemRequest(byte[] EncryptedBlob);
}
