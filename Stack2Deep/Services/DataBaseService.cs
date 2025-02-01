using Stack2Deep.Dal;

namespace Stack2Deep.Services;

public abstract class DataBaseService
{
    public const Microsoft.EntityFrameworkCore.EntityState Detacted = Microsoft.EntityFrameworkCore.EntityState.Detached;
    
    protected readonly DataContext context;

    protected DataBaseService(DataContext dataContext)
    {
        context = dataContext;
    } 

    protected async Task<bool> AddEntity<T>(T entityToAdd) where T : Item
    {
        if (entityToAdd == null)
            return false;

        var addEntry = context.Add(entityToAdd);

        await context.SaveChangesAsync();
        addEntry.State = Detacted;
        return true;
    }

    protected async Task<bool> RemoveEntity<T>(T entityToRemove) where T : Item
    {
        if (entityToRemove == null)
            return false;

        context.Remove(entityToRemove);
        await context.SaveChangesAsync();

        return true;
    }
    
    protected async Task<bool> ModifyEntity<T>(T entity, Action<T> modification) where T : Item
    {
        modification.Invoke(entity);

        return await UpdateEntity(entity);
    }

    protected async Task<bool> UpdateEntity<T>(T entityToUpdate) where T : Item
    {
        if (entityToUpdate == null)
            return false;

        var updateEntry = context.Update(entityToUpdate);

        await context.SaveChangesAsync();
        updateEntry.State = Detacted;
        return true;
    }
}