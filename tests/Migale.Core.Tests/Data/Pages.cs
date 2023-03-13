namespace Migale.Core.Tests.Data;

public static class Pages
{
    public static readonly string ExampleDotCom = File.ReadAllText(Path.Combine("Data", "Html", "example.com.html"));
    
    public static readonly string IanaIpV4 = File.ReadAllText(Path.Combine("Data", "Html", "iana.ipv4.html"));
}