using Cosmos.Kernel.HAL.Devices.Storage;
using Cosmos.Kernel.System.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos.Api.Common.FS.Partitions;

/// <summary>
/// A common implementation of a partition manager for Tinos file system.
/// </summary>
public class TinPartManager
{
    /// <summary>
    /// Lists all partitions available on the disk.
    /// Uses the Cosmos OS API to retrieve the list of partitions.
    /// </summary>
    public void ListPartitions(int diskIndex)
    {
        IBlockDevice? disk = GetDisk(diskIndex);

        if (disk is null)
            return;

        if (Gpt.IsGpt(disk))
        {
            var partitions = Gpt.Parse(disk);
            Console.WriteLine($"FS: GPT, {partitions.Count} partition(s)");
        }
        else if (Mbr.IsMbr(disk))
        {
            var partitions = Mbr.Parse(disk);
            Console.WriteLine($"FS: MBR, {partitions.Count} partition(s)");
        }
        else
        {
            Console.WriteLine("FS: No recognized partition table");
        }
    }

    /// <summary>
    /// Initializes a partition table on the specified disk with the given kind (GPT or MBR).
    /// WARNING: This operation is destructive and will erase all existing data on the disk. Use with caution.
    /// This shouldn't be used by apps directly, but rather through a higher-level API that the OS provides.
    /// </summary>
    /// <param name="diskIndex">The index of the disk on which to initialize the partition table.</param>
    /// <param name="kind">The kind of partition table to initialize.</param>
    /// <returns>true if the partition table is initialized successfully, false otherwise.</returns>
    public static bool InitializePartitionTable(
        int diskIndex,
        PartitionTableKind kind)
    {
        IBlockDevice? disk = GetDisk(diskIndex);

        if (disk is null)
            return false;

        if (Gpt.IsGpt(disk) || Mbr.IsMbr(disk))
        {
            Console.WriteLine("FS: A partition table already exists");
            return false;
        }

        try
        {
            switch (kind)
            {
                case PartitionTableKind.GPT:
                    Gpt.Create(disk);
                    break;

                case PartitionTableKind.MBR:
                    Mbr.Create(disk);
                    break;

                default:
                    Console.WriteLine("FS: Unsupported partition table type");
                    return false;
            }

            // Verify that the requested table now exists.
            bool initialized = kind switch
            {
                PartitionTableKind.GPT => Gpt.IsGpt(disk),
                PartitionTableKind.MBR => Mbr.IsMbr(disk),
                _ => false
            };

            if (!initialized)
            {
                Console.WriteLine("FS: Partition table initialization failed");
                return false;
            }

            StorageManager.RescanPartitions(disk);

            Console.WriteLine($"FS: Initialized {kind}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"FS: Partition table initialization failed: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Creates a partition using COSMOS APIs.
    /// </summary>
    /// <param name="diskIndex">The index of the disk, first disk is usually index 0</param>
    /// <param name="startSector">The sector where the created partition will start from</param>
    /// <param name="sectorCount">The number of sectors the partition will occupy</param>
    /// <param name="mbrSystemId">the MBR system ID for the partition</param>
    /// <returns>True if the partition was created successfully, false otherwise</returns>
    public static bool CreatePartition(
       int diskIndex,
       ulong startSector,
       ulong sectorCount,
       byte mbrSystemId = 0x0C)
    {
        IBlockDevice? disk = GetDisk(diskIndex);

        if (disk is null)
            return false;

        if (!Gpt.IsGpt(disk) && !Mbr.IsMbr(disk))
        {
            Console.WriteLine(
                "FS: Initialize a partition table first");
            return false;
        }

        if (!IsRangeWithinDisk(disk, startSector, sectorCount))
        {
            Console.WriteLine(
                "FS: Invalid partition range or range exceeds disk boundaries");
            return false;
        }

        try
        {
            if (startSector > disk.BlockCount)
                return false;

            ulong remainingSectors = disk.BlockCount - startSector;

            if (sectorCount > remainingSectors)
            {
                sectorCount = remainingSectors;
            }

            bool success = PartitionManager.Create(
                disk,
                startSector: startSector,
                sectorCount: sectorCount,
                mbrSystemId: mbrSystemId,
                gptType: Gpt.BasicDataPartitionType);

            if (!success)
            {
                Console.WriteLine(
                    "FS: Partition creation failed");
                return false;
            }

            StorageManager.RescanPartitions(disk);

            Console.WriteLine(
                $"FS: Created partition at sector {startSector}, " +
                $"size {sectorCount} sectors");

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"FS: Partition creation failed: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Deletes a partition using COSMOS APIs.
    /// </summary>
    /// <param name="diskIndex">the index of the disk containing the partition</param>
    /// <param name="startSector">start sector of the partition</param>
    /// <param name="sectorCount">number of sectors in the partition</param>
    /// <returns>true if the partition was deleted successfully, false otherwise</returns>
    public bool DeletePartition(
       int diskIndex,
       ulong startSector,
       ulong sectorCount)
    {
        IBlockDevice? disk = GetDisk(diskIndex);

        if (disk is null)
            return false;

        if (!IsRangeWithinDisk(disk, startSector, sectorCount))
        {
            Console.WriteLine("FS: Invalid partition range");
            return false;
        }

        try
        {
            PartitionManager.PartitionLocation location =
                new(startSector, sectorCount);

            if (!PartitionManager.Delete(disk, location))
            {
                Console.WriteLine("FS: Partition deletion failed");
                return false;
            }

            StorageManager.RescanPartitions(disk);

            Console.WriteLine("FS: Partition deleted");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"FS: Partition deletion failed: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Resizes a partition using COSMOS APIs.
    /// WARNING: This operation is destructive and may result in data loss. Use with caution.
    /// </summary>
    /// <param name="diskIndex">the index of the disk containing the partition</param>
    /// <param name="startSector">the start sector of the partition</param>
    /// <param name="oldSectorCount">the number of sectors in the existing partition</param>
    /// <param name="newSectorCount">the number of sectors in the new partition</param>
    /// <returns>true if the partition was resized successfully, false otherwise</returns>
    public bool ResizePartition(
        int diskIndex,
        ulong startSector,
        ulong oldSectorCount,
        ulong newSectorCount)
    {
        IBlockDevice? disk = GetDisk(diskIndex);

        if (disk is null)
            return false;

        if (!IsRangeWithinDisk(disk, startSector, oldSectorCount))
        {
            Console.WriteLine("FS: Invalid existing partition range");
            return false;
        }

        if (!IsRangeWithinDisk(disk, startSector, newSectorCount))
        {
            Console.WriteLine(
                "FS: New partition size exceeds disk boundaries");
            return false;
        }

        try
        {
            PartitionManager.PartitionLocation location =
                new(startSector, oldSectorCount);

            if (!PartitionManager.Resize(
                    disk, location, newSectorCount))
            {
                Console.WriteLine("FS: Partition resize failed");
                return false;
            }

            StorageManager.RescanPartitions(disk);

            Console.WriteLine(
                $"FS: Resized partition to {newSectorCount} sectors");

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"FS: Partition resize failed: {ex.Message}");
            return false;
        }
    }

    // Checks physical disk boundaries without overflowing ulong.
    private static bool IsRangeWithinDisk(
        IBlockDevice disk,
        ulong startSector,
        ulong sectorCount)
    {
        return sectorCount > 0
            && startSector > 0
            && startSector < disk.BlockCount
            && sectorCount <= disk.BlockCount - startSector;
    }

    private static IBlockDevice? GetDisk(int diskIndex)
    {
        if (diskIndex < 0 ||
            diskIndex >= StorageManager.DeviceCount)
        {
            Console.WriteLine("FS: Invalid disk index");
            return null;
        }

        IBlockDevice? disk = StorageManager.GetDevice(diskIndex);

        if (disk is null)
            Console.WriteLine("FS: No disk detected");

        return disk;
    }
}
