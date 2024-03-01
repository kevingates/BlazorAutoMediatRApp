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

		public List<PersonModel> GetAllPeople()
		{
			return people;
		}

		public PersonModel InsertPerson(string firstName, string lastName)
		{
			PersonModel person = new PersonModel { FirstName = firstName, LastName = lastName };
			person.Id = people.Max(x => x.Id) + 1;
			people.Add(person);
			people.Sort((x, y) => x.Id.CompareTo(y.Id));
			return person;
		}

		public PersonModel GetPerson(int id)
		{
			return people.Where(x => x.Id == id).FirstOrDefault();
		}

		public void DeletePerson(int id)
		{
			people.Remove(people.Where(x => x.Id == id).FirstOrDefault());
		}

		public PersonModel UpdatePerson(PersonModel person)
		{
			var p = people.Where(x => x.Id == person.Id).FirstOrDefault();
			people.Remove(p);
			p.FirstName = person.FirstName;
			p.LastName = person.LastName;
			people.Add(p);
			people.Sort((x, y) => x.Id.CompareTo(y.Id));
			return p;
		}

		public List<PersonModel> GetPeople(string filter)
		{
			return people.Where(x => x.FirstName.Contains(filter) || x.LastName.Contains(filter)).ToList();
		}
	}
}
