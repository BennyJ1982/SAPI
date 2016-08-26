namespace Facton.ServiceApi.Domain.Model.Entities
{
	using System;

	using Facton.Infrastructure.Entities;
	using Facton.Infrastructure.Metadata;
	using Facton.ServiceApi.Domain.Model.Core;

	public interface IStructuralPropertyFactory
	{
		bool TryCreate(IProperty property, out IStructuralProperty structuralProperty);

		bool TryCreate(IProperty property, string edmPropertyName, out IStructuralProperty structuralProperty);

		bool TryCreate(IProperty property, string edmPropertyName, bool canBeNull, out IStructuralProperty structuralProperty);

		bool TryCreate<T>(string edmPropertyName, Func<IEntity, T> valueGetter, bool canBeNull, out IStructuralProperty structuralProperty);

		bool TryCreate<T>(
			string edmPropertyName,
			Func<IEntity, T> valueGetter,
			Action<IEntity, T> valueSetter,
			bool canBeNull,
			out IStructuralProperty structuralProperty);
	}
}
