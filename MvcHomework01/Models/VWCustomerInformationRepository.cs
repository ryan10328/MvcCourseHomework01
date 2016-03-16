using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MvcHomework01.Models
{   
	public  class VWCustomerInformationRepository : EFRepository<VWCustomerInformation>, IVWCustomerInformationRepository
	{

	}

	public  interface IVWCustomerInformationRepository : IRepository<VWCustomerInformation>
	{

	}
}