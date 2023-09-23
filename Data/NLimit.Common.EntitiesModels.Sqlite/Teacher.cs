﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.NLimit.Common.EntitiesModels.Sqlite;

public partial class Teacher
{
    [Key]
    public int TeacherId { get; set; }

    [Column(TypeName = "nvarchar (20)")]
    public string LastName { get; set; } = null!;

    [Column(TypeName = "nvarchar (20)")]
    public string FirstName { get; set; } = null!;

    [Column(TypeName = "nvarchar (20)")]
    public string? MiddleName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BirthDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "nvarchar (60)")]
    public string? Address { get; set; }

    [Column(TypeName = "nvarchar (15)")]
    public string? City { get; set; }

    [Column(TypeName = "nvarchar (15)")]
    public string? Region { get; set; }

    [Column(TypeName = "nvarchar (15)")]
    public string? Country { get; set; }

    [Column(TypeName = "nvarchar (24)")]
    public string? HomePhone { get; set; }

    [Column(TypeName = "nvarchar (24)")]
    public string? MobilePhone { get; set; }

    [Column(TypeName = "nvarchar (24)")]
    public string? Email { get; set; }

    [Column(TypeName = "image")]
    public byte[]? Photo { get; set; }

    [Column(TypeName = "ntext")]
    public string? Notes { get; set; }

    [Column(TypeName = "INT")]
    public int? ReportsTo { get; set; }

    [Column(TypeName = "nvarchar (255)")]
    public string? PhotoPath { get; set; }

    [InverseProperty("Teacher")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    [InverseProperty("Teacher")]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    [InverseProperty("Teacher")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
