using FinalExam.DAL;
using FinalExam.Models;
using FinalExam.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExam.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<OurTeam> teams = await _context.OurTeams.Where(t=>t.IsDeleted==false).ToListAsync();
            if(teams == null) return NotFound();

            HomeVM vm = new HomeVM()
            {
                Teams = teams
            };

            return View(vm);
        }
    }
}
