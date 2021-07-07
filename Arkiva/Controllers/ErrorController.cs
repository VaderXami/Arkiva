/**
* Versioni: V 1.0.0
* Data: 10/06/2021
* Programuesi: Arvin Xamo
* Pershkrimi: Perdorimi i kontrollerit per menaxhimin e Erroreve te cilat mund ti hasim ne te ardhmen si pasoj e nje gabimi njerezore.
(c) Copyright by Soft & Solution (opsionale)
**/

using System.Web.Mvc;

namespace Arkiva.Controllers
{
    /**
     * Data: Data e 10/06/2021
     * Programuesi: Arvin Xamo
     * Klasa: ErrorController
     * Arsyeja: Per Shfaqen e View kur kemi Error.
     * Pershkrimi: Kontrolleri do te bej te mundur mbajtjen e metodave te permendura me posht si qellim
     * shfaqen e njeres prej metodave te nderlidhura Views/Error/PageNotFound dhe kjo e fundit me Shared/Error.
     * Trashegon nga: Controller
     * Interfaces:
     * Constants:
     * Metodat:
     * Index(), ActionResult
     * PageNotFound(), ActionResult
     **/
    public class ErrorController : Controller
    {
        // GET: Error

        /**
        * Data: 10/06/2021
        * Programuesi: Arvin Xamo
        * Metoda: PageNotFound
        * Arsyeja: Kthimin e View qe te shfaqi error message
        * Pershkrimi: Ben te mundur Shfaqen e mesazhit te gabimit ku nje faqe nuk ekzistone ose kerkimi per ate nuk eshte i sakte nese perdoruesi e provon direkt nga linku.
        * E vlefshme dhe ne rastin e modifikimit te id gjat Editimit (kte te fundit e menaxhojme ndryshe te te 3 kontrollerat)
        * ose gjat Listimit ne berthamen e folderit te Subjektit, Inspektimit ose Dokumentit.
        * Para kushti: 
        * Post kushti: 
        * Parametrat:
        * Return: Kthen nje View e cila shfaq nje error message.
       **/

        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}