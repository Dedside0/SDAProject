using Microsoft.AspNetCore.Mvc;
using SDA.Db.Repositories;

namespace SDA.Web.Views.Shared.Components.Tickets
{
    public class TicketsViewComponent(ITicketRepository ticketRepository): ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var all = await ticketRepository.GetAll();
            return View("Tickets",all);
        }
    }
}
