using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Features;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace GymSystem.PL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService memberService;

        public MemberController(IMemberService memberService)
        {
            this.memberService = memberService;
        }
        // Get BaseUrl/Member/Index
        // index - get all members

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await memberService.GetallAsync(ct);
            return View(members);
        }

        // Get BaseUrl/Member/Details/{id}
        // MemberDetails - get member by id

       [HttpGet]
        public async Task<IActionResult> MemberDetails(int id , CancellationToken ct)
        {
            var member = await memberService.GetMemberByIdAsync(id, ct);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        //Get BaseUrl/Member/healthrecord/{id}
        // MemberHealthRecord - get member health record by id

        public async Task<IActionResult> MemberHealthRecord(int id, CancellationToken ct)
        {
            var memberHealthRecord = await memberService.GetHealthRecordByIdAsync(id, ct);
            if (memberHealthRecord == null)
            {
                return RedirectToAction(nameof(Index));
            } return View(memberHealthRecord);

        }

        #region Create Member

        // Get BaseUrl/Member/Create
        // Create - Show empty form 

        [HttpGet]
        public IActionResult Create()
            {
            return View();
        }

        // Post BaseUrl/Member/Create
        // Create - Save member to database

        [HttpPost]

        public async Task<IActionResult> CreateMember(CreateMemberViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(nameof(Create), model);

            var result = await memberService.CreateMemberAsync(model, ct);
            if(result.succes)
                TempData["SuccessMessage"] = "Member created successfully.";
            else
             TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));

            
        }


        #endregion

        #region Edit Member

    //    Get BaseUrl/Member/Edit/{id
    //}
    //Edit - Show member data in form

   [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
    {
        var memberToUpdate = await memberService.GetMemberToUpdateAsync(id, ct);
        if (memberToUpdate == null)
        {
            return RedirectToAction(nameof(Index));
        }
        return View(memberToUpdate);
    }

    // Post BaseUrl/Member/Edit/{id}    
    // Edit - Update member data in database

    [HttpPost]

    public async Task<IActionResult> EditMember([FromRoute] int id, MemberToUpdateViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(nameof(EditMember), model);
        var result = await memberService.UpdateMemberDetailsAsync(id, model, ct);
        if (result)
            TempData["SuccessMessage"] = "Member updated successfully.";
        else
            TempData["ErrorMessage"] = "Failed to update member.";
        return RedirectToAction(nameof(Index));
    }

        #endregion

        #region Delete Member

        // Get BaseUrl/Member/Delete/{id}
        // Delete - Show confirmation to delete member

        [HttpGet]

        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var member = await memberService.GetMemberByIdAsync(id, ct);
            if (member == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        // Post BaseUrl/Member/Delete/{id}
        // Delete - Remove member from database

        [HttpPost]

        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id, CancellationToken ct)
        {
            var result = await memberService.RemoveMemberAsync(id, ct);
            if (result)
                TempData["SuccessMessage"] = "Member deleted successfully.";
            else
                TempData["ErrorMessage"] = "Failed to delete member.";
            return RedirectToAction(nameof(Index));

        }




        #endregion
    }
}
