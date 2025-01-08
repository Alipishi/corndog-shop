using HelmonyCornDog.Data;
using HelmonyCornDog.Data.Repositories;
using HelmonyCornDog.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelmonyCornDog.Components
{
    public class ProductGroupsComponent:ViewComponent
    {
       IGroupRepository _groupRepository;

       public ProductGroupsComponent(IGroupRepository groupRepository)
       {
           _groupRepository = groupRepository;
       }

       public async Task<IViewComponentResult> InvokeAsync()
        {
           
            return View("/Views/Components/ProductGroupsComponents.cshtml",_groupRepository.GetGroupForShow());
        }
    }
}
