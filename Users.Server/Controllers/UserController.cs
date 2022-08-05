using Microsoft.AspNetCore.Mvc;
using Users.Server.Data;
using Users.Server.Models;
using static System.Console;
using System.IO;
using static System.IO.Directory;
using static System.IO.Path;
using static System.Environment;

using Microsoft.EntityFrameworkCore;

using System.Linq;


namespace Users.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly DataContext _context;
    private readonly  IWebHostEnvironment _hostEnvironment;

    public UserController(  DataContext context, IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
        _context = context;
    }

    [HttpGet]
     public async Task<ActionResult<IEnumerable<User>>> GetUsers()
     {
        return await _context.Users
        .Select(x=>new User(){
            userId = x.userId,
            userName = x.userName,
            Email = x.Email,
            Profession = x.Profession,
            imageName = x.imageName,
            imageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.imageName)
        }).ToListAsync();
        
     }

     [HttpGet("{id}")]
        public async Task<ActionResult<User>>GetById(int id)
        {
              
        var user = await _context.Users.FindAsync(id);
        if(user == null)
        {
            return NotFound();
        }
        return user;
               
                    
        }

         [HttpPost]
        public async Task<ActionResult> PostUserModel([FromForm] User user)
        {
            user.imageName =await SaveImage(user.imageFile);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return StatusCode(201); 
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Putuser(int id, [FromForm]User user)
        {
            if(id!= user.userId)
                {
                    return BadRequest();
                }
            if(user.imageFile !=null)
                {
                    DeleteImage(user.imageName);
                    user.imageName = await SaveImage(user.imageFile);
                }
         _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch(DbUpdateConcurrencyException)
        {
            if(!EmployeeExist(id))
            {
                return NotFound();
            }
            else {throw; }
        }
            return NoContent();      
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
        DeleteImage(user.imageName);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private bool EmployeeExist(int id)
        {
            return _context.Users.Any(e=>e.userId==id);
        } 

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName =new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName+Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            using(var fileStream = new FileStream(imagePath, FileMode.Create))
            {
               await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }

    [NonAction]
    public void DeleteImage(string imageName)
    {
        var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
        if (System.IO.File.Exists(imagePath))
            System.IO.File.Delete(imagePath);
                
    }
}
