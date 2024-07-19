using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Configuration
{
    public class JwtConfig
    {
        public string Secret {  get; set; }
    }
}
