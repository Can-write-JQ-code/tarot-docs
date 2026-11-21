using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

internal static class Program
{
    private static string Quote(string value)
    {
        if (value.Length > 0 && value.IndexOfAny(new[] { ' ', '\t', '"', '\\' }) < 0) return value;
        var result = new StringBuilder("\"");
        var slashes = 0;
        foreach (var ch in value)
        {
            if (ch == '\\') { slashes++; continue; }
            if (ch == '"') { result.Append('\\', slashes * 2 + 1); result.Append('"'); slashes = 0; continue; }
            result.Append('\\', slashes); slashes = 0; result.Append(ch);
        }
        result.Append('\\', slashes * 2); result.Append('"');
        return result.ToString();
    }

    public static int Main(string[] args)
    {
        var real = Environment.GetEnvironmentVariable("TAROT_REAL_FFMPEG");
        if (String.IsNullOrWhiteSpace(real)) real = @"C:\Users\12144\.local\bin\ffmpeg.exe";
        var converted = new string[args.Length];
        for (var i = 0; i < args.Length; i++)
        {
            converted[i] = args[i].Replace("pcm_f32le", "pcm_s16le");
            converted[i] = Regex.Replace(converted[i], @"setparams=[^,;]+", "copy");
        }
        var log = Environment.GetEnvironmentVariable("TAROT_FFMPEG_COMPAT_LOG");
        if (converted.Length > 0 && converted[converted.Length - 1].EndsWith(".m4a", StringComparison.OrdinalIgnoreCase))
        {
            var withFormat = new List<string>(converted);
            var outputIndex = withFormat.Count - 1;
            withFormat.Insert(outputIndex, "-f");
            withFormat.Insert(outputIndex + 1, "mp4");
            converted = withFormat.ToArray();
        }
        if (!String.IsNullOrWhiteSpace(log)) File.AppendAllText(log, String.Join(" ", converted) + Environment.NewLine);
        var start = new ProcessStartInfo(real) { UseShellExecute = false, Arguments = String.Join(" ", Array.ConvertAll(converted, Quote)) };
        var peakMode = Array.Exists(converted, value => value.Contains("ebur128=peak=true"));
        if (peakMode)
        {
            start.Arguments = String.Join(" ", Array.ConvertAll(converted, value => Quote(value.Replace("ebur128=peak=true", "loudnorm=print_format=json"))));
            start.RedirectStandardError = true;
            start.RedirectStandardOutput = true;
            using (var process = Process.Start(start))
            {
                var stderr = process.StandardError.ReadToEnd();
                var stdout = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                Console.Error.Write(stderr);
                Console.Out.Write(stdout);
                var match = Regex.Match(stderr, "\"input_tp\"\\s*:\\s*\"([^\"]+)\"");
                if (process.ExitCode == 0 && match.Success)
                {
                    Console.Error.WriteLine("FTPK:" + match.Groups[1].Value + " dBFS");
                    Console.Error.WriteLine("  Peak: " + match.Groups[1].Value + " dBFS");
                }
                return process.ExitCode;
            }
        }
        using (var process = Process.Start(start)) { process.WaitForExit(); return process.ExitCode; }
    }
}
