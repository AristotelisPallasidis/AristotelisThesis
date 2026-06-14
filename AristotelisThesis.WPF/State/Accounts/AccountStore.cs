using AristotelisThesis.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AristotelisThesis.WPF.State.Accounts
{
    public class AccountStore : IAccountStore
    {
        public Account CurrentAccount { get; set; }

        public DateTime? LoginTime { get; set; }
    }
}
