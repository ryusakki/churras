using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChurrasAPI.Models.Requests
{
    public class AuthRequest
    {
        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Senha { get; set; }
    }
}
