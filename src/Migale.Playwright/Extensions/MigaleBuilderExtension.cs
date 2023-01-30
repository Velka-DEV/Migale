using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Migale.Core;
using Migale.Playwright.Models;

namespace Migale.Playwright.Extensions;

public static class MigaleBuilderExtension
{
    private static readonly IPlaywright Playwright = Await(Microsoft.Playwright.Playwright.CreateAsync());
    private static readonly List<IBrowser> Browsers = new();

    public static MigaleBuilder WithPlaywrightCrawler(this MigaleBuilder builder, PlaywrightCrawlerOptions options)
    {
        var browserType = GetBrowserType(options.BrowserType);

        for (var i = 0; i < options.BrowserInstances; i++)
        {
            Browsers.Add(Await(browserType.LaunchAsync(GetLaunchOptions(options))));
        }
        
        builder.WithCrawlers(Browsers.Select(b => new PlaywrightCrawler(CreateBrowserContext(b, options))));

        return builder;
    }

    public static MigaleBuilder WithPlaywrightCrawler(this MigaleBuilder builder, IEnumerable<Proxy> proxies, PlaywrightCrawlerOptions options)
    {
        throw new NotImplementedException();
        return builder;
    }
    
    private static IBrowserType GetBrowserType(string? browserName)
    {
        return browserName switch
        {
            "chromium" => Playwright.Chromium,
            "firefox" => Playwright.Firefox,
            "webkit" => Playwright.Webkit,
            null => Playwright.Chromium,
            _ => throw new ArgumentException("Invalid browser name", nameof(browserName))
        };
    }
    
    private static BrowserTypeLaunchOptions GetLaunchOptions(PlaywrightCrawlerOptions options)
    {
        return new BrowserTypeLaunchOptions
        {
            Headless = options.Headless,
            ExecutablePath = options.BrowserExecutablePath,
            Timeout = options.Timeout
        };
    }

    private static BrowserNewContextOptions GetContextOptions(PlaywrightCrawlerOptions options)
    {
        return new BrowserNewContextOptions
        {
            JavaScriptEnabled = !options.BlockScripts,
            IsMobile = options.IsMobile,
            HasTouch = options.IsMobile
        };
    }
    
    private static IBrowser CreateBrowser(IBrowserType browserType, PlaywrightCrawlerOptions options)
    {
        return Await(browserType.LaunchAsync(GetLaunchOptions(options)));
    }
    
    private static IBrowserContext CreateBrowserContext(IBrowser browser, PlaywrightCrawlerOptions options)
    {
        var context = Await(browser.NewContextAsync(GetContextOptions(options)));
        
        if (options.BlockStyles) Await(context.RouteAsync(new Regex(@"(?:.*\.)(css|scss)(?:.*)", RegexOptions.Compiled), r => r.AbortAsync()));
        if (options.BlockImages) Await(context.RouteAsync(new Regex(@"(?:.*\.)(png|jpg|svg|gif|avif|webp|apng|jpeg)(?:.*)", RegexOptions.Compiled), r => r.AbortAsync()));
        if (options.BlockFonts) Await(context.RouteAsync(new Regex(@"(?:.*\.)(woff|ttf|eot|otf|woff2)(?:.*)", RegexOptions.Compiled), r => r.AbortAsync()));
        if (options.BlockMedia) Await(context.RouteAsync(new Regex(@"(?:.*\.)(mp4|webm|ogg|mp3|wav|flac|aac)(?:.*)", RegexOptions.Compiled), r => r.AbortAsync()));

        return context;
    }
    
    private static void Await(Task task)
    {
        task.GetAwaiter().GetResult();
    }
    
    private static T Await<T>(Task<T> task)
    {
        return task.GetAwaiter().GetResult();
    }
}