using AppServer.Models;

namespace AppServer.Infrastructure.Container;

public class FileStateContainer
{
    private bool refresh = false;
    public bool Refresh
    {
        get => refresh;
        set
        {
            refresh = value;
            NotifyStateChanged();
        }
    }
    public string Component
    {
        get;
        set;
    }



    public event Action OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();



    public List<FileModel> Data { get; set; } = [];
    public void AddData(FileModel entity, string component)
    {
        Component = component;
        Data.Add(entity);
        Refresh = true;
    }
}