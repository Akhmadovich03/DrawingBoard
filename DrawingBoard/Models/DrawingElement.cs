using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrawingBoard.Models;

public class DrawingElement
{
    public int Id { get; set; }

    [Required]
    public string Tool { get; set; }

    [Required]
    public string Color { get; set; }

    [Required]
    public int LineWidth { get; set; }

    [Required]
    public float StartX { get; set; }

    [Required]
    public float StartY { get; set; }

    [Required]
    public float EndX { get; set; }

    [Required]
    public float EndY { get; set; }

    [ForeignKey("Board")]
    public int BoardId { get; set; }

    public Board Board { get; set; }
}