using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos.Api.Common.FS.Partitions
{
    /// <summary>
    /// Represents the kind of partition table to be used when initializing a disk.
    /// </summary>
    public enum PartitionTableKind
    {
        GPT,
        MBR,
    }
}
