using Domain.Core;

namespace Domain.DataAccess
{
	public interface IDataAccess
	{
		List<PersonModel> GetPeople();
		PersonModel InsertPerson(string firstName, string lastName);
	}
}