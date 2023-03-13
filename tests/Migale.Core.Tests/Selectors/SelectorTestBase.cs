using Migale.Core.DataFlow;
using Migale.Core.Tests.Data;

namespace Migale.Core.Tests.Selectors;

[TestClass]
public abstract class SelectorTestBase : ISelectorTest
{
    protected DataContext ExampleDotComContext { get; }
    
    protected DataContext IanaIpV4Context { get; }

    protected SelectorTestBase(DataContext ianaIpV4Context)
    {
        IanaIpV4Context = new DataContext(Pages.IanaIpV4);
        ExampleDotComContext = new DataContext(Pages.ExampleDotCom);
    }
    
    
    [TestMethod]
    public abstract string? SelectTest(DataContext context);

    [TestMethod]
    public abstract IEnumerable<string> SelectManyTest(DataContext context);
}