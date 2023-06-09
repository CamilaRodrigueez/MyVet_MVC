﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infraestructure.Entity.Models
{
    [Table("RolesPermissions", Schema = "Security")]
    public class RolPermissionEntity
    {
       [Key] 
        public int Id { get; set; }
        [ForeignKey("RolEntity")]
        public int IdRol { get; set; }
        public RolEntity RolEntity { get; set; }
        [ForeignKey("PermissionEntity")]
        public int IdPermission { get; set; }
        public PermissionEntity PermissionEntity { get; set; }
    }   
}
