namespace BlazorAutoMediatR.MediatRPipelines
{
	public partial class AuthorizationBehavior<TRequest, TResponse>
	{
		public class UnknownUserException : Exception
		{
			public UnknownUserException(string message) : base(message) { }
		}
	}
}
