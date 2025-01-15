using System.Collections.Generic;
using UnityEngine;

public class IdleGameManager : MonoBehaviour
{
    public static IdleGameManager instance;

    public List<Factory> factories;
    public GameObject factoryPrefab;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("instance of" + GetType() + " already exists");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddFactory();
        }
    }


    public void AddFactory()
    {
        Debug.Log("New factory!");
        var newFactory = Instantiate(factoryPrefab);

        factories.Add(newFactory.GetComponent<Factory>());
        SaveDataManager.instance.localPlayerData.factories = factories;
        SaveDataManager.SavePlayer();
    }

    public void LoadFactory(int i)
    {
        var newFactory = Instantiate(factoryPrefab);
        newFactory.GetComponent<Factory>().LoadData(SaveDataManager.instance.localPlayerData.factories[i]);

        factories.Add(newFactory.GetComponent<Factory>());
        SaveDataManager.instance.localPlayerData.factories = factories;
        SaveDataManager.SavePlayer();
    }
}
