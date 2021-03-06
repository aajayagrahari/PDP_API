﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Models
{
    [Table("tGeneratorAvailableAlternator")]
    public class GeneratorAvailableAlternator : IEntity
    {
        public int ID { get; set; }

        public int GeneratorID { get; set; }

        public int AlternatorID { get; set; }

        public virtual Generator Generator { get; set; }

        public virtual Alternator Alternator { get; set; }

        public string CreatedBy { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifiedDateTime { get; set; }
    }
}
