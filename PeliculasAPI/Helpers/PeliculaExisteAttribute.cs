using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Helpers
{
    public class PeliculaExisteAttribute : Attribute, IAsyncResourceFilter
    {
        private readonly ApplicationDbContext DBcontext;

        public PeliculaExisteAttribute(ApplicationDbContext context)
        {
            this.DBcontext = context;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var peliculaIdObject = context.HttpContext.Request.RouteValues["peliculaId"];

            if(peliculaIdObject == null)
            {
                return;
            }

            var peliculaId = int.Parse(peliculaIdObject.ToString());

            var existePelicula = await DBcontext.Peliculas.AnyAsync(x => x.Id == peliculaId);

            if (!existePelicula)
            {
                context.Result = new NotFoundResult();
            }
            else
            {
                await next();
            }
        }
    }
}
