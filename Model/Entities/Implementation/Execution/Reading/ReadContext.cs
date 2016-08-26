namespace Facton.ServiceApi.Domain.Model.Entities.Execution.Reading
{
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Infrastructure.Entities;

	public class ReadContext
	{
		public static readonly IEnumerable<IEntity> EmptyResult = Enumerable.Empty<IEntity>();

		private IEnumerable<IEntity> result;

		public ReadContext(IEnumerable<IEntity> result, IBindableModelContext modelContext)
		{
			this.Model = modelContext;
			this.result = result;
		}

		public ReadContext(IBindableModelContext modelContext)
		{
			this.result = EmptyResult;
			this.Model = modelContext;
		}

		public IBindableModelContext Model { get; private set; }

		public IEnumerable<IEntity> Result
		{
			get
			{
				return this.result;
			}
		}

		internal void SetResult(IEnumerable<IEntity> entities)
		{
			this.result = entities;
		}
	}
}
