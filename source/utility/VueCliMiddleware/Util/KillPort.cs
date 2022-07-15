using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VueCliMiddleware
{
    public static class PidUtils
    {

        const string ssPidRegex = @"(?:^|"",|"",pid=)(\d+)";
        const string portRegex = @"[^]*[.:](\\d+)$";

        public static int GetPortPid(ushort port)
        {
            int pidOut = -1;

            int portColumn = 1; // windows
            int pidColumn = 4;  // windows
            string pidRegex = null;

            List<string[]> results = null;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                results = RunProcessReturnOutputSplit("netstat", "-anv -p tcp");
                results.AddRange(RunProcessReturnOutputSplit("netstat", "-anv -p udp"));
                portColumn = 3;
                pidColumn = 8;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                results = RunProcessReturnOutputSplit("ss", "-tunlp");
                portColumn = 4;
                pidColumn = 6;
                pidRegex = ssPidRegex;
            }
            else
            {
                results = RunProcessReturnOutputSplit("netstat", "-ano");
            }


            foreach (var line in results)
            {
                if (line.Length <= portColumn || line.Length <= pidColumn) continue;
                try
                {
                    // split lines to words
                    var portMatch = Regex.Match(line[portColumn], $"[.:]({port})");
                    if (portMatch.Success)
                    {
                        int portValue = int.Parse(portMatch.Groups[1].Value);

                        if (pidRegex == null)
                        {
                            pidOut = int.Parse(line[pidColumn]);
                            return pidOut;
                        }
                        else
                        {
                            var pidMatch = Regex.Match(line[pidColumn], pidRegex);
                            if (pidMatch.Success)
                            {
                                pidOut = int.Parse(pidMatch.Groups[1].Value);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // ignore line error
                }
            }

            return pidOut;
        }

        private static List<string[]> RunProcessReturnOutputSplit(string fileName, string arguments)
        {
            string result = RunProcessReturnOutput(fileName, arguments);
            if (result == null) return new List<string[]>();

            string[] lines = result.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var lineWords = new List<string[]>();
            foreach (var line in lines)
            {
                lineWords.Add(line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            }
            return lineWords;
        }

        private static string RunProcessReturnOutput(string fileName, string arguments)
        {
            Process process = null;
            try
            {
                var si = new ProcessStartInfo(fileName, arguments)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                process = Process.Start(si);
                var stdOutT = process.StandardOutput.ReadToEndAsync();
                var stdErrorT = process.StandardError.ReadToEndAsync();
                if (!process.WaitForExit(10000))
                {
                    try { process?.Kill(); } catch { }
                }

                if (Task.WaitAll(new Task[] { stdOutT, stdErrorT }, 10000))
                {
                    // if success, return data
                    return (stdOutT.Result + Environment.NewLine + stdErrorT.Result).Trim();                   
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                process?.Close();
            }
        }

        public static bool Kill(string process, bool ignoreCase = true, bool force = false, bool tree = true)
        {
            var args = new List<string>();
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    if (force) { args.Add("-9"); }
                    if (ignoreCase) { args.Add("-i"); }
                    args.Add(process);
                    RunProcessReturnOutput("pkill", string.Join(" ", args));
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    if (force) { args.Add("-9"); }
                    if (ignoreCase) { args.Add("-I"); }
                    args.Add(process);
                    RunProcessReturnOutput("killall", string.Join(" ", args));
                }
                else
                {
                    if (force) { args.Add("/f"); }
                    if (tree) { args.Add("/T"); }
                    args.Add("/im");
                    args.Add(process);
                    return RunProcessReturnOutput("taskkill", string.Join(" ", args))?.StartsWith("SUCCESS") ?? false;
                }
                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }

        public static bool Kill(int pid, bool force = false, bool tree = true)
        {
            if (pid == -1) { return false; }

            var args = new List<string>();
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    if (force) { args.Add("-9"); }
                    args.Add(pid.ToString());
                    RunProcessReturnOutput("kill", string.Join(" ", args));
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    if (force) { args.Add("-9"); }
                    args.Add(pid.ToString());
                    RunProcessReturnOutput("kill", string.Join(" ", args));
                }
                else
                {
                    if (force) { args.Add("/f"); }
                    if (tree) { args.Add("/T"); }
                    args.Add("/PID");
                    args.Add(pid.ToString());
                    return RunProcessReturnOutput("taskkill", string.Join(" ", args))?.StartsWith("SUCCESS") ?? false;
                }
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public static bool KillPort(ushort port, bool force = false, bool tree = true) => Kill(GetPortPid(port), force: force, tree: tree);

    }


}