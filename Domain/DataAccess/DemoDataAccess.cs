using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DataAccess
{
	public class DemoDataAccess : IDataAccess
	{
		private List<PersonModel> people = new();

		public DemoDataAccess()
		{
			people.Add(new PersonModel { Id = 1, FirstName = "John", LastName = "Wick" });
			people.Add(new PersonModel { Id = 2, FirstName = "Sue", LastName = "Storm" });

		}

		public List<PersonModel> GetPeople()
		{
			return people;
		}

		public PersonModel InsertPerson(string firstName, string lastName)
		{
			PersonModel person = new PersonModel { FirstName = firstName, LastName = lastName };
			person.Id = people.Max(x => x.Id) + 1;
			people.Add(person);
			return person;
		}
	}
}
