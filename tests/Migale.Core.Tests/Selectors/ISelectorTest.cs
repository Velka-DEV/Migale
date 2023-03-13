using Migale.Core.DataFlow;

namespace Migale.Core.Tests.Selectors;

public interface ISelectorTest
{
    public string? SelectTest(DataContext context);

    public IEnumerable<string> SelectManyTest(DataContext context);
}