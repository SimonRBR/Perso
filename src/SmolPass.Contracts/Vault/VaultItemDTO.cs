using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmolPass.Contracts.Vault
{
    //C'est ce que le serveur renvoie quand le client demande ses items
    public record VaultItemDTO
    (
        Guid Id,
        byte[] EncryptedBlob,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}
