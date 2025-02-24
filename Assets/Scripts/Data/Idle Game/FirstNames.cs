using UnityEngine;

public static class FirstNames
{
    public static string[,] china =
    {
        //Male names
        {
            "Yichen", "Haoran", "Yuxuan", "Yuhang", "Zimo",
            "Yuchen", "Haoyu", "Peng", "Quan", "Rui"
        },
        //Female names
        {
            "Yi nuo", "Xin yi", "Zi han", "Yu tong", "Xin yan",
            "Ke xin", "Yu xi", "Meng yao", "Chan juan", "Hui fen"
        }
    };

    public static string[,] india =
    {
        //Male names
        {
            "Ram", "Mohammed", "Santosh", "Sanjay", "Sunil",
            "Rajesh", "Ramesh", "Ashok", "Manoj", "Anil"
        },
        //Female names
        {
            "Sunita", "Anita", "Gita", "Rekha", "Shanti",
            "Usha", "Asha", "Mina", "Laxmi", "Sita"
        }
    };

    public static string[,] japan =
    {
        //Male names
        {
            "Haruto", "Minato", "Riku", "Aoto", "Haruki",
            "Souta", "Sora", "Yuito", "Hinata", "Haru"
        },
        //Female names
        {
            "Sana", "Ema", "Mei", "Mio", "Tsumugi",
            "Sui", "Koharu", "Ena", "Nagisa", "Rio"
        }
    };

    public static string[,] malaysia =
    {
        //Male names
        {
            "Muhamad", "Ahmad", "Adam", "Amar", "Wan",
            "Iman", "Harraz", "Izz", "Umar", "Aariz"
        },
        //Female names
        {
            "Nur", "Wan", "Nurul", "Aisyah", "Maryam",
            "Siti", "Dhia", "Naura", "Ayra", "Puteri"
        }
    };

    public static string[,] mexico =
    {
        //Male names
        {
            "Mateo", "Matías", "Santiago", "Liam", "Leonardo",
            "Sebastián", "Alejandro", "Emiliano", "Dylan", "Diego"
        },
        //Female names
        {
            "Sofia", "Regina", "Valentina", "Victoria", "Isabella",
            "Camila", "Emma", "Julieta", "Romina", "Emily"
        }
    };

    public static string[,] philippines =
    {
        //Male names
        {
            "Nathaniel", "Jacob", "Ezekiel", "Gabriel", "Nathan",
            "Ethan", "Noah", "Liam", "James", "Matthew"
        },
        //Female names
        {
            "Althea", "Angel", "Samantha", "Princess", "Nathalie",
            "Chloe", "Sofia", "Zia", "Athena", "Sophia"
        }
    };

    public static string[,] russia =
    {
        //Male names
        {
            "Alexander", "Sergei", "Dmitry", "Andrei", "Alexey",
            "Maxim", "Evgeny", "Ivan", "Mikhail", "Artyom"
        },
        //Female names
        {
            "Anastasia", "Yelena", "Olga", "Natalia", "Yekaterina",
            "Anna", "Tatiana", "Maria", "Irina", "Yulia"
        }
    };

    public static string GetName(Country country, Sex sex)
    {
        int sexInt = (int)sex;
        var nameIndex = Random.Range(0, 10);


        return country switch
        {
            Country.China => china[sexInt, nameIndex],
            Country.India => india[sexInt, nameIndex],
            Country.Japan => japan[sexInt, nameIndex],
            Country.Malaysia => malaysia[sexInt, nameIndex],
            Country.Mexico => mexico[sexInt, nameIndex],
            Country.ThePhilippines => philippines[sexInt, nameIndex],
            Country.Russia => russia[sexInt, nameIndex],
            _ => (sex == Sex.Male) ? "John" : "Jane"
        };
    }
}
