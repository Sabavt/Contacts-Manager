using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using FluentAssertions;
using Moq;
using RepositoryContracts;
using Entities;

namespace CRUDTests;

public class CountriesServiceTest
{
    private readonly ICountriesAdderService _countriesService; 
    private readonly Mock<ICountriesRepository> _countriesRepositoryMock;

    public CountriesServiceTest()
    {  
        _countriesRepositoryMock = new Mock<ICountriesRepository>(); 
        _countriesService = new CountriesService(_countriesRepositoryMock.Object);
    }

    [Fact]
    public async Task AddCountry_CountryNameIsNull_ToBeArgumentException()
    { 
        CountryAddRequest? request = new CountryAddRequest() { CountryName = null };

        Func<Task> act = async () => await _countriesService.AddCountry(request);
         
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AddCountry_NullCountry_ToBeArgumentNullException()
    { 
        CountryAddRequest? request = null;

        Func<Task> act = async () => await _countriesService.AddCountry(request);
         
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task AddCountry_DublicatedCountryName_ToBeArgumentException()
    { 
        CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "USA" };

        _countriesRepositoryMock.Setup(t => t
            .GetCountryByCountryName(It.IsAny<string>()))
            .ReturnsAsync(request1.ToCountry());

        Func<Task> act = async () => await _countriesService.AddCountry(request1);
        Func<Task> act2 = async () => await _countriesService.AddCountry(request2);

        await act2.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AddCountry_ProperCountryDetails_ToBeSuccessful()
    { 
        CountryAddRequest? request = new CountryAddRequest() { CountryName = "Japan" };

        _countriesRepositoryMock.Setup(t => t
            .AddCountry(It.IsAny<Country>()))
            .ReturnsAsync(request.ToCountry());
         
        CountryResponse response = await _countriesService.AddCountry(request); 

        response.CountryID.Should().NotBe(Guid.Empty); 
    }

    [Fact] 
    public async Task GetCountryList_EmptyList_ToBeEmpty()
    {
        _countriesRepositoryMock.Setup(t => t.GetAllCountries())
            .ReturnsAsync(new List<Country>());
         
        List<CountryResponse> actualCountry = await _countriesService.GetAllCountries();
         
        actualCountry.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCountryList_AddFewCountries_ToBeSuccessful()
    { 
        List<Country> countries = [
            new Country() { CountryName = "USA" }, 
            new Country() { CountryName = "Germany" },
            new Country() { CountryName = "Belgium" }
        ];

        _countriesRepositoryMock.Setup(t => t.GetAllCountries()).ReturnsAsync(countries);

        List<CountryResponse> actualCountryResponseList = await _countriesService.GetAllCountries();
         
        actualCountryResponseList.Should().BeEquivalentTo(countries.Select(t => t.ToCountryResponse()));
    }

    [Fact]
    public async Task GetCountryByCountryID_NullCountryID_ToBeArgumentNullException()
    { 
        Guid? guid = null;
         
        Func<Task> country_response = async () => await _countriesService.GetCountryByCountryID(guid); 

        await country_response.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetCountryByCountryID_ValidCountyID_ToBeSuccessful()
    { 
        Country? country = new() { CountryName = "Egypt" }; 
        _countriesRepositoryMock.Setup(t => t.GetCountryByCountryID(It.IsAny<Guid>()))
            .ReturnsAsync(country);
         
        CountryResponse? county_response_from_get = await _countriesService.GetCountryByCountryID(country.CountryID);
         
        country.ToCountryResponse().Should().BeEquivalentTo(county_response_from_get);
    }
}
