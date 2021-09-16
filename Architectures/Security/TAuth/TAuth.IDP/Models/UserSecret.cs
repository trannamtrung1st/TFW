﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAuth.IDP.Models
{
    public class UserSecret
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; }

        [Required]
        public string Secret { get; set; }

        [Required]
        public string UserId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual AppUser User { get; set; }
    }
}
