Imports Sys = Cosmos.Kernel.System
Imports Tinos.Api.Common

''' <summary>
''' Main kernel class - inherits from Cosmos.Kernel.System.Kernel.
''' </summary>
Public Class Kernel
    Inherits Sys.Kernel

    Sub BootBanner(Version As String)
        Console.WriteLine("Tinos API Example - File System")
        Console.WriteLine($"Version: {Version}")
        Console.WriteLine("Author: Randomusert")
        Console.WriteLine("------------------------")
    End Sub

    Protected Overrides Sub BeforeRun()
        Console.WriteLine("Cosmos booted successfully!")
        Console.WriteLine("Type a command to get it executed.")
        Dim exammpleVersion = New Version(0, 1, 0)

        BootBanner(exammpleVersion.ToString())

    End Sub

    Protected Overrides Sub Run()
        Console.Write("> ")
        Dim input As String = Console.ReadLine()

        If String.IsNullOrEmpty(input) Then
            Return
        End If

        Select Case input.ToLower()
            Case "help"
                Console.WriteLine("Available commands:")
                Console.WriteLine("  help     - Show this help message")
                Console.WriteLine("  clear    - Clear the screen")
                Console.WriteLine("  halt     - Halt the system")

            Case "clear"
                Console.Clear()

            Case "halt"
                Console.WriteLine("Halting system...")
                Me.Stop()

            Case "createpartition"
                Dim fs As New FS()
                fs.CreatePartition()

            Case "deletepartition"
                Dim fs As New FS()
                fs.DeletePartition()

            Case "listpartitions"
                Dim fs As New FS()
                fs.ListPartitions()

            Case "resizepartition"
                Dim fs As New FS()
                fs.ResizePartition()

            Case "initpartitiontable"
                Dim fs As New FS()
                fs.InitPartitionTable()

            Case Else
                Console.WriteLine($"""{input}"" is not a command")
        End Select
    End Sub
End Class
