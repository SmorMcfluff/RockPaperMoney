using System.Collections;
using UnityEngine;

public static class LastNames
{
    public static string[] china =
    {
        "Li", "Wang", "Zhang", "Liú", "Chen",
        "Yang", "Zhao", "Huang", "Zhou", "Wu"
    };

    public static string[] india =
    {
        "Devi", "Singh", "Kumar", "Das", "Kaur",
        "Ram", "Yadav", "Kumari", "Lal", "Bai"
    };

    public static string[] japan =
    {
        "Satou", "Suzuki", "Takahashi", "Tanaka", "Watanabe",
        "Itou", "Nakamura", "Kobayashi", "Yamamoto", "Katou"
    };

    public static string[] mexico =
    {
        "Hernández", "García", "Martínez", "González", "López",
        "Rodriguez", "Pérez", "Sánchez", "Ramírez", "Flores"
    };

    public static string[] philippines =
    {
        "dela Cruz", "García", "Reyes", "Ramos", "Mendoza",
        "Santos", "Flores", "Gonzales", "Bautista", "Villanueva"
    };

    public static string[] russia =
    {
        "Ivanov", "Smirnov", "Petrov", "Sidorov", "Kuznetsov",
        "Popov", "Vassiliev", "Sokolov", "Mikhailov", "Novikov"
    };

    public static string GetName(Country country, Sex sex)
    {
        var nameIndex = Random.Range(0, 10);
        return country switch
        {
            Country.China => china[nameIndex],
            Country.India => india[nameIndex],
            Country.Japan => japan[nameIndex],
            Country.Malaysia => SetPatronym(country, sex),
            Country.Mexico => mexico[nameIndex],
            Country.ThePhilippines => philippines[nameIndex],
            Country.Russia => SetRussianName(russia[nameIndex], sex),
            _ => "Smith"
        };
    }

    private static string SetPatronym(Country country, Sex sex)
    {
        var fathersName = FirstNames.GetName(country, Sex.Male);

        return (country, sex) switch
        {
            (Country.Malaysia, Sex.Male) => $"bin {fathersName}",
            (Country.Malaysia, Sex.Female) => $"binti {fathersName}",
            _ => "Smith"
        };
    }

    private static string SetRussianName(string rootName, Sex sex)
    {
        if(sex == Sex.Female)
        {
            return rootName + "a";
        }
        else
        {
            return rootName;
        }
    }
}
