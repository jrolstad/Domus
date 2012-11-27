namespace Domus.Commands
{
    public interface ICommand<in TRequest, out TResponse>
    {
        TResponse Execute(TRequest request);
    }
}