using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using AristotelisThesis.Domain.Services;

namespace AristotelisThesis.WPF.Services
{
    /// <summary>
    /// Starts the Python face-embedding service (uvicorn) as a child process on app launch,
    /// unless one is already answering, and tears it down on exit. The model loads once in
    /// that process, so subsequent /encode calls are fast.
    /// </summary>
    public class PythonServiceLauncher : IDisposable
    {
        private readonly IFaceRecognitionService _faceRecognition;
        private Process? _process;

        public PythonServiceLauncher(IFaceRecognitionService faceRecognition)
        {
            _faceRecognition = faceRecognition;
        }

        /// <summary>
        /// Ensures the service is reachable: returns immediately if one is already up,
        /// otherwise spawns uvicorn and waits (with backoff) for /health. Returns false
        /// if the service could not be started/located.
        /// </summary>
        public async Task<bool> StartAsync()
        {
            if (await _faceRecognition.IsHealthyAsync())
            {
                return true; // already running (e.g. started manually for dev)
            }

            string? serviceDir = LocateServiceDirectory();
            if (serviceDir == null)
            {
                Debug.WriteLine("[PythonServiceLauncher] face_service directory not found.");
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
            startInfo.ArgumentList.Add(FaceServiceConfig.Host);
            startInfo.ArgumentList.Add("--port");
            startInfo.ArgumentList.Add(FaceServiceConfig.Port.ToString());

            try
            {
                _process = Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[PythonServiceLauncher] failed to start python: {ex.Message}");
                return false;
            }

            // Wait for the model to load and the server to come up (first launch is the slow one).
            for (int attempt = 0; attempt < 30; attempt++)
            {
                if (_process is { HasExited: true })
                {
                    Debug.WriteLine("[PythonServiceLauncher] python process exited during startup.");
                    return false;
                }
                if (await _faceRecognition.IsHealthyAsync())
                {
                    return true;
                }
                await Task.Delay(1000);
            }

            Debug.WriteLine("[PythonServiceLauncher] service did not become healthy in time.");
            return false;
        }

        /// <summary>
        /// Walks up from the app's base directory looking for a face_service/app.py.
        /// Works both from the bin output folder and a published layout.
        /// </summary>
        private static string? LocateServiceDirectory()
        {
            DirectoryInfo? dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string candidate = Path.Combine(dir.FullName, "face_service");
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
        /// PATH python, then the Windows "py" launcher (this machine exposes only "py").
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
