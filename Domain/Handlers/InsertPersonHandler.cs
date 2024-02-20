using Domain.Core;
using Domain.Core.Commands;
using Domain.DataAccess;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Handlers
{
	public class InsertPersonHandler : IRequestHandler<InsertPersonCommand,PersonModel>
	{
		private readonly IDataAccess _dataAccess;

		public InsertPersonHandler(IDataAccess dataAccess)
        {
			_dataAccess = dataAccess;
		}

		public async Task<PersonModel> Handle(InsertPersonCommand request, CancellationToken cancellationToken)
		{
			var newPerson = _dataAccess.InsertPerson(request.FirstName, request.LastName);
			return newPerson;

			/*
			 * _dbContext.People.Add(new Person { FirstName = request.FirstName, LastName = request.LastName });
			 * _dbContext.SaveChanges();
			 * 
			 */
		}
	}
}
