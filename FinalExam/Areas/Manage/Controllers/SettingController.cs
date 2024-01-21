using FinalExam.Areas.Manage.ViewModels.Setting;
using FinalExam.DAL;
using FinalExam.Models;
using FinalExam.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExam.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="Admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page)
        {
            int take = 3;
            decimal count= await _context.Settings.CountAsync();
            List<Setting> settingList = await _context.Settings.Where(s=>s.IsDeleted==false).Skip((page - 1) * take).Take(take).ToListAsync();

            PaginateVM<Setting> paginateVM = new PaginateVM<Setting>()
            {
                Take = take,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = settingList
            };
            return View(paginateVM);
        }


        public async Task<IActionResult> Update()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateSettingVM updateSettingVM)
        {
            if(id<=0) return BadRequest();
            if (!ModelState.IsValid) return View();

            Setting setting = await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);
            if (setting == null) return NotFound();

            setting.Value = updateSettingVM.Value;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Setting");
        }


    }
}
