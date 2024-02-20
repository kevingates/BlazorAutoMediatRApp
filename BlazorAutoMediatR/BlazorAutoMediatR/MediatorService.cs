using Domain.Core;
using MediatR;

namespace BlazorAutoMediatR
{
	public class MediatorService : IMediatorService
	{
		private readonly IMediator _mediator;

		public MediatorService(IMediator mediator)
		{
			_mediator = mediator;
		}

		public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
		{
			return _mediator.Send(request, cancellationToken);
		}
	}
}
