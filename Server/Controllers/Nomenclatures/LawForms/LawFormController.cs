using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Nomenclatures;
using Sc.Models.Entities.Nomenclatures;
using Sc.Models.FilterDtos.Nomenclatures.LawForms;
using Sc.Repositories.Nomenclatures;
using Sc.Services.Nomenclatures.LawForms;
using Server.Controllers.Base.Nomenclatures;

namespace Server.Controllers.Nomenclatures.LawForms
{
    [ApiController]
    [Route("api/nomenclatures/lawForms")]
    public class LawFormController : NomenclatureSearchController<LawForm, LawFormDto, LawFormDto, LawFormFilterDto, ILawFormRepository, LawFormService>
    {
        public LawFormController(
            LawFormService lawFormService
            ) : base(lawFormService)
        {
        }
    }
}
