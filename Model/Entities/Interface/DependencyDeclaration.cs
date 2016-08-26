namespace Facton.ServiceApi.Domain.Model.Entities
{
	public class DependencyDeclaration
	{
		public DependencyDeclaration(object dependableElement, bool isOptional)
		{
			this.DependableElement = dependableElement;
			this.IsOptional = isOptional;
		}

		public DependencyDeclaration(object dependableElement) : this(dependableElement, false)
		{
		}

		public object DependableElement { get; }

		public bool IsOptional { get; }
	}
}
