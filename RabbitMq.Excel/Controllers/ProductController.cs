using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMq.Excel.Models;
using RabbitMq.Excel.Services;
using SharedLibrary.Messages;

namespace RabbitMq.Excel.Controllers
{
    public class ProductController : Controller
    {

        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _users;
        private readonly RabbitMQPublisher _publisher;


        public ProductController(AppDbContext context, UserManager<IdentityUser> users, RabbitMQPublisher publisher)
        {
            _context = context;
            _users = users;
            _publisher = publisher;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateProductExcel()
        {
            var user = await _users.FindByNameAsync(User.Identity.Name);
            var fileName = $"product-excel-{Guid.NewGuid()}";

            UserFile userFile = new()
            {
                UserId = user.Id,
                FileName = fileName,
                FileState = FileState.Creating,
            };

            await _context.AddAsync(userFile);
            await _context.SaveChangesAsync();

            if (!_publisher.ConnectionIsOpen())
                return View("error : RabbitMQ is down");

            _publisher.Publish(new CreateProductExcelMessage() { FileId = userFile.Id, UserId = user.Id });

            return RedirectToAction(nameof(Files));
        }

        public async Task<IActionResult> Files()
        {

            var user = await _users.FindByNameAsync(User.Identity.Name);
            return View(
                await _context.UserFiles.Where(x => x.UserId == user.Id).ToListAsync());
        }


    }
}
