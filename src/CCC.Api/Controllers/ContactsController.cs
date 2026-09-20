using CCC.Api.Contracts;
using CCC.Application.Common;
using CCC.Application.Features.Contacts;
using CCC.Application.Features.Contacts.CreateContact;
using CCC.Application.Features.Contacts.DeleteContact;
using CCC.Application.Features.Contacts.GetContact;
using CCC.Application.Features.Contacts.GetContacts;
using CCC.Application.Features.Contacts.UpdateContact;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CCC.Api.Controllers;

[ApiController]
[Route("api/contacts")]
public sealed class ContactsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(
        typeof(PagedResult<ContactDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<ContactDto>>> GetAll(
        [FromQuery] GetContactsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new GetContactQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ContactDto>> Create(
        CreateContactRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateContactCommand(
            request.Name,
            request.Phone,
            request.Address,
            request.AvatarUrl,
            request.BirthMonth,
            request.BirthDay);

        var result = await sender.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactDto>> Update(
        Guid id,
        UpdateContactRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateContactCommand(
            id,
            request.Name,
            request.Phone,
            request.Address,
            request.AvatarUrl,
            request.BirthMonth,
            request.BirthDay);

        var result = await sender.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new DeleteContactCommand(id),
            cancellationToken);

        return NoContent();
    }
}