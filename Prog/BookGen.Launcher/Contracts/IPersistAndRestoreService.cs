namespace BookGen.Launcher.Contracts.Services;

public interface IPersistAndRestoreService
{
    void RestoreData();

    void PersistData();
}
