﻿using FormHelper;
using MagazineManagment.DTO.ViewModels;
using MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagazineManagment.ClientApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryClientController : Controller
    {
        private readonly ICategoryApiCalls _categoryApiCalls;
        private string GetToken()
        {
            return HttpContext.Session.GetString("Token");
        }

        public CategoryClientController(ICategoryApiCalls categoryApiCalls)
        {
            _categoryApiCalls = categoryApiCalls;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryApiCalls.GetAllCategories(GetToken());

            if (categories is null)
                return BadRequest();

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, FormValidator]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateViewModel category)
        {

            var postResult = await _categoryApiCalls.PostCreateCategory(category, GetToken());

            if (postResult.IsSuccessStatusCode)
                return FormResult.CreateSuccessResult("Category created successfully", Url.Action("Index"));

            return FormResult.CreateErrorResult(await postResult.Content.ReadAsStringAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await _categoryApiCalls.GetEditCategory(id,GetToken());

            if (category.CategoryName == null)
                return RedirectToAction("Index", "ErrorHandler"); 

            return View(category);
        }

        [HttpPost, FormValidator]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryUpdateViewModel categoryUpdate)
        {
            var editResult = await _categoryApiCalls.PostEditCategory(categoryUpdate, GetToken());

            if (editResult.IsSuccessStatusCode)
                return FormResult.CreateSuccessResult("Category updated successfully", Url.Action("Index"));

            return FormResult.CreateErrorResult(await editResult.Content.ReadAsStringAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var category = await _categoryApiCalls.GetEditCategory(id, GetToken());

            if (category.CategoryName is null)
                return RedirectToAction("Index", "ErrorHandler");

            return View(category);
        }

        [HttpPost, FormValidator]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CategoryUpdateViewModel category)
        {
            var deleteResult = await _categoryApiCalls.PostDeleteCategory(category.Id, GetToken());
            
            if (deleteResult.IsSuccessStatusCode)
                return FormResult.CreateSuccessResult("Category deleted successfully", Url.Action("Index"));

            return FormResult.CreateErrorResult(await deleteResult.Content.ReadAsStringAsync());
        }

        [HttpGet]
        public async Task<IActionResult> ActivateCategory(Guid id)
        {
            await _categoryApiCalls.ActivateCategory(id, GetToken());
            return RedirectToAction("Index");
        }
    }
}