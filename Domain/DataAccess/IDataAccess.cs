using Domain.Core;

namespace Domain.DataAccess
{
	public interface IDataAccess
	{
		void DeletePerson(int id);
		List<PersonModel> GetAllPeople();
		List<PersonModel> GetPeople(string filter);
		PersonModel GetPerson(int id);
		PersonModel InsertPerson(string firstName, string lastName);
		PersonModel UpdatePerson(PersonModel person);
	}
}