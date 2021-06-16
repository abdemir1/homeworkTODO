﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todoHW.Data;

namespace todoHW.View_Components
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryMenuViewComponent(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool ShowEmpty = true)
        {
            var items = await dbContext.Categories.Where(c => ShowEmpty || c.ToDos.Any()).ToListAsync();
            return View(items);
        }
    }
}
