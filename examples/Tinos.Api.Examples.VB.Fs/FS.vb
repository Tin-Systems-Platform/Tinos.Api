Imports Tinos.Api.Common.FS
Imports Tinos.Api.Common.FS.Partitions


Public Class FS
    Public Sub CreatePartition()
        TinPartManager.CreatePartition(0, 2048, 129024)
    End Sub

    Public Sub DeletePartition()
        Dim partManager As New TinPartManager()
        partManager.DeletePartition(0, 2048, 129024)
    End Sub

    Public Sub ListPartitions()
        Dim partManager As New TinPartManager()
        partManager.ListPartitions(0)
    End Sub

    Public Sub InitPartitionTable()
        TinPartManager.InitializePartitionTable(0, PartitionTableKind.GPT)
    End Sub

    Public Sub ResizePartition()
        Dim partManager As New TinPartManager()
        partManager.ResizePartition(0, 2048, 129024, 129000)
    End Sub
End Class
