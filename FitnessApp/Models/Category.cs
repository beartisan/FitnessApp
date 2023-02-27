﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        internal static void ForEach(Action<object> value)
        {
            throw new NotImplementedException();
        }
    }
    public class CategoriesDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}