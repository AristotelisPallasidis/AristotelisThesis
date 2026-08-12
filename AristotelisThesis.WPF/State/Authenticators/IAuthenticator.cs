using AristotelisThesis.Domain.Models;
using AristotelisThesis.Domain.Services.AuthenticationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AristotelisThesis.WPF.State.Authenticators
{
    public interface IAuthenticator
    {
        Account CurrentAccount { get; }
        bool IsLoggedIn { get; }

        /// <summary>The moment the current session started, or null when logged out.</summary>
        DateTime? LoginTime { get; }

        event Action StateChanged;

        /// <summary>
        /// Logs in the student whose enrolled face matches the given probe embedding.
        /// Returns true on a successful match (account set, check-in recorded), false otherwise.
        /// </summary>
        Task<bool> LoginWithFace(float[] probeEmbedding);

        /// <summary>
        /// Logs in the student whose enrolled palmprint matches the given probe embedding.
        /// Returns true on a successful match (account set, check-in recorded), false otherwise.
        /// </summary>
        Task<bool> LoginWithPalmprint(float[] probeEmbedding);

        void Logout();
    }
}
