﻿using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Categories
{
    public class ChangeNameUpdateAction : UpdateAction<Category>
    {
        public string Action => "changeName";
        [Required]
        public LocalizedString Name { get; set; }
    }
}