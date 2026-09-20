using CCC.Domain.Exceptions;

namespace CCC.Domain.Entities;

public sealed class Contact
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string? Phone { get; private set; }

    public string? Address { get; private set; }

    public string? AvatarUrl { get; private set; }

    public DateTimeOffset? CreatedAt { get; private set; }

    public short? BirthMonth { get; private set; }

    public short? BirthDay { get; private set; }

    public bool HasBirthday =>
        BirthMonth.HasValue && BirthDay.HasValue;

    private Contact()
    {
    }

    private Contact(
        Guid id,
        string name)
    {
        Id = id;
        Name = name;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static Contact Create(
        string name,
        string? phone = null,
        string? address = null,
        string? avatarUrl = null,
        short? birthMonth = null,
        short? birthDay = null)
    {
        var contact = new Contact(
            Guid.NewGuid(),
            NormalizeName(name));

        contact.Phone = Normalize(phone);
        contact.Address = Normalize(address);
        contact.AvatarUrl = Normalize(avatarUrl);
        contact.SetBirthday(birthMonth, birthDay);

        return contact;
    }

    public void Update(
        string name,
        string? phone,
        string? address,
        string? avatarUrl)
    {
        Name = NormalizeName(name);
        Phone = Normalize(phone);
        Address = Normalize(address);
        AvatarUrl = Normalize(avatarUrl);
    }

    public void SetBirthday(
        short? month,
        short? day)
    {
        if (month is null && day is null)
        {
            BirthMonth = null;
            BirthDay = null;
            return;
        }

        if (day is null)
        {
            throw new DomainException(
                "Birth month and birth day must be provided together.");
        }

        if (month is < 1 or > 12)
        {
            throw new DomainException(
                "Birth month must be between 1 and 12.");
        }

        if (day is < 1 or > 31)
        {
            throw new DomainException(
                "Birth day must be between 1 and 31.");
        }

        if (month is not null && day > DaysInMonth(month.Value))
        {
            throw new DomainException(
                $"Month {month} does not have {day} days.");
        }

        BirthMonth = month;
        BirthDay = day;
    }

    public void ClearBirthday() => SetBirthday(null, null);

    private static short DaysInMonth(short month) => month switch
    {
        2 => 29,
        4 or 6 or 9 or 11 => 30,
        _ => 31
    };

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException(
                "Contact name is required.");
        }

        return name.Trim();
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
}