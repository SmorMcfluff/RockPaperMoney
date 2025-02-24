using UnityEngine;

[System.Serializable]
public class AdWatcherInfo
{
    public CharacterAppearance appearance;

    public Country country;
    public Sex sex;
    public string firstName;
    public string lastName;
    public int age;

    public AdWatcherInfo()
    {
        if (!SaveDataManager.Instance.localPlayerData.hasAdam && Random.Range(0, 69) == 0)
        {
            country = Country.Yrgo;
            sex = Sex.Male;
            firstName = "Adam";
            lastName = "Mcfluff";
            age = GetAdamsAge();
            return;
        }

        var countryCount = System.Enum.GetNames(typeof(Country)).Length - 1;
        country = (Country)Random.Range(0, countryCount);
        sex = (Sex)Random.Range(0, 2);

        firstName = FirstNames.GetName(country, sex);
        lastName = LastNames.GetName(country, sex);
        age = Random.Range(14, 81);

        appearance = new CharacterAppearance(this);
    }

    private int GetAdamsAge()
    {
        System.DateTime today = System.DateTime.UtcNow.Date;
        System.DateTime birthDate = new(2001, 4, 13);

        int age = today.Year - birthDate.Year;

        if (birthDate.Date > today.AddYears(-age))
        {
            age--;
        }
        return age;
    }
}


public enum Country
{
    China, India, Japan,
    Malaysia, Mexico, ThePhilippines,
    Russia, Yrgo
}


public enum Sex
{
    Male, Female
}
