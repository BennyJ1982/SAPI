namespace Facton.ServiceApi.Domain.Model.Initialization.Common.QueryAttributes
{
	using Facton.Infrastructure.Metadata;
	using Facton.ServiceApi.Domain.Model.Core.Queries;

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
