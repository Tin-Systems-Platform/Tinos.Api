using System;
using System.Collections.Generic;
using System.Text;
using Tinos.Api.Common.FS;
using Tinos.Api.Common.FS.Partitions;

namespace Tinos.Api.Examples.Fs
{
    public class FS
    {
        private static TinPartManager _partManager = new TinPartManager();

        public static void createPart()
        {
            TinPartManager.CreatePartition(0, 2048, 129024);
        }

        public static void deletePart()
        {
            _partManager.DeletePartition(0, 2048, 129024);
        }

        public static void initPartTable()
        {
            TinPartManager.InitializePartitionTable(0, PartitionTableKind.GPT);
        }

        public static void listParts()
        {
            _partManager.ListPartitions(0);
        }

        public static void resizePart()
        {
            _partManager.ResizePartition(0, 2048, 129024, 129000);
        }
    }
}
