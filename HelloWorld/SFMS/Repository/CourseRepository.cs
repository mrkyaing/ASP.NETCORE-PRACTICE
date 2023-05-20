﻿using Microsoft.EntityFrameworkCore;
using SFMS.Models;
using SFMS.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;


namespace SFMS.Repository {
    public class CourseRepository : ICourseRepository {
        
        private readonly ApplicationDbContext _applicationDbContext;

        public CourseRepository(ApplicationDbContext applicationDbContext) {
            _applicationDbContext = applicationDbContext;
        }
        public void Create(Course model) {
            //set the  value that does not include UI
            model.Id = Guid.NewGuid().ToString();
            model.IP = GetLocalIPAddress();
            _applicationDbContext.Courses.Add(model);
            _applicationDbContext.SaveChanges();
        }

        public void Delete(string id) {
            var model = _applicationDbContext.Courses.Find(id);
            if (model != null) {
                model.IsActive = false;
                _applicationDbContext.Entry(model).State = EntityState.Modified;//Updating the existing recrod in db set 
                _applicationDbContext.SaveChanges();//Updating  the record to the database
            }
        }

        public IEnumerable<Course> ReteriveActive() {
            return _applicationDbContext.Courses.Where(x => x.IsActive == true).ToList();
        }

        public void Update(Course model) {
            //set the  value that does not include UI
            model.UpdatedAt = DateTime.Now;
            model.IP = GetLocalIPAddress();//calling the method 
            _applicationDbContext.Entry(model).State = EntityState.Modified;//Updating the existing recrod in db set 
            _applicationDbContext.SaveChanges();//Updating  the record to the database
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

        public Course FindById(string id) {
          return  _applicationDbContext.Courses.Where(w => w.Id == id).SingleOrDefault();
        }
    }
}
