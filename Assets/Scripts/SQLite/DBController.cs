using System.Collections.Generic;
using UnityEngine;

public class DBController : MonoBehaviour
{
    private DataService dataService;

    public void CreateDatabase()
    {
        dataService = new DataService($"{GameConstants.DATABASE_NAME}.db");
    }

    public void CreateTable<T>(string name = "")
    {
        dataService?.CreateTable<T>();
    }

    public T Insert<T>(T model)
    {
        return dataService.Insert(model);
    }

    public void Delete<T>(T model)
    {
        dataService.Delete(model);
    }

    public void Modify<T>(T model)
    {
        dataService.Modify(model);
    }

    public IEnumerable<T> GetAll<T>() where T : class, new()
    {
        return dataService.GetAll<T>();
    }

    public T Get<T>(int id) where T : BaseModel, new()
    {
        return dataService.Get<T>(id);
    }
}
