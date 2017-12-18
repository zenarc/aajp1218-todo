# Cosmos DB .NET Core Todo Sample App

ASP .NET Core based apps for Azure Cosmos DB sample code.

## Requiements

* Visual Studio 2017 or Visual Studio for Mac
* .NET Core 2.0

## Note

before build, change the code of ```appsettings.Development.json``` to your connection strings.

```json
"AppConfiguration": {
    "Endpoint": "{ENDPOINT}",
    "Key": "{KEY}",
    "DatabaseId": "{DATABASE}",
    "CollectionId": "{COLLECTION}",
    "Region": "{REGION}"
}
```