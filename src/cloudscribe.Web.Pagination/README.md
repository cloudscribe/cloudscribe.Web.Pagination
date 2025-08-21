# cloudscribe.Web.Pagination

An ASP.NET Core TagHelper for pagination. Provides flexible, customizable, and Bootstrap-compatible pagination controls for web applications.

## Usage

1. Install the NuGet package:
   ```shell
   dotnet add package cloudscribe.Web.Pagination
   ```
2. Add the TagHelper to your `_ViewImports.cshtml`:
   ```csharp
   @addTagHelper *, cloudscribe.Web.Pagination
   ```
3. Use the `<cs-pager>` TagHelper in your Razor views:
   ```html
   <cs-pager ... />
   ```

For full documentation and advanced usage, see the [GitHub repo](https://github.com/joeaudette/cloudscribe.Web.Pagination).

## License

Licensed under the Apache-2.0 License.
