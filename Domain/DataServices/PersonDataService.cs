using Domain.Core;
using Domain.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DataServices
{
	public class PersonDataService
	{
		private readonly IDataAccess _dataAccess;

		public PersonDataService(IDataAccess dataAccess)
        {
			_dataAccess = dataAccess;
		}

		public List<PersonModel> GetAllPeople()
		{
			return _dataAccess.GetAllPeople();
		}

		public PersonModel InsertPerson(string firstName, string lastName)
		{
			return _dataAccess.InsertPerson(firstName, lastName);
		}

		public PersonModel GetPerson(int id)
		{
			return _dataAccess.GetPerson(id);
		}

		public void DeletePerson(int id)
		{
			_dataAccess.DeletePerson(id);
		}

		public PersonModel UpdatePerson(PersonModel person)
		{
			return _dataAccess.UpdatePerson(person);
		}

		public List<PersonModel> GetPeople(string filter)
		{
			return _dataAccess.GetPeople(filter);
		}

		public List<PersonModel> ReplacePeopleInList(List<PersonModel> people)
		{
			var oldPeople = GetAllPeople();
            foreach (var person in oldPeople)
            {
				DeletePerson(person.Id);
            }

			foreach (var person in people)
			{ 
				InsertPerson(person.FirstName, person.LastName);
			}
			return GetAllPeople();
        }
    }
}
