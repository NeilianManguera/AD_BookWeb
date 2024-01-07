using AD_Book.DataAccess.Repository;
using AD_Book.DataAccess.Repository.IRepository;
using AD_Book.Models;
using AD_Book.Models.ViewModels;
using AD_Book.Utility;
using AD_BookWeb.AD_Book.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AD_BookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return View(objCompanyList);
        }

        #region --Create Company--
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                //Create
                return View(new Company());
            }
            else
            {
                //Update
                Company company = _unitOfWork.Company.Get(u => u.Id == id);
                return View(company);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            if (ModelState.IsValid)
            {

                if (CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                }
                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(CompanyObj);
            }
        }
        #endregion

        #region --API Calls--
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
