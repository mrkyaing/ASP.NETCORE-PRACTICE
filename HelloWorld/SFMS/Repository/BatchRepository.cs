using SFMS.Models;
using SFMS.Models.DAO;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SFMS.Repository {
    
    public class BatchRepository : IBatchRepository {

        private readonly ApplicationDbContext _applicationDbContext;

        public BatchRepository(ApplicationDbContext applicationDbContext) {
            _applicationDbContext = applicationDbContext;
        }
        public void Create(Batch model) {
            //set the  value that does not include UI
            model.Id = Guid.NewGuid().ToString();
            model.IP = GetLocalIPAddress();
            _applicationDbContext.Batches.Add(model);
            _applicationDbContext.SaveChanges();
        }

        public void Delete(string id) {
            var model = _applicationDbContext.Batches.Find(id);
            if (model != null) {
                model.IsActive = false;
                _applicationDbContext.Entry(model).State = EntityState.Modified;//Updating the existing recrod in db set 
                _applicationDbContext.SaveChanges();//Updating  the record to the database
            }
        }

        public Batch FindById(string id) {
            return _applicationDbContext.Batches.Where(w => w.Id == id).SingleOrDefault();
        }

        public IEnumerable<Batch> ReteriveActive() {
            return _applicationDbContext.Batches.Where(x => x.IsActive == true).ToList();
        }
        private static string GetLocalIPAddress() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork) {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        public void Update(Batch model) {
            //set the  value that does not include UI
            model.UpdatedAt = DateTime.Now;
            model.IP = GetLocalIPAddress();//calling the method 
            _applicationDbContext.Entry(model).State = EntityState.Modified;//Updating the existing recrod in db set 
            _applicationDbContext.SaveChanges();//Updating  the record to the database
        }
    }
}
