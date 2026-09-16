using FluentAssertions; 
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace CRUDTests;

public class PersonsControllerIntegrationTests : IClassFixture<MyWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PersonsControllerIntegrationTests(MyWebApplicationFactory myWeb)
    {
       _client = myWeb.CreateClient();
    }

    [Fact]
    public async Task Index_ToReturnView()
    {
       HttpResponseMessage responseMessage = await _client.GetAsync("/Persons/Index", TestContext.Current.CancellationToken);
         
       responseMessage.IsSuccessStatusCode.Should().BeTrue();

       string body = await responseMessage.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        HtmlDocument document = new HtmlDocument(); 
        document.LoadHtml(body);

        var doc = document.DocumentNode;
        doc.QuerySelectorAll("table.saba-contacts").Should().NotBeNull();
    }
}
