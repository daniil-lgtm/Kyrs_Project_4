using System;
using System.Collections.Generic;

namespace Library_project.Models;

public partial class Recordbook
{
    public int UsersId { get; set; }

    public int BookId { get; set; }

    public DateTime DataTimeTake { get; set; }

    public int Quantity { get; set; }

    public DateTime DataTimeReturn { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User Users { get; set; } = null!;
}
