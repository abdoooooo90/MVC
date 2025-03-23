﻿using IKEA.DAL.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Common.Services.EmailSettings
{
	public interface IEmailSettings
	{
		public void SendEmail(Email email);
	}
}
