using MediatR;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Commands
{
    [Authorize(Policy = "CEO")]
	[Authorize(Policy = "Admin")]
	[Authorize(Policy = "Shrek")]
	public class InsertPersonCommand : IRequest<PersonModel>
    {
        [Required]
        [MinLength(length: 2)]
        public string FirstName { get; set; }
        [Required]
		[MinLength(length: 2)]
		public string LastName { get; set; }
    }
}
