using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WTLicVerify.DBContexts;
using WTLicVerify.dbRepository;
using WTLicVerify.EvatoExternallCall;
using WTLicVerify.Models;
using WTLicVerify.utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WTLicVerify
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicController : ControllerBase
    {
        private WTDBContext db;
        private readonly ILogger<LicController> _logger;
        public LicController(WTDBContext _db, ILogger<LicController> logger)
        {
            db = _db;
            _logger = logger;
        }

        ///api/lic/testlog
        [HttpGet("version")]
        [Obsolete]
        public IActionResult version()
        {
            return Ok("v2.1");
        }


        [HttpGet("activate/{data}")]
        [Obsolete]
        public async Task<IActionResult> Activate(string data)
        {
            try
            {
                var messageFromClient = HelpUtils.Base64Decode(data);
                var contentInfo = messageFromClient.Split('|');
                var activationCode = contentInfo[0];
                var machine_id = contentInfo[1] + "|" + contentInfo[2] + "|" + contentInfo[3] + "|" + contentInfo[4] + "|" + contentInfo[5] ;
                var machine_name = contentInfo[1];
                var application_name = contentInfo[2];
                var application_version = contentInfo[3];
                var extenstion_name = contentInfo[4];
                var extenstion_version = contentInfo[5];

                SaleRepository sr = new SaleRepository(db);
                var authorSales = await sr.GetAuthorSalesByActivationCode(activationCode);

                _logger.LogInformation("Activate - {0} ", messageFromClient);
                if (activationCode == "071c3638-b08f-43a8-a289-a9989780917f")
                {

                    var envato_admin_entities = await sr.GetEnvatoAccessByMachineID(machine_id);
                    if (envato_admin_entities.Any(x => x.activationCode == activationCode))
                    {
                        _logger.LogDebug("Activate - same admin Activate : {0}", machine_id);
                        foreach (var envato_admin_entity in envato_admin_entities.Where(x => x.activationCode == activationCode))
                        {
                            EnvatoAccess envato = envato_admin_entity;
                            envato.machineId = machine_id;
                            envato.machine_name = machine_name;
                            envato.application_name = application_name;
                            envato.activate = true;
                            envato.application_version = application_version;
                            envato.extenstion_name = extenstion_name;
                            envato.extenstion_version = extenstion_version;
                            envato.connected_at = DateTime.Now;
                            await sr.UpdateEnvatoAccess(envato);
                        }
                    }
                    else
                    {
                        var autorsale_admin = await sr.GetAuthorSalesByActivationCode(activationCode);
                        EnvatoAccess envato = new EnvatoAccess();
                        envato.machineId = machine_id;
                        envato.machine_name = machine_name;
                        envato.application_name = application_name;
                        envato.activate = true;
                        envato.application_version = application_version;
                        envato.extenstion_name = extenstion_name;
                        envato.extenstion_version = extenstion_version;
                        envato.connected_at = DateTime.Now;
                        envato.activated_at = DateTime.Now;
                        envato.activationCode = activationCode;
                        envato.authorSaleId = autorsale_admin.First().Id;
                        await sr.UpdateEnvatoAccess(envato);
                        _logger.LogDebug("Activate - new admin Activate : {0}", machine_id);
                    }
                    _logger.LogDebug("Activate - ok - line 107 : {0}", machine_id);
                    return Ok(new ServerResult(true, activationCode));

                }

                foreach (var authorSale in authorSales)
                {
                    if (authorSale.EnvatoAccess.Where(x => x.activate == true).Count() > 1)
                    {
                        _logger.LogDebug("Activate - wrong - line 117 : {0}", machine_id);
                        return Ok(new ServerResult(false, "Already two client activated ! Go to verificaition web site to disabele one of them!"));
                    }
                }

                if (authorSales.Count <= 0)
                {
                    _logger.LogDebug("Activate - wrong - line 124 : {0}", machine_id);
                    return Ok(new ServerResult(false, "Activation code is wrong!"));
                }
                else if (authorSales.Count > 1)
                {
                    _logger.LogDebug("Activate - wrong - line 129 : {0}", machine_id);
                    return Ok(new ServerResult(false, "Another client Activated with this code!\n Go to verificaition web site to get new activation code "));
                }
                else
                {
                    if (authorSales.First().EnvatoAccess.Any(x => x.activate == true && x.activationCode == activationCode))
                    {
                        _logger.LogDebug("Activate - wrong - line 136 : {0}", machine_id);
                        return Ok(new ServerResult(false, "Another client Activated with this code! Go to verificaition web site to get new activation code!"));
                    }

                    if (!authorSales.First().EnvatoAccess.Any(x => x.activationCode == activationCode))
                    {
                        _logger.LogDebug("Activate - wrong - line 142 : {0}", machine_id);
                        return Ok(new ServerResult(false, "Activation code is wrong!"));
                    }

                    EnvatoAccess envato = authorSales.First().EnvatoAccess.First(x => x.activationCode == activationCode);
                    envato.machineId = machine_id;
                    envato.machine_name = machine_name;
                    envato.activate = true;
                    envato.application_name = application_name;
                    envato.application_version = application_version;
                    envato.extenstion_name = extenstion_name;
                    envato.extenstion_version = extenstion_version;
                    envato.activated_at = DateTime.Now;
                    envato.connected_at = DateTime.Now;
                    await sr.UpdateEnvatoAccess(envato);
                    _logger.LogDebug("Activate - done - line 142 : {0}", machine_id);
                }

                return Ok(new ServerResult(true, activationCode));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Activate - unhandled");
                return Ok(new ServerResult(false, "unhandled"));
            }
        }

        [HttpGet("envatocomplete/{data}")]
        [Obsolete]
        public async Task<IActionResult> EnvatoComplete(string data)
        {
            try
            {
                var messageFromClient = HelpUtils.Base64Decode(data);
                var contentInfo = messageFromClient.Split('|');
                var purchaseCode = contentInfo[0];
                var eMail = contentInfo[1];
                var code = contentInfo[2];
                var appName = contentInfo[3];
                SaleRepository repository = new SaleRepository(db);
                AuthorSale authorSale_toActive = new AuthorSale();
                Regex purchaseCode_reg = new Regex("^([a-f0-9]{8})-(([a-f0-9]{4})-){3}([a-f0-9]{12})$");
                Regex email_reg = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

                var envatoAccsByPCode = await repository.GetActiveSeasonsByPurchaseCode(purchaseCode);
                JsonSerializerSettings jsSetting = new JsonSerializerSettings();
                jsSetting.NullValueHandling = NullValueHandling.Ignore;
                var envatoAccsByPCode_jsonStr = JsonConvert.SerializeObject(envatoAccsByPCode, jsSetting);
                _logger.LogDebug("EnvatoComplete - active seasons for {0} - {1}", purchaseCode, envatoAccsByPCode.Count);

                _logger.LogInformation("EnvatoComplete - {0} ", messageFromClient);

                #region mock for admin
                if (purchaseCode == "4b07e115-3e58-01dd-3f6c-935a76de26e1" && eMail == "wonderAdmin@mail.com")
                {
                    _logger.LogDebug("EnvatoComplete - admin");
                    var adminAutors = await repository.GetAuthorSalesByPurchaseCode(purchaseCode);
                    var AdminEnvatoAccess = await EnvatoCalls.GetAccessToekn(code);

                    if (AdminEnvatoAccess == null)
                    {
                        return Ok(new ServerResult(false, "cant get the access, login again"));
                    }

                    if (adminAutors.Count == 0)
                    {
                        AuthorSale authorSale_admin = new AuthorSale();
                        authorSale_admin.amount = "0";
                        authorSale_admin.code = purchaseCode;
                        authorSale_admin.email = eMail;
                        authorSale_admin.license = "admin";
                        authorSale_admin.purchase_count = 0;
                        authorSale_admin.sold_at = DateTime.Now.ToString();
                        authorSale_admin.support_amount = "0";
                        authorSale_admin.supported_until = DateTime.Now.ToString(); ;
                        authorSale_admin.Item = new SaleItem()
                        {
                            author_url = "admin",
                            author_username = "admin",
                            name = "wondercallouts",
                            site = "wondertools-official.com",
                            number_of_sales = 0,
                            updated_at = DateTime.Now.ToString(),
                            url = "admin",
                        };
                        EnvatoAccess admin_access = new EnvatoAccess()
                        {
                            access_token = AdminEnvatoAccess.access_token,
                            activate = false,
                            activationCode = "071c3638-b08f-43a8-a289-a9989780917f",
                            AuthorSale = authorSale_admin,
                            connected_at = DateTime.Now,
                        };

                        var new_admin_autor = await repository.InsertAutorSale(authorSale_admin);
                        admin_access.authorSaleId = new_admin_autor.Id;
                        await repository.InsertRawEnvatoAccess(admin_access);

                    }
                    else
                    {
                        foreach (var adminAutor in adminAutors)
                        {
                            foreach (var envatoAccessitem in adminAutor.EnvatoAccess)
                            {
                                _logger.LogInformation("EnvatoComplete - update token {0} - {1}", AdminEnvatoAccess.access_token, envatoAccessitem.access_token);
                                envatoAccessitem.access_token = AdminEnvatoAccess.access_token;
                                await repository.UpdateEnvatoAccess(envatoAccessitem);
                            }
                        }
                    }

                    _logger.LogDebug("EnvatoComplete - admin (return ok)");
                    return Ok(new ServerResult(true, "071c3638-b08f-43a8-a289-a9989780917f", envatoAccsByPCode_jsonStr));
                }
                #endregion mock for admin

                if (!purchaseCode_reg.IsMatch(purchaseCode))
                {
                    return Ok(new ServerResult(false, "wrong purchase code"));
                }

                if (!email_reg.IsMatch(eMail))
                {
                    return Ok(new ServerResult(false, "wrong purchase code"));
                }

                #region check and GetAccessToekn 

                EnvatoAccess envatoAccess = new EnvatoAccess();
                if (code.Length > 0)
                {
                    try
                    {
                        envatoAccess = await EnvatoCalls.GetAccessToekn(code);
                        if (envatoAccess == null)
                        {
                            return Ok(new ServerResult(false, "cant get the access, login again"));
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "EnvatoComplete - unhandled - check and GetAccessToekn");
                        return Ok(new ServerResult(false, "error"));
                    }
                }
                #endregion check and GetAccessToekn

                #region GetUserEmail and GetUserPurchaseCodes and check
                if (envatoAccess.access_token.Length > 0)
                {
                    try
                    {

                        string userEmail = await EnvatoCalls.GetUserEmail(envatoAccess.access_token);
                        if (userEmail == null)
                        {
                            return Ok(new ServerResult(false, "cant get the access, login again"));
                        }

                        if (eMail != userEmail)
                        {
                            return Ok(new ServerResult(false, "Email not match with Envato User!"));
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "EnvatoComplete - unhandled - GetUserEmail");
                        return Ok(new ServerResult(false, "cant get the access, login again"));
                    }

                    try
                    {
                        _logger.LogDebug("EnvatoComplete - GetUserPurchaseCodes - " + envatoAccess.access_token);
                        Dictionary<string, AuthorSale> purchases = await EnvatoCalls.GetUserPurchaseCodes(envatoAccess.access_token,_logger);

                        if (!purchases.Any())
                        {
                            return Ok(new ServerResult(false, "No purchase Found at all!"));
                        }

                        if (!purchases.Any(x => x.Key == purchaseCode))
                        {
                            return Ok(new ServerResult(false, "Wrong Purchase Code!"));
                        }

                        //if (purchases.Where(x => x.Value.Item.name != appName).Any(x => x.Key == purchaseCode))
                        //{
                        //    return Ok(new ServerResult(false, "Wrong App Selected!"));
                        //}

                        authorSale_toActive = purchases.Where(x => x.Key == purchaseCode).Select(x => x.Value).First();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "EnvatoComplete - unhandled - GetUserPurchaseCodes");
                        return Ok(new ServerResult(false, "cant get the access, login again"));
                    }
                }
                #endregion GetUserEmail and GetUserPurchaseCodes and check

                var activationCode = Guid.NewGuid().ToString();
                envatoAccess.activationCode = activationCode;
                envatoAccess.activate = false;
                envatoAccess.connected_at = null;
                envatoAccess.activated_at = null;

                authorSale_toActive.code = purchaseCode;
                authorSale_toActive.email = eMail;
                authorSale_toActive.Item.Id = 0;

                var autor_DB = await repository.InsertAutorSale(authorSale_toActive);

                if (autor_DB.EnvatoAccess != null && autor_DB.EnvatoAccess.Where(x => x.activate == true).Count() > 1)
                {
                    return Ok(new ServerResult(false, "Already 2 active season found! selecct a season to deactivate!", envatoAccsByPCode_jsonStr));
                }

                envatoAccess.authorSaleId = autor_DB.Id;

                await repository.DeleteAllTrashEnvatoAccess(autor_DB.Id);

                await repository.InsertRawEnvatoAccess(envatoAccess);


                return Ok(new ServerResult(true, activationCode, envatoAccsByPCode_jsonStr));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "EnvatoComplete - unhandled");
                return Ok(new ServerResult(false, e.Message));
            }
        }

        [HttpGet("check/{code}")]
        [Obsolete]
        public async Task<IActionResult> Check(string code)
        {
            try
            {
                var messageFromClient = HelpUtils.Base64Decode(code);
                var contentInfo = messageFromClient.Split('|');
                var activationCode = contentInfo[0];
                var machine_id = contentInfo[1] + "|" + contentInfo[2] + "|" + contentInfo[3] + "|" + contentInfo[4] + "|" + contentInfo[5];

                SaleRepository sr = new SaleRepository(db);
                var envatoAccesses_machineId = await sr.GetEnvatoAccessByMachineID(machine_id);

                _logger.LogInformation("Check - {0}", messageFromClient);
                if (activationCode == "071c3638-b08f-43a8-a289-a9989780917f")
                {
                    _logger.LogDebug("Check - admin - {0}", messageFromClient);
                    if (envatoAccesses_machineId.Count <= 0)
                    {
                        _logger.LogDebug("Check - no admin found - {0}", messageFromClient);
                        return Ok(new ServerResult(false, "wrong activation code!"));
                    }

                    _logger.LogDebug("Check - admin ok! - {0}", messageFromClient);
                    return Ok(new ServerResult(true, "ok!"));
                }

                if (envatoAccesses_machineId.Count > 0)
                {
                    if (!envatoAccesses_machineId.Any(x => x.activationCode == activationCode))
                    {
                        _logger.LogDebug("Check - wrong! line 405 - {0}", messageFromClient);
                        return Ok(new ServerResult(false, "wrong activation code!"));
                    }
                }
                else if (envatoAccesses_machineId.Count > 1)
                {
                    _logger.LogDebug("Check - wrong! line 412 - {0}", messageFromClient);
                    return Ok(new ServerResult(false, "wrong activation code!"));
                }
                else
                {
                    _logger.LogDebug("Check - wrong! line 417 - {0}", messageFromClient);
                    return Ok(new ServerResult(false, "wrong activation code!"));
                }

                var envatoAccesses_activationCode = await sr.GetEnvatoAccessByActivationCode(activationCode);

                if (envatoAccesses_activationCode.Count > 0)
                {
                    if (!envatoAccesses_machineId.Any(x => x.machineId == machine_id))
                    {
                        _logger.LogDebug("Check - wrong! line 427 - {0}", messageFromClient);
                        return Ok(new ServerResult(false, "wrong activation code!"));
                    }
                }
                else if (envatoAccesses_activationCode.Count > 1)
                {
                    _logger.LogDebug("Check - wrong! line 433 - {0}", messageFromClient);
                    return Ok(new ServerResult(false, "wrong activation code!"));
                }
                else
                {
                    _logger.LogDebug("Check - wrong! line 438 - {0}", messageFromClient);
                    return Ok(new ServerResult(false, "wrong activation code!"));
                }

                if (envatoAccesses_machineId.Where(x => x.activationCode == activationCode).First().activate == false)
                {
                    _logger.LogDebug("Check - wrong! line 444 - {0}", messageFromClient);
                    return Ok(new ServerResult(false, "This client is Deactivated! go to verification website to active it again"));
                }

                if (envatoAccesses_activationCode.Where(x => x.machineId == machine_id).First().activate == false)
                {
                    _logger.LogDebug("Check - wrong! line 450 - {0}", messageFromClient);
                    return Ok(new ServerResult(false, "This client is Deactivated! go to verification website to active it again"));
                }

                _logger.LogDebug("Check - ok - {0}", messageFromClient);
                return Ok(new ServerResult(true, "ok!"));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Check - unhandled");
                return Ok(new ServerResult(true, "ok!"));
            }
        }

        [HttpGet("deactive/{id}")]
        [Obsolete]
        public async Task<IActionResult> DeactiveById(string id)
        {
            try
            {
                var idforDeactivate = HelpUtils.Base64Decode(id);

                SaleRepository sr = new SaleRepository(db);
                var envatoAccess = await sr.GetEnvatoAccessById(long.Parse(idforDeactivate));
                try
                {
                    string userEmail = await EnvatoCalls.GetUserEmail(envatoAccess.access_token);
                    if (userEmail == null)
                    {
                        return Ok(new ServerResult(false, "cant get the access, login again"));
                    }

                }
                catch (Exception e)
                {
                    return Ok(new ServerResult(false, "cant get the access, login again"));
                }

                var deactiveEnvato = await sr.DeactivateEnvatoById(long.Parse(idforDeactivate));

                var envatoAccsByPCode = await sr.GetActiveSeasonsByPurchaseCode(envatoAccess.AuthorSale.code);
                var envatoAccsByPCode_jsonStr = JsonConvert.SerializeObject(envatoAccsByPCode);

                return Ok(new ServerResult(true, ""));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "DeactiveById - unhandled");
                return Ok(new ServerResult(false, "something is wrong try again!"));
            }
        }

        [HttpGet("deactivebc/{code}")]
        [Obsolete]
        public async Task<IActionResult> DeactiveByActivationcCode(string code)
        {
            try
            {
                var aCodeforDeactivate = HelpUtils.Base64Decode(code);
                var contentInfo = aCodeforDeactivate.Split('|');
                var activationCode = contentInfo[0];
                var machine_id = contentInfo[1] + "|" + contentInfo[2] + "|" + contentInfo[3] + "|" + contentInfo[4] + "|" + contentInfo[5];

                SaleRepository sr = new SaleRepository(db);
                var envatoAccess = await sr.GetEnvatoAccessById(long.Parse(activationCode));

                if (envatoAccess.machineId != machine_id)
                {
                    return Ok(new ServerResult(false, "Request from wrong machine! Deactive from website.."));
                }


                var deactiveEnvato = await sr.DeactivateEnvatoByActivationCode(activationCode);

                return Ok(new ServerResult(true, ""));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "DeactiveByActivationcCode - unhandled");
                return Ok(new ServerResult(false, "notHandled"));
            }
        }
    }
}
