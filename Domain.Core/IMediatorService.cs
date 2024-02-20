using MediatR;

namespace Domain.Core
{
	public interface IMediatorService
	{
		Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
	}
}