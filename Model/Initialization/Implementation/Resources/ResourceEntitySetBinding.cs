namespace Facton.ServiceApi.Domain.Model.Initialization.Resources
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Domain.Resources;
	using Facton.Domain.Resources.Metadata;
	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Entities;
	using Facton.Infrastructure.Metadata;
	using Facton.ServiceApi.Domain.Model.Entities;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;

	/// <summary>
	/// Binding for resource entity sets.
	/// </summary>
	public class ResourceEntitySetBinding : IEntitySetBinding
	{
		private readonly IResourceService resourceService;

		private readonly ISignature abstractResourceSignature;

		private readonly IEntitySetBinding commonEntitySetBinding;

		public ResourceEntitySetBinding(
			IResourceService resourceService,
			ISignature abstractResourceSignature,
			IEntitySetBinding commonEntitySetBinding)
		{
			this.resourceService = resourceService;
			this.abstractResourceSignature = abstractResourceSignature;
			this.commonEntitySetBinding = commonEntitySetBinding;
		}

		public IEnumerable<IEntity> GetAll()
		{
			return this.commonEntitySetBinding.GetAll();
		}

		public bool TryGetByKeys(IEnumerable<KeyValuePair<string, object>> keys, out IEntity entity)
		{
			return this.commonEntitySetBinding.TryGetByKeys(keys, out entity);
		}

		public IEntity CreateAndAdd(IDictionary<string, IDependency> dependencies)
		{
			var resourceSignature = this.GetConreteResourceSignature(ExtractQuantityType(dependencies));

			IId entityId;
			if (dependencies.TryGetEntityId(out entityId))
			{
				if (this.resourceService.CanCreatePublicResource(resourceSignature, entityId))
				{
					return this.resourceService.CreatePublicResource(resourceSignature, entityId);
				}
			}
			else if (this.resourceService.CanCreatePublicResource(resourceSignature))
			{
				return this.resourceService.CreatePublicResource(resourceSignature);
			}

			throw new InvalidOperationException("Could not create resource.");
		}

		public void Delete(IEntity parentEntity, IEntity entityToDelete)
		{
			throw new NotImplementedException();
		}

		private IResourceSignature GetConreteResourceSignature(string quantityType)
		{
			var concreteSignature =
				this.abstractResourceSignature.As<IAbstractResourceSignature>()
					.ResourceSignatures.FirstOrDefault(s => s.QuantityType.Name == quantityType);

			if (concreteSignature == null)
			{
				throw new InvalidOperationException("QuantityType not supported.");
			}

			return concreteSignature;
		}

		private static string ExtractQuantityType(IDictionary<string, IDependency> dependencies)
		{
			IDependency quantityTypeDependency;
			if (dependencies.TryGetValue(FactonModelKeywords.QuantityTypePropertyName, out quantityTypeDependency))
			{
				return quantityTypeDependency.Value.ToString();
			}

			throw new InvalidOperationException("Quantity type not specified.");
		}
	}
}
