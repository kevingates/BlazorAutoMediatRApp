using Domain.Core;
using Domain.Core.Queries;
using Domain.DataAccess;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Handlers
{
	public class GetPersonListHandler : IRequestHandler<GetPersonListQuery, List<PersonModel>>
	{
		private readonly IDataAccess data;

		public GetPersonListHandler(IDataAccess data)
        {
			this.data = data;
		}
        public Task<List<PersonModel>> Handle(GetPersonListQuery request, CancellationToken cancellationToken)
		{
			return Task.FromResult(data.GetAllPeople());

			/*
			 * DB CONTEXT VERSION
			 * var result = _dbContext.Subscribers.Where(x => x.Id == request.Id).ToList();
			 * return result.Select(x => new SubscriberModel(x)).ToList();
			 * 
			 * 
			 * 
			 * REPO VERSION
			 * var filter = new Filter<Subscriber>();
			 * filter.Where(x => x.Id == request.Id);
			 * if (is not null)
			 * 
			 * var result = await _subscriberRepository.Get(filter);
			 * return result.ToList().Select(x=> new SubscriberModel(x)).ToList();
			 * 
			 */
		}
	}
}
