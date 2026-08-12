using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace AristotelisThesis.WPF.Services
{
    /// <summary>
    /// Starts a Python FastAPI service (uvicorn) as a child process on app launch, unless one is
    /// already answering, and tears it down on exit. Config-driven so it can run any of the app's
    /// Python services (face on 8500, palmprint on 8501) - the model loads once in that process.
    /// </summary>
    public class PythonServiceLauncher : IDisposable
    {
        private readonly string _serviceDirName;
        private readonly string _host;
        private readonly int _port;
        private readonly Func<Task<bool>> _healthCheck;
        private Process? _process;

        public PythonServiceLauncher(string serviceDirName, string host, int port, Func<Task<bool>> healthCheck)
        {
            _serviceDirName = serviceDirName;
            _host = host;
            _port = port;
            _healthCheck = healthCheck;
        }

        /// <summary>
        /// Ensures the service is reachable: returns immediately if one is already up, otherwise
        /// spawns uvicorn and waits (with backoff) for the health check. Returns false if the
        /// service could not be started/located.
        /// </summary>
        public async Task<bool> StartAsync()
        {
            if (await _healthCheck())
            {
                return true; // already running (e.g. started manually for dev)
            }

            string? serviceDir = LocateServiceDirectory(_serviceDirName);
            if (serviceDir == null)
            {
                Debug.WriteLine($"[PythonServiceLauncher] {_serviceDirName} directory not found.");
                return false;
            }

            string python = ResolvePythonExecutable(serviceDir);

            var startInfo = new ProcessStartInfo
            {
                FileName = python,
                WorkingDirectory = serviceDir,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            startInfo.ArgumentList.Add("-m");
            startInfo.ArgumentList.Add("uvicorn");
            startInfo.ArgumentList.Add("app:app");
            startInfo.ArgumentList.Add("--host");
            startInfo.ArgumentList.Add(_host);
            startInfo.ArgumentList.Add("--port");
            startInfo.ArgumentList.Add(_port.ToString());

            try
            {
                _process = Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[PythonServiceLauncher] failed to start {_serviceDirName}: {ex.Message}");
                return false;
            }

            // Wait for the model to load and the server to come up (first launch is the slow one).
            for (int attempt = 0; attempt < 30; attempt++)
            {
                if (_process is { HasExited: true })
                {
                    Debug.WriteLine($"[PythonServiceLauncher] {_serviceDirName} process exited during startup.");
                    return false;
                }
                if (await _healthCheck())
                {
                    return true;
                }
                await Task.Delay(1000);
            }

            Debug.WriteLine($"[PythonServiceLauncher] {_serviceDirName} did not become healthy in time.");
            return false;
        }

        /// <summary>
        /// Walks up from the app's base directory looking for &lt;serviceDirName&gt;/app.py.
        /// </summary>
        private static string? LocateServiceDirectory(string serviceDirName)
        {
            DirectoryInfo? dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string candidate = Path.Combine(dir.FullName, serviceDirName);
                if (File.Exists(Path.Combine(candidate, "app.py")))
                {
                    return candidate;
                }
                dir = dir.Parent;
            }
            return null;
        }

        /// <summary>
        /// Prefers the service's own virtualenv interpreter if present, else falls back to a
        /// PATH python, then the Windows "py" launcher.
        /// </summary>
        private static string ResolvePythonExecutable(string serviceDir)
        {
            string venvPython = Path.Combine(serviceDir, "venv", "Scripts", "python.exe");
            if (File.Exists(venvPython))
            {
                return venvPython;
            }
            string dotVenvPython = Path.Combine(serviceDir, ".venv", "Scripts", "python.exe");
            if (File.Exists(dotVenvPython))
            {
                return dotVenvPython;
            }
            if (CommandExists("python"))
            {
                return "python";
            }
            return "py"; // Windows Python launcher
        }

        private static bool CommandExists(string command)
        {
            try
            {
                using var probe = Process.Start(new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = "--version",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                });
                probe?.WaitForExit(3000);
                return probe != null;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            try
            {
                if (_process is { HasExited: false })
                {
                    _process.Kill(entireProcessTree: true);
                    _process.WaitForExit(3000);
                }
            }
            catch
            {
                // best-effort shutdown
            }
            finally
            {
                _process?.Dispose();
                _process = null;
            }
        }
    }
}
