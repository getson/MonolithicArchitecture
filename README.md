# Monolithic architecture based on CQRS and DDD principles 
This repository presents an approach on how to build an application using Monolithic architecture, ASP.NET Core, EntityFrameworkCore, Identity Server, CQRS, DDD etc.

![Build Status](https://github.com/getson/MonolithicArchitecture/workflows/Build%20Status/badge.svg?branch=master)

## Packages
| Package | Latest Stable |
| --- | --- |
| [BinaryOrigin.SeedWork.Core](https://www.nuget.org/packages/BinaryOrigin.SeedWork.Core) | [![Nuget Package](https://img.shields.io/badge/nuget-1.1.2-blue.svg)](https://www.nuget.org/packages/BinaryOrigin.SeedWork.Core) |
| [BinaryOrigin.SeedWork.Infrastructure](https://www.nuget.org/packages/BinaryOrigin.SeedWork.Infrastructure) | [![Nuget Package](https://img.shields.io/badge/nuget-1.1.1-blue.svg)](https://www.nuget.org/packages/BinaryOrigin.SeedWork.Infrastructure) |
| [BinaryOrigin.SeedWork.Messages](https://www.nuget.org/packages/BinaryOrigin.SeedWork.Messages) | [![Nuget Package](https://img.shields.io/badge/nuget-1.1.1-blue.svg)](https://www.nuget.org/packages/BinaryOrigin.SeedWork.Messages) |
| [BinaryOrigin.SeedWork.Persistence.Ef](https://www.nuget.org/packages/BinaryOrigin.SeedWork.Persistence.Ef) | [![Nuget Package](https://img.shields.io/badge/nuget-1.1.3-blue.svg)](https://www.nuget.org/packages/BinaryOrigin.SeedWork.Persistence.Ef) |
| [BinaryOrigin.SeedWork.WebApi](https://www.nuget.org/packages/BinaryOrigin.SeedWork.WebApi) | [![Nuget Package](https://img.shields.io/badge/nuget-1.1.3-blue.svg)](https://www.nuget.org/packages/BinaryOrigin.SeedWork.WebApi) |

## Providers
| Package | Latest Stable |
| --- | --- |
| [BinaryOrigin.SeedWork.Persistence.MySql](https://www.nuget.org/packages/BinaryOrigin.SeedWork.Persistence.MySql) | [![Nuget Package](https://img.shields.io/badge/nuget-1.1.1-blue.svg)](https://www.nuget.org/packages/BinaryOrigin.SeedWork.Persistence.MySql) |
| [BinaryOrigin.SeedWork.Persistence.SqlServer](https://www.nuget.org/packages/BinaryOrigin.SeedWork.Persistence.SqlServer) | [![Nuget Package](https://img.shields.io/badge/nuget-1.1.3-blue.svg)](https://www.nuget.org/packages/BinaryOrigin.SeedWork.Persistence.SqlServer) |

# Getting started:

> 1 - Ensure that identity server is started  
> 2 - Use this request body for generating a token:  
  >> client_id:clientId  
  >> client_secret:secret  
  >> scope:myApi profile openid  
  >> username:getson  
  >> password:password  
  >> grant_type:password  
  
> 3 - Copy access token and paste it to swagger  
> 4 - Enjoy debugging.  

## Your feedback is welcomed :)  

If you want to use these small libraries you can use : https://github.com/getson/Monolith-CQRS-DDD as a quickstart.
