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

        /// <summary>
        /// The moment the current session started (set on successful login, cleared on logout).
        /// Shared app-wide so any page can display the live session duration.
        /// </summary>
        DateTime? LoginTime { get; set; }
    }
}
