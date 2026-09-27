using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmolPass.Api.Extensions;
using SmolPass.Application.Common;
using SmolPass.Application.UseCases.Vault;
using SmolPass.Contracts.Vault;
using SmolPass.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SmolPass.Api.Controllers;

[ApiController]
[Route("api/vault")]
[Authorize]                    // ← sur LA CLASSE : toutes les actions exigent le badge
public class VaultController : ControllerBase
{
    private readonly GetVaultItemsUseCase _getVaultItemsUseCase;
    private readonly GetVaultItemByIdUseCase _getVaultItemByIdUseCase;
    private readonly AddVaultItemUseCase _addVaultItemUseCase;
    private readonly UpdateVaultItemUseCase _updateVaultItemUseCase;
    private readonly DeleteVaultItemUseCase _deleteVaultItemUseCase;

    public VaultController(GetVaultItemsUseCase getVaultItemsUseCase, GetVaultItemByIdUseCase getVaultItemByIdUseCase, AddVaultItemUseCase addVaultItemUseCase
    , UpdateVaultItemUseCase updateVaultItemUseCase, DeleteVaultItemUseCase deleteVaultItemUseCase)
    {
        _getVaultItemsUseCase = getVaultItemsUseCase;
        _getVaultItemByIdUseCase = getVaultItemByIdUseCase;
        _addVaultItemUseCase = addVaultItemUseCase;
        _updateVaultItemUseCase = updateVaultItemUseCase;
        _deleteVaultItemUseCase = deleteVaultItemUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetItems(CancellationToken cancellationToken)
    {
        if (!User.TryGetUserId(out Guid userId))
        {
            return Unauthorized();
        }
        IReadOnlyList<VaultItem> vaultItems = await _getVaultItemsUseCase.ExecuteAsync(userId, cancellationToken);
        IReadOnlyList<VaultItemResponse> response = vaultItems.Select(item => new VaultItemResponse(item.Id, item.EncryptedBlob, item.CreatedAt, item.UpdatedAt)).ToList();

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetItemById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (!User.TryGetUserId(out Guid userId))
        {
            return Unauthorized();
        }
        Result<VaultItem> result = await _getVaultItemByIdUseCase.ExecuteAsync(id, userId, cancellationToken);
        if (result.IsFailure)
        {
            return NotFound();
        }
        VaultItem item = result.Value;
        VaultItemResponse response = new (item.Id, item.EncryptedBlob, item.CreatedAt, item.UpdatedAt);
        
        return Ok(response);
    }
    [HttpPost]
    public async Task<IActionResult> AddItem(
    [FromBody] CreateVaultItemRequest request, CancellationToken cancellationToken)
    {
        if (!User.TryGetUserId(out Guid userId))
        {
            return Unauthorized();
        }

        Result<VaultItem> result = await _addVaultItemUseCase.ExecuteAsync(userId, request, cancellationToken);
        VaultItem item = result.Value;
        VaultItemResponse response = new(item.Id, item.EncryptedBlob, item.CreatedAt, item.UpdatedAt);

        return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateItem(
    [FromRoute] Guid id, [FromBody] UpdateVaultItemRequest request, CancellationToken cancellationToken
    )
    {
        if (!User.TryGetUserId(out Guid userId))
        {
            return Unauthorized();
        }
        Result<VaultItem> result = await _updateVaultItemUseCase.ExecuteAsync(id, userId, request, cancellationToken);
        if (result.IsFailure)
        {
            return NotFound();
        }
        VaultItem vaultItem = result.Value;
        VaultItemResponse response = new(vaultItem.Id, vaultItem.EncryptedBlob, vaultItem.CreatedAt, vaultItem.UpdatedAt);
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteItem(
    [FromRoute] Guid id, CancellationToken cancellationToken
    )
    {
        if (!User.TryGetUserId(out Guid userId))
        {
            return Unauthorized();
        }
        await _deleteVaultItemUseCase.ExecuteAsync(id, userId, cancellationToken);
        return NoContent();
    }
}