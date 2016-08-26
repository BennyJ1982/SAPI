namespace Facton.ServiceApi.Domain.Model.Entities
{
	using Facton.ServiceApi.Domain.Model.Core;

	public class PropertyDependency : IDependency
	{
		private readonly IStructuralProperty property;

		public PropertyDependency(IStructuralProperty property, object value)
		{
			this.property = property;
			this.Value = value;
		}

		public object DependableElement => this.property;

		public object Value { get; }
	}
}