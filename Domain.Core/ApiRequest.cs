using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core
{
	public class ApiRequest
	{
        public string TypeFullName { get; set; }
        public IRequest<dynamic> Request { get; set; }
    }
}
