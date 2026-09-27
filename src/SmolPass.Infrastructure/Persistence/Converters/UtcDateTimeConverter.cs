using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SmolPass.Infrastructure.Persistence.Converters;

public sealed class UtcDateTimeConverter : ValueConverter<DateTime, DateTime>
{
    public UtcDateTimeConverter()
        : base(
            convertToProviderExpression: valeur => valeur,
            convertFromProviderExpression: valeur => DateTime.SpecifyKind(valeur, DateTimeKind.Utc))
    {
    }
}