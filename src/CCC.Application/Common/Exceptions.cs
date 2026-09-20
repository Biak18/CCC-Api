namespace CCC.Application.Common;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message)
    {
    }

    public static NotFoundException For<TEntity>(object key) =>
        new($"{typeof(TEntity).Name} '{key}' was not found.");
}

public sealed class ConflictException : Exception
{
    public ConflictException(string message)
        : base(message)
    {
    }
}

public sealed class ForbiddenException : Exception
{
    public ForbiddenException(string message)
        : base(message)
    {
    }
}
