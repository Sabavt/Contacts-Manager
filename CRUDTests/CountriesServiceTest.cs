using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;

    public CountriesServiceTest()
    {
        _countriesService = new CountriesService(false);
    }

    [Fact]
    public void AddCountry_CountryNameIsNull()
    {
        //Arrange
        CountryAddRequest? request = new CountryAddRequest() { CountryName = null };

        //Assert
        Assert.Throws<ArgumentException>(() => _countriesService.AddCountry(request));
    }

    [Fact]
    public void AddCountry_NullCountry()
    {
        //Arrange
        CountryAddRequest? request = null;

        //Assert
        Assert.Throws<ArgumentNullException>(() => _countriesService.AddCountry(request));
    }

    [Fact]
    public void AddCountry_DublicatedCountryName()
    {
        //Arrange
        CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "USA" };

        //Assert
        Assert.Throws<ArgumentException>(() =>
        {
            //Act
            _countriesService.AddCountry(request1);
            _countriesService.AddCountry(request2);

        });
    }

    [Fact]
    public void AddCountry_ProperCountryDetails()
    {
        //Arrange
        CountryAddRequest? request = new CountryAddRequest() { CountryName = "Japan" };

        //Act
        CountryResponse response = _countriesService.AddCountry(request);
        List<CountryResponse> countries_from_GetAllCountries = _countriesService.GetAllCountries();

        //Assert
        Assert.True(response.CountryID != Guid.Empty);
        Assert.Contains(response, countries_from_GetAllCountries);
    }

    [Fact]
    //The list of countries should be empty by default (before adding any countries) 
    public void GetCountryList_EmptyList()
    {   
        //Act
        List<CountryResponse> actualCountry = _countriesService.GetAllCountries();

        //Assert
        Assert.Empty(actualCountry);
    }
    
    [Fact] 
    public void GetCountryList_AddFewCountries()
    {   
        //Arrange
        List<CountryAddRequest> country_request_list =[ new CountryAddRequest() { CountryName = "USA"},new CountryAddRequest() { CountryName = "Germany"},new CountryAddRequest() { CountryName = "Belgium"}]; 
        List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();

        //Assert
        foreach (var country_request in country_request_list)
        {
            countries_list_from_add_country.Add(_countriesService.AddCountry(country_request));
        } 

        List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();

        //Assert
        foreach(var expected_country in countries_list_from_add_country)
        {
            Assert.Contains(expected_country, actualCountryResponseList);
        }
    }

    [Fact]
    public void GetCountryByCountryID_NullCountryID()
    {
        //Arrange
        Guid? guid = null;

        //Act
        _countriesService.GetCountryByCountryID(guid);
    } 
      
    [Fact]
    public void GetCountryByCountryID_ValidCountyID()
    {
        //Arrange
        CountryAddRequest? country_add_request = new CountryAddRequest() { CountryName = "Egypt"}; 
        CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

        //Act
        CountryResponse? county_response_from_get = _countriesService.GetCountryByCountryID(country_response_from_add.CountryID);

        //Assert
        Assert.Equal(country_response_from_add, county_response_from_get);
    }
}
