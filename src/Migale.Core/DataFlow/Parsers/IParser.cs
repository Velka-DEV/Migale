namespace Migale.Core.DataFlow.Parsers;

public interface IParser
{
    public object Parse(DataContext source);
}