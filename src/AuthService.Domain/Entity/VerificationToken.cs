﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entity
{
    public class VerificationToken
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set;}
        public string ActionType { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
