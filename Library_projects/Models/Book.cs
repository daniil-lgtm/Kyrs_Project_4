using System;
using System.Collections.Generic;

namespace Library_project.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Avtor { get; set; } = null!;

    public int Manufacturer { get; set; }

    public DateOnly YearBook { get; set; }

    public int Category { get; set; }

    public int QuantityPages { get; set; }

    public int Quantity { get; set; }

    public virtual Categorybook CategoryNavigation { get; set; } = null!;

    public virtual Manufacturer ManufacturerNavigation { get; set; } = null!;

    public virtual ICollection<Recordbook> Recordbooks { get; } = new List<Recordbook>();
}
