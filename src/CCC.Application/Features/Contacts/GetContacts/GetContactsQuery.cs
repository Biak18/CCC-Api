using CCC.Application.Common;
using MediatR;

namespace CCC.Application.Features.Contacts.GetContacts;

public sealed record GetContactsQuery(
    int Page = 1,
    int PageSize = 20,
    string? Search = null,
    short? BirthMonth = null,
    string SortBy = "name",
    string SortDirection = "asc")
    : IRequest<PagedResult<ContactDto>>;