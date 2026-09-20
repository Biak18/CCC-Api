using CCC.Api.Contracts;
using CCC.Application.Common;
using CCC.Application.Features.PushTokens;
using CCC.Application.Features.PushTokens.DeletePushToken;
using CCC.Application.Features.PushTokens.GetPushToken;
using CCC.Application.Features.PushTokens.GetPushTokens;
using CCC.Application.Features.PushTokens.RegisterPushToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CCC.Api.Controllers;

[ApiController]
[Route("api/push-tokens")]
public sealed class PushTokensController(ISender sender) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(
        typeof(PagedResult<PushTokenDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<PushTokenDto>>> GetAll(
        [FromQuery] GetPushTokensQuery query,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PushTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PushTokenDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new GetPushTokenQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PushTokenDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(PushTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PushTokenDto>> Register(
        RegisterPushTokenRequest request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RegisterPushTokenCommand(request.Token),
            cancellationToken);

        if (!result.Created)
        {
            return Ok(result.PushToken);
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.PushToken.Id },
            result.PushToken);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new DeletePushTokenCommand(id),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteByValue(
        [FromQuery] string token,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new DeletePushTokenByValueCommand(token),
            cancellationToken);

        return NoContent();
    }
}