namespace Facton.Spikes.ServiceApi.ODataMapping.Model.QueryProviders.QueryAttributes
{
	using Facton.Infrastructure.Metadata;
	using Facton.Spikes.ServiceApi.ODataMapping.Queries;

	public class SignatureQueryAttribute : IQueryAttribute
	{
		public SignatureQueryAttribute(ISignature signature)
		{
			this.Signature = signature;
		}

		public ISignature Signature { get; }

		public string FqlQueryTextFragment => $"SIGNATURE {this.Signature.Name}";
	}
}
