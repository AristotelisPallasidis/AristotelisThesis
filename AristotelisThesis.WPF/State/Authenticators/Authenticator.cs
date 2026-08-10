using AristotelisThesis.Domain.Models;
using AristotelisThesis.Domain.Services;
using AristotelisThesis.Domain.Services.AuthenticationServices;
using AristotelisThesis.WPF.State.Accounts;

namespace AristotelisThesis.WPF.State.Authenticators
{
    public class Authenticator : IAuthenticator
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountStore _accountStore;
        private readonly ISessionTrackingService _sessionTrackingService;

        public Authenticator(IAuthenticationService authenticationService, IAccountStore accountStore, ISessionTrackingService sessionTrackingService)
        {
            _authenticationService = authenticationService;
            _accountStore = accountStore;
            _sessionTrackingService = sessionTrackingService;
        }

        public Account CurrentAccount
        {
            get
            {
                return _accountStore.CurrentAccount;
            }
            private set
            {
                _accountStore.CurrentAccount = value;
                StateChanged?.Invoke();
            }
        }

        /// <summary>
        /// This function is used to check if a student is logged in.
        /// </summary>
        public bool IsLoggedIn => CurrentAccount != null;

        /// <summary>
        /// The moment the current session started. Backed by the shared account store
        /// so it is the same value on every page while logged in.
        /// </summary>
        public DateTime? LoginTime => _accountStore.LoginTime;

        public event Action StateChanged;



        /// <summary>
        /// Logs in a student via face recognition: stamps the session start, exposes the
        /// account, and records the attendance check-in.
        /// </summary>
        /// <param name="probeEmbedding">The 128-d embedding of the captured face.</param>
        /// <returns>True when a matching enrolled face is found and the student is logged in.</returns>
        public async Task<bool> LoginWithFace(float[] probeEmbedding)
        {
            Account account = await _authenticationService.LoginWithFace(probeEmbedding);
            if (account == null)
            {
                return false;
            }

            _accountStore.LoginTime = DateTime.Now;
            CurrentAccount = account;

            if (CurrentAccount?.AccountHolder != null)
            {
                await _sessionTrackingService.RecordCheckIn(CurrentAccount.AccountHolder.Id);
            }

            return true;
        }

        /// <summary>
        /// Logs in a student via palmprint recognition. Mirrors <see cref="LoginWithFace"/>.
        /// </summary>
        public async Task<bool> LoginWithPalmprint(float[] probeEmbedding)
        {
            Account account = await _authenticationService.LoginWithPalmprint(probeEmbedding);
            if (account == null)
            {
                return false;
            }

            _accountStore.LoginTime = DateTime.Now;
            CurrentAccount = account;

            if (CurrentAccount?.AccountHolder != null)
            {
                await _sessionTrackingService.RecordCheckIn(CurrentAccount.AccountHolder.Id);
            }

            return true;
        }

        /// <summary>
        /// This function is used to logout a student from the system.
        /// </summary>
        public void Logout()
        {
            // Close the attendance session for the outgoing student before clearing state.
            int? studentId = CurrentAccount?.AccountHolder?.Id;
            if (studentId.HasValue)
            {
                // Run off the UI thread to avoid deadlocking on the async EF call.
                Task.Run(() => _sessionTrackingService.RecordCheckOut(studentId.Value)).GetAwaiter().GetResult();
            }

            _accountStore.LoginTime = null;
            CurrentAccount = null;
        }
    }
}
