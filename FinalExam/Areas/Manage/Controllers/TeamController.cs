using FinalExam.Areas.Manage.ViewModels.OurTeams;
using FinalExam.DAL;
using FinalExam.Models;
using FinalExam.Utilities.Extensions;
using FinalExam.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExam.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(int page=1)
        {
            int take = 3;
            decimal count = await _context.OurTeams.CountAsync();
            List<OurTeam> teams = await _context.OurTeams.Skip((page-1)*take).Take(take).ToListAsync();

            PaginateVM<OurTeam> paginateVM = new PaginateVM<OurTeam>()
            {
                Take = take,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = teams
            };

            return View(paginateVM);
        }


        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTeamVM createTeamVM)
        {
            if(!ModelState.IsValid) return View(createTeamVM);

            if (!createTeamVM.Image.CheckType("image/"))
            {
                if (createTeamVM.Image.CheckLength(200))
                {
                    ModelState.AddModelError("Image", "Wrong image length");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("Image", "Wrong image type");
                return View();
            }

            OurTeam ourTeam = new OurTeam()
            {
                FullName = createTeamVM.FullName,
                Position = createTeamVM.Position,
                Description = createTeamVM.Description,
                ImageUrl = createTeamVM.Image.CreateFile(_env.WebRootPath, "Uploads/TeamImages"),
                Facebook = createTeamVM.Facebook,
                Instagram = createTeamVM.Instagram,
                Twitter = createTeamVM.Twitter,
                Linkedin = createTeamVM.Linkedin
            };

            await _context.OurTeams.AddAsync(ourTeam);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index),"Team");
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            OurTeam ourTeam = await _context.OurTeams.Where(t=>t.IsDeleted==false).FirstOrDefaultAsync(t=>t.Id==id);
            if(ourTeam == null) return NotFound();

            UpdateTeamVM updateTeamVM = new UpdateTeamVM()
            {
                FullName = ourTeam.FullName,
                Position = ourTeam.Position,
                Description = ourTeam.Description,
                Facebook = ourTeam.Facebook,
                Instagram = ourTeam.Instagram,
                Twitter = ourTeam.Twitter,
                Linkedin = ourTeam.Linkedin
            };

            return View(updateTeamVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateTeamVM existTeamVM)
        {
            if (id <= 0) return BadRequest();
            if(!ModelState.IsValid) return View();

            OurTeam ourTeam = await _context.OurTeams.Where(t => t.IsDeleted == false).FirstOrDefaultAsync(t => t.Id == id);
            if (ourTeam == null) return NotFound();

            if(existTeamVM.FullName != null)
            {
                ourTeam.FullName = existTeamVM.FullName;
            }

            if (existTeamVM.Position != null)
            {
                ourTeam.Position = existTeamVM.Position;
            }

            if (existTeamVM.Description != null)
            {
                ourTeam.Description = existTeamVM.Description;
            }

            if (existTeamVM.Facebook != null)
            {
                ourTeam.Facebook = existTeamVM.Facebook;
            }

            if (existTeamVM.Instagram != null)
            {
                ourTeam.Instagram = existTeamVM.Instagram;
            }

            if (existTeamVM.Twitter != null)
            {
                ourTeam.Twitter = existTeamVM.Twitter;
            }

            if (existTeamVM.Linkedin != null)
            {
                ourTeam.Linkedin = existTeamVM.Linkedin;
            }

            if(existTeamVM.Image != null)
            {
                ourTeam.ImageUrl.DeleteFile(_env.WebRootPath, "Uploads/TeamImages");

                ourTeam.ImageUrl = existTeamVM.Image.CreateFile(_env.WebRootPath, "Uploads/TeamImages");
            }

            return RedirectToAction(nameof(Index), "Team");
        }


        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            OurTeam ourTeam = await _context.OurTeams.Where(t => t.IsDeleted == false).FirstOrDefaultAsync(t => t.Id == id);
            if (ourTeam == null) return NotFound();

            ourTeam.IsDeleted = true;

            return RedirectToAction(nameof(Index), "Team");
        }

    }
}
