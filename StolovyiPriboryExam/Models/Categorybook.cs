using System;
using System.Collections.Generic;

namespace Library_project.Models;

public partial class Categorybook
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Book> Books { get; } = new List<Book>();
}
