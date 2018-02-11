using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace MPQNet.UnitTests
{
    public static class MPYQ
    {
        public static dynamic Instance { get; }

        static MPYQ()
        {
            var engine = Python.CreateEngine();
            var searchPath = engine.GetSearchPaths();
            searchPath.Add(Path.Combine(Environment.CurrentDirectory, @"Lib\"));
            engine.SetSearchPaths(searchPath);
            Instance = engine.ExecuteFile("mpyq.py");
        }
    }
}
