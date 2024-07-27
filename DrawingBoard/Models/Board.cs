using System.ComponentModel.DataAnnotations;

namespace DrawingBoard.Models;

public class Board
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public byte[] DrawingData { get; set; } = [];
}