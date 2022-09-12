using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTLicVerify.DBContexts;
using WTLicVerify.EvatoExternallCall;
using WTLicVerify.Models;
using WTLicVerify.utils;

namespace WTLicVerify.dbRepository
{
    public class SaleRepository
    {
        private WTDBContext db;
        public SaleRepository(WTDBContext _db)
        {
            db = _db;
        }

        public async Task<EnvatoAccess> InsertRawEnvatoAccess(EnvatoAccess envatoAccess)
        {
            EntityEntry<EnvatoAccess> newEnvatoEntity = null;
            var envatoEntities = await db.EnvatoAccesses.Where(x => x.access_token == envatoAccess.access_token).ToListAsync();

            if (!envatoEntities.Any())
            {
                //envatoAccess.activated_at = DateTime.Now;
                newEnvatoEntity = await db.EnvatoAccesses.AddAsync(envatoAccess);
                await db.SaveChangesAsync();
            }
            else
            {
                return envatoEntities.First();
            }

            return newEnvatoEntity.Entity;
        }

        public async Task DeleteAllTrashEnvatoAccess(long autorSaleId)
        {
            EntityEntry<EnvatoAccess> newEnvatoEntity = null;
            var envatoEntities = db.EnvatoAccesses.Where(x => x.authorSaleId == autorSaleId && x.connected_at == null && x.activated_at == null);

            if (envatoEntities.Any())
            {
                db.EnvatoAccesses.RemoveRange(envatoEntities);
                await db.SaveChangesAsync();
            }
        }

        public async Task<EnvatoAccess> GetEnvatoAccessById(long id)
        {
            var envatoEntity = await db.EnvatoAccesses.Include(x=>x.AuthorSale).ThenInclude(x=>x.Item).FirstOrDefaultAsync(x => x.Id == id);
            return envatoEntity;
        }

        public async Task<List<EnvatoAccess>> GetEnvatoAccessByPurchaseCode(string pCode)
        {
            var envatoEntities = await db.AuthorSales.Include(x => x.EnvatoAccess).FirstOrDefaultAsync(x => x.code == pCode);
            return envatoEntities.EnvatoAccess;
        }

        public async Task<List<EnvatoAccess>> GetActiveSeasonsByPurchaseCode(string pCode)
        {
            List<EnvatoAccess> activeSeasons = new List<EnvatoAccess>();
            var envatoEntities = await db.AuthorSales.Include(x => x.EnvatoAccess).FirstOrDefaultAsync(x => x.code == pCode);
            if (envatoEntities != null)
            {
                foreach (var item in envatoEntities.EnvatoAccess.Where(x => x.activate == true))
                {
                    activeSeasons.Add(new EnvatoAccess()
                    {
                        activate = item.activate,
                        activated_at = item.activated_at,
                        application_name = item.application_name,
                        application_version = item.application_version,
                        extenstion_name = item.extenstion_name,
                        extenstion_version = item.extenstion_version,
                        machine_name = item.machine_name,
                        Id = item.Id
                    });
                }
            }
            return activeSeasons;
        }

        public async Task<List<AuthorSale>> GetAuthorSalesByActivationCode(string aCode)
        {
            var authorSale = await db.AuthorSales.Include(x => x.EnvatoAccess).Include(x => x.Item)
                .Where(x => x.EnvatoAccess.Any(z => z.activationCode == aCode)).ToListAsync();
            return authorSale;
        }

        public async Task<List<AuthorSale>> GetAuthorSalesByPurchaseCode(string pCode)
        {
            var authorSale = await db.AuthorSales.Include(x => x.EnvatoAccess).Include(x => x.Item).Where(x => x.code == pCode).ToListAsync();
            return authorSale;
        }

        public async Task<List<EnvatoAccess>> GetEnvatoAccessByActivationCode(string aCode)
        {
            var envatoEntities = await db.EnvatoAccesses.Where(x => x.activationCode == aCode).ToListAsync();
            return envatoEntities;
        }

        public async Task<List<EnvatoAccess>> GetEnvatoAccessByMachineID(string machineId)
        {
            var envatoEntities = await db.EnvatoAccesses.Include(x=>x.AuthorSale).Where(x => x.machineId == machineId).ToListAsync();
            return envatoEntities;
        }

        public async Task<EnvatoAccess> DeactivateEnvatoById(long id)
        {
            var envatoEntity = await db.EnvatoAccesses.FirstOrDefaultAsync(x => x.Id == id);
            if (envatoEntity != null)
            {
                envatoEntity.activate = false;
                var updatedEntity = db.EnvatoAccesses.Update(envatoEntity);
                await db.SaveChangesAsync();
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<EnvatoAccess> DeactivateEnvatoByActivationCode(string aCode)
        {
            var envatoEntity = await db.EnvatoAccesses.FirstOrDefaultAsync(x => x.activationCode == aCode);
            if (envatoEntity != null)
            {
                envatoEntity.activate = false;
                var updatedEntity = db.EnvatoAccesses.Update(envatoEntity);
                await db.SaveChangesAsync();
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task ActivateEnvatoById(long id)
        {
            var envatoEntity = await db.EnvatoAccesses.FirstOrDefaultAsync(x => x.Id == id);
            if (envatoEntity != null)
            {
                envatoEntity.activate = true;
                db.EnvatoAccesses.Update(envatoEntity);
                db.SaveChanges();
            }
        }

        public async Task UpdateEnvatoAccess(EnvatoAccess envatoAccess)
        {
            db.Update<EnvatoAccess>(envatoAccess);
            await db.SaveChangesAsync();
        }

        internal async Task<AuthorSale> InsertAutorSale(AuthorSale authorSale_toActive)
        {
            EntityEntry<AuthorSale> newAutorEntity = null;
            var envatoEntities = await db.AuthorSales.Include(x=>x.EnvatoAccess).Where(x => x.code == authorSale_toActive.code).ToListAsync();

            if (!envatoEntities.Any())
            {
                newAutorEntity = await db.AuthorSales.AddAsync(authorSale_toActive);
                await db.SaveChangesAsync();
            }
            else
            {
                return envatoEntities.First();
            }

            return newAutorEntity.Entity;
        }


        //public async Task<bool> AnyFootPrintExists(string purchaseCode, string email, string machineName)
        //{
        //    try
        //    {
        //        bool unique_ok = true;
        //        bool purchase_ok = true;
        //        bool email_ok = true;
        //        bool machineName_ok = true;

        //        var allSales = await GetAllSaleCodes(email, purchaseCode, machineName).ToListAsync();
        //        if (allSales.Count > 0)
        //        {
        //            unique_ok = false;
        //        }

        //        var saleCode_purchase = await GetAllSaleCodesByCode(purchaseCode);
        //        if (saleCode_purchase.Count > 0)
        //        {
        //            foreach (var saleCode in saleCode_purchase)
        //            {
        //                if (saleCode.customerEmail.ToLower() != email.ToLower() ||
        //                    saleCode.CustomerMachine.machineName.ToLower() != machineName.ToLower())
        //                {
        //                    purchase_ok = false;
        //                }
        //            }
        //        }

        //        var saleCode_email = await GetAllSaleCodesByMail(email);
        //        if (saleCode_email.Count > 0)
        //        {
        //            foreach (var saleCode in saleCode_purchase)
        //            {
        //                if (saleCode.customerEmail.ToLower() != email.ToLower() ||
        //                    saleCode.CustomerMachine.machineName.ToLower() != machineName.ToLower())
        //                {
        //                    email_ok = false;
        //                }
        //            }
        //        }

        //        var machine_name = await GetAllMachinerByName(machineName);
        //        if (machine_name.Count == 0) { machineName_ok = true; }

        //        if (unique_ok == true && purchase_ok == true && email_ok == true && machineName_ok == true)
        //        {
        //            return true;
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //        throw;
        //    }
        //}
    }
}
