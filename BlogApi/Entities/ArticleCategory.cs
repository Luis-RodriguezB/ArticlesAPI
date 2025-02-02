﻿using BlogApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ArticlesAPI.Entities;
public class ArticleCategory
{
    [Required]
    public int ArticleId { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public virtual Article Article { get; set; }
    public virtual Category Category { get; set; }
}
