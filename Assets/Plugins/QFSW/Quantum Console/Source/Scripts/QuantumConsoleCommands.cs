using QFSW.QC.Suggestors.Tags;
using QFSW.QC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QFSW.QC
{
    public static partial class QuantumConsoleProcessor
    {
        /// <summary>
        /// Gets all loaded unique commands. Unique excludes multiple overloads of the same command from appearing.
        /// </summary>
        /// <returns>All loaded unique commands.</returns>
        public static IEnumerable<CommandData> GetUniqueCommands()
        {
            return GetAllCommands()
                .DistinctBy(x => x.CommandName)
                .OrderBy(x => x.CommandName);
        }
    }
}
