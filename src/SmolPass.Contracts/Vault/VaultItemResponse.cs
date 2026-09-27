namespace SmolPass.Contracts.Vault;

public record VaultItemResponse 
(
    Guid Id,
    byte[] EncryptedBlob,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
