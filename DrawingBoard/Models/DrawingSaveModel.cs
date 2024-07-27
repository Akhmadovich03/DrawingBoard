namespace DrawingBoard.Models;

public class DrawingSaveModel
{
    public int BoardId { get; set; }
    public List<byte> DrawingData { get; set; }
    public List<DrawingElement> Shapes { get; set; }
}