namespace Facton.ServiceApi.Domain.Model.Core.Queries
{
    public interface IQueryAttribute
    {
        string FqlQueryTextFragment { get; }
    }
}
