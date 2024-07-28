using DrawingBoard.Data;
using DrawingBoard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using System.Text.Json;
using System.Net.Mime;
using Microsoft.AspNetCore.SignalR;

namespace DrawingBoard.Controllers;

public class BoardController(BoardDbContext context) : Controller
{
    private readonly BoardDbContext _context = context;

    public async Task<IActionResult> Index()
    {
        var boards = await _context.Boards.ToListAsync();
        var images = new List<string>();

        foreach (var board in boards)
        {
            if (board.DrawingData != null && board.DrawingData.Length > 0)
            {
                string base64Data = Convert.ToBase64String(board.DrawingData);
                string imgDataURL = string.Format("data:image/png;base64,{0}", base64Data);
                images.Add(imgDataURL);
            }
            else
            {
                images.Add("data:image/png;base64,DEFAULT_BASE64_IMAGE_STRING");
            }
        }

        return View((boards, images));
    }


    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Board board)
    {
        if (ModelState.IsValid)
        {
            _context.Add(board);
            await _context.SaveChangesAsync();

            await GenerateThumbnail(board.Id, new List<DrawingElement>());

            return RedirectToAction("Draw", new { id = board.Id });
        }
        return View(board);
    }

    public async Task<IActionResult> Draw(int id)
    {
        var board = await _context.Boards.FindAsync(id);

        if (board == null)
        {
            return NotFound();
        }

        var drawingElements = await _context.DrawingElements
            .Where(de => de.BoardId == id)
            .ToListAsync();

        string base64Data = Convert.ToBase64String(board.DrawingData);
        string imgDataURL = string.Format("data:image/png;base64,{0}", base64Data);

        ViewBag.ImageData = imgDataURL;
        ViewBag.BoardId = id;
        ViewBag.DrawingElements = drawingElements;

        return View(board);
    }

    [HttpPost]
    public async Task<IActionResult> SaveDrawing([FromBody] DrawingSaveModel model)
    {
        if (model == null)
        {
            return BadRequest();
        }

        var board = await _context.Boards.FindAsync(model.BoardId);

        if (board == null)
        {
            return NotFound();
        }

        board.DrawingData = model.DrawingData.ToArray();

        var existingElements = _context.DrawingElements.Where(de => de.BoardId == model.BoardId);
        _context.DrawingElements.RemoveRange(existingElements);

        var drawingElements = model.Shapes.Select(shape => new DrawingElement
        {
            Tool = shape.Tool,
            Color = shape.Color,
            LineWidth = shape.LineWidth,
            StartX = shape.StartX,
            StartY = shape.StartY,
            EndX = shape.EndX,
            EndY = shape.EndY,
            BoardId = model.BoardId
        }).ToList();

        _context.DrawingElements.AddRange(drawingElements);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetDrawingElements(int id)
    {
        var drawingElements = await _context.DrawingElements
            .Where(de => de.BoardId == id)
            .ToListAsync();

        return Ok(drawingElements);
    }

    [HttpPost]
    public async Task<IActionResult> ClearBoard([FromBody] int boardId)
    {
        var board = await _context.Boards.FirstOrDefaultAsync(x => x.Id == boardId);

        if (board == null)
        {
            return NotFound();
        }

        board.DrawingData = [];

        var existingElements = _context.DrawingElements.Where(de => de.BoardId == boardId);
        _context.DrawingElements.RemoveRange(existingElements);

        await _context.SaveChangesAsync();

        return Ok();
    }

    private async Task GenerateThumbnail(int boardId, List<DrawingElement> drawingElements)
    {
        const int thumbnailWidth = 200;
        const int thumbnailHeight = 200;

        using (var bitmap = new SKBitmap(thumbnailWidth, thumbnailHeight))
        using (var canvas = new SKCanvas(bitmap))
        using (var paint = new SKPaint())
        {
            canvas.Clear(SKColors.White);
            paint.IsAntialias = true;

            foreach (var element in drawingElements)
            {
                paint.Color = SKColor.Parse(element.Color);
                paint.StrokeWidth = element.LineWidth;

                switch (element.Tool)
                {
                    case "pencil":
                    case "eraser":
                    case "line":
                        canvas.DrawLine(element.StartX, element.StartY, element.EndX, element.EndY, paint);
                        break;
                    case "rectangle":
                        canvas.DrawRect(element.StartX, element.StartY, element.EndX - element.StartX, element.EndY - element.StartY, paint);
                        break;
                    case "circle":
                        var radiusX = Math.Abs(element.EndX - element.StartX) / 2;
                        var radiusY = Math.Abs(element.EndY - element.StartY) / 2;
                        var centerX = (element.EndX + element.StartX) / 2;
                        var centerY = (element.EndY + element.StartY) / 2;
                        canvas.DrawOval(centerX, centerY, radiusX, radiusY, paint);
                        break;
                }
            }

            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            {
                var filePath = Path.Combine("wwwroot", "thumbnails", $"{boardId}.png");
                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    data.SaveTo(stream);
                }
            }
        }
    }

    public async Task Thumbnail(int id)
    {
        var board = await _context.Boards.FindAsync(id);

        if (board == null)
        {
            return;
        }

        string base64Data = Convert.ToBase64String(board.DrawingData);
        string imgDataURL = string.Format("data:image/png;base64,{0}", base64Data);

        ViewBag.ThumbnailData = imgDataURL;
    }
}