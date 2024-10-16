using AristotelisThesis.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AristotelisThesis.WPF.State.Accounts
{
    /// <summary>
    /// This is Store the current account of the application.
    /// </summary>
    public interface IAccountStore
    {
        Account CurrentAccount { get; set; }
    }
}
