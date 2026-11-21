    using System;
    using Sys = Cosmos.Kernel.System;
    using Tinos.Api.Common.Utils;
    using Version = Tinos.Api.Common.Utils.Version;


    namespace Tinos.Api.Examples.Fs;

    /// <summary>
    /// Main kernel class - inherits from Cosmos.Kernel.System.Kernel.
    /// </summary>
    public class Kernel : Sys.Kernel
    {
        private void StartupBanner(Version ver)
        {
            Console.WriteLine("Tinos API Example - File System");
            Console.WriteLine($"Version: {ver}");
            Console.WriteLine("Author: Randomusert");
            Console.WriteLine();
        }

        protected override void BeforeRun()
        {
            Console.WriteLine("Cosmos booted successfully!");
            Console.WriteLine("Type a command to get it executed.");
            Version FsExampleVer = new Version(0, 1, 0);

            StartupBanner(FsExampleVer);

        
        }

        protected override void Run()
        {
            Console.Write("> ");
            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
                return;

            switch (input.ToLower())
            {
                case "help":
                    Console.WriteLine("Available commands:");
                    Console.WriteLine("  help     - Show this help message");
                    Console.WriteLine("  clear    - Clear the screen");
                    Console.WriteLine("  halt     - Halt the system");
                    Console.WriteLine("  createpart - Create a partition");
                    Console.WriteLine("  deletepart - Delete a partition");
                    Console.WriteLine("  initparttable - Initialize partition table");
                    Console.WriteLine("  listparts - List partitions");
                    Console.WriteLine("  resizepart - Resize a partition");
                    break;

                case "clear":
                    Console.Clear();
                    break;

                case "halt":
                    Console.WriteLine("Halting system...");
                    Stop();
                    break;

                case "createpart":
                    FS.createPart();
                    break;

                case "deletepart":
                    FS.deletePart();
                    break;

                case "initparttable":
                    FS.initPartTable();
                    break;

                case "listparts":
                    FS.listParts();
                    break;

                case "resizepart":
                    FS.resizePart();
                    break;

                default:
                    Console.WriteLine($"\"{input}\" is not a command");
                    break;
            }
        }
    }
