using UnityEngine;

public class AdWatcherInfo
{
    public Country country;
    public Sex sex;
    public string firstName;
    public string lastName;
    public int age;
    public CharacterAppearance appearance;

    public AdWatcherInfo() //new Factory
    {
        var countryCount = System.Enum.GetNames(typeof(Country)).Length;
        country = (Country)Random.Range(0, countryCount);
        sex = (Sex)Random.Range(0, 2);

        firstName = FirstNames.GetName(country, sex);
        lastName = LastNames.GetName(country, sex);
        age = Random.Range(14, 81);
    }
}

public enum Country
{
    China, India, Japan,
    Malaysia, Mexico, ThePhilippines,
    Russia
}

public enum Sex
{
    Male, Female
}
