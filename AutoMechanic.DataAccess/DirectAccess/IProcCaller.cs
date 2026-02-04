
namespace AutoMechanic.DataAccess.DirectAccess
{
    public interface IProcCaller
    {
        Task<int> CallProc(string procName, object param = null, string connection = null);
        Task<T> CallProc<T>(string procName, object param = null, string connection = null);
    }
}