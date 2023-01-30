namespace Migale.Playwright.Models;

public class PlaywrightCrawlerOptions
{
    public int BrowserInstances { get; set; } = 1;
    
    /// <summary>
    /// Maximum redirects to follow. 0 = unlimited.
    /// </summary>
    public int MaxRedirects { get; set; } = 3;
    
    /// <summary>
    /// Time before timeout in milliseconds.
    /// </summary>
    public int Timeout { get; set; } = 10000;
    
    public string? BrowserExecutablePath { get; set; }
    
    public string? BrowserType { get; set; } = "chromium";
    
    public bool Headless { get; set; } = true;
    
    public bool IsMobile { get; set; } = false;

    public bool BlockStyles { get; set; } = true;

    public bool BlockImages { get; set; } = true;

    public bool BlockFonts { get; set; } = true;

    public bool BlockMedia { get; set; } = true;

    public bool BlockScripts { get; set; } = false;
}