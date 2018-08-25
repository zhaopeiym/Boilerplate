using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Boilerplate.Core.Entitys
{
    [Table("Boilerplates")]
    public class BoilerplatePO
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public DateTime LastModificationTime { get; set; } = DateTime.Now;
        public int CreationNumber { get; set; }
        public int DownloadNumber { get; set; }
    }
}
