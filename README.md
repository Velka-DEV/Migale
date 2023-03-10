[![GitHub](https://img.shields.io/github/license/Velka-DEV/Migale)](https://github.com/Velka-DEV/Migale/blob/dev/LICENCE) [![Nuget](https://img.shields.io/nuget/dt/Migale.Core)](https://www.nuget.org/packages/Migale.Core/)
# Migale: A lightweight .NET spider with fast kickstarting

Migale was born out of a need to extract quickly and with a very low development cost. This package is not intended to replace complete and structured libraries like [DotnetSpider](https://github.com/dotnetcore/DotnetSpider). 

## Features

- [x] Multi-Threaded
- [x] Fail & retries handling
- [x] Event-Driven
- [x] Extensible
- [ ] Document parsing (HTML, JSON, XML) (Work in progress !)

## Samples

You can find samples with the crawlers implementation [here](https://github.com/Velka-DEV/Migale/tree/main/src/Migale.Samples) !

## Packages

| Package | Description | Nuget |
| ----------- | ----------- | ----------- |
| [Migale.Core](https://github.com/Velka-DEV/Migale/tree/main/src/Migale.Core) | The core of the project | [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Migale.Core)](https://www.nuget.org/packages/Migale.Core/) |
| [Migale.Crawlers.HttpClient](https://github.com/Velka-DEV/Migale/tree/main/src/Migale.Crawlers.HttpClient) | The HttpClient crawler implementation with RequestMessages | [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Migale.Crawlers.HttpClient)](https://www.nuget.org/packages/Migale.Crawlers.HttpClient/) |
| [Migale.Crawlers.Playwright](https://github.com/Velka-DEV/Migale/tree/main/src/Migale.Crawlers.Playwright) | The Playwright crawler implementation with browser automation | [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Migale.Crawlers.Playwright)](https://www.nuget.org/packages/Migale.Crawlers.Playwright/) |

## Contributing

We would love community contributions here.

There is actually no contributation guide or convention as i'm actually the only maintainer of the project.

## License

This project is licensed with the [MIT license](LICENSE).

## Related Projects

You should take a look at these related projects:

- [DotnetSpider](https://github.com/dotnetcore/DotnetSpider)
- [GO-Colly](https://github.com/gocolly/colly)
- [Anglesharp](https://github.com/AngleSharp/AngleSharp)
