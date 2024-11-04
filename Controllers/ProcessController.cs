using Microsoft.AspNetCore.Mvc;
using SignInClick.DTOS;
using SignInClick.Services;
using System.Threading.Tasks;

namespace SignInClick.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessController : ControllerBase
    {
        private readonly ProcessClickSign _processClickSign;

        public ProcessController(ProcessClickSign processClickSign)
        {
            _processClickSign = processClickSign;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessEnvelope(IFormFile file ,string name, string signerOne , string emailOne , string signerTwo,string emailTwo)
        {
            if (name == null)
            {
                return BadRequest("Os dados do envelope não foram fornecidos.");
            }

            if (file == null)
            {
                return BadRequest("File é Obrigatório.");
            }
       
            var envelopeId = await _processClickSign.CreateEnvelopeAsync(name);


            if (envelopeId.Contains("Erro"))
            {
                return BadRequest(new { error = envelopeId });
            }
                                 
             var Document = await _processClickSign.ProcessDocument(envelopeId,file);

            if (Document.Contains("Erro"))
            {
                return BadRequest(new { error = Document });
            }

           var signatureOne= await _processClickSign.CreateSigner(envelopeId,signerOne,emailOne);


            if (signatureOne.Contains("Erro"))
            {
                return BadRequest(new { error = signatureOne });
            }

            var signatureTwo= await _processClickSign.CreateSigner(envelopeId,signerTwo,emailTwo);


            if (signatureTwo.Contains("Erro"))
            {
                return BadRequest(new { error = signatureTwo });
            }

            return Ok(new { envelopeId });
        }



    }
}
